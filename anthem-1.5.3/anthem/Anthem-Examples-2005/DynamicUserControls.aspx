<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void Page_Load()
    {
        place.Controls.Add(LoadControl("UserControlWithCheckBox.ascx"));
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <p>
        The user control you see below this paragraph is being dynamically loaded and inserted
        into a PlaceHolder control.</p>
    <asp:PlaceHolder ID="place" runat="server" />
</asp:Content>
