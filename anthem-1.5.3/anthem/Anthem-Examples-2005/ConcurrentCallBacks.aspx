<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<script runat="server">

    void slowButton_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(4000);
        slowResult.Text = DateTime.Now.ToString();
        slowResult.UpdateAfterCallBack = true;
    }

    void fastButton_Click(object sender, ImageClickEventArgs e)
    {
        fastResult.Text = DateTime.Now.ToString();
        fastResult.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <h2>
        Example</h2>
    <p>
        Clicking the "Slow" button will sleep for 4 seconds on the server. If you click
        that button and then click the "Fast" button, you'll see the result for clicking
        the "Fast" button appear before the result for clicking the "Slow" button.</p>
    <asp:Label ID="slowLabel" runat="server" AssociatedControlID="slowButton" Text="Slow:">
    </asp:Label>
    <anthem:ImageButton ID="slowButton" runat="server" AlternateText="Slow" ImageUrl="button_run.gif"
        ImageUrlDuringCallBack="button_run-down.gif" OnClick="slowButton_Click"></anthem:ImageButton>
    <anthem:Label ID="slowResult" runat="server" />
    <br />
    <asp:Label ID="fastLabel" runat="server" AssociatedControlID="fastButton" Text="Fast:">
    </asp:Label>
    <anthem:ImageButton ID="fastButton" runat="server" AlternateText="Slow" ImageUrl="button_run.gif"
        ImageUrlDuringCallBack="button_run-down.gif" OnClick="fastButton_Click"></anthem:ImageButton>
    <anthem:Label ID="fastResult" runat="server" />
</asp:Content>
