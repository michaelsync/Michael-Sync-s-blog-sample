using System;
using System.ComponentModel;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Anthem.Examples
{
    public class CustomControl2 : ASP.WebControl, IPostBackEventHandler, ICallBackControl, IUpdatableControl
    {
        /// <summary>
        /// Override OnLoad to register the control with Anthem
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Anthem.Manager.Register(this);
        }

        /// <summary>
        /// Add the client onclick event handler that will fire the callback
        /// </summary>
        /// <param name="writer"></param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", ClientID);
            Anthem.Manager.AddScriptAttribute(
                this,
                "onclick",
                Anthem.Manager.GetCallbackEventReference(
                    this,           // Reference to this ICallbackControl
                    string.Empty,   // The event argument
                    false,          // Causes validation
                    string.Empty,   // Validation group
                    string.Empty    // Image during callback
                )
            );
            base.AddAttributesToRender(writer);
        }

        /// <summary>
        /// Catch the callback (ASP.NET thinks this is a PostBack) and redirect
        /// to the OnClick handler. You could check the eventArgument (if you
        /// add one in GetCallbackEventReference) to determine what to do.
        /// </summary>
        /// <param name="eventArgument"></param>
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            this.OnClick(EventArgs.Empty);
        }

        /// <summary>
        /// Handle the click. In this example, the Value property is set
        /// to the current datetime and UpdateAfterCallBack is used to 
        /// update the display on the client.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClick(EventArgs e)
        {
            ViewState["Value"] = DateTime.Now.ToString();
            this.UpdateAfterCallBack = true;
        }

        /// <summary>
        /// Render the contents of the control
        /// </summary>
        /// <param name="writer"></param>
        protected override void RenderContents(HtmlTextWriter writer)
        {
            writer.Write(Text);
            if (Value != string.Empty)
                writer.Write(" : " + Value);
        }

        #region Control Properties

        [DefaultValue(""),
        Description("The prompt text")]
        public string Text
        {
            get
            {
                if (null == ViewState["Text"])
                    return string.Empty;
                else
                    return (string)ViewState["Text"];
            }
            set
            {
                ViewState["Text"] = value;
            }
        }

        [Browsable(false)]
        public string Value
        {
            get
            {
                if (null == ViewState["Value"])
                    return string.Empty;
                else
                    return (string)ViewState["Value"];
            }
        }

        #endregion

        // The rest of this control is standard stuff for any Anthem that
        // implements ICallBackControl and IUpdatableControl.

        private const string parentTagName = "span";
        protected override void Render(HtmlTextWriter writer)
        {
#if !V2
            bool DesignMode = this.Context == null;
#endif
            if (!DesignMode)
            {
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
}
