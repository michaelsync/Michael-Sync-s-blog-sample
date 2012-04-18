<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>RedirectingClientsOnErrors</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Click the button to trigger an unhandled exception on the server. This should 
				redirect you to the Anthem home page.</p>
			<anthem:Button id="button" runat="server" Text="Click Me!" OnClick="button_Click" />
			<p>You could also do this with the Anthem_Error function but doing it in Page_Error 
				lets you do server-side processing like logging the error.</p>
		</form>
	</body>
</html>
<script runat="server">

	void button_Click(object sender, EventArgs e)
	{
		object o = null;
		o.ToString();
	}

	void Page_Error()
	{
		Anthem.Manager.AddScriptForClientSideEval("window.location = 'http://anthem-dot-net.sf.net/'");
	}

</script>
