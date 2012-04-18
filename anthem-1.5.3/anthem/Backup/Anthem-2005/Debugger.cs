using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ASP = System.Web.UI.WebControls;

namespace Anthem
{
    /// <summary>
    /// 
    /// </summary>
    public class Debugger : ASP.WebControl, INamingContainer, IUpdatableControl
    {
        /// <summary></summary>
        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div><fieldset><legend>Anthem Debugger</legend>"));
            AddCheckBox("RequestText");
            AddCheckBox("ResponseText");
            AddCheckBox("Errors");
            Controls.Add(new LiteralControl("</fieldset></div>"));
        }

        void AddCheckBox(string option)
        {
            HtmlInputCheckBox cb = new HtmlInputCheckBox();
            Controls.Add(cb);
            cb.ID = option;
            cb.Attributes["onchange"] = string.Format("AnthemDebugger_Debug{0} = this.checked", option);
            Controls.Add(new LiteralControl(string.Format(" {0} ", option)));
        }

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Load"/> event and registers the control
        /// with <see cref="Anthem.Manager"/>.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/>.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Anthem.Manager.Register(this);

            if (Page.IsPostBack)
            {
                HtmlInputCheckBox requestText = FindControl("RequestText") as HtmlInputCheckBox;
                if (requestText != null)
                {
                    DebugRequestText = requestText.Checked;
                }

                HtmlInputCheckBox responseText = FindControl("ResponseText") as HtmlInputCheckBox;
                if (responseText != null)
                {
                    DebugResponseText = responseText.Checked;
                }

                HtmlInputCheckBox errors = FindControl("Errors") as HtmlInputCheckBox;
                if (errors != null)
                {
                    DebugErrors = errors.Checked;
                }
            }
        }

        /// <summary></summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            HtmlInputCheckBox requestText = FindControl("RequestText") as HtmlInputCheckBox;
            if (requestText != null)
            {
                requestText.Checked = DebugRequestText;
            }

            HtmlInputCheckBox responseText = FindControl("ResponseText") as HtmlInputCheckBox;
            if (responseText != null)
            {
                responseText.Checked = DebugResponseText;
            }

            HtmlInputCheckBox errors = FindControl("Errors") as HtmlInputCheckBox;
            if (errors != null)
            {
                errors.Checked = DebugErrors;
            }

            string script = string.Format(
                @"<script language='javascript' type='text/javascript'>
var AnthemDebugger_DebugRequestText = {0};
var AnthemDebugger_DebugResponseText = {1};
var AnthemDebugger_DebugErrors = {2};
function Anthem_DebugRequestText(text) {{
    if (AnthemDebugger_DebugRequestText) {{
        alert('Anthem Debugger (RequestText):\n\n' + text);
    }}
}}
function Anthem_DebugResponseText(text) {{
    if (AnthemDebugger_DebugResponseText) {{
        alert('Anthem Debugger (ResponseText):\n\n' + text);
    }}
}}
function Anthem_DebugError(text) {{
    if (AnthemDebugger_DebugErrors) {{
        alert('Anthem Debugger (Error):\n\n' + text);
    }}
}}
</script>",
                DebugRequestText ? "true" : "false",
                DebugResponseText ? "true" : "false",
                DebugErrors ? "true" : "false"
            );

#if V2
            Page.ClientScript.RegisterClientScriptBlock(typeof(Debugger), "script", script);
#else
            Page.RegisterClientScriptBlock(typeof(Debugger).FullName, script);
#endif
        }

        /// <summary>
        /// Renders the server control wrapped in an additional element so that the
        /// element.innerHTML can be updated after a callback.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            Anthem.Manager.WriteBeginControlMarker(writer, "div", this);
            if (Visible)
            {
                base.Render(writer);
            }
            Anthem.Manager.WriteEndControlMarker(writer, "div", this);
        }

        /// <summary></summary>
        public bool DebugRequestText
        {
            get
            {
                return ViewState["DebugRequestText"] != null
                    ? (bool)ViewState["DebugRequestText"]
                    : false;
            }

            set
            {
                if (IsTrackingViewState && Anthem.Manager.IsCallBack && DebugRequestText != value)
                {
                    Anthem.Manager.AddScriptForClientSideEval(
                        string.Format(
                        "AnthemDebugger_DebugRequestText = {0}",
                        value ? "true" : "false"));
                }
                ViewState["DebugRequestText"] = value;
            }
        }

        /// <summary></summary>
        public bool DebugResponseText
        {
            get
            {
                return ViewState["DebugResponseText"] != null
                    ? (bool)ViewState["DebugResponseText"]
                    : false;
            }

            set
            {
                if (IsTrackingViewState && Anthem.Manager.IsCallBack && DebugResponseText != value)
                {
                    Anthem.Manager.AddScriptForClientSideEval(
                        string.Format(
                            "AnthemDebugger_DebugResponseText = {0}",
                            value ? "true" : "false"));
                }
                ViewState["DebugResponseText"] = value;
            }
        }

        /// <summary></summary>
        public bool DebugErrors
        {
            get
            {
                return ViewState["DebugErrors"] != null
                    ? (bool)ViewState["DebugErrors"]
                    : false;
            }

            set
            {
                if (IsTrackingViewState && Anthem.Manager.IsCallBack && DebugErrors != value)
                {
                    Anthem.Manager.AddScriptForClientSideEval(
                        string.Format(
                        "AnthemDebugger_DebugErrors = {0}",
                        value ? "true" : "false"));
                }
                ViewState["DebugErrors"] = value;
            }
        }

        #region Common Anthem control members (copied into each control)

#if V2
        /// <summary>
        /// Forces the server control to output content and trace information.
        /// </summary>
        public override void RenderControl(HtmlTextWriter writer)
        {
            base.Visible = true;
            base.RenderControl(writer);
        }
#endif

        /// <summary>
        /// Overrides the Visible property so that Anthem.Manager can track the visibility.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the control is rendered on the client; otherwise
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
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

        private bool _updateAfterCallBack = false;

        /// <summary>
        /// Gets or sets a value indicating whether the control should be updated after each callback.
        /// Also see <see cref="UpdateAfterCallBack"/>.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control should be updated; otherwise,
        /// <strong>false</strong>. The default is <strong>false</strong>.
        /// </value>
        /// <example>
        /// 	<code lang="CS" description="This is normally used declaratively as shown here.">
        /// &lt;anthem:Label id="label" runat="server" AutoUpdateAfterCallBack="true" /&gt;
        ///     </code>
        /// </example>
        [DefaultValue(false)]
        public virtual bool AutoUpdateAfterCallBack
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

        /// <summary>
        /// Gets or sets a value which indicates whether the control should be updated after the current callback.
        /// Also see <see cref="AutoUpdateAfterCallBack"/>.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control should be updated; otherwise,
        /// <strong>false</strong>. The default is <strong>false</strong>.
        /// </value>
        /// <example>
        /// 	<code lang="CS" description="This is normally used in server code as shown here.">
        /// this.Label = "Count = " + count;
        /// this.Label.UpdateAfterCallBack = true;
        ///     </code>
        /// </example>
        [Browsable(false), DefaultValue(false)]
        public virtual bool UpdateAfterCallBack
        {
            get { return _updateAfterCallBack; }
            set { _updateAfterCallBack = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control uses callbacks instead of postbacks to post data to the server.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control uses callbacks; otherwise,
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
        [DefaultValue(true)]
        public virtual bool EnableCallBack
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
    }
}
