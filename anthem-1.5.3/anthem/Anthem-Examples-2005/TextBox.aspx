<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    protected void textbox1_TextChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        message.Text = textbox1.Text;
    }
    
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>
        TextBox Examples</h1>
    <p>
        Type something in the box and then tab out. What ever you typed will be echoed below
        the text box.</p>
    <anthem:TextBox ID="textbox1" runat="server" AutoCallBack="True" OnTextChanged="textbox1_TextChanged"
        CausesValidation="false" TextDuringCallBack="working..." />
    <asp:Label ID="textboxlabel" runat="server" AssociatedControlID="textbox1" /><br />
    <anthem:Label ID="message" runat="server" AutoUpdateAfterCallBack="true" />
    <p>
        This second textbox is here to test the interaction with validators. The TextBox
        above does not cause validator to occur. The one below does.</p>
    <anthem:TextBox ID="textbox2" runat="server" AutoCallBack="True" CausesValidation="true" />
    <anthem:CompareValidator ID="textbox2validator" runat="server" ControlToValidate="textbox2"
        ValueToCompare="123" ErrorMessage="Please enter 123" />
</asp:Content>
