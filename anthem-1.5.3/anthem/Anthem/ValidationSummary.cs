using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Anthem
{
    /// <summary>
    /// Creates an updatable control that displays a summary of all validation errors inline on a Web page, in a message box, or both.
    /// </summary>
    [ToolboxBitmap(typeof(ASP.ValidationSummary))]
    public class ValidationSummary : ASP.ValidationSummary, IUpdatableControl
    {
        #region Unique Anthem control code

        /// <summary>
        /// Gets or sets a value which indicates whether the control should display the validation summary as a client-side alert.
        /// </summary>
        /// <value><strong>true</strong> if the control should display the validation summary on the client after a callback; otherwise, <strong>false</strong>. The default is <strong>true</strong>.</value>
        [Category("Anthem")]
        [DefaultValue(true)]
        [Description("Gets or sets a value which indicates whether the control should display the validation summary as a client-side alert.")]
        public virtual bool EnableCallBackScript
        {
            get
            {
                object obj = this.ViewState["EnableCallBackScript"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set 
            { 
                ViewState["EnableCallBackScript"] = value; 
            }
        }

        /// <summary>
        /// If ShowMessageBox=true, use Anthem.Manager to display the validation
        /// summary in a javascript alert.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            bool isValid = true;
            foreach (IValidator validator in Page.Validators)
                isValid &= validator.IsValid;

            if (Anthem.Manager.IsCallBack
                && !isValid
                && this.ShowMessageBox
                && this.EnableCallBackScript)
            {
                string s = string.Empty;
                if (this.HeaderText != null && this.HeaderText != string.Empty)
                    s += this.HeaderText + "\\n";
                int count = 0;
                foreach (ASP.BaseValidator validator in Page.Validators)
                {

#if V2
            if (
                ((
                (this.ValidationGroup == null || this.ValidationGroup == string.Empty)
                && (validator.ValidationGroup == null || validator.ValidationGroup == string.Empty)
                )
                ||
                (
                (this.ValidationGroup != null && this.ValidationGroup != string.Empty)
                && (validator.ValidationGroup != null && validator.ValidationGroup == this.ValidationGroup)
                ))
                && !validator.IsValid
                && validator.ErrorMessage != null
                && validator.ErrorMessage != string.Empty)
#else
                    if (!validator.IsValid 
                        && validator.ErrorMessage != null 
                        && validator.ErrorMessage != string.Empty)
#endif

                    {
                        count++;
                        switch (this.DisplayMode)
                        {
                            case System.Web.UI.WebControls.ValidationSummaryDisplayMode.List:
                                s += validator.ErrorMessage + "\\n";
                                break;
                            case System.Web.UI.WebControls.ValidationSummaryDisplayMode.BulletList:
                            default:
                                s += "  - " + validator.ErrorMessage + "\\n";
                                break;
                            case System.Web.UI.WebControls.ValidationSummaryDisplayMode.SingleParagraph:
                                s += validator.ErrorMessage + " ";
                                break;
                        }
                    }
                }
                if (count > 0)
                    Anthem.Manager.AddScriptForClientSideEval("alert('" + s + "');");
            }
        }

        private const string parentTagName = "div";
        /// <summary>
        /// Renders the server control wrapped in an additional element so that the
        /// element.innerHTML can be updated after a callback.
        /// </summary>
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

        #endregion

        #region IUpdatableControl implementation

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
        [Category("Anthem")]
        [DefaultValue(false)]
        [Description("True if this control should be updated after each callback.")]
        public virtual bool AutoUpdateAfterCallBack
        {
            get
            {
                object obj = this.ViewState["AutoUpdateAfterCallBack"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set
            {
                if (value) UpdateAfterCallBack = true;
                ViewState["AutoUpdateAfterCallBack"] = value;
            }
        }

        private bool _updateAfterCallBack = false;

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
        [Browsable(false)]
        [DefaultValue(false)]
        public virtual bool UpdateAfterCallBack
        {
            get { return _updateAfterCallBack; }
            set { _updateAfterCallBack = value; }
        }

        #endregion

        #region Common Anthem control code

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Load"/> event and registers the control
        /// with <see cref="Anthem.Manager"/>.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/>.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Anthem.Manager.Register(this);
        }

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

        #endregion
    }
}
