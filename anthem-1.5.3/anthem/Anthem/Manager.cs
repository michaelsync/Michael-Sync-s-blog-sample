using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ASP = System.Web.UI.WebControls;

#if V2
using System.Web.Configuration;
[assembly: WebResource("Anthem.Anthem.js", "text/javascript")]
#endif

namespace Anthem
{
    /// <summary>
    /// The Manager class is responsible for managing all of the interaction between ASP.NET
    /// and the Anthem controls.
    /// </summary>
    public class Manager
    {
        #region Public Static Properties

        /// <summary>
        /// A string which uniquely identifies the current callback.
        /// </summary>
        public static string CallBackID
        {
            get
            {
                return HttpContext.Current.Request.Form["Anthem_CallBackID"];
            }
        }

        /// <summary>
        /// The method to invoke during the callback.
        /// </summary>
        public static string CallBackMethod
        {
            get
            {
                return HttpContext.Current.Request.Form["Anthem_CallBackMethod"];
            }
        }

        /// <summary>
        /// A string value which indicates whether the callback is being made
        /// using XMLHttpRequest or a hidden IFRAME.
        /// </summary>
        public static string CallBackType
        {
            get
            {
                return HttpContext.Current.Request.Form["Anthem_CallBackType"];
            }
        }

        private static bool _includePageScripts = false;
        /// <summary>
        /// When true, Anthem.Manager will include all page level scripts in the 
        /// callback response. Use this if you add or show 3rd party controls during 
        /// the callback that add or change client scripts in the page.
        /// </summary>
        public static bool IncludePageScripts
        {
            get
            {
                if (!_includePageScripts)
                {
                    string appSetting = string.Empty;
#if V2
                    appSetting = WebConfigurationManager.AppSettings["Anthem.IncludePageScripts"];
#else
                    appSetting = ConfigurationSettings.AppSettings["Anthem.IncludePageScripts"];
#endif
                    if (null != appSetting && string.Empty != appSetting)
                        _includePageScripts = bool.Parse(appSetting);
                }
                return _includePageScripts;
            }
            set { _includePageScripts = value; }
        }

        /// <summary>
        /// Returns <strong>true</strong> if the current POST is a callback.
        /// </summary>
        public static bool IsCallBack
        {
            get
            {
                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    string callback = context.Request.Params["Anthem_CallBack"];
                    if (callback != null)
                    {
                        // If Anthem_CallBack appears multiple times, Params will return
                        // all values joined with a comma. For example "true,true". We
                        // are only interested in the first value.
                        if (callback.IndexOf(",") != -1) callback = callback.Split(',')[0];
                        return string.Compare(callback, "true", true) == 0;
                    }
                }
                return false;
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>Adds the script to the control's attribute collection.</summary>
        /// <remarks>
        /// If the attribute already exists, the script is prepended to the existing
        /// value.
        /// </remarks>
        /// <param name="control">The control to modify.</param>
        /// <param name="attributeName">The attribute to modify.</param>
        /// <param name="script">The script to add to the attribute.</param>
        public static void AddScriptAttribute(ASP.WebControl control, string attributeName, string script)
        {
            bool enableCallBack = !(control is ICallBackControl)
                || ((ICallBackControl)control).EnableCallBack;

            if (enableCallBack)
            {
                string newValue = script;
                string oldValue = control.Attributes[attributeName];
                if (oldValue != null && !oldValue.Equals(newValue))
                {
                    newValue = GetStringEndingWithSemicolon(oldValue) + script;
                }
                control.Attributes[attributeName] = newValue;
            }
        }

        /// <summary>Adds the script to the item's attribute collection.</summary>
        /// <param name="control">The control to modify.</param>
        /// <param name="item">The <see cref="System.Web.UI.WebControls.ListItem"/> to modify.</param>
        /// <param name="attributeName">The attribute to modify.</param>
        /// <param name="script">The script to add.</param>
        public static void AddScriptAttribute(ASP.WebControl control, ASP.ListItem item, string attributeName, string script)
        {
            bool enableCallBack = !(control is ICallBackControl)
                || ((ICallBackControl)control).EnableCallBack;

            if (enableCallBack)
            {
                string newValue = script;
                string oldValue = item.Attributes[attributeName];
                if (oldValue != null && !oldValue.Equals(newValue))
                {
                    newValue = GetStringEndingWithSemicolon(oldValue) + script;
                }
                item.Attributes[attributeName] = newValue;
            }
        }

        /// <summary>
        /// Add the script to a list of scripts to be evaluated on the client during the
        /// callback response processing.
        /// </summary>
        /// <remarks>To not include &lt;script&gt;&lt;/script&gt; tags.</remarks>
        /// <example>
        /// 	<code lang="CS" title="[New Example]" description="This example adds an alert message that will be displayed on the client during callback response processing.">
        /// Anthem.Manager.AddScriptForClientSideEval("alert('Hello');");
        ///     </code>
        /// </example>
        /// <param name="script">The script to evaluate.</param>
        public static void AddScriptForClientSideEval(string script)
        {
            GetManager()._clientSideEvalScripts.Add(script);
        }

        /// <summary>
        /// Obtains a reference to a clinet-side javascript function that causes, when
        /// invoked, the client to callback to the server.
        /// </summary>
        public static string GetCallbackEventReference(ICallBackControl control, bool causesValidation, string validationGroup)
        {
            return GetCallbackEventReference(control, string.Empty, causesValidation, validationGroup, string.Empty);
        }

        /// <summary>
        /// Obtains a reference to a clinet-side javascript function that causes, when
        /// invoked, the client to callback to the server.
        /// </summary>
        public static string GetCallbackEventReference(ICallBackControl control, string argument, bool causesValidation, string validationGroup)
        {
            return GetCallbackEventReference(control, argument, causesValidation, validationGroup, string.Empty);
        }

        /// <summary>
        /// Obtains a reference to a clinet-side javascript function that causes, when
        /// invoked, the client to callback to the server.
        /// </summary>
        public static string GetCallbackEventReference(ICallBackControl control, bool causesValidation, string validationGroup, string imageDuringCallback)
        {
            return GetCallbackEventReference(control, string.Empty, causesValidation, validationGroup, imageDuringCallback);
        }

        /// <summary>
        /// Obtains a reference to a clinet-side javascript function that causes, when
        /// invoked, the client to callback to the server.
        /// </summary>
        public static string GetCallbackEventReference(ICallBackControl control, string argument, bool causesValidation, string validationGroup, string imageDuringCallback)
        {
            return string.Format(
                "javascript:Anthem_FireCallBackEvent(this,event,'{0}','{1}',{2},'{3}','{4}','{5}',{6},{7},{8},{9},true,true);",
                ((Control)control).UniqueID,
                argument,
                causesValidation ? "true" : "false",
                validationGroup,
                imageDuringCallback,
                control.TextDuringCallBack,
                control.EnabledDuringCallBack ? "true" : "false",
                (control.PreCallBackFunction == null || control.PreCallBackFunction.Length == 0) ? "null" : control.PreCallBackFunction,
                (control.PostCallBackFunction == null || control.PostCallBackFunction.Length == 0) ? "null" : control.PostCallBackFunction,
                (control.CallBackCancelledFunction == null || control.CallBackCancelledFunction.Length == 0) ? "null" : control.CallBackCancelledFunction
            );
        }

        /// <summary>Returns a value indicating if the control is visible on the client.</summary>
        public static bool GetControlVisible(Control control, StateBag viewstate, bool designMode)
        {
            if (null == viewstate["AnthemVisible"] || (bool)viewstate["AnthemVisible"])
            {
                if (null != control.Parent && !designMode)
                    return control.Parent.Visible;
                return true;
            }
            return false;
        }

        /// <summary>Returns a refernce to <see cref="Anthem.Manager"/>.</summary>
        public static Manager GetManager()
        {
            Manager manager = HttpContext.Current.Items[GetAnthemManagerKey()] as Manager;
            if (manager == null)
                throw new ApplicationException("This page was never registered with Anthem.Manager!");
            return manager;
        }

        /// <summary>Returns the input string ending with a semicolon (;).</summary>
        public static string GetStringEndingWithSemicolon(string value)
        {
            if (value != null)
            {
                int length = value.Length;
                if (length > 0 && value[length - 1] != ';')
                {
                    return value + ";";
                }
            }
            return value;
        }

        /// <summary>Registers the page with <see cref="Anthem.Manager"/>.</summary>
        public static void Register(Page page)
        {
            Register(page, page);
        }

        /// <summary>Registers the control with <see cref="Anthem.Manager"/>.</summary>
        public static void Register(Control control)
        {
            Register(control.Page, control);
        }

#if V2
        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// type, key, and script literal.
        /// </summary>
        /// <param name="type">The type of the client script to register.</param>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key and its type. 
        /// Scripts with the same key and type are considered duplicates. 
        /// Only one script with a given type and key pair can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the Page.IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key and type pair is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>In this overload of the RegisterClientScriptBlock method, 
        /// you must make sure that the script provided in the script parameter 
        /// is wrapped in a &lt;script&gt; element block.</para>
        /// <para>The RegisterClientScriptBlock method adds a script block to the 
        /// top of the rendered page when it is first rendered. All script blocks are
        /// rendered in the &lt;head&gt; of the page during callback processing after
        /// the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterClientScriptBlock(Type type, String key, String script)
        {
            RegisterClientScriptBlock(type, key, script, false);
        }

        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// type, key, and script literal.
        /// </summary>
        /// <param name="type">The type of the client script to register.</param>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <param name="addScriptTags">A Boolean value indicating whether to add
        /// script tags.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key and its type. 
        /// Scripts with the same key and type are considered duplicates. 
        /// Only one script with a given type and key pair can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key and type pair is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>In this overload of the RegisterClientScriptBlock method, you can indicate 
        /// whether the script provided in the script parameter is wrapped with a &lt;script&gt;
        /// element block by using the addScriptTags parameter. Setting addScriptTags to 
        /// true indicates that script tags will be added automatically.</para>
        /// <para>The RegisterClientScriptBlock method adds a script block to the 
        /// top of the rendered page when it is first rendered. All script blocks are
        /// rendered in the &lt;head&gt; of the page during callback processing after
        /// the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterClientScriptBlock(Type type, String key, String script, Boolean addScriptTags)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.ClientScript.RegisterClientScriptBlock(type, key, script, addScriptTags);
            }
            RegisterPageScriptBlock(key, script);
        }
#else
        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// key and script literal.
        /// </summary>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key. 
        /// Scripts with the same key are considered duplicates. 
        /// Only one script with a given key can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the Page.IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>You must make sure that the script provided in the script parameter 
        /// is wrapped in a &lt;script&gt; element block.</para>
        /// <para>The RegisterClientScriptBlock method adds a script block to the 
        /// top of the rendered page when it is first rendered. All script blocks are
        /// rendered in the &lt;head&gt; of the page during callback processing after
        /// the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterClientScriptBlock(String key, String script)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.RegisterClientScriptBlock(key, script);
            }
            RegisterPageScriptBlock(key, script);
        }
#endif

        /// <summary>
        /// Registers the client script include with the Page object and Anthem.Manager 
        /// using a key, and a URL.
        /// </summary>
        /// <param name="key">The key of the client script include to register.</param>
        /// <param name="url">The URL of the client script include to register.</param>
        /// <remarks>
        /// <para>This overload of the RegisterClientScriptInclude method takes key and 
        /// url parameters to identify the script include.</para>
        /// <para>To resolve the client URL, use the ResolveClientUrl method. This method 
        /// uses the context of the URL on which it is called to resolve the path.</para>
        /// <para>This method adds a script block at the top of the rendered page during
        /// the initial load and in the &lt;head&gt; element block after each callback.
        /// </para>
        /// </remarks>
        public static void RegisterClientScriptInclude(String key, String url)
        {
#if V2
            RegisterClientScriptInclude(typeof(Page), key, url);
#else
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", url);
                page.RegisterClientScriptBlock(key, script);
            }
            RegisterPageScriptBlock(key, "src=" + url);

#endif
        }

#if V2
        /// <summary>
        /// Registers the client script include with the Page object and Anthem.Manager 
        /// using a type, a key, and a URL.
        /// </summary>
        /// <param name="type">The type of the client script include to register.</param>
        /// <param name="key">The key of the client script include to register.</param>
        /// <param name="url">The URL of the client script include to register.</param>
        /// <remarks>
        /// <para>This overload of the RegisterClientScriptInclude method takes key and 
        /// url parameters to identify the script, as well as a type parameter to specify 
        /// the identification of the client script include. You specify the type based 
        /// on the object that will be accessing the resource. For instance, when using a 
        /// Page instance to access the resource, you specify the Page type.</para>
        /// <para>To resolve the client URL, use the ResolveClientUrl method. This method 
        /// uses the context of the URL on which it is called to resolve the path.</para>
        /// <para>This method adds a script block at the top of the rendered page during
        /// the initial load and in the &lt;head&gt; element block after each callback.
        /// </para>
        /// </remarks>
        public static void RegisterClientScriptInclude(Type type, String key, String url)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.ClientScript.RegisterClientScriptInclude(type, key, url);
            }
            RegisterPageScriptBlock(key, "src=" + url);
        }
#endif
#if V2
        /// <summary>
        /// Registers the client script resource with the Page object and with Anthem.Manager
        /// using a type and a resource name.
        /// </summary>
        /// <param name="type">The type of the client script resource to register.</param>
        /// <param name="resourceName">The name of the client script resource to register.</param>
        /// <remarks>
        /// <para>The RegisterClientScriptResource method is used when accessing compiled-in 
        /// resources from assemblies through the WebResource.axd HTTP handler. The 
        /// RegisterClientScriptResource method registers the script with the Page object 
        /// and prevents duplicate scripts. This method wraps the contents of the resource 
        /// URL with a &lt;script&gt; element block.</para>
        /// </remarks>
        public static void RegisterClientScriptResource(Type type, String resourceName)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.ClientScript.RegisterClientScriptResource(type, resourceName);
            }
            RegisterPageScriptBlock(resourceName, "src=" + page.ClientScript.GetWebResourceUrl(type, resourceName));
        }
#endif

        /// <summary>
        /// Registers the client script with Anthem.Manager using a key and a script literal.
        /// </summary>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key. 
        /// Scripts with the same key are considered duplicates. 
        /// Only one script with a given key can be registered with Anthem.Manager. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>The script block added by the RegisterPageScriptBlock can be wrapped
        /// with a &lt;script&gt; element block, though this is not required.</para>
        /// <para>The script block added by RegisterPageScriptBlock will only be rendered
        /// by Anthem.Manager during callback processing. If you want to render the script
        /// during the initial load use RegisterClientScriptBlock or RegisterStartupScript.</para>
        /// <para>The script block added by RegisterPageScriptBlock will be rendered in
        /// the &lt;head&gt; element block after the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterPageScriptBlock(string key, string script)
        {
            GetManager()._pageScripts[key] = script;
        }

#if V2
        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// type, key, and script literal.
        /// </summary>
        /// <param name="type">The type of the client script to register.</param>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key and its type. 
        /// Scripts with the same key and type are considered duplicates. 
        /// Only one script with a given type and key pair can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the Page.IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key and type pair is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>In this overload of the RegisterStartupScript method, 
        /// you must make sure that the script provided in the script parameter 
        /// is wrapped in a &lt;script&gt; element block.</para>
        /// <para>The script block added by the RegisterStartupScript method executes 
        /// when the page finishes loading but before the page's OnLoad event is raised.
        /// All script blocks are rendered in the &lt;head&gt; of the page during callback 
        /// processing after the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterStartupScript(Type type, String key, String script)
        {
            RegisterStartupScript(type, key, script, false);
        }

        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// type, key, and script literal.
        /// </summary>
        /// <param name="type">The type of the client script to register.</param>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <param name="addScriptTags">A Boolean value indicating whether to add
        /// script tags.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key and its type. 
        /// Scripts with the same key and type are considered duplicates. 
        /// Only one script with a given type and key pair can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the Page.IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key and type pair is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>In this overload of the RegisterStartupScript method, you can indicate 
        /// whether the script provided in the script parameter is wrapped with a &lt;script&gt;
        /// element block by using the addScriptTags parameter. Setting addScriptTags to 
        /// true indicates that script tags will be added automatically.</para>
        /// <para>The script block added by the RegisterStartupScript method executes 
        /// when the page finishes loading but before the page's OnLoad event is raised.
        /// All script blocks are rendered in the &lt;head&gt; of the page during callback 
        /// processing after the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterStartupScript(Type type, String key, String script, Boolean addScriptTags)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.ClientScript.RegisterStartupScript(type, key, script, addScriptTags);
            }
            RegisterPageScriptBlock(key, script);
        }
#else
        /// <summary>
        /// Registers the client script with the Page object and with Anthem.Manager using a 
        /// key, and script literal.
        /// </summary>
        /// <param name="key">The key of the client script to register.</param>
        /// <param name="script">The client script literal to register.</param>
        /// <remarks>
        /// <para>A client script is uniquely identified by its key. 
        /// Scripts with the same key are considered duplicates. 
        /// Only one script with a given key can be registered with the page. 
        /// Attempting to register a script that is already registered does not create a 
        /// duplicate of the script.</para>
        /// <para>Call the Page.IsClientScriptBlockRegistered method to 
        /// determine whether a client script with a given key is already 
        /// registered and avoid unnecessarily attempting to add the script.</para>
        /// <para>You must make sure that the script provided in the script parameter 
        /// is wrapped in a &lt;script&gt; element block.</para>
        /// <para>The script block added by the RegisterStartupScript method executes 
        /// when the page finishes loading but before the page's OnLoad event is raised.
        /// All script blocks are rendered in the &lt;head&gt; of the page during callback 
        /// processing after the page updates have been applied.</para>
        /// </remarks>
        public static void RegisterStartupScript(String key, String script)
        {
            Page page = HttpContext.Current.Handler as Page;
            if (page != null)
            {
                page.RegisterStartupScript(key, script);
            }
            RegisterPageScriptBlock(key, script);
        }
#endif

        /// <summary>Sets the visibility of the control on the client.</summary>
        public static void SetControlVisible(StateBag viewState, bool value)
        {
            viewState["AnthemVisible"] = value;
        }

        /// <summary>
        /// This method needs to be called by custom controls that want their
        /// innerHTML to be automatically updated on the client pages during
        /// call backs. Call this at the top of the Render override. The
        /// parentTagName argument should be "div" or "span" depending on the
        /// type of control. It's this parent element that actually gets its
        /// innerHTML updated after call backs.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parentTagName"></param>
        /// <param name="control"></param>
        public static void WriteBeginControlMarker(HtmlTextWriter writer, string parentTagName, Control control)
        {
            writer.Write("<{0} id=\"{1}\">", parentTagName, "Anthem_" + control.ClientID + "__");
            IUpdatableControl updatableControl = control as IUpdatableControl;
            if (updatableControl != null && updatableControl.UpdateAfterCallBack && IsCallBack)
            {
                writer.Write(_beginControlMarker);
                writer.Write(GetUniqueIDWithDollars(control));
                writer.Write("-->");
            }
        }

        /// <summary>
        /// This method needs to be called by custom controls that want their
        /// innerHTML to be automatically updated on the client pages during
        /// call backs. Call this at the bottom of the Render override.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="parentTagName"></param>
        /// <param name="control"></param>
        public static void WriteEndControlMarker(HtmlTextWriter writer, string parentTagName, Control control)
        {
            IUpdatableControl updatableControl = control as IUpdatableControl;
            if (updatableControl != null && updatableControl.UpdateAfterCallBack && IsCallBack)
            {
                writer.Write(_endControlMarker);
                writer.Write(GetUniqueIDWithDollars(control));
                writer.Write("-->");
            }
            writer.Write("</{0}>", parentTagName);
        }

        /// <summary>Writes the val and error to the callback response.</summary>
        public static void WriteResult(HttpResponse resp, object val, string error)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                WriteValueAndError(sb, val, error, null, null, null, null, null);
            }
            catch (Exception ex)
            {
                // If an exception was thrown while formatting the
                // result value, we need to discard whatever was
                // written and start over with nothing but the error
                // message.
                sb.Length = 0;
                WriteValueAndError(sb, null, ex.Message, null, null, null, null, null);
            }
            resp.Write(sb.ToString());
        }

        /// <summary>
        /// Writes the val to sb in a format that can be interpreted by Anthem.js on the
        /// client.
        /// </summary>
        public static void WriteValue(StringBuilder sb, object val)
        {
            if (val == null || val == System.DBNull.Value)
            {
                sb.Append("null");
            }
            else if (val is string || val is Guid)
            {
                WriteString(sb, val.ToString());
            }
            else if (val is bool)
            {
                sb.Append(val.ToString().ToLower());
            }
            else if (val is double ||
                val is float ||
                val is long ||
                val is int ||
                val is short ||
                val is byte ||
                val is decimal)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture.NumberFormat, "{0}", val);
            }
            else if (val.GetType().IsEnum)
            {
                sb.Append((int)val);
            }
            else if (val is DateTime)
            {
                sb.Append("new Date(\"");
                sb.Append(((DateTime)val).ToString("MMMM, d yyyy HH:mm:ss", new CultureInfo("en-US", false).DateTimeFormat));
                sb.Append("\")");
            }
            else if (val is DataSet)
            {
                WriteDataSet(sb, val as DataSet);
            }
            else if (val is DataTable)
            {
                WriteDataTable(sb, val as DataTable);
            }
            else if (val is DataRow)
            {
                WriteDataRow(sb, val as DataRow);
            }
            else if (val is Hashtable)
            {
                WriteHashtable(sb, val as Hashtable);
            }
            else if (val is IEnumerable)
            {
                WriteEnumerable(sb, val as IEnumerable);
            }
            else
            {
                WriteObject(sb, val);
            }
        }

        #endregion

        #region Private Static Methods

        private static void AddManager(Page page)
        {
            Manager manager = HttpContext.Current.Items[GetAnthemManagerKey()] as Manager;
            if (manager == null)
            {
                manager = new Manager();
                page.PreRender += new EventHandler(manager.OnPreRender);
                page.Error += new EventHandler(manager.OnError);
                page.Unload += new EventHandler(manager.OnUnload);
                manager._targets[GetAnthemManagerKey()] = manager;
                HttpContext.Current.Items[GetAnthemManagerKey()] = manager;
            }
            manager.RegisterPageScript(page);
        }

        private static object[] ConvertParameters(MethodInfo methodInfo, HttpRequest req)
        {
            object[] parameters = new object[methodInfo.GetParameters().Length];
            int i = 0;
            foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
            {
                object param = null;
                string paramValue = req.Form["Anthem_CallBackArgument" + i];
                if (paramValue != null)
                {
                    if (paramInfo.ParameterType.IsArray)
                    {
                        Type type = paramInfo.ParameterType.GetElementType();
                        string[] values = req.Form.GetValues("Anthem_CallBackArgument" + i);
                        Array array = Array.CreateInstance(type, values.Length);
                        for (int index = 0; index < values.Length; index++)
                        {
                            array.SetValue(Convert.ChangeType(values[index], type), index);
                        }
                        param = array;
                    }
                    else
                    {
                        param = Convert.ChangeType(paramValue, paramInfo.ParameterType);
                    }
                }
                parameters[i] = param;
                ++i;
            }
            return parameters;
        }

        private static HtmlForm FindForm(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                HtmlForm form = child as HtmlForm;
                if (form != null && form.Visible)
                {
                    return form;
                }
                if (child.HasControls())
                {
                    HtmlForm htmlForm = FindForm(child);
                    if (htmlForm != null)
                    {
                        return htmlForm;
                    }
                }
            }
            return null;
        }

        private static MethodInfo FindTargetMethod(object target, string methodName)
        {
            Type type = target.GetType();
            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (methodInfo != null)
            {
                object[] methodAttributes = methodInfo.GetCustomAttributes(typeof(Anthem.MethodAttribute), true);
                if (methodAttributes.Length > 0)
                {
                    return methodInfo;
                }
            }
            return null;
        }

        private static string GetAnthemManagerKey()
        {
            return "Anthem/Manager" + HttpContext.Current.Request.Path;
        }

        private static string GetFormID(Control control)
        {
            HtmlForm form = FindForm(control.Page);
            if (form != null)
            {
                return form.ClientID;
            }
            return null;
        }

        private static string GetPageURL(HttpContext context)
        {
            string url = null;
            string currentExecutionFilePath = context.Request.CurrentExecutionFilePath;
            string filePath = context.Request.FilePath;
            if (string.Compare(currentExecutionFilePath, filePath, true) == 0)
            {
                url = currentExecutionFilePath;
            }
            else
            {
                Uri from = new Uri("file://foo" + filePath);
                Uri to = new Uri("file://foo" + currentExecutionFilePath);
#if V2
                url = from.MakeRelativeUri(to).ToString();
#else
                url = from.MakeRelative(to);
#endif
            }
            return url;
        }

        internal static string GetUniqueIDWithDollars(Control control)
        {
            string uniqueIdWithDollars = control.UniqueID;
            if (uniqueIdWithDollars == null)
            {
                return null;
            }
            if (uniqueIdWithDollars.IndexOf(':') >= 0)
            {
                return uniqueIdWithDollars.Replace(':', '$');
            }
            return uniqueIdWithDollars;
        }
 
        private static object InvokeMethod(object target, MethodInfo methodInfo, object[] parameters)
        {
            object val = null;
            try
            {
                val = methodInfo.Invoke(target, parameters);
            }
            catch (TargetInvocationException ex)
            {
                // TargetInvocationExceptions should have the actual
                // exception the method threw in its InnerException
                // property.
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
            return val;
        }

        private static void Register(Page page, Control control)
        {
            AddManager(page);
            Manager manager = GetManager();
            if (!object.ReferenceEquals(page, control))
                manager.AddTarget(control);
        }

        private static void WriteDataRow(StringBuilder sb, DataRow row)
        {
            sb.Append("{");
            foreach (DataColumn column in row.Table.Columns)
            {
                sb.AppendFormat("\"{0}\":", column.ColumnName);
                WriteValue(sb, row[column]);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (row.Table.Columns.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteDataSet(StringBuilder sb, DataSet ds)
        {
            sb.Append("{\"Tables\":{");
            foreach (DataTable table in ds.Tables)
            {
                sb.AppendFormat("\"{0}\":", table.TableName);
                WriteDataTable(sb, table);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (ds.Tables.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("}}");
        }

        private static void WriteDataTable(StringBuilder sb, DataTable table)
        {
            sb.Append("{\"Rows\":[");
            foreach (DataRow row in table.Rows)
            {
                WriteDataRow(sb, row);
                sb.Append(",");
            }
            // Remove the trailing comma.
            if (table.Rows.Count > 0)
            {
                --sb.Length;
            }
            sb.Append("]}");
        }

        private static void WriteEnumerable(StringBuilder sb, IEnumerable e)
        {
            bool hasItems = false;
            sb.Append("[");
            foreach (object val in e)
            {
                WriteValue(sb, val);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("]");
        }

        private static void WriteHashtable(StringBuilder sb, Hashtable e)
        {
            bool hasItems = false;
            sb.Append("{");
            foreach (string key in e.Keys)
            {
                sb.AppendFormat("\"{0}\":", key.ToLower());
                WriteValue(sb, e[key]);
                sb.Append(",");
                hasItems = true;
            }
            // Remove the trailing comma.
            if (hasItems)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteObject(StringBuilder sb, object o)
        {
            MemberInfo[] members = o.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public);
            sb.Append("{");
            bool hasMembers = false;
            foreach (MemberInfo member in members)
            {
                bool hasValue = false;
                object val = null;
                if ((member.MemberType & MemberTypes.Field) == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)member;
                    val = field.GetValue(o);
                    hasValue = true;
                }
                else if ((member.MemberType & MemberTypes.Property) == MemberTypes.Property)
                {
                    PropertyInfo property = (PropertyInfo)member;
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        val = property.GetValue(o, null);
                        hasValue = true;
                    }
                }
                if (hasValue)
                {
                    sb.Append("\"");
                    sb.Append(member.Name);
                    sb.Append("\":");
                    WriteValue(sb, val);
                    sb.Append(",");
                    hasMembers = true;
                }
            }
            if (hasMembers)
            {
                --sb.Length;
            }
            sb.Append("}");
        }

        private static void WriteString(StringBuilder sb, string s)
        {
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");
        }

        private static void WriteValueAndError(
            StringBuilder sb, 
            object val, 
            string error, 
            string viewState, 
            string viewStateEncrypted,
            string eventValidation,
            Hashtable controls,
            string[] scripts)
        {
            sb.Append("{\"value\":");
            WriteValue(sb, val);
            sb.Append(",\"error\":");
            WriteValue(sb, error);
            if (viewState != null)
            {
                sb.Append(",\"viewState\":");
                WriteValue(sb, viewState);
            }
            if (viewStateEncrypted != null)
            {
                sb.Append(",\"viewStateEncrypted\":");
                WriteValue(sb, viewStateEncrypted);
            }
            if (eventValidation != null)
            {
                sb.Append(",\"eventValidation\":");
                WriteValue(sb, eventValidation);
            }
            if (controls != null && controls.Count > 0)
            {
                sb.Append(",\"controls\":{");
                foreach (DictionaryEntry control in controls)
                {
                    sb.Append("\"" + control.Key + "\":");
                    WriteValue(sb, control.Value);
                    sb.Append(",");
                }
                --sb.Length;
                sb.Append("}");
            }
            if (scripts != null && scripts.Length > 0)
            {
                sb.Append(",\"pagescript\":[");
                foreach (string script in scripts)
                {
                    WriteValue(sb, script);
                    sb.Append(",");
                }
                --sb.Length;
                sb.Append("]");
            }
            if (GetManager()._clientSideEvalScripts.Count > 0)
            {
                sb.Append(",\"script\":[");
                foreach (string script in GetManager()._clientSideEvalScripts)
                {
                    WriteValue(sb, script);
                    sb.Append(",");
                }
                --sb.Length;
                sb.Append("]");
            }
            sb.Append("}");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This is an empty method used as the target for the
        /// Anthem_FireEvent function. That function sets the
        /// __EVENTTARGET to the desired ID which causes the
        /// appropriate event to fire on the server so nothing
        /// needs to be done here.
        /// </summary>
        [Anthem.Method]
        public void FireEvent() {}

        #region Support for container controls

        /// <summary>
        /// Add generic callbacks events to all the child controls in the
        /// container. This is used by template controls (eg. DataGrid).
        /// </summary>
        /// <param name="control">The container control.</param>
        /// <param name="enabledDuringCallBack">
        /// 	<strong>true</strong> if the control should be enabled on the client during a
        /// callback.
        /// </param>
        /// <param name="textDuringCallBack">The text to display during a callback.</param>
        /// <param name="preCallBackFunction">The javascript function to execute before starting the callback.</param>
        /// <param name="postCallBackFunction">
        /// The javascript function to execute after the callback response is
        /// received.
        /// </param>
        /// <param name="callBackCancelledFunction">
        /// The javascript function to execute if the callback is cancelled by the
        /// pre-callback function.
        /// </param>
        public static void AddCallBacks(
            Control control, 
            bool enabledDuringCallBack, 
            string textDuringCallBack,
            string preCallBackFunction, 
            string postCallBackFunction, 
            string callBackCancelledFunction)
        {
            foreach (Control child in GetAllChildControls(control))
            {
#if V2
                if (child is ASP.GridView)
                {
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );
                }

                else if (child is ASP.DetailsView)
                {
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );
                }

                else if (child is ASP.FormView)
                {
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );
                }

                else if (child is ASP.IButtonControl && child is ASP.WebControl)
                {
                    if (child.Parent is ASP.DataControlFieldCell && ((ASP.DataControlFieldCell)child.Parent).ContainingField is ASP.CommandField)
                    {
                        AddEventHandler(
                            control,
                            (ASP.WebControl)child,
                            "onclick",
                            ((ASP.IButtonControl)child).CommandName,
                            ((ASP.IButtonControl)child).CommandArgument,
                            ((ASP.IButtonControl)child).CausesValidation,
                            ((ASP.IButtonControl)child).ValidationGroup,
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                    }
                    else
                    {
                        AddEventHandler(
                            child,
                            (ASP.WebControl)child,
                            "onclick",
                            ((ASP.IButtonControl)child).CommandName,
                            ((ASP.IButtonControl)child).CommandArgument,
                            ((ASP.IButtonControl)child).CausesValidation,
                            ((ASP.IButtonControl)child).ValidationGroup,
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                    }
                }
#else
                if (child is ASP.Button)
                    AddEventHandler(
                        child,
                        (ASP.WebControl)child,
                        "onclick",
                        ((ASP.Button)child).CommandName,
                        ((ASP.Button)child).CommandArgument,
                        ((ASP.Button)child).CausesValidation,
                        string.Empty,
                        textDuringCallBack,
                        enabledDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction);

                else if (child is ASP.ImageButton)
                    AddEventHandler(
                        child,
                        (ASP.WebControl)child,
                        "onclick",
                        ((ASP.ImageButton)child).CommandName,
                        ((ASP.ImageButton)child).CommandArgument,
                        ((ASP.ImageButton)child).CausesValidation,
                        string.Empty,
                        textDuringCallBack,
                        enabledDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction);

                else if (child is ASP.LinkButton)
                    AddEventHandler(
                        child,
                        (ASP.WebControl)child,
                        "onclick",
                        ((ASP.LinkButton)child).CommandName,
                        ((ASP.LinkButton)child).CommandArgument,
                        ((ASP.LinkButton)child).CausesValidation,
                        string.Empty,
                        textDuringCallBack,
                        enabledDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction);
#endif
                else if (child is ASP.CheckBox)
                {
                    if (((ASP.CheckBox)child).AutoPostBack)
                    {
                        AddEventHandler(
                            child,
                            (ASP.WebControl)child,
                            "onclick",
                            string.Empty,
                            string.Empty,
#if V2
                            ((ASP.CheckBox)child).CausesValidation,
                            ((ASP.CheckBox)child).ValidationGroup,
#else
                            false,
                            string.Empty,
#endif
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                        ((ASP.CheckBox)child).AutoPostBack = false;
                    }
                }

                else if (child is ASP.CheckBoxList)
                {
                    if (((ASP.CheckBoxList)child).AutoPostBack)
                    {
                        AddScriptAttribute(
                            (ASP.WebControl)child,
                            "onclick",
                            string.Format("AnthemListControl_OnClick(event,{0},'{1}','{2}',{3},{4},{5},{6},true,true)",
#if V2
                                ((ASP.CheckBoxList)child).CausesValidation ? "true" : "false",
                                ((ASP.CheckBoxList)child).ValidationGroup,
#else
                                "false",
                                string.Empty,
#endif
                                textDuringCallBack,
                                enabledDuringCallBack ? "true" : "false",
                                IsNullOrEmpty(preCallBackFunction) ? "null" : preCallBackFunction,
                                IsNullOrEmpty(postCallBackFunction) ? "null" : postCallBackFunction,
                                IsNullOrEmpty(callBackCancelledFunction) ? "null" : callBackCancelledFunction
                            )
                        );
                        ASP.CheckBox controlToRepeat = (ASP.CheckBox)((ASP.CheckBoxList)child).Controls[0];
                        controlToRepeat.AutoPostBack = false;
                    }
                }

                else if (child is ASP.DataGrid)
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );

                else if (child is ASP.DropDownList)
                {
                    if (((ASP.DropDownList)child).AutoPostBack)
                    {
                        AddEventHandler(
                            child,
                            (ASP.WebControl)child,
                            "onchange",
                            string.Empty,
                            string.Empty,
#if V2
                            ((ASP.DropDownList)child).CausesValidation,
                            ((ASP.DropDownList)child).ValidationGroup,
#else
                            false,
                            string.Empty,
#endif
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                        ((ASP.DropDownList)child).AutoPostBack = false;
                    }
                }

                else if (child is ASP.ListBox)
                {
                    if (((ASP.ListBox)child).AutoPostBack)
                    {
                        AddEventHandler(
                            child,
                            (ASP.WebControl)child,
                            "onchange",
                            string.Empty,
                            string.Empty,
#if V2
                            ((ASP.ListBox)child).CausesValidation,
                            ((ASP.ListBox)child).ValidationGroup,
#else
                            false,
                            string.Empty,
#endif
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                        ((ASP.ListBox)child).AutoPostBack = false;
                    }
                }

                else if (child is ASP.Panel)
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );

                else if (child is ASP.RadioButtonList)
                {
                    if (((ASP.RadioButtonList)child).AutoPostBack)
                    {
                        AddScriptAttribute(
                            (ASP.WebControl)child,
                            "onclick",
                            string.Format("AnthemListControl_OnClick(event,{0},'{1}','{2}',{3},{4},{5},{6},true,true)",
#if V2
                                ((ASP.RadioButtonList)child).CausesValidation ? "true" : "false",
                                ((ASP.RadioButtonList)child).ValidationGroup,
#else
                                "false",
                                string.Empty,
#endif
                                textDuringCallBack,
                                enabledDuringCallBack ? "true" : "false",
                                IsNullOrEmpty(preCallBackFunction) ? "null" : preCallBackFunction,
                                IsNullOrEmpty(postCallBackFunction) ? "null" : postCallBackFunction,
                                IsNullOrEmpty(callBackCancelledFunction) ? "null" : callBackCancelledFunction
                            )
                        );
                        ((ASP.RadioButtonList)child).AutoPostBack = false;
                    }
                }

                else if (child is ASP.Repeater)
                    AddCallBacks(
                        child,
                        enabledDuringCallBack,
                        textDuringCallBack,
                        preCallBackFunction,
                        postCallBackFunction,
                        callBackCancelledFunction
                    );

                else if (child is ASP.TextBox)
                {
                    if (((ASP.TextBox)child).AutoPostBack)
                    {
                        AddEventHandler(
                            child,
                            (ASP.WebControl)child,
                            "onchange",
                            string.Empty,
                            string.Empty,
#if V2
                            ((ASP.TextBox)child).CausesValidation,
                            ((ASP.TextBox)child).ValidationGroup,
#else
                            false,
                            string.Empty,
#endif
                            textDuringCallBack,
                            enabledDuringCallBack,
                            preCallBackFunction,
                            postCallBackFunction,
                            callBackCancelledFunction
                        );
                        ((ASP.TextBox)child).AutoPostBack = false;
                    }
                }
            }
        }

        /// <summary>
        /// Returns a flattened list of all the non-Anthem controls
        /// in a container. Container controls such as Panel are not
        /// expanded.
        /// </summary>
        /// <param name="control">The container control.</param>
        /// <returns>An ArrayList of non-Anthem controls.</returns>
        private static ArrayList GetAllChildControls(Control control)
        {
            ArrayList controls = new ArrayList();
            foreach (Control child in control.Controls)
            {
                if (!(child is IUpdatableControl) && !(child is LiteralControl))
                {
                    controls.Add(child);
#if V2
                    if (!(child is ASP.DataGrid
                        || child is ASP.GridView
                        || child is ASP.DetailsView
                        || child is ASP.FormView
                        || child is ASP.Panel
                        || child is ASP.Repeater))
                        controls.AddRange(GetAllChildControls(child));
#else
                    if (!(child is ASP.DataGrid
                        || child is ASP.Panel
                        || child is ASP.Repeater
                        || child is ASP.CheckBoxList
                        || child is ASP.RadioButtonList))
                        controls.AddRange(GetAllChildControls(child));
#endif
                }
            }
            return controls;
        }

        /// <summary>
        /// Add a generic callback to the target control.
        /// </summary>
        /// <remarks>The target control is most often the same as the control that
        /// is raising the event, but the GridView (for example) is the target for all of it's 
        /// generated child controls.</remarks>
        private static void AddEventHandler(
            Control parent, 
            ASP.WebControl control,
            string eventName,
            string commandName, 
            string commandArgument, 
            bool causesValidation, 
            string validationGroup,
            string textDuringCallBack,
            bool enabledDuringCallBack,
            string preCallBackFunction,
            string postCallBackFunction,
            string callBackCancelledFunction)
        {
#if V2
            if (!IsNullOrEmpty(commandName) || !IsNullOrEmpty(commandArgument))
            {
                parent.Page.ClientScript.RegisterForEventValidation(parent.UniqueID,
                    string.Format("{0}${1}", commandName, commandArgument));
            }
#endif
            AddScriptAttribute(
                control,
                eventName,
                string.Format(
                    "javascript:Anthem_FireCallBackEvent(this,event,'{0}','{1}',{2},'{3}','','{4}',{5},{6},{7},{8},true,true);return false;",
                    parent.UniqueID,
                    IsNullOrEmpty(commandName) && IsNullOrEmpty(commandArgument) ? "" : commandName + "$" + commandArgument,
                    causesValidation ? "true" : "false",
#if V2
                    validationGroup,
#else
                    string.Empty,
#endif
                    textDuringCallBack,
                    enabledDuringCallBack ? "true" : "false",
                    IsNullOrEmpty(preCallBackFunction)  ? "null" : preCallBackFunction,
                    IsNullOrEmpty(postCallBackFunction) ? "null" : postCallBackFunction,
                    IsNullOrEmpty(callBackCancelledFunction) ? "null" : callBackCancelledFunction
                )
            );
        }

        private static bool IsNullOrEmpty(string s)
        {
            return ((s == null) || (s.Length == 0));
        }

        #endregion

        #endregion

        #region Private Methods

        private Hashtable _targets;
        private ArrayList _clientSideEvalScripts;
        private NameValueCollection _pageScripts;
        private object _value;
        private string _error;
        private bool _updatePage;
        private bool _updateValidationScripts;

        private Manager()
        {
            _targets = new Hashtable();
            _clientSideEvalScripts = new ArrayList();
            _includePageScripts = false;
            _pageScripts = new NameValueCollection();
        }

        private void AddTarget(Control control)
        {
            _targets[control.ClientID] = control;
        }

        private bool CheckIfRedirectedToLoginPage()
        {
            HttpContext context = HttpContext.Current;
            HttpRequest req = context.Request;
            string returnURL = req.QueryString["ReturnURL"];
            if (returnURL != null && returnURL.Length > 0)
            {
                returnURL = context.Server.UrlDecode(returnURL);
                if (returnURL.EndsWith("?Anthem_CallBack=true") ||
                    returnURL.EndsWith("&Anthem_CallBack=true"))
                {
                    HttpResponse resp = context.Response;
                    WriteResult(resp, null, "LOGIN");
                    resp.End();
                    return true;
                }
            }
            return false;
        }

        private void ConfigureResponse(HttpResponse resp)
        {
            string contentEncoding = null;
            string contentType = null;
#if V2
            contentEncoding = WebConfigurationManager.AppSettings["Anthem.ResponseEncoding"];
            contentType = WebConfigurationManager.AppSettings["Anthem.ResponseType"];
#else
            contentEncoding = ConfigurationSettings.AppSettings["Anthem.ResponseEncoding"];
            contentType = ConfigurationSettings.AppSettings["Anthem.ResponseType"];
#endif
            if (contentEncoding != null)
                resp.ContentEncoding = Encoding.GetEncoding(contentEncoding);
            if (contentType != null)
                resp.ContentType = contentType;
            resp.Cache.SetCacheability(HttpCacheability.NoCache);
        }

        const string _beginControlMarker = "<!--START:";
        const string _endControlMarker = "<!--END:";
        /// <summary>
        /// Returns a hashtable of the HTML for the AJAX-ified controls
        /// on the page. These strings are returned to the client which
        /// updates the page (using innerHTML).
        /// This method is pretty messy but it seems to work (it's hard to
        /// to tell without proper unit tests--shame on me).
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private Hashtable GetControls(string html)
        {
            Hashtable controls = new Hashtable();

            // Find the first begin marker.
            int i = html.IndexOf(_beginControlMarker);
            // Keep looping while we've got markers.
            while (i != -1)
            {
                i += _beginControlMarker.Length;
                // Find the end of the begin marker.
                int j = html.IndexOf("-->", i);
                if (j == -1)
                {
                    break;
                }
                else
                {
                    // The string between i and j should be the ClientID.
                    string id = html.Substring(i, j - i);

                    // Point past the end of the begin marker.
                    i = j + 3;
                    string endMarker = _endControlMarker + id + "-->";
                    // Find the end marker for the current control.
                    j = html.IndexOf(endMarker, i);
                    if (j == -1)
                    {
                        break;
                    }
                    else
                    {
                        // The string between i and j is now the HTML.
                        string control = html.Substring(i, j - i);
                        controls[id] = control;
                        // Point past the end of the end marker.
                        i = j + endMarker.Length;
                    }
                }
                // Find the next begin marker.
                i = html.IndexOf(_beginControlMarker, i);
            }
            return controls;
        }

        private string GetHiddenInputValue(string html, string marker)
        {
            string value = null;
            int i = html.IndexOf(marker);
            if (i != -1)
            {
                value = html.Substring(i + marker.Length);
                value = value.Substring(0, value.IndexOf('\"'));
            }
            return value;
        }

        // Returns a list of all embedded scripts in the page
        private static Regex scriptEmbeddedRegex = new Regex(@"<script.*?>(?<script>.*?)</script>", 
            RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex scriptTagsRegex = new Regex(@"<script(?<attributes>.*?)(>|/>)",
            RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex scriptTypeRegex = new Regex(@"type\s*=\s*['""]text/javascript['""]",
            RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex scriptSrcRegex = new Regex(@"src\s*=\s*['""](?<src>.+?)['""]",
            RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private string[] GetScripts(string html)
        {
            ArrayList scripts = new ArrayList();

            // Add all of the scripts that were manually registered.
            for (int index = 0; index < _pageScripts.Count; index++)
            {
                string script = _pageScripts[index];
                // Strip off any <script> tags
                if (scriptEmbeddedRegex.IsMatch(script))
                {
                    foreach (Match scriptMatch in scriptEmbeddedRegex.Matches(script))
                    {
                        string innerScript = scriptMatch.Groups["script"].ToString();
                        if (innerScript != string.Empty)
                        {
                            scripts.Add(innerScript);
                        }
                    }
                }
                else
                {
                    scripts.Add(script);
                }
            }

            // Load the script libraries in case they are used by the embedded scripts
            if (_includePageScripts)
            {
                foreach (Match attributesMatch in scriptTagsRegex.Matches(html))
                {
                    string attributes = attributesMatch.Groups["attributes"].ToString().Trim();
                    if (scriptTypeRegex.Match(attributes).Success)
                    {
                        foreach (Match srcMatch in scriptSrcRegex.Matches(attributes))
                        {
                            string src = srcMatch.Groups["src"].ToString();
                            if (src != string.Empty)
                                scripts.Add("src=" + src);
                        }
                    }
                }
            }

            // Now load the embedded scripts
            if (this._updateValidationScripts || _includePageScripts)
            {
                // These scripts will reset page validation. Page_Validators is an array
                // of validators to be invoked. WebForm_OnSubmit (ASP.NET 2.0) and 
                // ValidatorOnSubmit (ASP.NET 1.1) are called by the form's onsubmit
                // event handler by postback (non-callback) controls that cause
                // validation. Resetting these will ensure that no javascript errors
                // occur if validators have been removed from the page. If there are 
                // still validators on the page, then the actual array and function will
                // override these values.
#if V2
                scripts.Add(@"
//<![CDATA[
var Page_Validators = new Array();
function WebForm_OnSubmit() {
    return true;
}
//]]>");
#else
                scripts.Add(@"
//<![CDATA[
var Page_Validators = new Array();
var Page_ValidationSummaries = new Array();
function ValidatorOnSubmit() {
    return true;
}
//]]>");
#endif
                // This loop will look for any scripts that were injected into the
                // page. If they are found, then they are added to the page.
                foreach (Match scriptMatch in scriptEmbeddedRegex.Matches(html))
                {
                    string script = scriptMatch.Groups["script"].ToString();
                    if (script != string.Empty)
                    {
                        if (_includePageScripts && script.IndexOf("Anthem.Manager.GetScripts: false") == -1)
                            scripts.Add(script);

                        // This sequence of regular expressions match all of the client
                        // side validation scripts that may be added by the validation
                        // controls for both ASP.NET 1.1 and 2.0.
                        else if (Regex.IsMatch(script, "var Page_ValidationSummaries", RegexOptions.IgnoreCase))
                            scripts.Add(script);
                        else if (Regex.IsMatch(script, "var Page_Validators", RegexOptions.IgnoreCase))
                            scripts.Add(script);
                        else if (Regex.IsMatch(script, "var Page_ValidationActive", RegexOptions.IgnoreCase))
                            scripts.Add(script);
                        else if (Regex.IsMatch(script, "function WebForm_OnSubmit", RegexOptions.IgnoreCase))
                            scripts.Add(script);
                        else if (Regex.IsMatch(script, "\\.evaluationfunction =", RegexOptions.IgnoreCase))
                            scripts.Add(script);

                        // If validators with client side validation were added to the 
                        // page during the callback and if there are postback controls 
                        // on the page that cause validation, then we need to add an
                        // onsubmit event handler to the form that enforces the client
                        // side validation. This is how postback (non-callback) controls
                        // cause validation to occur.
                        //
                        // This next script is executed during callback response processing 
                        // on the client. It will attach the ASP.NET validation function
                        // to the onsubmit event of the form.
#if V2
                        if (Regex.IsMatch(script, "function WebForm_OnSubmit", RegexOptions.IgnoreCase))
                        {
                            scripts.Add(@"
//<![CDATA[
var form = Anthem_GetForm();
if (typeof(form) != ""undefined"" && form != null) {
  Anthem_AddEvent(form, ""onsubmit"", ""return WebForm_OnSubmit();"");
}
//]]>");
                        }
#else
                        if (Regex.IsMatch(script, "function ValidatorOnSubmit", RegexOptions.IgnoreCase))
                        {
                            scripts.Add(@"
//<![CDATA[
var form = Anthem_GetForm();
if (typeof(form) != ""undefined"" && form != null) {
    Anthem_AddEvent(form, ""onsubmit"", ""if (!ValidatorOnSubmit()) return false;"");
}
//]]>");
                        }
#endif
                    }
                }
            }

            return (string[]) scripts.ToArray(typeof(string));
        }

        const string _beginViewState = "<input type=\"hidden\" name=\"__VIEWSTATE\" value=\"";
        private string GetViewState(string html)
        {
#if V2
            return GetHiddenInputValue(html, _beginViewState2);
#else
            return GetHiddenInputValue(html, _beginViewState);
#endif
        }

#if V2
        const string _beginViewState2 = "<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=\"";
        const string _beginViewStateEncrypted = "<input type=\"hidden\" name=\"__VIEWSTATEENCRYPTED\" id=\"__VIEWSTATEENCRYPTED\" value=\"";
        const string _beginEventValidation = "<input type=\"hidden\" name=\"__EVENTVALIDATION\" id=\"__EVENTVALIDATION\" value=\"";
        private string GetViewStateEncrypted(string html)
        {
            return GetHiddenInputValue(html, _beginViewStateEncrypted);
        }

        private string GetEventValidation(string html)
        {
            return GetHiddenInputValue(html, _beginEventValidation);
        }
#endif

        private void OnError(object source, EventArgs e)
        {
            if (IsCallBack)
            {
                _error = HttpContext.Current.Error.ToString();
                HttpContext.Current.ClearError();
                WriteResult(HttpContext.Current.Response, null, _error);
            }
        }

        private void OnPreRender(object source, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            HttpRequest req = context.Request;
            HttpResponse resp = context.Response;
            Page page = source as Page;

            if (!CheckIfRedirectedToLoginPage() && IsCallBack)
            {
                object targetObject = null;
                string methodName = null;
                bool invokeMethod = true;

                if (req.Form["Anthem_PageMethod"] != null)
                {
                    targetObject = page;
                    methodName = req.Form["Anthem_PageMethod"];
                }
#if V2
                else if (req.Form["Anthem_MasterPageMethod"] != null)
                {
                    if (page != null)
                    {
                        // TODO: Since master pages can nest, we might need to do a search for the method up
                        // to the root master page.
                        targetObject = page.Master;
                        methodName = req.Form["Anthem_MasterPageMethod"];
                    }
                }
#endif
                else if (req.Form["Anthem_ControlID"] != null && req.Form["Anthem_ControlMethod"] != null)
                {
                    targetObject = _targets[req.Form["Anthem_ControlID"]];
                    methodName = req.Form["Anthem_ControlMethod"];
                }
                else
                {
                    invokeMethod = false;
                }

                object val = null;
                string error = null;

                if (invokeMethod)
                {
                    if (targetObject == null)
                    {
                        error = "CONTROLNOTFOUND";
                    }
                    else
                    {
                        if (methodName != null && methodName.Length > 0)
                        {
                            MethodInfo methodInfo = FindTargetMethod(targetObject, methodName);
                            if (methodInfo == null)
                            {
                                error = "METHODNOTFOUND";
                            }
                            else
                            {
                                try
                                {
                                    object[] parameters = ConvertParameters(methodInfo, req);
                                    val = InvokeMethod(targetObject, methodInfo, parameters);
                                }
                                catch (MethodAccessException ex)
                                {
                                    if (!methodInfo.IsPublic)
                                        error = string.Format("Anthem.Manager does not have permission to invoke method \"{0}\" in the current trust level. Please try making the method Public.", methodName);
                                    else
                                        error = ex.Message;
                                }
                                catch (Exception ex)
                                {
                                    error = ex.Message;
                                }
                            }
                        }
                    }
                }

                ConfigureResponse(resp);
                resp.Filter = new CallBackFilter(this, resp.Filter);
                _value = val;
                _error = error;
                _updatePage = string.Compare(req["Anthem_UpdatePage"], "true", true) == 0;
                _updateValidationScripts = false;

                if (_updatePage)
                {
                    // If there are any validators on the page, then include the validation
                    // scripts in the callback response in case any validators were added to
                    // the page during the callback processing.
                    _updateValidationScripts = ValidationScriptsRequired(page);
                    if (_updateValidationScripts)
                    {
#if V2
                        this._pageScripts.Add(GetAnthemManagerKey()+"WebForm.js", "src=" + page.ClientScript.GetWebResourceUrl(typeof(Page), "WebForms.js"));
                        this._pageScripts.Add(GetAnthemManagerKey()+"WebUIValidation.js", "src=" + page.ClientScript.GetWebResourceUrl(typeof(ASP.BaseValidator), "WebUIValidation.js"));
#else
                        string path = string.Empty;
                        IDictionary dictionary = (IDictionary)HttpContext.Current.GetConfig("system.web/webControls");
                        if (dictionary != null)
                        {
                            path = (string)dictionary["clientScriptsLocation"];
                        }
                        if (path != null && path.IndexOf("{0}") >= 0)
                        {
                            string version = Environment.Version.ToString();
                            path = string.Format(path, "system_web", version.Substring(0, version.LastIndexOf('.')).Replace('.', '_'));
                        }
                        this._pageScripts.Add(GetAnthemManagerKey()+"WebUIValidation.js", "src=" + path + "WebUIValidation.js");
#endif
                    }
                }
            }
        }

        private bool ValidationScriptsRequired(Page page)
        {
#if V2
            if (page != null && page.Validators.Count > 0 && page.Request.Browser.W3CDomVersion.Major >= 1)
            {
                foreach (ASP.BaseValidator validator in page.Validators)
                {
                    if (validator.EnableClientScript)
                    {
                        if (page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) >= 0)
                            return true;
                    }
                }
            }
            return false;
#else
            if (page != null && page.Validators.Count > 0 && page.Request.Browser.MSDomVersion.Major >= 4)
            {
                foreach (ASP.BaseValidator validator in page.Validators)
                {
                    if (validator.EnableClientScript)
                    {
                        if (page.Request.Browser.EcmaScriptVersion.CompareTo(new Version(1,2)) >= 0)
                            return true;
                    }
                }
            }
            return false;
#endif
        }

        /// <summary>
        /// Used to catch Response.Redirect() during a callback. If it is a redirect
        /// the response is converted back into a normal response and the appropriate
        /// javascript is returned to redirect the client.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnUnload(object sender, EventArgs e)
        {
            HttpResponse response = HttpContext.Current.Response;

            if (Manager.IsCallBack && response.StatusCode == 302)
            {
                string href = response.RedirectLocation.Replace("\\", "\\\\").Replace("'", "\\'");
                response.RedirectLocation = string.Empty;
                response.Clear();
                response.StatusCode = 200;
                StringBuilder sb = new StringBuilder();
                Manager.AddScriptForClientSideEval("window.location='" + href + "';");
                Manager.WriteValueAndError(sb, null, null, null, null, null, null, null);
                response.Write(sb.ToString());
                response.End();
            }
        }

        private void RegisterPageScript(Page page)
        {
            string formID = GetFormID(page);

            string pageScript = @"<script type=""text/javascript"">
//<![CDATA[
var Anthem_FormID = """ + formID + @""";
//]]>
</script>";

#if V2
            if (!String.IsNullOrEmpty(formID))
                page.ClientScript.RegisterClientScriptBlock(typeof(Manager), "pageScript", pageScript);
#else
            page.RegisterClientScriptBlock(typeof(Manager).FullName, pageScript);
#endif

#if V2
            page.ClientScript.RegisterClientScriptResource(typeof(Anthem.Manager), "Anthem.Anthem.js");
#else
            string baseUri = ConfigurationSettings.AppSettings["Anthem.BaseUri"];
            if (baseUri != null && baseUri.Length > 0)
            {
                if (baseUri.StartsWith("~"))
                    baseUri = page.ResolveUrl(baseUri);
                string scriptUri = System.IO.Path.Combine(baseUri, "Anthem.js");
                page.RegisterClientScriptBlock("Anthem.js", string.Format("<script type='text/javascript' src='{0}'></script>", scriptUri));
            }
            else
            {
                Stream stream = typeof(Anthem.Manager).Assembly.GetManifestResourceStream("Anthem.Anthem.js");
                StreamReader sr = new StreamReader(stream);
                page.RegisterClientScriptBlock("Anthem.js",
                    @"<script type=""text/javascript"">
//<![CDATA[
" + sr.ReadToEnd() + @"
//]]>
</script>");
            }
#endif
        }

        internal void WriteResult(Stream stream, MemoryStream htmlBuffer)
        {
            string viewState = null;
            string viewStateEncrypted = null;
            string eventValidation = null;
            Hashtable controls = null;
            string[] scripts = null;
            if (_updatePage)
            {
                string html = HttpContext.Current.Response.ContentEncoding.GetString(htmlBuffer.GetBuffer());
                viewState = GetViewState(html);
#if V2
                viewStateEncrypted = GetViewStateEncrypted(html);
                eventValidation = GetEventValidation(html);
#endif
                controls = GetControls(html);
                foreach (object o in _targets.Values)
                {
                    Control c = o as Control;
                    if (c != null && !c.Visible)
                    {
                        if (c.ID != null && controls.ContainsKey(c.ID))
                            controls[c.ID] = "";
                    }
                }

                scripts = GetScripts(html);
            }
            StringBuilder sb = new StringBuilder();
            try
            {
                WriteValueAndError(sb, _value, _error, viewState, viewStateEncrypted, eventValidation, controls, scripts);
            }
            catch (Exception ex)
            {
                // If an exception was thrown while formatting the
                // result value, we need to discard whatever was
                // written and start over with nothing but the error
                // message.
                sb.Length = 0;
                WriteValueAndError(sb, null, ex.Message, null, null, null, null, null);
            }

            // If an IOFrame was used to make this callback, then wrap the response in a <textarea> element
            // so the iframe will not mess with the text of the JSON object.
            string response = sb.ToString();
            if (string.Compare(HttpContext.Current.Request["Anthem_IOFrame"], "true", true) == 0)
            {
                // If the response text contains any </textarea> tags they will truncate the response
                // on the client. To avoid this, </textarea> tags are converted to </anthemarea> tags
                // here and converted back to </textarea> on the client.
                response = "<textarea id=\"response\">" + Regex.Replace(response, "</textarea>", "</anthemarea>", RegexOptions.IgnoreCase) + "</textarea>";
            }

            byte[] buffer = HttpContext.Current.Response.ContentEncoding.GetBytes(response);
            stream.Write(buffer, 0, buffer.Length);
        }

        #endregion
    }

    #region Internal Class CallBackFilter

    internal class CallBackFilter : Stream
    {
        private Anthem.Manager _manager;
        private Stream _next;
        private MemoryStream _buffer;

        internal CallBackFilter(Anthem.Manager manager, Stream next)
        {
            _manager = manager;
            _next = next;
            _buffer = new MemoryStream();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return 0; }
        }

        public override long Position
        {
            get { return 0; }
            set { }
        }

        public override void Close()
        {
            _manager.WriteResult(_next, _buffer);
            base.Close();
        }

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _buffer.Write(buffer, offset, count);
        }
    }
    #endregion
}
