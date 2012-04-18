<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>UnhandledExceptions</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Click the button to throw an unhandled exception on the server. You should see a 
				message box appear telling you there was a <code>NullReferenceException</code> on 
				the server.</p>
			<anthem:Button id="button" runat="server" Text="Click Me!" OnClick="button_Click" />
			<script type="text/javascript">
				function Anthem_Error(result) {
					alert('Anthem_Error was invoked with the following error message: ' + result.error);
				}
			</script>
			<p>The message box appeared because of the custom <code>Anthem_Error</code> function 
				defined on this page (view source to see it). Whenever attempting to do a call 
				back returns an error, <code>Anthem_Error</code> gets invoked, if it exists.</p>
		</form>
	</body>
</html>
<script runat="server">

	void button_Click(object sender, EventArgs e)
	{
		object o = null;
		o.ToString();
	}

</script>
