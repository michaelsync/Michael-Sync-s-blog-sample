<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
    <title>ListBox</title>
  </HEAD>
  <body>
    <form id="form1" runat="server">
      <h2>Example</h2>
      <p>Selecting different items in this list box triggers a call back to the server 
        which updates a label letting you know what you selected.</p>
      <anthem:ListBox id="listBox" runat="server" AutoCallBack="true" OnSelectedIndexChanged="listBox_SelectedIndexChanged"
        SelectionMode="Multiple">
				<asp:ListItem>foo</asp:ListItem>
				<asp:ListItem>bar</asp:ListItem>
				<asp:ListItem>baz</asp:ListItem>
      </anthem:ListBox>
      <br>
      <anthem:Label ID="label" runat="server" />
    </form>
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
  </body>
</HTML>
