<%@ Control Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<anthem:CheckBox id="check" runat="server" Text="Check Me!" AutoCallBack="true" OnCheckedChanged="check_CheckedChanged" />
<br />
<anthem:Label id="message" runat="server" />
<script runat="server">

void check_CheckedChanged(object sender, EventArgs e)
{
	message.Text = check.Checked ? "Checked" : "Not Checked";
	message.UpdateAfterCallBack = true;
}

</script>