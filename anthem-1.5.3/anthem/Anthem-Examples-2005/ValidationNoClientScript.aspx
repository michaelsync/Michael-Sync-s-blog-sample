<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        label.Text = string.Format("IsValid = {0}", IsValid);
        label.UpdateAfterCallBack = true;
        summary.UpdateAfterCallBack = true;
    }

    void imageButton_Click(object sender, ImageClickEventArgs e)
    {
        button_Click(sender, e);
    }

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
        There are several validators on this page:
        <ul>
            <li>There are two <code>RequiredFieldValidator</code> controls that require values in
                both text boxes.
                <li>There is a <code>CompareValidator</code> on the first text box to make sure the
                    value is an integer.
                    <li>There is also a <code>RangeValidator</code> on the first text box to make sure the
                        value is between 1 and 10.
                        <li>There is a <code>RegularExpressionValidator</code> on the second text box to make
                            sure the value is 2 digits.
                            <li>And there is a <code>Custom Validator</code>
            on the second text box to make sure the value is an even number.
        </ul>
        All of the validators have <code>EnableClientScript="false"</code>. This simulates
        the condition of running the page on a downlevel browser.</p>
    <p>
        Clicking any of the buttons should do a call back to the server where the controls
        are validated. If the page is not valid, it should show red asterisks next to the
        text boxes and also display the <code>ValidationSummory</code> control at the bottom
        of the page and in a message box.</p>
    <p>
        Once you fix the validation errors, the asterisks and the summary should disappear
        and a label should be updated on the page with the current value of the <code>IsValid</code>
        property on the server.</p>
    <p>
        This is all done without doing any post backs to the server.</p>
    <div>
        <asp:TextBox ID="textBox1" runat="server" />
        <anthem:RequiredFieldValidator ID="required1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="Enter a value into the first text box!"
            Text="*">
        </anthem:RequiredFieldValidator>
        <anthem:CompareValidator ID="compare1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="The value must be a number!"
            Operator="DataTypeCheck" Text="*" Type="Integer">
        </anthem:CompareValidator>
        <anthem:RangeValidator ID="range1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox1" EnableClientScript="false" ErrorMessage="The value must be from 1 to 10!"
            MinimumValue="1" MaximumValue="10" Text="*" Type="Integer">
        </anthem:RangeValidator>
        <br />
        <asp:TextBox ID="textbox2" runat="server" />
        <anthem:RequiredFieldValidator ID="required2" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox2" EnableClientScript="false" ErrorMessage="Enter a value into the second text box!"
            Text="*">
        </anthem:RequiredFieldValidator>
        <anthem:RegularExpressionValidator ID="regex1" runat="server" AutoUpdateAfterCallBack="true"
            ControlToValidate="textBox2" EnableClientScript="false" ErrorMessage="You must enter 2 digits!"
            Text="*" ValidationExpression="\d{2}">
        </anthem:RegularExpressionValidator>
        <anthem:CustomValidator ID="custom1" runat="server" ControlToValidate="textBox2"
            EnableClientScript="false" ErrorMessage="You must enter an even number!" OnServerValidate="ServerValidation"
            Text="*">
        </anthem:CustomValidator>
    </div>
    <br />
    <anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />
    <anthem:LinkButton ID="linkButton" runat="server" Text="Click Me!" OnClick="button_Click" />
    <anthem:ImageButton ID="imageButton" runat="server" ImageUrl="button_run.gif" OnClick="imageButton_Click" />
    <br />
    <anthem:Label ID="label" runat="server" />
    <br />
    <anthem:ValidationSummary ID="summary" runat="server" DisplayMode="BulletList" EnableCallBackScript="true"
        EnableClientScript="false" HeaderText="Found these errors:" ShowMessageBox="true"
        ShowSummary="true"></anthem:ValidationSummary>
</asp:Content>
