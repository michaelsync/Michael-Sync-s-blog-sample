<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head id="Head1" runat="server">
		<title>Untitled Page</title>
		<script type="text/javascript">
        function ClientSideFunction() {
            alert("Invoked!");
        }
		</script>
	</head>
	<body>
		<form id="form1" runat="server">
			<h2>Example</h2>
			<p>Click the button to invoke perform a call back to the server which will then 
				invoke a client-side JavaScript function here on the client after the call back 
				returns.</p>
			<anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />
		</form>
	</body>
</html>
<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("ClientSideFunction();");
    }

</script>
