<%@ Control Language="C#" ClassName="Validation" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<script runat="server">
    protected void button1_Click(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("alert('Page.IsValid=" + Page.IsValid + "');");
    }

    protected void button2_Click(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("alert('Page.IsValid=" + Page.IsValid + "');");
    }
    
    protected void button3_Click(object sender, EventArgs e)
    {
        this.panel.Visible = !this.panel.Visible;
        Anthem.Panel p = this.panel as Anthem.Panel;
        if (p != null)
            p.UpdateAfterCallBack = true;
    }
</script>

<p>There is a panel directly below the toggle button. All of the validation controls are in the panel.</p>
<anthem:Button ID="button3" runat="server" Text="Toggle Panel Visibility" CausesValidation="false" OnClick="button3_Click" />
<hr />
<anthem:Panel ID="panel" runat="server" Visible="false">
    <asp:Panel ID="ValidationGroup1" runat="server" GroupingText='Validation Group "first"'>
        <p>There are 5 validators attached to this text box. To pass you must enter the number 5, but you can play with other numbers or text to see which validators fire.</p>
        <anthem:TextBox ID="textbox1" Runat="server" ValidationGroup="first" />
        <anthem:RequiredFieldValidator ID="required1" Runat="server" ControlToValidate="textbox1" ErrorMessage="Required" Display="Dynamic" ValidationGroup="first" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
        <anthem:RangeValidator ID="range1" runat="server" ControlToValidate="textbox1" ErrorMessage="Out of Range" Display="Dynamic" MaximumValue="10" MinimumValue="0" Type="Integer" ValidationGroup="first" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
        <anthem:CompareValidator ID="CompareValidator1" runat="server" ControlToValidate="textbox1" Display="Dynamic" ErrorMessage="Comparison failed" ValidationGroup="first" ValueToCompare="5" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
        <anthem:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="textbox1" Display="Dynamic" ErrorMessage="Custom validation failed" ValidationGroup="first" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
        <anthem:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="textbox1" Display="Dynamic" ErrorMessage="Regular expression failed" ValidationExpression="\d{1}" ValidationGroup="first" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
    </asp:Panel>
    <asp:Panel ID="ValidationGroupEmpty" runat="server" GroupingText="No Validation Group">
        <p>There is only one RequiredFieldValidator attached to this text box.</p>
        <anthem:TextBox ID="textbox2" Runat="server" />
        <anthem:RequiredFieldValidator ID="required2" Runat="server" ControlToValidate="textBox2" ErrorMessage="required" Display="Dynamic" AutoUpdateAfterCallBack="true" SetFocusOnError="True" />
    </asp:Panel>
    <table border="0" cellpadding="10">
        <tr>
            <td>
		        <anthem:Button ID="button1" runat="server" Text='"first" Validation Group' ValidationGroup="first" OnClick="button1_Click" />
            </td>
            <td>
		        <anthem:Button id="button2" runat="server" Text="No Validation Group" OnClick="button2_Click" />
            </td>
        </tr>
        <tr>
            <td>
                <anthem:ValidationSummary ID="summary1" runat="server" ValidationGroup="first" AutoUpdateAfterCallBack="true" />
            </td>
            <td>
		            <anthem:ValidationSummary ID="summary2" Runat="server" AutoUpdateAfterCallBack="true" />
            </td>
        </tr>
    </table>
</anthem:Panel>
<hr />
<p>The next three buttons are plain ASP.NET Button's that cause postbacks. These are used to 
verify that Anthem creates a page that that works for both callback and postback controls.</p>
<asp:Button ID="button4" runat="server" CausesValidation="true" Text='"first" Validation Group' ValidationGroup="first" />
<asp:Button ID="button5" runat="server" CausesValidation="true" Text="No Validation Group" />
<asp:Button ID="button6" runat="server" CausesValidation="false" Text="No Validation" />
