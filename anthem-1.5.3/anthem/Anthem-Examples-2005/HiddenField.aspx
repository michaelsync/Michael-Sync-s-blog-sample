<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void getButton_Click(object sender, EventArgs e)
    {
        label.Text = hiddenField.Value;
        label.UpdateAfterCallBack = true;
    }

    void setButton_Click(object sender, EventArgs e)
    {
        hiddenField.Value = textBox.Text;
        hiddenField.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div>
        <anthem:HiddenField ID="hiddenField" runat="server" Value="foo" />
        <p>
            The hidden field on this page has an initial value of "foo". Click the "Get Value"
            button to trigger a call back to the server which will update a label on the page
            with the field's current value. Click the "Set Value" button to trigger a call back
            to the server to set a new value on the field and update it on the page. Click "Get
            Value" again to see that the value did change.
        </p>
        <anthem:Button ID="getButton" runat="server" Text="Get Value" OnClick="getButton_Click" />
        <anthem:Label ID="label" runat="server" />
        <br />
        <anthem:Button ID="setButton" runat="server" Text="Set Value" OnClick="setButton_Click" />
        <asp:TextBox ID="textBox" runat="server" />
    </div>
</asp:Content>
