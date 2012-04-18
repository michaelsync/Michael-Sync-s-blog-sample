<%@ Control Language="C#" ClassName="AutoComplete" %>
<%@ Register TagPrefix="AnthemExtensions" Namespace="AnthemExtensions" Assembly="AnthemExtensions" %>

<script runat="server">
    [Anthem.Method]
    protected string[][] acuser_Search(string sQuery)
    {
        ArrayList result = new ArrayList();
        result.Add(new string[] { sQuery + "a", "user control" });
        result.Add(new string[] { sQuery + "b", "user control" });
        result.Add(new string[] { sQuery + "c", "user control" });
        return (string[][])result.ToArray(typeof(string[]));
    }
</script>
<fieldset>
    <legend>User Control</legend>
    <div class="container">
        <asp:TextBox ID="textuser" runat="server" CssClass="textbox" /><br />
        <AnthemExtensions:YuiAutoComplete ID="acuser" runat="server" OnSearch="acuser_Search" formatResult="formatResult" TextBox="textuser" CssClass="results" animHoriz="true" animVert="false" />
    </div>
</fieldset>