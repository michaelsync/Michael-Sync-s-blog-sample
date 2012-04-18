<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    private void button_Click(object sender, System.EventArgs e)
    {
        label.Text = Request.Path;
        label.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <p>
        Look at the address bar in your browser. It should say ServerTransferExample.aspx
        if that's where you started and yet this is really ServerTransferDestination.aspx.</p>
    <p>
        Clicking the following button should call back to ServerTransferDestination.aspx
        and not ServerTransferExample.aspx.</p>
    <anthem:Button ID="button" runat="server" Text="Get Request.Path" OnClick="button_Click" />
    <anthem:Label ID="label" runat="server" Text="?" />
</asp:Content>
