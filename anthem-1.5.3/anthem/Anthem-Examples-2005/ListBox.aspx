<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void listBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder("You selected ");
        foreach (ListItem item in listBox.Items)
            if (item.Selected)
                sb.AppendFormat("\"{0}\", ", item.Value);
        sb.Length -= 2;
        label.Text = sb.ToString();
        label.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Example</h2>
    <p>
        Selecting different items in this list box triggers a call back to the server which
        updates a label letting you know what you selected.</p>
    <anthem:ListBox ID="listBox" runat="server" AutoCallBack="true" OnSelectedIndexChanged="listBox_SelectedIndexChanged"
        SelectionMode="Multiple">
        <Items>
            <asp:ListItem>foo</asp:ListItem>
            <asp:ListItem>bar</asp:ListItem>
            <asp:ListItem>baz</asp:ListItem>
        </Items>
    </anthem:ListBox>
    <br />
    <anthem:Label ID="label" runat="server" />
</asp:Content>
