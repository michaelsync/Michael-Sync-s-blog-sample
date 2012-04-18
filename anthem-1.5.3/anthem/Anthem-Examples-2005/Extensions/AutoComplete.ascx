<%@ Control Language="C#" ClassName="AutoComplete" %>
<%@ Register TagPrefix="Anthem" Namespace="Anthem" Assembly="Anthem" %> 
<%@ Register TagPrefix="AnthemExtensions" Namespace="AnthemExtensions" Assembly="AnthemExtensions" %>

<script runat="server">
    
    [Anthem.Method]
    public string[][] acuser_Search(string query)
    {
        ArrayList result = new ArrayList();
        query = HttpUtility.UrlDecode(query).Trim();
        result.Add(new string[] { query + "a", "user control" });
        result.Add(new string[] { query + "b", "user control" });
        result.Add(new string[] { query + "c", "user control" });
        return (string[][])result.ToArray(typeof(string[]));
    }

    protected void acuser_SelectedItemChanged(object sender, EventArgs e)
    {
        this.labeluser.Text = "You selected: " + acuser.SelectedItem;
        this.labeluser.UpdateAfterCallBack = true;
    }
    
</script>
<fieldset>
    <legend>User Control</legend>
    <div class="container">
        <asp:TextBox ID="textuser" runat="server" CssClass="textbox" />
        <AnthemExtensions:YuiAutoComplete ID="acuser" runat="server" 
            AnimHoriz="true" 
            AnimVert="false" 
            AutoCallBack="true"
            CssClass="results" 
            FormatResult="formatResult" 
            OnSearch="acuser_Search" 
            OnSelectedItemChanged="acuser_SelectedItemChanged" 
            TextBox="textuser">
        </AnthemExtensions:YuiAutoComplete>
    </div>
</fieldset>
<div style="text-align:center">
    <Anthem:Label ID="labeluser" runat="server" />
</div>
