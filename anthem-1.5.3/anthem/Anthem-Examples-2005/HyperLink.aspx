<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        HyperLink1.NavigateUrl = "http://anthem-dot-net.sourceforge.net";
        HyperLink1.Text = HyperLink1.NavigateUrl;
        HyperLink1.ToolTip = "Click me to redirect your browser to " + HyperLink1.NavigateUrl;
        HyperLink1.UpdateAfterCallBack = true;
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div>
        <h1>
        HyperLink Example</h1>
        Click on the button to update the HyperLink control with a new URL. Then click on
        the HyperLink control to link to redirect the page.<br />
        <br />
        <anthem:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Click Me!" /><br />
        <br />
        <anthem:HyperLink ID="HyperLink1" runat="server">HyperLink</anthem:HyperLink></div>
</asp:Content>
