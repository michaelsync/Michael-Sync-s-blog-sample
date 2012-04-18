<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    void ServerValidation(object sender, ServerValidateEventArgs args)
    {
        try
        {
            // Test whether the value entered into the text box is even.
            int i = int.Parse(args.Value);
            args.IsValid = ((i % 2) == 0);
        }
        catch
        {
            args.IsValid = false;
        }
        ((Anthem.CustomValidator)sender).UpdateAfterCallBack = true;
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <p>
        There are several validators on this page, divided into two ValidationGroup's: Group1
        and Group2.</p>
    <h3>
        Group1</h3>
    <div>
        1.
        <asp:TextBox ID="textBox1" runat="server" ValidationGroup="Group1" />
        <anthem:RequiredFieldValidator ID="required1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="Enter a value into text box #1"
            Text="*" ValidationGroup="Group1"></anthem:RequiredFieldValidator>
        <anthem:CompareValidator ID="compare1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="The value must be a number!"
            Operator="DataTypeCheck" Text="*" Type="Integer" ValidationGroup="Group1"></anthem:CompareValidator>
        <anthem:RangeValidator ID="range1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="The value must be from 1 to 10!"
            MinimumValue="1" MaximumValue="10" Text="*" Type="Integer" ValidationGroup="Group1"></anthem:RangeValidator>
        <br />
        2.
        <asp:TextBox ID="textbox2" runat="server" ValidationGroup="Group1" />
        <anthem:RequiredFieldValidator ID="required2" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox2" EnableClientScript="false" ErrorMessage="Enter a value into text box #2"
            Text="*" ValidationGroup="Group1"></anthem:RequiredFieldValidator>
        <anthem:RegularExpressionValidator ID="regex1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox2" EnableClientScript="false" ErrorMessage="You must enter 2 digits!"
            Text="*" ValidationExpression="\d{2}" ValidationGroup="Group1"></anthem:RegularExpressionValidator>
        <anthem:CustomValidator ID="custom1" runat="server" ControlToValidate="textBox2"
            EnableClientScript="false" ErrorMessage="You must enter an even number!" OnServerValidate="ServerValidation"
            Text="*" ValidationGroup="Group1"></anthem:CustomValidator>
    </div>
    <anthem:Button ID="button1" runat="server" Text="Click Me!" ValidationGroup="Group1" />
    <br />
    <anthem:ValidationSummary ID="summary1" runat="server" DisplayMode="BulletList" EnableCallBackScript="true"
        EnableClientScript="False" HeaderText="Found these errors in Group1:" ShowSummary="true"
        ValidationGroup="Group1" ShowMessageBox="True" AutoUpdateAfterCallBack="True" />
    <hr />
    <h3>
        Group2</h3>
    3.
    <asp:TextBox ID="TextBox3" runat="server" ValidationGroup="Group2" />
    <anthem:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" AutoUpdateAfterCallBack="true"
        ControlToValidate="textBox3" EnableClientScript="false" ErrorMessage="Enter a value into text box #3"
        Text="*" ValidationGroup="Group2"></anthem:RequiredFieldValidator><anthem:CompareValidator
            ID="CompareValidator1" runat="server" AutoUpdateAfterCallBack="true" ControlToValidate="textBox3"
            EnableClientScript="false" ErrorMessage="The value must be a number!" Operator="DataTypeCheck"
            Text="*" Type="Integer" ValidationGroup="Group2"></anthem:CompareValidator><anthem:RangeValidator
                ID="RangeValidator1" runat="server" AutoUpdateAfterCallBack="true" ControlToValidate="textBox3"
                EnableClientScript="false" ErrorMessage="The value must be from 1 to 10!" MaximumValue="10"
                MinimumValue="1" Text="*" Type="Integer" ValidationGroup="Group2"></anthem:RangeValidator><br />
    <anthem:Button ID="button2" runat="server" Text="Click Me!" ValidationGroup="Group2" />
    <br />
    <anthem:ValidationSummary ID="summary2" runat="server" DisplayMode="BulletList" EnableCallBackScript="true"
        EnableClientScript="false" HeaderText="Found these errors in Group2:" ShowSummary="true"
        ValidationGroup="Group2" ShowMessageBox="True" AutoUpdateAfterCallBack="True"></anthem:ValidationSummary>
    <hr />
    <h3>
        No Group</h3>
    4.
    <asp:TextBox ID="textBox4" runat="server" />
    <anthem:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" AutoUpdateAfterCallBack="true"
        ControlToValidate="textBox4" EnableClientScript="false" ErrorMessage="Enter a value into text box #4"
        Text="*"></anthem:RequiredFieldValidator><anthem:CompareValidator ID="CompareValidator2"
            runat="server" AutoUpdateAfterCallBack="true" ControlToValidate="textBox4" EnableClientScript="false"
            ErrorMessage="The value must be a number!" Operator="DataTypeCheck" Text="*"
            Type="Integer"></anthem:CompareValidator><anthem:RangeValidator ID="RangeValidator2"
                runat="server" AutoUpdateAfterCallBack="true" ControlToValidate="textBox4" EnableClientScript="false"
                ErrorMessage="The value must be from 1 to 10!" MaximumValue="10" MinimumValue="1"
                Text="*" Type="Integer"></anthem:RangeValidator><br />
    <anthem:Button ID="button3" runat="server" Text="Click Me!" /><br />
    <anthem:ValidationSummary ID="summary3" runat="server" DisplayMode="BulletList" EnableCallBackScript="true"
        EnableClientScript="false" HeaderText="Found these errors in the ungrouped controls:"
        ShowSummary="true" ShowMessageBox="True" AutoUpdateAfterCallBack="True"></anthem:ValidationSummary>
</asp:Content>
