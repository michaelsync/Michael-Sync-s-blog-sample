using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Diagnostics;
using Anthem;

namespace AnthemExtensions {
    // <httpModules><add name="AnthemDebuggerModule" type="AnthemExtensions.DebuggerModule,Anthem" /></httpModules>
    public class DebuggerModule : IHttpModule {

        #region Event Handlers
        private void DebuggerModule_PreRequestHandlerExecute(object sender, EventArgs e) {
            HttpApplication app = (HttpApplication)sender;
            Page page = app.Context.Handler as Page;
            if (page != null) {
                page.Error += new EventHandler(this.Page_Error);
            }
        }


        private void Page_Error(object sender, EventArgs e) {
            Page page = (Page)sender;
            Exception ex = page.Server.GetLastError();
            if (ex != null && Manager.IsCallBack)
                Manager.AddScriptForClientSideEval("alert('" + ex.ToString() + "')");
        }

        private void DebuggerModule_Error(object sender, EventArgs e) {
            HttpApplication app = (HttpApplication)sender;
            Exception ex = app.Server.GetLastError();
            if (ex != null) {
                if (!this.IsCallBack)
                    HttpContext.Current.Response.Write("<script>alert('" + ex.ToString() + "');</script>");
                else {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"value\":");
                    Manager.WriteValue(sb, null);
                    sb.Append(",\"error\":");
                    Manager.WriteValue(sb, ex.ToString());
                    sb.Append("}");
                    HttpContext.Current.Response.Write(sb.ToString());
                }
            }
            app.Server.ClearError();
        }

        #endregion

        #region Methods

        private bool IsCallBack {
            get {
                string callback = HttpContext.Current.Request.QueryString["Anthem_CallBack"];
                return string.Compare(callback, "true", true) == 0;
            }
        }

        #endregion

        #region IHttpModule Members
        public void Dispose() {
        }

        public void Init(HttpApplication application) {
            application.PreRequestHandlerExecute += new EventHandler(this.DebuggerModule_PreRequestHandlerExecute);
            application.Error += new EventHandler(this.DebuggerModule_Error);
        }

        #endregion
    }
}