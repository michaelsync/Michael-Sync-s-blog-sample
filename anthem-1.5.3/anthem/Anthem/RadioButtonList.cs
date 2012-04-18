using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Anthem
{
    /// <summary>
    /// Creates an updatable list control that encapsulates a group of radio button controls.
    /// </summary>
    [ToolboxBitmap(typeof(ASP.RadioButtonList))]
    public class RadioButtonList : ASP.RadioButtonList, IUpdatableControl, ICallBackControl
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
                if (null == ViewState["AutoCallBack"])
                    return false;
                else
                    return (bool)ViewState["AutoCallBack"];
            }
            set { ViewState["AutoCallBack"] = value; }
        }

#if V2
        /// <summary>
        /// Override Items collection to force PersistenceMode.InnerProperty. This will cause the control to
        /// wrap the ListItems inside of an &lt;Items&gt; tag which the Visual Studio designer will validate.
        /// If you don't do this, the designer will complain that the "Element 'ListItem' is not a known element."
        /// </summary>
        [Category("Default")]
        [DefaultValue("")]
        [MergableProperty(false)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public override ASP.ListItemCollection Items
        {
            get
            {
                return base.Items;
            }
        }
#endif

#if V2
        /// <summary>
        /// Excecutes <see cref="System.Web.UI.WebControls.BaseDataList.OnSelectedIndexChanged"/>, 
        /// then sets <see cref="UpdateAfterCallBack"/> to true.
        /// </summary>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            this.UpdateAfterCallBack = true;
        }

        /// <summary>
        /// Adds the <strong>onclick</strong> attribute to each item to invoke a callback from the client, 
        /// then renders the item.
        /// </summary>
        protected override void RenderItem(ASP.ListItemType itemType, int repeatIndex, ASP.RepeatInfo repeatInfo, HtmlTextWriter writer)
        {
            if (AutoCallBack)
            {
                ASP.ListItem item = this.Items[repeatIndex];
                if (item.Selected)
                {
                    item.Attributes.Remove("onclick");
                }
                else
                {
                    item.Attributes["onclick"] = Anthem.Manager.GetCallbackEventReference(
                        this,
                        repeatIndex.ToString(),
                        this.CausesValidation,
                        this.ValidationGroup
                    );
                }
            }

            base.RenderItem(itemType, repeatIndex, repeatInfo, writer);
        }
#else
        // The ASP.NET 1.1 version of RadioButtonList does not have an override for creating each
        // item in the list, so we need some to use some method that takes affect on the entire
        // control. One option is to let the base control render to a string and then use Regex
        // to replace __doPostBack with Anthem_FireCallBackEvent. That is how the Calendar
        // control is handled. However, that technique is "fragile" because it will not work
        // if the base control changes how it is rendered (such as a change in HtmlWriter).
        // During my testing, I noticed that ASP.NET 1.1 always rendered the __doPostBack for
        // each item, even if it was already selected. I decided to take advantage of this
        // and also the fact that javascript events bubble up to the parent element. So this
        // override of OnPreRender adds a single Anthem callback to the parent element.
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (AutoCallBack)
            {
                Anthem.Manager.AddScriptAttribute(
                    this,
                    "onclick",
                    string.Format(
                    "AnthemListControl_OnClick(event,{0},'{1}','{2}',{3},{4},{5},{6});",
                    "false",
                    string.Empty,
                    this.TextDuringCallBack,
                    this.EnabledDuringCallBack ? "true" : "false",
                    (this.PreCallBackFunction == null || this.PreCallBackFunction.Length == 0) ? "null" : this.PreCallBackFunction,
                    (this.PostCallBackFunction == null || this.PostCallBackFunction.Length == 0) ? "null" : this.PostCallBackFunction,
                    (this.CallBackCancelledFunction == null || this.CallBackCancelledFunction.Length == 0) ? "null" : this.CallBackCancelledFunction
                    )
                );
            }
        }
#endif

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
                Anthem.Manager.WriteBeginControlMarker(writer, this.RepeatLayout == ASP.RepeatLayout.Flow ? "span" : "div", this);
            }
            if (Visible)
            {
                base.Render(writer);
            }
            if (!DesignMode)
            {
                Anthem.Manager.WriteEndControlMarker(writer, this.RepeatLayout == ASP.RepeatLayout.Flow ? "span" : "div", this);
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
                string text = (string)ViewState["CallBackCancelledFunction"];
                if (text == null)
                    return string.Empty;
                else
                    return text;
            }
            set
            {
                ViewState["CallBackCancelledFunction"] = value;
            }
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
                object obj = this.ViewState["EnableCallBack"];
                if (obj == null)
                    return true;
                else
                    return (bool)obj;
            }
            set
            {
                ViewState["EnableCallBack"] = value;
            }
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
                object obj = this.ViewState["EnabledDuringCallBack"];
                if (obj == null)
                    return true;
                else
                    return (bool)obj;
            }
            set
            {
                ViewState["EnabledDuringCallBack"] = value;
            }
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
                string text = (string)this.ViewState["PostCallBackFunction"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set
            {
                ViewState["PostCallBackFunction"] = value;
            }
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
                string text = (string)this.ViewState["PreCallBackFunction"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set
            {
                ViewState["PreCallBackFunction"] = value;
            }
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
                string text = (string)this.ViewState["TextDuringCallBack"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set
            {
                ViewState["TextDuringCallBack"] = value;
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
