#if V2

using System;
using System.ComponentModel;
using System.Drawing;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using WebParts = System.Web.UI.WebControls.WebParts;

namespace Anthem
{
    /// <summary>
    /// Creates an updatable control in the Web Parts control set for hosting WebPart controls on a Web page. 
    /// </summary>
    [ToolboxBitmap(typeof(WebParts.WebPartZone))]
    public class WebPartZone : WebParts.WebPartZone, IUpdatableControl
    {
        #region Unique Anthem control code

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterStartupScript(
                typeof(WebPartZone),
                "hack",
                @"<script type='text/javascript'>

function WebPartManager_SubmitPage(eventTarget, eventArgument) {
    if ((typeof(this.menu) != 'undefined') && (this.menu != null)) {
        this.menu.Hide();
    }
    Anthem_FireEvent(eventTarget, eventArgument, function() { }, null, true, true);
}

function Zone_AddWebPart(webPartElement, webPartTitleElement, allowZoneChange) {
    var webPart = null;
    var zoneIndex = this.webParts.length;

    for (var i = 0; i < this.webParts.length; ++i) {
        if (this.webParts[i].webPartElement.id == webPartElement.id) {
            zoneIndex = i;
            break;
        }
    }

    if (this.allowLayoutChange && __wpm.IsDragDropEnabled()) {
        webPart = new WebPart(webPartElement, webPartTitleElement, this, zoneIndex, allowZoneChange);
    }
    else {
        webPart = new WebPart(webPartElement, null, this, zoneIndex, allowZoneChange);
    }
    this.webParts[zoneIndex] = webPart;
    return webPart;
}

function ReAddWebParts(zoneID) {
    for (var i = 0; i < __wpm.zones.length; ++i) {
        var zone = __wpm.zones[i];
        if (zone.uniqueID == zoneID) {
            for (var j = 0; j < zone.webParts.length; ++j) {
                var webPartID = zone.webParts[j].webPartElement.id;
                var webPart = document.getElementById(webPartID);
                if (webPart) {
                    zone.AddWebPart(webPart, null, false);
                    var webPartVerbs = document.getElementById(webPartID + 'Verbs');
                    var webPartVerbsPopup = document.getElementById(webPartID + 'VerbsPopup');
                    var webPartVerbsMenu = document.getElementById(webPartID + 'VerbsMenu');
                    var menu = new WebPartMenu(webPartVerbs, webPartVerbsPopup, webPartVerbsMenu);
                    menu.itemStyle = '';
                    menu.itemHoverStyle = '';
                    menu.labelHoverColor = '';
                    menu.labelHoverClassName = '';
                }
            }
            break;
        }
    }
}

</script>");
        }

        /// <summary>
        /// Excecutes <see cref="System.Web.UI.WebControls.WebParts.WebPartZoneBase.MinimizeWebPart"/>, 
        /// then sets <see cref="UpdateAfterCallBack"/> to true.
        /// </summary>
        protected override void MinimizeWebPart(WebParts.WebPart webPart)
        {
            base.MinimizeWebPart(webPart);
            UpdateAndReAddWebParts();
        }

        /// <summary>
        /// Excecutes <see cref="System.Web.UI.WebControls.WebParts.WebPartZoneBase.RestoreWebPart"/>, 
        /// then sets <see cref="UpdateAfterCallBack"/> to true.
        /// </summary>
        protected override void RestoreWebPart(WebParts.WebPart webPart)
        {
            base.RestoreWebPart(webPart);
            UpdateAndReAddWebParts();
        }

        /// <summary>
        /// Excecutes <see cref="System.Web.UI.WebControls.WebParts.WebPartZoneBase.CloseWebPart"/>, 
        /// then sets <see cref="UpdateAfterCallBack"/> to true.
        /// </summary>
        protected override void CloseWebPart(System.Web.UI.WebControls.WebParts.WebPart webPart)
        {
            base.CloseWebPart(webPart);
            UpdateAndReAddWebParts();
        }

        private const string parentTagName = "span";
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

        #region Private Methods

        private void UpdateAndReAddWebParts()
        {
            UpdateAfterCallBack = true;
            Anthem.Manager.AddScriptForClientSideEval(string.Format("ReAddWebParts('{0}')", ID));
        }

        #endregion
    }
}

#endif
