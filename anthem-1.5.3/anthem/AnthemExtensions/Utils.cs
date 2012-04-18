using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Globalization;

namespace AnthemExtensions {
#if V2
    internal static class Utils {
#else
    internal sealed class Utils {
        private Utils() {}
#endif
        #region Helpers
        public static string ToJavascriptFunction(string functionName) {
            return ToJavascriptFunction(functionName, null);
        }

        public static string ToJavascriptFunction(string functionName, string argName) {
            string value = "null";
            if (!IsEmpty(functionName)) {
                if (IsEmpty(argName)) {
                    if (functionName.EndsWith(");"))
                        value = "function() { " + functionName + " }";
                    else 
                        value = "function() { " + functionName + "(); }";
                }
                else {
                    if (functionName.EndsWith(");"))
                        throw new ArgumentException("Javascript Function has already been terminated");
                    value = "function() { " + functionName + "(" + argName + "); }";                    
                }
            }
            return value;
        }

        public static string ToStringBool(bool value) {
            return (value ? "true" : "false");
        }

        public static bool IsEmpty(string value) {
            return (value == null || value.Trim().Length == 0);
        }

        #endregion

        #region Script Registration
        public static void RegisterScript(Control control, string key, string script) {
            bool addScriptTags = !script.StartsWith("<script");
#if V2
            if (!control.Page.ClientScript.IsStartupScriptRegistered(key))
                control.Page.ClientScript.RegisterStartupScript(control.GetType(), key, script, addScriptTags);
#else
            if (!control.Page.IsStartupScriptRegistered(key)) {
                if (addScriptTags)
                    script = "<script language=\"javascript\" type=\"text/javascript\">\n" + script + "\n</script>\n";
                control.Page.RegisterClientScriptBlock(control.ClientID, script);
            }
#endif
        }

        public static void RegisterScriptInclude(Control control, string script) {
#if V2
#if DEBUG
            if (!control.Page.ClientScript.IsStartupScriptRegistered(script))
                control.Page.ClientScript.RegisterStartupScript(control.GetType(), control.GetType().FullName, GetAssemblyResource(script), true);
#else
            if (!control.Page.ClientScript.IsClientScriptIncludeRegistered(script))
                control.Page.ClientScript.RegisterClientScriptResource(control.GetType(), script);
#endif
#else
            if (!control.Page.IsStartupScriptRegistered(script))
                control.Page.RegisterClientScriptBlock(control.GetType().FullName, "<script language=\"javascript\" type=\"text/javascript\">\n" + GetAssemblyResource(script) + "\n</script>\n");
#endif
        }

#if DEBUG || !V2
        public static string GetAssemblyResource(string resourceName) {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            StreamReader sr = new StreamReader(stream); 
            return sr.ReadToEnd();
        }
#endif

        public static void SendScript(Control control, string script) {
            if (Anthem.Manager.IsCallBack) 
                Anthem.Manager.AddScriptForClientSideEval(script);
            else 
                Utils.RegisterScript(control, control.ClientID, script);
        }

        #endregion

        #region Reflection
        public static bool ContainsControlType(Control control, params Type[] types) {
            foreach (Type type in types) {
                if (InheritsFrom(control, types))
                    return true;
                else {
                    foreach (Control ctrl in control.Controls) 
                        if (ContainsControlType(ctrl, type))
                            return true;                    
                }
            }
            return false;
        }

        /// <summary></summary>
        /// <param name="objectType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool InheritsFrom(Type objectType, params Type[] types) {
            if (types == null || types.Length == 0)
                throw new Exception("You must pass at least 1 type to check against");

            foreach (Type type in types) {
                if (type.IsInterface) 
                    return SubscribesToInterface(objectType, type);
                else if (objectType.IsSubclassOf(type) || objectType == type)
                    return true;
            }
            return false;
        }

        /// <summary></summary>
        /// <param name="obj"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool InheritsFrom(object obj, params Type[] types) {
            return InheritsFrom(obj.GetType(), types);
        }

        /// <summary></summary>
        /// <param name="obj"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool SubscribesToInterface(object obj, Type interfaceType) {
            return SubscribesToInterface(obj.GetType(), interfaceType);
        }

        /// <summary></summary>
        /// <param name="objectType"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool SubscribesToInterface(Type objectType, Type interfaceType) {
            if (!interfaceType.IsInterface)
                throw new ArgumentException(String.Format("{0} is not an interface type", interfaceType));
            return (objectType.GetInterface(interfaceType.ToString()) != null);
        }

        #endregion

        #region Styles
        /// <summary> Stringfies Style class </summary>
        /// <remarks> Ignores CssClass, check prior to entering this method.</remarks>
        /// <param name="style"></param>
        public static string GetStyleString(Style style) {
            if (!Utils.IsStyleSet(style))
                return String.Empty;
            else {
                // way to go M$ on making the CssTextWriter untouchable AND making the style class useless outside of the webcontrol :P
                StringBuilder sb = new StringBuilder();
                AddStyleColor(sb, "color", style.ForeColor);
                AddStyleColor(sb, "background-color", style.BackColor);
                AddStyleColor(sb, "border-color", style.BorderColor);

                if (!style.BorderWidth.IsEmpty) {
                    AddStyleUnit(sb, "border-width", style.BorderWidth);
                    if (style.BorderStyle == BorderStyle.NotSet) 
                        if (style.BorderWidth.Value != 0)
                            sb.Append("border-style:solid;");
                    
                }
                else if (style.BorderStyle != BorderStyle.NotSet) 
                    sb.AppendFormat("border-style:{0};", style.BorderStyle.ToString());
                
                FontInfo font = style.Font;
                if (font.Names.Length > 0)
                    sb.AppendFormat("font-family:{0};", FormatStringArray(font.Names, ','));
                if (!font.Size.IsEmpty)
                    sb.AppendFormat("font-size:{0};", font.Size.ToString(CultureInfo.InvariantCulture));

                if (font.Bold)
                    sb.Append("font-weight:bold;");
                if (font.Italic)
                    sb.Append("font-style:italic;");

                string fontStyle = String.Empty;
                if (font.Underline)
                    fontStyle = "underline";
                if (font.Overline)
                    fontStyle += " overline";
                if (font.Strikeout)
                    fontStyle += " line-through";
                if (fontStyle.Length > 0)
                    sb.AppendFormat("text-decoration:{0};", fontStyle);
                
                AddStyleUnit(sb, "height", style.Height);
                AddStyleUnit(sb, "width", style.Width);
                return sb.ToString();
            }
        }

        public static string GetJavaScriptStyleString(Style style) {
            return GetJavaScriptStyleString(style, "this");
        }

        public static string GetJavaScriptStyleString(Style style, string jsObjectName) {
            if (!Utils.IsStyleSet(style))
                return String.Empty;
            else if (style.CssClass != String.Empty) 
                return String.Format("{0}.className='{1}';", jsObjectName, style.CssClass);                
            else {                
                // way to go M$ on making the CssTextWriter untouchable AND making the style class useless outside of the webcontrol :P
                StringBuilder sb = new StringBuilder();
                AddJsStyleColor(sb, jsObjectName, "color", style.ForeColor);
                AddJsStyleColor(sb, jsObjectName, "backgroundColor", style.BackColor);
                AddJsStyleColor(sb, jsObjectName, "borderColor", style.BorderColor);

                if (!style.BorderWidth.IsEmpty) {
                    AddJsStyleUnit(sb, jsObjectName, "borderWidth", style.BorderWidth);
                    if (style.BorderStyle == BorderStyle.NotSet) 
                        if (style.BorderWidth.Value != 0)
                            sb.Append("borderStyle:solid;");
                    
                }
                else if (style.BorderStyle != BorderStyle.NotSet) 
                    sb.AppendFormat("{0}.style.borderStyle = '{0}';", style.BorderStyle.ToString());
                
                FontInfo font = style.Font;
                if (font.Names.Length > 0)
                    sb.AppendFormat("{0}.style.fontFamily='{0}';", FormatStringArray(font.Names, ','));
                if (!font.Size.IsEmpty)
                    sb.AppendFormat("{0}.style.fontSize='{0}';", font.Size.ToString(CultureInfo.InvariantCulture));

                if (font.Bold)
                    sb.AppendFormat("{0}.style.fontWeight='bold';", jsObjectName);
                if (font.Italic)
                    sb.AppendFormat("{0}.style.fontStyle='italic';", jsObjectName);

                if (font.Underline)
                    sb.AppendFormat("{0}.style.textDecorationUnderline=true;", jsObjectName);                
                if (font.Overline)
                    sb.AppendFormat("{0}.style.textDecorationOverline=true;", jsObjectName);
                if (font.Strikeout)
                    sb.AppendFormat("{0}.style.textDecorationLineThrough=true;", jsObjectName);
                
                AddJsStyleUnit(sb, jsObjectName, "height", style.Height);
                AddJsStyleUnit(sb, jsObjectName, "width", style.Width);
                return sb.ToString();                
            }
        }

        public static bool IsStyleSet(Style style) {
#if V2
            return (style != null && !style.IsEmpty);
#else
            return (style != null && (style.Font != null || !style.BorderColor.IsEmpty || !style.ForeColor.IsEmpty || !style.BackColor.IsEmpty));
#endif
        }

        private static string FormatStringArray(string[] array, char delimiter) {
            switch (array.Length) {
                case 0  : return String.Empty;
                case 1  : return array[0];
                default : return String.Join(delimiter.ToString(CultureInfo.InvariantCulture), array);
            }
        }

        private static void AddStyleColor(StringBuilder sb, string property, Color color) {
            if (!color.IsEmpty) 
                sb.AppendFormat("{0}:{1};", property, ColorTranslator.ToHtml(color));
        }

        private static void AddStyleUnit(StringBuilder sb, string property, Unit unit) {
            if (!unit.IsEmpty)
                sb.AppendFormat("{0}:{1};", property, unit.ToString(CultureInfo.InvariantCulture));
        }

        public static void AddJsStyleColor(StringBuilder sb, string objName, string property, Color color) {
            if (!color.IsEmpty)
                sb.AppendFormat("{0}.style.{1} = '{2}';", objName, property, ColorTranslator.ToHtml(color));
        }

        public static void AddJsStyleUnit(StringBuilder sb, string objName, string property, Unit unit) {
            if (!unit.IsEmpty)
                sb.AppendFormat("{0}.style.{1} = '{2}';", objName, property, unit.ToString(CultureInfo.InvariantCulture));
        }

        #endregion
    }
}
