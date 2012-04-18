<%@ Page Language="C#" ResponseEncoding="Shift-JIS" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>ResponseEncoding</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>Anthem sends all its requests and generates all its responses using UTF-8 which 
				means all Unicode text should be supported.</p>
			<h2>Example</h2>
			<p>This page has its <code>ResponseEncoding</code> set to Shift-JIS to force you to 
				install the Japanese Language Pack if you don't already have it installed.</p>
			<p>Click the button to update the label next to it with some Japanese text.</p>
			<anthem:Button id="button" runat="server" Text="Get Text" OnClick="button_Click" />
			<anthem:Label id="label" runat="server" />
		</form>
	</body>
</html>
<script runat="server">

	void button_Click(object sender, EventArgs e)
	{
		label.Text = "\u541B\u306E\u9774\u3068\u672A\u6765";
		label.UpdateAfterCallBack = true;
	}

</script>
