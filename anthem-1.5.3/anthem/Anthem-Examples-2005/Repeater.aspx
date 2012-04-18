<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void Page_Load()
    {
        string[] data = new string[] { "a", "b", "c", "d", "e" };
        repeater.DataSource = data;
    }

    void sort_Click(object sender, EventArgs e)
    {
        bool reversed = ViewState["reversed"] != null ? (bool)ViewState["reversed"] : false;
        if (!reversed)
        {
            string[] data = repeater.DataSource as string[];
            Array.Reverse(data);
        }
        ViewState["reversed"] = !reversed;
    }

    void Page_PreRender()
    {
        DataBind();
        repeater.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Example</h2>
    <p>
        Here is a repeater that outputs a list of strings. Click the button to update the
        repeater with a reversed data source.</p>
    <anthem:Repeater ID="repeater" runat="server">
        <HeaderTemplate>
            <ul>
        </HeaderTemplate>
        <ItemTemplate>
            <li>
                <%# Container.DataItem %>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul></FooterTemplate>
    </anthem:Repeater>
    <br />
    <anthem:Button ID="sort" runat="server" Text="Sort" OnClick="sort_Click" />
</asp:Content>
