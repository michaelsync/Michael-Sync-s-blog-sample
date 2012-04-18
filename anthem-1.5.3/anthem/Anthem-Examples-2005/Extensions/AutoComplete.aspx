<%@ Page Language="C#" MasterPageFile="~/Extensions/AutoComplete.master" %>
<%@ Register TagPrefix="Anthem" Namespace="Anthem" Assembly="Anthem" %> 
<%@ Register TagPrefix="AnthemExtensions" Namespace="AnthemExtensions" Assembly="AnthemExtensions" %>
<%@ Register TagPrefix="MY" TagName="MyAutoComplete" Src="~/Extensions/AutoComplete.ascx" %>

<script runat="server">
    
    /// <summary>
    /// Note that the Search handler must be decorated with the [Anthem.Method]
    /// attribute so that Anthem.Manager can find it. Also note that the method
    /// must be public if your web site is running in Medium trust. Otherwise
    /// Anthem.Manager will not be able to invoke the method.
    /// </summary>
    [Anthem.Method]
    public string[][] acpage_Search(string query)
    {
        ArrayList result = new ArrayList();
        query = HttpUtility.UrlDecode(query).Trim();
        result.Add(new string[] { query + "a", "page" });
        result.Add(new string[] { query + "b", "page" });
        result.Add(new string[] { query + "c", "page" });
        return (string[][])result.ToArray(typeof(string[]));
    }

    protected void acpage_SelectedItemChanged(object sender, EventArgs e)
    {
        this.labelpage.Text = "You selected: " + acpage.SelectedItem;
        this.labelpage.UpdateAfterCallBack = true;
    }
    
</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Notice that you will need to use CSS to position the results where you want them to appear -->
    <style type="text/css">
        fieldset {margin:1em}
        .container {position:relative;margin:1em;padding:1em 0;width:10em;}
        .textbox {position:absolute;top:0;width:95%;}
        .results {position:absolute;top:1.3em;width:95%;}
        .results .yui-ac-content {position:absolute;width:100%;height:11em;border:1px solid #404040;background:#fff;overflow:auto;overflow-x:hidden;z-index:9050;}
        .results .yui-ac-shadow {position:absolute;margin:.3em;width:100%;background:#a0a0a0;z-index:9049;}
        .results ul {padding:5px 0;width:100%;margin:0}
        .results li {padding:0 5px;cursor:default;white-space:nowrap}
        .results li.yui-ac-highlight {background:#ff0;}        
    </style>

    <script type="text/javascript">
        // This function is used to customize the list format. See the 'FormatResult'
        // property of YuiAutoComplete.
        function formatResult(item, query) {
            return item[0] + ' (' + item[1] + ')';
        }
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td valign="top">
                <fieldset>
                    <legend>Page</legend>
                    <div class="container">
                        <asp:TextBox ID="textpage" runat="server" CssClass="textbox" />
                        <AnthemExtensions:YuiAutoComplete id="acpage" runat="server" 
                            AutoCallBack="true"
                            CssClass="results" 
                            FormatResult="formatResult" 
                            HeaderMarkup="&lt;div style='color:#666'&gt;Please select one...&lt;/div&gt;"
                            OnSearch="acpage_Search" 
                            OnSelectedItemChanged="acpage_SelectedItemChanged" 
                            TextBox="textpage">
                        </AnthemExtensions:YuiAutoComplete>
                    </div>
                </fieldset>
                <div style="text-align:center">
                    <Anthem:Label ID="labelpage" runat="server" /><br />
                    <asp:DropDownList ID="selectpage" runat="server" ToolTip="This SELECT control is here to make sure that the suggestions appear above the SELECT.">
                        <asp:ListItem>One</asp:ListItem>
                        <asp:ListItem>Two</asp:ListItem>
                        <asp:ListItem>Three</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </td>
            <td valign="top">
                <MY:MyAutoComplete ID="myac" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>