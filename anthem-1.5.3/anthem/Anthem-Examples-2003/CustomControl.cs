using System;
using System.Web.UI;

namespace Anthem.Examples
{
    public class CustomControl : Control
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Anthem.Manager.Register(this);

#if V2
            Page.ClientScript.RegisterClientScriptBlock(
                typeof(CustomControl),
                "script",
#else
            Page.RegisterClientScriptBlock(
                typeof(CustomControl).FullName,
#endif
                @"<script type='text/javascript'>
function GetNow(control) {
    Anthem_InvokeControlMethod(
        control.id,
        'GetNow',
        [],
        function (result) {
            control.innerHTML = result.value;
        }
    );
}
</script>");
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.AddAttribute("id", ClientID);
            writer.AddAttribute("onclick", "GetNow(this)");
            writer.RenderBeginTag("span");
            writer.Write(DateTime.Now);
            writer.RenderEndTag();
        }

        [Anthem.Method]
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}
