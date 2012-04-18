<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void list1_SelectedIndexChanged(object sender, EventArgs e)
    {
        message.Text = string.Format("You chose \"{0}\"!", list1.SelectedValue);
        message.UpdateAfterCallBack = true;
    }

    void list2_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        message.Text = string.Format("You chose \"{0}\"!", list2.SelectedValue);
        message.UpdateAfterCallBack = true;
    }

    protected void button_CheckedChanged(object sender, EventArgs e)
    {
        Anthem.RadioButton button = (Anthem.RadioButton)sender;
        message.Text = string.Format("You chose \"{0}\"!", button.Text);
        message.UpdateAfterCallBack = true;
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Example</h2>
    <p>
        Selecting different items in this radio button list triggers a call back to the
        server which updates a label letting you know what you selected.</p>
    <h3>
        RadioButtons</h3>
    <p>
        <anthem:RadioButton ID="button1" runat="server" AutoCallBack="true" GroupName="Test"
            Text="One" OnCheckedChanged="button_CheckedChanged" />
        <anthem:RadioButton ID="button2" runat="server" AutoCallBack="true" GroupName="Test"
            Text="Two" OnCheckedChanged="button_CheckedChanged" />
        <anthem:RadioButton ID="button3" runat="server" AutoCallBack="true" GroupName="Test"
            Text="Three" OnCheckedChanged="button_CheckedChanged" />
    </p>
    <h3>
        Fast RadioButtonList</h3>
    <p>
        This one is fast (no delay on server).<br />
        <anthem:RadioButtonList ID="list1" runat="server" AutoCallBack="true" OnSelectedIndexChanged="list1_SelectedIndexChanged"
            RepeatDirection="Horizontal" RepeatLayout="Flow">
            <Items>
                <asp:ListItem>one</asp:ListItem>
                <asp:ListItem>two</asp:ListItem>
                <asp:ListItem>three</asp:ListItem>
            </Items>
        </anthem:RadioButtonList>
    </p>
    <h3>
        Slow RadioButtonList</h3>
    <p>
        This one is artifically show (delayed on server), so it displays text while waiting.<br />
        <anthem:RadioButtonList ID="list2" runat="server" AutoCallBack="true" OnSelectedIndexChanged="list2_SelectedIndexChanged"
            TextDuringCallBack="working..." RepeatDirection="Horizontal" RepeatLayout="Flow">
            <Items>
                <asp:ListItem>one</asp:ListItem>
                <asp:ListItem>two</asp:ListItem>
                <asp:ListItem>three</asp:ListItem>
            </Items>
        </anthem:RadioButtonList>
    </p>
    <p>
        <anthem:Label ID="message" runat="server" />
    </p>
</asp:Content>
