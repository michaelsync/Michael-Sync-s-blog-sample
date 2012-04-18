<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<script runat="server">

    void checkBox_OnCheckedChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        label.Text = checkBox.Checked ? "Checked!" : "Not checked.";
        label.UpdateAfterCallBack = true;
    }

    protected void checkboxlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        StringBuilder sb = new StringBuilder();
        sb.Append("You picked ");
        if (checkboxlist.SelectedItem == null)
            sb.Length = 0;
        else
        {
            foreach (ListItem item in checkboxlist.Items)
            {
                if (item.Selected)
                    sb.Append(item.Value + ", ");
            }
            sb.Length = sb.Length - 2;
        }
        checkboxlistlabel.Text = sb.ToString();
        checkboxlistlabel.UpdateAfterCallBack = true;
    }
</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
			<h2>Example</h2>
			<p>Checking and unchecking the check box will trigger a call back to the server 
				which will update a label indicating the state of the check box. There is an
				artificial delay in each event so you can see the TextDuringCallBack (which
				is set to display an image).</p>
			<anthem:CheckBox ID="checkBox" runat="server" 
			    AutoCallBack="true" 
			    Text="Check Me!" 
			    TextDuringCallBack="&lt;img src=&quot;tiny_red.gif&quot; border=0 /&gt;"
			    OnCheckedChanged="checkBox_OnCheckedChanged" />
			<br />
			<anthem:Label ID="label" runat="server" />
			<p>Here is a CheckBoxList</p>
			<anthem:CheckBoxList ID="checkboxlist" runat="server" 
			    AutoCallBack="true"
			    AutoPostBack="true" 
			    OnSelectedIndexChanged="checkboxlist_SelectedIndexChanged" 
			    RepeatDirection="Horizontal" 
			    RepeatLayout="Flow"
			    TextDuringCallBack="wait...">
			    <Items>
                    <asp:ListItem>One</asp:ListItem>
                    <asp:ListItem>Two</asp:ListItem>
                    <asp:ListItem>Three</asp:ListItem>
                </Items>
            </anthem:CheckBoxList>
            <br />
            <anthem:Label ID="checkboxlistlabel" runat="server" />
</asp:Content>

