using System;
using System.Text;

using Anthem;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ASP = System.Web.UI.WebControls;

namespace AnthemExtensions
{
	public class ProgressBar : ASP.Label, IUpdatableControl
	{
		private const string parentTagName = "span";
		private int _percent = 0;
		private string _backgroundcolor = "red";
		private int _barwidth = 100;
		private int _multiplier = 2; // (_barwidth / 50)
		private int _callbackinterval = 250;

		#region Properties

		[DefaultValue(0)]
		public int Percent
		{
			get { return _percent; }
			set { _percent = value; }
		}

		[DefaultValue("red")]
		public string BackGroundColor
		{
			get { return _backgroundcolor; }
			set { _backgroundcolor = value; }
		}

		[DefaultValue(100)]
		public int BarWidth
		{
			get { return _barwidth; }
			set
			{   // validate width
				if (value == 50 || value == 100 || value == 150 || value == 200 || value == 250)
					_barwidth = value;
			}
		}

		[DefaultValue(2)]
		public int Multiplier
		{
			get { return _multiplier; }
			set
			{   // validate multiplier
				if (value == (_barwidth / 50))
					_multiplier = value;
			}
		}

		[DefaultValue(250)]
		public int CallBackInterval
		{
			get { return _callbackinterval; }
			set { _callbackinterval = value; }
		}

		public void SetPercent()
		{
			ASP.Label lbl = FindControl("lblPercent") as ASP.Label;
			lbl.Width = ASP.Unit.Pixel(_percent);
		}

		#endregion

		#region Events
		#endregion

		#region WebControl Overrides

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			Anthem.Manager.Register(this);
		}

#if V2
    public override void RenderControl(HtmlTextWriter writer)
    {
      base.Visible = true;
      base.RenderControl(writer);
    }
#endif

		protected override void Render(HtmlTextWriter writer)
		{
#if !V2
			bool DesignMode = this.Context == null;
#endif
			if (!DesignMode)
			{
				// parentTagName must be defined as a private const string field in this class.
				Anthem.Manager.WriteBeginControlMarker(writer, parentTagName, this);
			}
			if (Visible)
			{
				base.Render(writer);
			}
			if (!DesignMode)
			{
				Anthem.Manager.WriteEndControlMarker(writer, parentTagName, this);
			}
		}

		protected override void CreateChildControls()
		{
			Controls.Add(new LiteralControl("<div style='FONT-SIZE: 10pt; border: solid; border-color:Maroon; border-width: 1px; width: " + Convert.ToString(BarWidth) + "px; height: 10px;'>"));
			Controls.Add(new LiteralControl("<div id='lblPercent' style='FONT-SIZE: 10pt; width: 0px; height: 10px; border-width: 1px;'></div>"));
			Controls.Add(new LiteralControl("</div>"));
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			string script = string.Format(
				@"<script language='javascript' type='text/javascript'>
//![CDATA[ 
var countStatus = 0;
var ft;

function populateTable() {{
  window.status = 'Getting data from web server... ';
  if (countStatus == -1) {{
      ClearProgressBar();
  }}
  Anthem_InvokePageMethod(
      'UpdateCounter',
      [countStatus],
      function(result) {{
          document.getElementById('lblPercent').style.backgroundColor = '{0}';
          document.getElementById('lblPercent').style.width = result.value*{2} + 'px';
          countStatus = result.value;
          if (countStatus == 50) {{
              countStatus = -1;
          }}
      }}
  );
  ft = window.setTimeout('populateTable()', {1});
  if (countStatus == 49)
  {{
      window.clearTimeout(ft);
      countStatus = -1;
      window.status = 'Getting data from web server... done.';
  }}
}}

function ClearProgressBar() {{
  document.getElementById('lblPercent').style.background = 'white';
  countStatus = 0;
}}
//]]> 
</script>",
				BackGroundColor,
				CallBackInterval,
				Multiplier
				);
#if V2
      Page.ClientScript.RegisterClientScriptBlock(typeof(ProgressBar), "script", script);
#else
			Page.RegisterClientScriptBlock(typeof(ProgressBar).FullName, script);
#endif
		}

		public override bool Visible
		{
			get
			{
#if !V2 
				bool DesignMode = this.Context == null; 
#endif
				return Anthem.Manager.GetControlVisible(this, ViewState, DesignMode);
			}
			set { Anthem.Manager.SetControlVisible(ViewState, value); }
		}

		#endregion

		#region ICallBackControl Members

		[DefaultValue(true)]
		public bool EnableCallBack
		{
			get
			{
				if (ViewState["EnableCallBack"] == null)
					return true;
				else
					return (bool)ViewState["EnableCallBack"];
			}
			set
			{
				ViewState["EnableCallBack"] = value;
			}
		}

		#endregion

		#region IPostBackEventHandler Members
		#endregion

		#region Anthem Methods
		#endregion

		#region Public Methods
		#endregion

		#region Private Methods
		#endregion

		#region IUpdatableControl Members

		[DefaultValue(false)]
		public bool AutoUpdateAfterCallBack
		{
			get
			{
				if (ViewState["AutoUpdateAfterCallBack"] == null)
					return false;
				else
					return (bool)ViewState["AutoUpdateAfterCallBack"];
			}
			set
			{
				if (value) UpdateAfterCallBack = true;
				ViewState["AutoUpdateAfterCallBack"] = value;
			}
		}

		private bool _updateAfterCallBack = false;

		[Browsable(false)]
		public bool UpdateAfterCallBack
		{
			get { return _updateAfterCallBack; }
			set { _updateAfterCallBack = value; }
		}

		[
		Category("Misc"),
		Description("Fires before the control is rendered with updated values.")
		]
		public event EventHandler PreUpdate
		{
			add { Events.AddHandler(EventPreUpdateKey, value); }
			remove { Events.RemoveHandler(EventPreUpdateKey, value); }
		}

		private static readonly object EventPreUpdateKey = new object();

		public virtual void OnPreUpdate()
		{
			EventHandler EditHandler = (EventHandler)Events[EventPreUpdateKey];
			if (EditHandler != null)
				EditHandler(this, EventArgs.Empty);
		}

		#endregion
	}
}
