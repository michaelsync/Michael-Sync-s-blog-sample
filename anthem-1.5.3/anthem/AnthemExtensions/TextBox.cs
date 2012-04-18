using System;
using System.Web;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using Anthem;

namespace AnthemExtensions {
#if V2
    // not ready for 1.1 yet
    public class TextBox : Anthem.TextBox {
        private ASP.Style focusStyle;

        #region Overrides
        protected override void AddAttributesToRender(HtmlTextWriter writer) {
            base.AddAttributesToRender(writer);
            if (Utils.IsStyleSet(this.focusStyle)) {
                writer.AddAttribute("onfocus", Utils.GetJavaScriptStyleString(this.FocusStyle));
                if (Utils.IsStyleSet(this.ControlStyle))
                    this.ControlStyle.BackColor = Color.White;
                writer.AddAttribute("onblur", Utils.GetJavaScriptStyleString(this.ControlStyle));
            }
        }

        #endregion

        #region Properties
#if V2
        [Themeable(true)]
#endif
        [Browsable(true)]        
        [Category("Styles")]
        [Description("Set the CSS (ie. watermarks :P) of the control when it gains focus")]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ASP.Style FocusStyle {
            get {
                if (this.focusStyle == null)
                    this.focusStyle = new ASP.Style();
                return this.focusStyle;
            }
        }

        #endregion
    }
#endif
}
