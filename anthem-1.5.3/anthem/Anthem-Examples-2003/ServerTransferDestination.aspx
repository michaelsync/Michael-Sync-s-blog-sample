<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ServerTransferDestination</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Look at the address bar in your browser. It should say ServerTransferExample.aspx 
				if that's where you started and yet this is really 
				ServerTransferDestination.aspx.</p>
			<p>Clicking the following button should call back to ServerTransferDestination.aspx 
				and not ServerTransferTest.aspx.</p>
			<anthem:Button id="button" runat="server" Text="Get Request.Path" OnClick="button_Click" />
			<anthem:Label id="label" runat="server" Text="?" />
		</form>
	</body>
</HTML>

<script runat="server">

	private void button_Click(object sender, System.EventArgs e)
	{
		label.Text = Request.Path;
		label.UpdateAfterCallBack = true;
	}

</script>
