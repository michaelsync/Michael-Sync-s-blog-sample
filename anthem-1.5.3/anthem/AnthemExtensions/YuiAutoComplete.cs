using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;

using Anthem;
using ASP = System.Web.UI.WebControls;

namespace AnthemExtensions
{
    /// <summary>
    /// Wrapper control for the AutoComplete widget in the YAHOO User Interface library.
    /// 
    /// 1. Assign the TextBox that will provide the input.
    /// 2. Assign the Search event handler that will provide the data. Decorate the event
    ///    handler with the Anthem.MethodAttribute so it can be called by the client.
    ///    
    /// Created by Andy Miller (amiller@structured-solutions.net) Mar 7, 2007
    /// </summary>
    [Designer(typeof(YuiAutoCompleteDesigner))]
    public class YuiAutoComplete : Control, IPostBackDataHandler
    {
        #region Constructors

        public YuiAutoComplete() : base() { }

        #endregion

        #region Events

        public delegate string[][] SearchHandler(string sQuery);

        /// <summary>
        /// Return an array of string arrays.
        /// The first element in each string array is required and should be the
        /// value that will be entered into the TextBox. Any additional values in
        /// the string array are available to the formatResult function on the
        /// client and the SelectedItem property on the server.
        /// </summary>
        [Browsable(true)]
        [Description("The Search event handler will be invoked by the client using Anthem each time a new search is needed. Remember to decorate your Search event handler with Anthem.AnthemMethodAttribute.")]
        public event SearchHandler Search;
        protected string[][] OnSearch(string sQuery)
        {
            if (Search != null)
                return Search(sQuery);
            else
                return null;
        }

        public event EventHandler SelectedItemChanged;
        protected void OnSelectedItemChanged(EventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, e);
        }

        #endregion

        #region Properties

        [Category("Behavior")]
        [DefaultValue(false)]
        [Description("When true, the control will make a callback when an item is selected")]
        public bool AutoCallBack
        {
            get
            {
                object obj = this.ViewState["AutoCallBack"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AutoCallBack"] = value; }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The CSS class to assign to the <div> container.")]
        public string CssClass
        {
            get
            {
                string text = (string)this.ViewState["CssClass"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["CssClass"] = value; }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The default text to display in the TextBox. This value is cleared when the TextBox receives focus.")]
        public string DefaultText
        {
            get
            {
                string text = (string)this.ViewState["DefaultText"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text.Trim();
            }
            set { this.ViewState["DefaultText"] = value; }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The footer markup to appear after the suggestion list.")]
        public string FooterMarkup
        {
            get
            {
                string text = (string)this.ViewState["FooterMarkup"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text.Trim();
            }
            set { this.ViewState["FooterMarkup"] = value; }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The header markup to appear before the suggestion list")]
        public string HeaderMarkup
        {
            get
            {
                string text = (string)this.ViewState["HeaderMarkup"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text.Trim();
            }
            set { this.ViewState["HeaderMarkup"] = value; }
        }

        [Browsable(false)]
        [DefaultValue("")]
        [Description("The item (if any) that was selected via mouse click, ENTER key, or TAB key.")]
        public string SelectedItem
        {
            get
            {
                string text = (string)this.ViewState["SelectedItem"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["SelectedItem"] = value; }
        }

        [Category("Behavior")]
        [DefaultValue("")]
        [Description("The input TextBox field.")]
#if V2
        [IDReferenceProperty(typeof(ASP.TextBox))]
        [TypeConverter(typeof(System.Web.UI.WebControls.ControlIDConverter))]
#else
        [TypeConverter(typeof(ControlIDConverter))]
#endif
        public string TextBox
        {
            get
            {
                string text = (string)this.ViewState["TextBox"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["TextBox"] = value; }
        }

        #endregion

        #region YUI AutoComplete Properties

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(true)]
        [Description("Whether or not to allow browsers to cache user-typed input in the input field. Disabling this feature will prevent the widget from setting the autocomplete=\"off\" on the input field. When autocomplete=\"off\" and users click the back button after form submission, user-typed input can be prefilled by the browser from its cache. This caching of user input may not be desired for sensitive data, such as credit card numbers, in which case, implementers should consider setting allowBrowserAutocomplete to false.")]
        public bool AllowBrowserAutocomplete
        {
            get
            {
                object obj = this.ViewState["AllowBrowserAutocomplete"];
                if (obj == null)
                    return true;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AllowBrowserAutocomplete"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not the results container should always be displayed. Enabling this feature displays the container when the widget is instantiated and prevents the toggling of the container to a collapsed state.")]
        public bool AlwaysShowContainer
        {
            get
            {
                object obj = this.ViewState["AlwaysShowContainer"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AlwaysShowContainer"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not to animate the expansion/collapse of the results container in the horizontal direction.")]
        public bool AnimHoriz
        {
            get
            {
                object obj = this.ViewState["AnimHoriz"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AnimHoriz"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(0.3)]
        [Description("Speed of container expand/collapse animation, in seconds.")]
        public double AnimSpeed
        {
            get
            {
                object obj = this.ViewState["AnimSpeed"];
                if (obj == null)
                    return 0.3;
                else
                    return (double)obj;
            }
            set { this.ViewState["AnimSpeed"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(true)]
        [Description("Whether or not to animate the expansion/collapse of the results container in the vertical direction.")]
        public bool AnimVert
        {
            get
            {
                object obj = this.ViewState["AnimVert"];
                if (obj == null)
                    return true;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AnimVert"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(true)]
        [Description("Whether or not the first item in results container should be automatically highlighted on expand.")]
        public bool AutoHighlight
        {
            get
            {
                object obj = this.ViewState["AutoHighlight"];
                if (obj == null)
                    return true;
                else
                    return (bool)obj;
            }
            set { this.ViewState["AutoHighlight"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue("")]
        [Description("Query delimiter. A single character separator for multiple delimited selections. A null value or empty string indicates that query results cannot be delimited. This feature is not recommended if you need forceSelection to be true.")]
        public string DelimChar
        {
            get
            {
                string text = (string)this.ViewState["DelimChar"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["DelimChar"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not to force the user's selection to match one of the query results. Enabling this feature essentially transforms the input field into a <select> field. This feature is not recommended with delimiter character(s) defined.")]
        public bool ForceSelection
        {
            get
            {
                object obj = this.ViewState["ForceSelection"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["ForceSelection"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue("yui-ac-highlight")]
        [Description("Class name of a highlighted item within results container.")]
        public string HighlightClassName
        {
            get
            {
                string text = (string)this.ViewState["HighlightClassName"];
                if (text == null)
                {
                    return "yui-ac-highlight";
                }
                return text;
            }
            set { this.ViewState["HighlightClassName"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(10)]
        [Description("Maximum number of results to display in results container.")]
        public int MaxResultsDisplayed
        {
            get
            {
                object obj = this.ViewState["MaxResultsDisplayed"];
                if (obj == null)
                    return 10;
                else
                    return (int)obj;
            }
            set { this.ViewState["MaxResultsDisplayed"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(1)]
        [Description("Number of characters that must be entered before querying for results. A negative value effectively turns off the widget. A value of 0 allows queries of null or empty string values.")]
        public int MinQueryLength
        {
            get
            {
                object obj = this.ViewState["MinQueryLength"];
                if (obj == null)
                    return 1;
                else
                    return (int)obj;
            }
            set { this.ViewState["MinQueryLength"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue("")]
        [Description("Class name of a pre-highlighted item within results container.")]
        public string PrehighlightClassName
        {
            get
            {
                string text = (string)this.ViewState["PrehighlightClassName"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["PrehighlightClassName"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(0.5)]
        [Description("Number of seconds to delay before submitting a query request. If a query request is received before a previous one has completed its delay, the previous request is cancelled and the new request is set to the delay.")]
        public double QueryDelay
        {
            get
            {
                object obj = this.ViewState["QueryDelay"];
                if (obj == null)
                    return 0.5;
                else
                    return (double)obj;
            }
            set { this.ViewState["QueryDelay"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not the input field should be automatically updated with the first query result as the user types, auto-selecting the substring that the user has not typed.")]
        public bool TypeAhead
        {
            get
            {
                object obj = this.ViewState["TypeAhead"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["TypeAhead"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not to use an iFrame to layer over Windows form elements in IE. Set to true only when the results container will be on top of a <select> field in IE and thus exposed to the IE z-index bug (i.e., 5.5 < IE < 7).")]
        public bool UseIFrame
        {
            get
            {
                object obj = this.ViewState["UseIFrame"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["UseIFrame"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue(false)]
        [Description("Whether or not the results container should have a shadow.")]
        public bool UseShadow
        {
            get
            {
                object obj = this.ViewState["UseShadow"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["UseShadow"] = value; }
        }

        [Category("YUI AutoComplete Properties")]
        [DefaultValue("2.2.1")]
        [Description("The YUI Library version to include.")]
        public string YuiVersion
        {
            get
            {
                string text = (string)this.ViewState["YuiVersion"];
                if (text == null)
                {
                    return "2.2.1";
                }
                return text;
            }
            set { this.ViewState["YuiVersion"] = value; }
        }

        #endregion

        #region YUI AutoComplete Methods

        [Category("YUI AutoComplete Methods")]
        [DefaultValue("")]
        [Description("Overridable method that converts a result item object into HTML markup for display. Return data values are accessible via the oResultItem object, and the key return value will always be oResultItem[0]. Markup will be displayed within <li> element tags in the container.")]
        public string FormatResult
        {
            get
            {
                string text = (string)this.ViewState["FormatResult"];
                if (text == null)
                {
                    return string.Empty;
                }
                return text;
            }
            set { this.ViewState["FormatResult"] = value; }
        }

        #endregion

        #region YUI DataSource Properties

        [Category("YUI DataSource Properties")]
        [DefaultValue(15)]
        [Description("Max size of the local cache. Set to 0 to turn off caching. Caching is useful to reduce the number of server connections. Recommended only for data sources that return comprehensive results for queries or when stale data is not an issue.")]
        public int MaxCacheEntries
        {
            get
            {
                object obj = this.ViewState["MaxCacheEntries"];
                if (obj == null)
                    return 15;
                else
                    return (int)obj;
            }
            set { this.ViewState["MaxCacheEntries"] = value; }
        }

        [Category("YUI DataSource Properties")]
        [DefaultValue(false)]
        [Description("Enables query case-sensitivity matching. If caching is on and queryMatchCase is true, queries will only return results for case-sensitive matches.")]
        public bool QueryMatchCase
        {
            get
            {
                object obj = this.ViewState["QueryMatchCase"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["QueryMatchCase"] = value; }
        }

        [Category("YUI DataSource Properties")]
        [DefaultValue(false)]
        [Description("Use this to equate cache matching with the type of matching done by your live data source. If caching is on and queryMatchContains is true, the cache returns results that \"contain\" the query string. By default, queryMatchContains is set to false, meaning the cache only returns results that \"start with\" the query string.")]
        public bool QueryMatchContains
        {
            get
            {
                object obj = this.ViewState["QueryMatchContains"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["QueryMatchContains"] = value; }
        }

        [Category("YUI DataSource Properties")]
        [DefaultValue(false)]
        [Description("Enables query subset matching. If caching is on and queryMatchSubset is true, substrings of queries will return matching cached results. For instance, if the first query is for \"abc\" susequent queries that start with \"abc\", like \"abcd\", will be queried against the cache, and not the live data source. Recommended only for DataSources that return comprehensive results for queries with very few characters.")]
        public bool QueryMatchSubset
        {
            get
            {
                object obj = this.ViewState["QueryMatchSubset"];
                if (obj == null)
                    return false;
                else
                    return (bool)obj;
            }
            set { this.ViewState["QueryMatchSubset"] = value; }
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // This control may be in a MasterPage, Page, or UserControl. Find out which
            // one and then register the target so that the Search event handler can be
            // found during the callback.
            Control container = GetTargetControl();
            if (container != null)
                Anthem.Manager.Register(container);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (this.TextBox == "") return;
            if (this.Search == null || this.Search.Method == null) return;

            ASP.TextBox textbox = this.NamingContainer.FindControl(this.TextBox) as ASP.TextBox;
            if (textbox == null) throw new InvalidOperationException(string.Format("Can not find TextBox id=\"{0}\" in naming container.", this.TextBox));

            // The Search method will be called by Anthem. Anthem can only find methods that
            // are decorated with the Anthem.MethodAttribute. This code verifies that the 
            // Search Method is an Anthem.Method. Note that even though the Search uses a
            // multicast delegate, only the first method will be invoked.
            object[] methodAttributes = this.Search.Method.GetCustomAttributes(typeof(Anthem.MethodAttribute), true);
            if (methodAttributes.Length == 0) throw new InvalidOperationException(string.Format("Search method '{0}' must be decorated with the Anthem.MethodAttribute.", this.Search.Method.Name));

            // The Anthem callback needs to tell Anthem.Manager where to look for the Search
            // method. This code finds the target control and then determines if the control
            // is in a MasterPage, Page, or User Control. Note that this code assumes the
            // Search method is in the same class that is hosting the YuiAutoComplete control.
            string methodScope = string.Empty;
            string methodScopeId = string.Empty;
            Control target = GetTargetControl();
#if V2
            if (target is MasterPage)
            {
                methodScope = "MasterPage";
            }
            else if (target is ASP.ContentPlaceHolder)
            {
                methodScope = "Page";
            }
            else
#endif
                if (target is Page)
                {
                    methodScope = "Page";
                }
                else if (target is UserControl)
                {
                    methodScope = "Control";
                    methodScopeId = target.ClientID;
                }

            // Load the YUI libraries
            Anthem.Manager.RegisterClientScriptInclude("yahoo-dom-event", string.Format("http://yui.yahooapis.com/{0}/build/yahoo-dom-event/yahoo-dom-event.js", YuiVersion));
            Anthem.Manager.RegisterClientScriptInclude("connection", string.Format("http://yui.yahooapis.com/{0}/build/connection/connection-min.js", YuiVersion));
            Anthem.Manager.RegisterClientScriptInclude("animation", string.Format("http://yui.yahooapis.com/{0}/build/animation/animation-min.js", YuiVersion));
            Anthem.Manager.RegisterClientScriptInclude("json", "http://www.json.org/json.js");
            Anthem.Manager.RegisterClientScriptInclude("autocomplete", string.Format("http://yui.yahooapis.com/{0}/build/autocomplete/autocomplete-min.js", YuiVersion));

            // Start the javascript that constructs the YUI AutoComplete object in the page and
            // connects it to the TextBox.
            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\">\n");

            // Create a unique javascript object for this AutoComplete widget
            script.AppendFormat("var {0} = {{}};\n", this.ClientID);
            string widget = this.ClientID + ".widget";
            script.AppendFormat("{0} = function() {{\n", widget);
            script.Append("  var dataSource;\n");
            script.Append("  var autoComplete;\n\n");
            script.AppendFormat("  var selectedItemField = document.getElementById('{0}');", this.ClientID);
            script.AppendFormat("  var textboxField = document.getElementById('{0}');", textbox.ClientID);
            script.Append("  return {\n");

            // Create the function that uses Anthem to retrieve the values from the server
            script.Append("    getData: function(query) {\n");
            script.AppendFormat("      var result = Anthem_CallBack(null, \"{0}\", \"{1}\", \"{2}\", [query], null, null, false, false);\n", methodScope, methodScopeId, this.Search.Method.Name);
            script.Append("      return result.value;\n");
            script.Append("    },\n\n");

            // Create the function that clears the hidden field before each data request
            script.Append("    dataRequested: function(type, args) {\n");
            script.Append("      selectedItemField.value = '';\n");
            script.Append("    },\n\n");

            // Create the function that updates the hidden field with the selected item
            script.Append("    itemSelected: function(type, args) {\n");
            script.Append("      var autoComplete = args[0];\n");
            script.Append("      var listitem = args[1];\n");
            script.Append("      var data = args[2];\n");
            script.Append("      if (data.length > 0) {\n");
            script.Append("        selectedItemField.value = data.toString()\n");
            script.Append("      }\n");
            if (this.AutoCallBack)
            {
                // If AutoCallBack is true, then fire a simple callback that includes the form fields
                // and updates the page so that the server can send feedback like showing the selected
                // item for redirecting the page based on the selected item.
                script.AppendFormat("      Anthem_FireEvent('{0}', '{1}', null, null, true, true);\n",
                    this.UniqueID,
                    string.Empty
                );
            }
            script.Append("    },\n\n");

            if (this.DefaultText.Length > 0)
            {
                // Create the function that clears the default text when the text box receives focus
                script.Append("    clearDefaultText: function(self) {\n");
                script.AppendFormat("      if (textboxField.value == '{0}') {{\n", this.DefaultText);
                script.Append("        textboxField.value = '';\n");
                script.Append("      }\n");
                script.Append("    },\n\n");

                // Create the function that sets the default text when the text box does not have focus
                script.Append("    setDefaultText: function(self) {\n");
                script.Append("      if (textboxField.value == '') {\n");
                script.AppendFormat("        textboxField.value = '{0}';\n", this.DefaultText);
                script.Append("      }\n");
                script.Append("    },\n\n");
            }

            // Create the page init function
            script.Append("    init: function() {\n");

            // Create the DataSource that uses the data function
            script.AppendFormat("      dataSource = new YAHOO.widget.DS_JSFunction({0}.getData);\n", widget);
            if (this.MaxCacheEntries != 15) { script.AppendFormat("      dataSource.maxCacheEntries = {0};\n", this.MaxCacheEntries); }
            if (this.QueryMatchCase != false) { script.AppendFormat("      dataSource.queryMatchCase = {0};\n", this.QueryMatchCase ? "true" : "false"); }
            if (this.QueryMatchContains != false) { script.AppendFormat("      dataSource.queryMatchContains = {0};\n", this.QueryMatchContains ? "true" : "false"); }
            if (this.QueryMatchSubset != false) { script.AppendFormat("      dataSource.queryMatchSubset = {0};\n\n", this.QueryMatchSubset ? "true" : "false"); }

            // Create the AutoComplete that uses the DataSource
            script.AppendFormat("      autoComplete = new YAHOO.widget.AutoComplete(\"{0}\",\"{1}\",dataSource);\n", this.NamingContainer.FindControl(this.TextBox).ClientID, this.GetContainerID());
            if (this.AllowBrowserAutocomplete != true) script.Append("      autoComplete.allowBrowserAutocomplete = false;\n");
            if (this.AlwaysShowContainer != false) script.Append("      autoComplete.alwaysShowContainer = true;\n");
            if (this.AnimHoriz != false) script.Append("      autoComplete.animHoriz = true;\n");
            if (this.AnimSpeed != 0.3) script.AppendFormat("      autoComplete.animSpeed = {0};\n", this.AnimSpeed);
            if (this.AnimVert != true) script.Append("      autoComplete.animVert = false;\n");
            if (this.AutoHighlight != true) script.Append("      autoComplete.autoHighlight = false;\n");
            if (this.DelimChar != "") script.AppendFormat("      autoComplete.delimChar = {0};\n", this.DelimChar);
            if (this.ForceSelection != false) script.Append("      autoComplete.forceSelection = true;\n");
            if (this.HighlightClassName != "yui-ac-highlight") script.AppendFormat("      autoComplete.highlightClassName = {0};\n", this.HighlightClassName);
            if (this.MaxResultsDisplayed != 10) script.AppendFormat("      autoComplete.maxResultsDisplayed = {0};\n", this.MaxResultsDisplayed);
            if (this.MinQueryLength != 1) script.AppendFormat("      autoComplete.minQueryLength = {0};\n", this.MinQueryLength);
            if (this.PrehighlightClassName != "") script.AppendFormat("      autoComplete.prehighlightClassName = {0};\n", this.PrehighlightClassName);
            if (this.QueryDelay != 0.5) script.AppendFormat("      autoComplete.queryDelay = {0};\n", this.QueryDelay);
            if (this.TypeAhead != false) script.Append("      autoComplete.typeAhead = true;\n");
            if (this.UseIFrame != false) script.Append("      autoComplete.useIFrame = true;\n");
            if (this.UseShadow != false) script.Append("      autoComplete.useShadow = true;\n");
            if (this.FormatResult != "") script.AppendFormat("      autoComplete.formatResult = {0};\n\n", this.FormatResult);

            script.AppendFormat("      autoComplete.dataRequestEvent.subscribe({0}.dataRequested);\n", widget);
            script.AppendFormat("      autoComplete.itemSelectEvent.subscribe({0}.itemSelected);\n", widget);
            if (this.DefaultText.Length > 0)
            {
                script.AppendFormat("      autoComplete.textboxBlurEvent.subscribe({0}.setDefaultText);\n", widget);
                script.AppendFormat("      autoComplete.textboxFocusEvent.subscribe({0}.clearDefaultText);\n", widget);
            }

            if (this.HeaderMarkup.Length > 0)
            {
                script.AppendFormat("      autoComplete.setHeader(\"{0}\");\n", this.HeaderMarkup);
            }
            if (this.FooterMarkup.Length > 0)
            {
                script.AppendFormat("      autoComplete.setFooter(\"{0}\");\n", this.FooterMarkup);
            }

            script.Append("    }\n"); // Close init
            script.Append("  };\n"); // Close return
            script.Append("}();\n"); // Close widget and run the function to create the objects

            // Register the init function so it runs after all the libraries are loaded
            script.AppendFormat("YAHOO.util.Event.addListener(this,'load',{0}.init);\n", widget);

            // Set the default text
            if (this.DefaultText.Length > 0)
            {
                script.AppendFormat("{0}.setDefaultText();\n", widget);
            }

            script.Append("</script>\n");

#if V2
            Anthem.Manager.RegisterStartupScript(this.GetType(), script.ToString(), script.ToString(), false);
#else
            Anthem.Manager.RegisterStartupScript(script.ToString(), script.ToString());
#endif
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // Create a <div> element that will be used to display the AutoComplete search results
            HtmlGenericControl div = new HtmlGenericControl("div");
            div.ID = GetContainerID();
            if (this.CssClass.Length > 0) div.Attributes.Add("class", this.CssClass);
            if (this.HeaderMarkup.Length > 0)
            {
                HtmlGenericControl header = new HtmlGenericControl("div");
                header.Attributes.Add("class", "yui-ac-hd");
                div.Controls.Add(header);
            }
            HtmlGenericControl body = new HtmlGenericControl("div");
            body.Attributes.Add("class", "yui-ac-bd");
            div.Controls.Add(body);
            if (this.FooterMarkup.Length > 0)
            {
                HtmlGenericControl footer = new HtmlGenericControl("div");
                footer.Attributes.Add("class", "yui-ac-ft");
                div.Controls.Add(footer);
            }
            div.RenderControl(writer);

            // Create a hidden element that will be used to store the selected item (if any) from
            // the search results. Note that this element uses this.UniqueID so that the value
            // is available as PostBackData.
            HtmlInputHidden selectedValue = new HtmlInputHidden();
            selectedValue.ID = this.UniqueID;
            selectedValue.Value = string.Empty;
            selectedValue.RenderControl(writer);
        }

        #endregion

        #region Private Methods

        private string GetContainerID()
        {
            return this.ClientID + "_div";
        }

        private Control GetTargetControl()
        {
            if (this.Search != null)
                return (Control)this.Search.Target;
            else
                return null;
        }

        #endregion

        #region IPostBackDataHandler Implementation

        public bool LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            string presentKey = this.SelectedItem.Split(',')[0];
            string postedKey = postCollection[postDataKey].Split(',')[0];
            if (string.Compare(presentKey, postedKey, true) != 0)
            {
                this.SelectedItem = postCollection[postDataKey];
                return true;
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            OnSelectedItemChanged(EventArgs.Empty);
        }

        #endregion
    }

    #region Control Designer (to create sample HTML in designer)

    public class YuiAutoCompleteDesigner : ControlDesigner
    {
        public override bool AllowResize
        {
            get { return false; }
        }

        public override string GetDesignTimeHtml()
        {
            try
            {
                YuiAutoComplete autocomplete = (YuiAutoComplete)Component;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("<div id=\"{0}\" class=\"{1}\">", autocomplete.ID, autocomplete.CssClass);
                sb.AppendFormat("<ul><li>aaaaa</li><li class=\"{0}\">abbbb</li><li>acccc</li></ul>", autocomplete.HighlightClassName);
                sb.Append("</div>");
                return sb.ToString();
            }
            catch (Exception ex)
            {
                return base.GetErrorDesignTimeHtml(ex);
            }
        }
    }

    #endregion

    #region ControlIDConverter for ASP.NET 1.1

#if !V2
    public class ControlIDConverter : ASP.ValidatedControlConverter
    {
        private object[] GetControls(IContainer container)
        {
            ArrayList list = new ArrayList();
            foreach (IComponent component in container.Components)
            {
                ASP.TextBox textbox = component as ASP.TextBox;
                if (textbox != null)
                {
                    if (textbox.ID != null && textbox.ID.Trim().Length > 0)
                    {
                        list.Add(textbox.ID);
                    }
                }
            }
            list.Sort();
            return list.ToArray();
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            if (context == null || context.Container == null) return null;
            object[] controls = GetControls(context.Container);
            if (controls != null)
                return new StandardValuesCollection(controls);
            return null;
        }
    }
#endif

    #endregion
}