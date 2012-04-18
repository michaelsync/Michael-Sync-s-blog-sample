<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void modes_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (modes.SelectedValue)
        {
            case "Browse":
                man.DisplayMode = WebPartManager.BrowseDisplayMode;
                break;
            case "Catalog":
                man.DisplayMode = WebPartManager.CatalogDisplayMode;
                break;
        }
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div>
        <h2>
            Example</h2>
        <p>
            This currently only works with IE.</p>
        <asp:WebPartManager ID="man" runat="server" />
        <asp:DropDownList ID="modes" runat="server" AutoPostBack="true" OnSelectedIndexChanged="modes_SelectedIndexChanged">
            <asp:ListItem>Browse</asp:ListItem>
            <asp:ListItem>Catalog</asp:ListItem>
        </asp:DropDownList>
        <asp:CatalogZone ID="catZone" runat="server">
            <ZoneTemplate>
                <asp:PageCatalogPart ID="pageCat" runat="server" />
            </ZoneTemplate>
        </asp:CatalogZone>
        <hr />
        <anthem:WebPartZone ID="zone1" runat="server" HeaderText="Zone 1">
            <ZoneTemplate>
                <asp:TextBox ID="textBox" runat="server" Title="A TextBox" />
            </ZoneTemplate>
        </anthem:WebPartZone>
        <hr />
        <anthem:WebPartZone ID="zone2" runat="server" HeaderText="Zone 2">
            <ZoneTemplate>
                <asp:Button ID="button" runat="server" Text="Click Me" Title="A Button" />
                <asp:LinkButton ID="linkButton" runat="server" Text="Click Me, too" Title="A Link Button" />
            </ZoneTemplate>
        </anthem:WebPartZone>
    </div>
</asp:Content>
