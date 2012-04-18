using System;
using System.Web;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using Anthem;

namespace AnthemExtensions {
    public class Calendar : Anthem.Calendar {
        private ASP.TableItemStyle mouseOverStyle;

        #region Methods
        protected override void OnDayRender(ASP.TableCell cell, ASP.CalendarDay day) {
            base.OnDayRender(cell, day);
            if (this.mouseOverStyle != null && !day.IsSelected) {
                cell.Attributes.Add("onmouseover", Utils.GetJavaScriptStyleString(this.mouseOverStyle));
                cell.Attributes.Add("onmouseout", Utils.GetJavaScriptStyleString(cell.ControlStyle));
            }
        }

        #endregion

        #region Properties
#if V2
        [Themeable(true)]
#endif
        [Bindable(false)]
        [Category("Styles")]
        [Description("Mouse over styling on each date")]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ASP.TableItemStyle MouseOverStyle {
            get {
                if (this.mouseOverStyle == null)
                    this.mouseOverStyle = new ASP.TableItemStyle();
                return this.mouseOverStyle;
            }
        }

        #endregion
    }
}
