using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Anthem;

namespace Anthem
{
    /// <summary>
    /// Creates an updatable control that allows the user to select a single item from a drop-down list.
    /// </summary>
    [ToolboxBitmap(typeof (System.Web.UI.HtmlControls.HtmlSelect))]
    public class HtmlSelect : System.Web.UI.HtmlControls.HtmlSelect, IUpdatableControl, ICallBackControl
    {
        #region Unique Anthem control code

        /// <summary>
        /// Gets or sets a value indicating whether the Checkbox state automatically calls back to
        /// the server when clicked. Mimics the AutoPostBack property.
        /// </summary>
        [DefaultValue(false)]
        public bool AutoCallBack
        {
            get
            {
                object obj = ViewState["AutoCallBack"]; 
                if (obj == null)
                    return false;
                else
                    return (bool) obj;
            }
            set { ViewState["AutoCallBack"] = value; }
        }

        public string CssClass
        {
            get 
            { 
                object obj = ViewState["CssClass"]; 
                if (obj == null)
                    return string.Empty;
                else
                    return (string) obj;
            }
            set { ViewState["CssClass"] = value; }
        }

        public bool CausesValidation
        {
            get 
            {
                object obj = ViewState["CausesValidation"]; 
                if (obj == null)
                    return false;
                else
                    return (bool) obj;
            }
            set { ViewState["CausesValidation"] = value; }
        }

        [Description("Get or set the selected value. If Multiple is true, all matching items are selected.")]
        public string SelectedValue
        {
            get
            {
                foreach (ListItem item in Items)
                    if (item.Selected) return item.Value;
                return null;
            }
            set
            {
                if (Multiple)
                {
                    foreach (ListItem item in Items)
                        item.Selected = string.Compare(item.Value, value, true) == 0;
                }
                else
                {
                    foreach (ListItem item in Items)
                        item.Selected = false;
                    foreach (ListItem item in Items)
                    {
                        if (string.Compare(item.Value, value, true) == 0)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                }
            }
        }

        [Description("Get and set the selected values. If Multiple is false, only the first matching item is set or returned.")]
        public string[] SelectedValues
        {
            get
            {
                ArrayList values = new ArrayList();
                foreach (ListItem item in Items)
                    if (item.Selected) values.Add(item.Value);
                return (string[]) values.ToArray(typeof(string));
            }
            set
            {
                if (!Multiple)
                {
                    foreach (ListItem item in Items)
                        item.Selected = false;
                }
                foreach (ListItem item in Items)
                {
                    foreach (string oneValue in value)
                    {
                        if (String.Compare(item.Value, oneValue, true) == 0)
                        {
                            item.Selected = true;
                            break;
                        }
                    }
                    if (!Multiple && item.Selected) break;
                }
            }
        }
        
        [Description("The tooltip to display")]
        [Category("Style")]
        [DefaultValue("")]
        public string ToolTip
        {
            get
            {
                object obj = ViewState["ToolTip"]; 
                if (obj == null)
                    return string.Empty;
                else
                    return (string) obj;
            }
            set { ViewState["ToolTip"] = value; }
        }

        public string ValidationGroup
        {
            get
            {
                object obj = ViewState["ValidationGroup"]; 
                if (obj == null)
                    return string.Empty;
                else
                    return (string) obj;
            }
            set { ViewState["ValidationGroup"] = value; }
        }

        private const string parentTagName = "div";

        /// <summary>
        /// Renders the server control wrapped in an additional element so that the
        /// element.innerHTML can be updated after a callback.
        /// </summary>
        protected override void Render(HtmlTextWriter writer)
        {
            if (Manager.IsViewStateDirty(ViewState))
                UpdateAfterCallBack = true;
#if !V2
            bool DesignMode = this.Context == null;
#endif
            if (!DesignMode)
            {
                // parentTagName must be defined as a private const string field in this class.
                Manager.WriteBeginControlMarker(writer, parentTagName, this);
            }
            if (Visible)
            {
                base.Render(writer);
            }
            else
            {
                writer.Write("<!-- -->");
            }
            if (!DesignMode)
            {
                Manager.WriteEndControlMarker(writer, parentTagName, this);
            }
        }

        #endregion

        #region ICallBackControl implementation

        /// <summary>
        /// Gets or sets the javascript function to execute on the client if the callback is
        /// cancelled. See <see cref="PreCallBackFunction"/>.
        /// </summary>
        [Category("Anthem")]
        [DefaultValue("")]
        [Description("The javascript function to call on the client if the callback is cancelled.")]
        public virtual string CallBackCancelledFunction
        {
            get
            {
                string text = (string) ViewState["CallBackCancelledFunction"];
                if (text == null)
                    return string.Empty;
                else
                    return text;
            }
            set { ViewState["CallBackCancelledFunction"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control uses callbacks instead of postbacks to post data to the server.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control uses callbacks; otherwise,
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
        [Category("Anthem")]
        [DefaultValue(true)]
        [Description("True if this control uses callbacks instead of postbacks to post data to the server.")]
        public virtual bool EnableCallBack
        {
            get
            {
                object obj = ViewState["EnableCallBack"];
                if (obj == null)
                    return true;
                else
                    return (bool) obj;
            }
            set { ViewState["EnableCallBack"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control is enabled on the client during callbacks.
        /// </summary>
        /// <value>
        /// 	<strong>true</strong> if the the control is enabled; otherwise,
        /// <strong>false</strong>. The default is <strong>true</strong>.
        /// </value>
        /// <remarks>Not all HTML elements support this property.</remarks>
        [Category("Anthem")]
        [DefaultValue(true)]
        [Description("True if this control is enabled on the client during callbacks.")]
        public virtual bool EnabledDuringCallBack
        {
            get
            {
                object obj = ViewState["EnabledDuringCallBack"];
                if (obj == null)
                    return true;
                else
                    return (bool) obj;
            }
            set { ViewState["EnabledDuringCallBack"] = value; }
        }


        /// <summary>
        /// Gets or sets the javascript function to execute on the client after the callback
        /// response is received.
        /// </summary>
        /// <remarks>
        /// The callback response is passed into the PostCallBackFunction as the one and only
        /// parameter.
        /// </remarks>
        /// <example>
        /// 	<code lang="JScript" description="This example shows a PostCallBackFunction that displays the error if there is one.">
        /// function AfterCallBack(result) {
        ///   if (result.error != null &amp;&amp; result.error.length &gt; 0) {
        ///     alert(result.error);
        ///   }
        /// }
        ///     </code>
        /// </example>
        [Category("Anthem")]
        [DefaultValue("")]
        [Description("The javascript function to call on the client after the callback response is received.")]
        public virtual string PostCallBackFunction
        {
            get
            {
                string text = (string) ViewState["PostCallBackFunction"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { ViewState["PostCallBackFunction"] = value; }
        }

        /// <summary>
        /// Gets or sets the javascript function to execute on the client before the callback
        /// is made.
        /// </summary>
        /// <remarks>The function should return false on the client to cancel the callback.</remarks>
        [Category("Anthem")]
        [DefaultValue("")]
        [Description("The javascript function to call on the client before the callback is made.")]
        public virtual string PreCallBackFunction
        {
            get
            {
                string text = (string) ViewState["PreCallBackFunction"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { ViewState["PreCallBackFunction"] = value; }
        }

        /// <summary>Gets or sets the text to display on the client during the callback.</summary>
        /// <remarks>
        /// If the HTML element that invoked the callback has a text value (such as &lt;input
        /// type="button" value="Run"&gt;) then the text of the element is updated during the
        /// callback, otherwise the associated &lt;label&gt; text is updated during the callback.
        /// If the element does not have a text value, and if there is no associated &lt;label&gt;,
        /// then this property is ignored.
        /// </remarks>
        [Category("Anthem")]
        [DefaultValue("")]
        [Description("The text to display during the callback.")]
        public virtual string TextDuringCallBack
        {
            get
            {
                string text = (string) ViewState["TextDuringCallBack"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { ViewState["TextDuringCallBack"] = value; }
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
                object obj = ViewState["AutoUpdateAfterCallBack"];
                if (obj == null)
                    return false;
                else
                    return (bool) obj;
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
            Manager.Register(this);
        }

        protected override void RenderAttributes(HtmlTextWriter writer)
        {
            if (ToolTip != null && ToolTip != string.Empty)
                Attributes["title"] = ToolTip;
            if (CssClass != null && CssClass != string.Empty)
                Attributes["class"] = CssClass;
            foreach (ListItem item in Items)
                item.Attributes["title"] = item.Text;
            if (AutoCallBack)
            {
                Manager.AddScriptAttribute(
                    this,
                    "onchange",
                    Manager.GetCallbackEventReference(
                    this,
                    CausesValidation,
                    ValidationGroup
                    )
                );
            }
            base.RenderAttributes (writer);
        }

        /// <summary>
        /// Overrides the Visible property so that Base.Manager can track the visibility.
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
                return Manager.GetControlVisible(this, ViewState, DesignMode); 
            }
            set { Manager.SetControlVisible(ViewState, value); }
        }

        #endregion
    }
}