using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

using Anthem;
using ASP = System.Web.UI.WebControls;

#if V2
[assembly: WebResource("AnthemExtensions.EditLabel.js", "text/javascript")]
[assembly: WebResource("AnthemExtensions.EditLabelIE.js", "text/javascript")]
#endif

namespace AnthemExtensions
{
    /// <summary>
    /// EditLabel looks and behaves like a Label until you click on it. If you are using Internet
    /// Explorer, the label immediately becomes editable. If you are using any other browser, the
    /// label is replaced by a textbox allowing you to edit the label.
    /// </summary>
    [
    ParseChildren(true),
    ToolboxData("<{0}:EditLabel runat=\"server\" Text=\"EditLabel\"></{0}:EditLabel>"), 
    ]
    public class EditLabel 
        : ASP.Label,            // Inherit WebControl to get standard WebControl properties
        ICallBackControl,       // Implements standard callback properties
        IPostBackEventHandler,  // Capture events from the client
        IUpdatableControl
    {
        private bool _showTextbox = false;

        #region Properties

        [
        Category("Behavior"),
        Description("Allow editing."),
        DefaultValue(false),
        ]
        public bool ReadOnly
        {
            get
            {
                if (ViewState["ReadOnly"] == null)
                    return false;
                else
                    return (bool)ViewState["ReadOnly"];
            }
            set
            {
                ViewState["ReadOnly"] = value;
            }
        }

        #endregion

        #region Events

        [
        Category("Misc"),
        Description("Fires before the control enters edit mode.")
        ]
        public event EventHandler Edit
        {
            add { Events.AddHandler(EventEditKey, value); }
            remove { Events.RemoveHandler(EventEditKey, value); }
        }
        private static readonly object EventEditKey = new object();

        /// <summary>
        /// Raise the Edit event
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnEdit()
        {
            EventHandler EditHandler = (EventHandler)Events[EventEditKey];
            if (EditHandler != null)
                EditHandler(this, EventArgs.Empty);
        }

        [
        Category("Misc"),
        Description("Fires after the control exits edit mode.")
        ]
        public event EditLabelSaveEventHandler Save
        {
            add { Events.AddHandler(EventSaveKey, value); }
            remove { Events.RemoveHandler(EventSaveKey, value); }
        }
        private static readonly object EventSaveKey = new object();

        /// <summary>
        /// Raise the Save event.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSave(EditLabelSaveEventArgs e)
        {
            EditLabelSaveEventHandler SaveHandler = (EditLabelSaveEventHandler)Events[EventSaveKey];
            if (SaveHandler != null)
                SaveHandler(this, e);
        }

        #endregion

        #region WebControl Overrides

        /// <summary>
        /// Add the attributes to invoke the appropriate callbacks.
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);

#if !V2
            bool DesignMode = this.Context == null;
#endif

            if (!DesignMode && !ReadOnly)
            {
                if (HttpContext.Current.Request.Browser.Browser == "IE")
                {
                    // Internet Explorer can edit in place
                    writer.AddAttribute("contenteditable", "true");
                    writer.AddAttribute("onfocus", 
                        string.Format("editLabelEdit(this,{0},'{1}','{2}','{3}',{4},{5},{6},{7});",
                            "false",        // Causes validation
                            string.Empty,   // Validation group
                            string.Empty,   // Image during callback
                            this.TextDuringCallBack,
                            this.EnabledDuringCallBack ? "true" : "false",
                            this.PreCallBackFunction == null || this.PreCallBackFunction.Length == 0 ? "null" : this.PreCallBackFunction,
                            this.PostCallBackFunction == null || this.PostCallBackFunction.Length == 0 ? "null" : this.PostCallBackFunction,
                            this.CallBackCancelledFunction == null || this.CallBackCancelledFunction.Length == 0 ? "null" : this.CallBackCancelledFunction
                        )
                    );
                    writer.AddAttribute("onblur",
                        string.Format("editLabelSave(this,{0},'{1}','{2}','{3}',{4},{5},{6},{7});",
                            "false",        // Causes validation
                            string.Empty,   // Validation group
                            string.Empty,   // Image during callback
                            this.TextDuringCallBack,
                            this.EnabledDuringCallBack ? "true" : "false",
                            this.PreCallBackFunction == null || this.PreCallBackFunction.Length == 0 ? "null" : this.PreCallBackFunction,
                            this.PostCallBackFunction == null || this.PostCallBackFunction.Length == 0 ? "null" : this.PostCallBackFunction,
                            this.CallBackCancelledFunction == null || this.CallBackCancelledFunction.Length == 0 ? "null" : this.CallBackCancelledFunction
                        )
                    );
                }
                else
                {
                    // Non-IE browsers will cause a Click event that is handled
                    // on the server (see RaisePostBackEvent) by replacing the
                    // label with a textbox.
                    writer.AddAttribute(
                        "onclick",          // The javascript event to capture
                        Anthem.Manager.GetCallbackEventReference(
                            this,           // Reference to this ICallbackControl
                            "Edit",         // The event argument
                            false,          // Causes validation
                            string.Empty,   // Validation group
                            string.Empty    // Image during callback
                        )
                    );
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Manager.Register(this);
        }

        protected override void OnPreRender(EventArgs e)
        {
#if !V2
            bool DesignMode = this.Context == null;
#endif

            if (!DesignMode)
            {
                if (HttpContext.Current.Request.Browser.Browser == "IE")
                {
#if V2
                    Page.ClientScript.RegisterClientScriptResource(typeof(EditLabel), "AnthemExtensions.EditLabelIE.js");
#else
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnthemExtensions.EditLabelIE.js");
                    StreamReader sr = new StreamReader(stream);
                    Page.RegisterClientScriptBlock(typeof(EditLabel).FullName,
                        @"<script type=""text/javascript"">
//<![CDATA[
" + sr.ReadToEnd() + @"
//]]>
</script>");
#endif
                }
                else
                {
#if V2
                    Page.ClientScript.RegisterClientScriptResource(typeof(EditLabel), "AnthemExtensions.EditLabel.js");
#else
                    Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("AnthemExtensions.EditLabel.js");
                    StreamReader sr = new StreamReader(stream);
                    Page.RegisterClientScriptBlock(typeof(EditLabel).FullName,
                        @"<script type=""text/javascript"">
//<![CDATA[
" + sr.ReadToEnd() + @"
//]]>
</script>");
#endif
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
#if !V2
            bool DesignMode = this.Context == null;
#endif
            if (!DesignMode)
            {
                Anthem.Manager.WriteBeginControlMarker(writer, "span", this);
            }
            if (Visible)
            {
                if (!DesignMode && _showTextbox)
                {
                    // Non-IE browsers get a textbox for editing.
                    ASP.TextBox textbox = new ASP.TextBox();
                    textbox.ID = this.ClientID;
                    textbox.Text = Text;
                    textbox.ApplyStyle(GetStyle());
#if V2
                    textbox.EnableTheming = this.EnableTheming;
#endif
                    textbox.Attributes["onblur"] = string.Format("editLabelSave(this,{0},'{1}','{2}','{3}',{4},{5},{6},{7});",
                        "false",
                        string.Empty,
                        string.Empty,
                        this.TextDuringCallBack,
                        this.EnabledDuringCallBack ? "true" : "false",
                        this.PreCallBackFunction == null || this.PreCallBackFunction.Length == 0 ? "null" : this.PreCallBackFunction,
                        this.PostCallBackFunction == null || this.PostCallBackFunction.Length == 0 ? "null" : this.PostCallBackFunction,
                        this.CallBackCancelledFunction == null || this.CallBackCancelledFunction.Length == 0 ? "null" : this.CallBackCancelledFunction
                    );
                    textbox.Attributes["onkeypress"] = "return editLabelCheckKey(event);";
                    textbox.RenderControl(writer);
                }
                else
                {
                    base.Render(writer);
                }
            }
            if (!DesignMode)
            {
                Anthem.Manager.WriteEndControlMarker(writer, "span", this);
            }
        }

        #endregion

        #region ICallBackControl Members

        [DefaultValue("")]
        public virtual string CallBackCancelledFunction
        {
            get
            {
                if (null == ViewState["CallBackCancelledFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["CallBackCancelledFunction"];
            }
            set { ViewState["CallBackCancelledFunction"] = value; }
        }

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

        [DefaultValue(true)]
        public virtual bool EnabledDuringCallBack
        {
            get
            {
                if (null == ViewState["EnabledDuringCallBack"])
                    return true;
                else
                    return (bool)ViewState["EnabledDuringCallBack"];
            }
            set { ViewState["EnabledDuringCallBack"] = value; }
        }

        [DefaultValue("")]
        public virtual string PostCallBackFunction
        {
            get
            {
                if (null == ViewState["PostCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PostCallBackFunction"];
            }
            set { ViewState["PostCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public virtual string PreCallBackFunction
        {
            get
            {
                if (null == ViewState["PreCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PreCallBackFunction"];
            }
            set { ViewState["PreCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public virtual string TextDuringCallBack
        {
            get
            {
                if (null == ViewState["TextDuringCallBack"])
                    return string.Empty;
                else
                    return (string)ViewState["TextDuringCallBack"];
            }
            set { ViewState["TextDuringCallBack"] = value; }
        }

        #endregion

        #region IPostBackEventHandler Members

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "Edit")
            {
                OnEdit();
                _showTextbox = true;
                UpdateAfterCallBack = true;
                Manager.AddScriptForClientSideEval(string.Format("document.getElementById('{0}').select();document.getElementById('{0}').focus();", this.ClientID));
            }
        }

        #endregion

        #region Anthem Methods

        [Anthem.Method]
        public void EditIELabel()
        {
            OnEdit();
        }

        [Anthem.Method]
        public EditLabelSaveEventArgs SaveLabel(string value, bool updateAfterCallBack)
        {
            EditLabelSaveEventArgs e = new EditLabelSaveEventArgs(value);
            OnSave(e);
            if (!e.Cancel)
                Text = e.Text;
            if (updateAfterCallBack)
                this.UpdateAfterCallBack = true;
            return e;
        }

        #endregion

        #region Private Methods

        private ASP.Style GetStyle()
        {
            ASP.Style style = new ASP.Style();
            style.BackColor = this.BackColor;
            style.BorderColor = this.BorderColor;
            style.BorderStyle = this.BorderStyle;
            style.BorderWidth = this.BorderWidth;
            style.CssClass = this.CssClass;
            style.Font.CopyFrom(this.Font);
            style.ForeColor = this.ForeColor;
            style.Height = this.Height;
            style.Width = this.Width;
            return style;
        }

        #endregion

        #region IUpdatableControl Members

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

        private bool _updateAfterCallBack = false;

        [Browsable(false)]
        public virtual bool UpdateAfterCallBack
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

    #region EditLabel Event Support Code

    /// <summary>
    /// Save event handler for the EditLabel
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EditLabelSaveEventHandler(object sender, EditLabelSaveEventArgs e);

    /// <summary>
    /// Event arguments for the EditLabel events
    /// </summary>
    public class EditLabelSaveEventArgs : CancelEventArgs
    {
        public EditLabelSaveEventArgs(string text)
            : base()
        {
            _text = text;
        }

        public EditLabelSaveEventArgs(string text, bool cancel)
            : base(cancel)
        {
            _text = text;
        }

        private string _text = string.Empty;
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
    }

    #endregion
}
