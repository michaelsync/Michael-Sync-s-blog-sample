<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void toggleImage_Click(object sender, EventArgs e)
    {
        if (image.ImageUrl == "button_run.gif")
        {
            image.ImageUrl = "button_run-down.gif";
        }
        else
        {
            image.ImageUrl = "button_run.gif";
        }
        image.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>Example</h2>
        <anthem:Button ID="toggleImage" runat="server" Text="Toggle Image" OnClick="toggleImage_Click" />
        <br />
        <anthem:Image ID="image" runat="server" ImageUrl="button_run.gif" />
</asp:Content>

