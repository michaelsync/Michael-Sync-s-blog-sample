<%@ Page Language="C#" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>MissingMethod</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>Starting with Anthem 1.1.0, trying to invoke methods that don't exist on the 
				page on the server will return an error of "METHODNOTFOUND".</p>
			<p>This is a developer error. It probably means one or more of the following:</p>
			<ul>
				<li>You forgot to put the <code>[Anthem.Method]</code> attribute on your method</li>
				<li>You forgot to make your method public</li>
				<li>You spelled the name wrong either on the server or in the call to <code>Anthem_InvokePageMethod</code></li>
			</ul>
			<h2>Example</h2>
			<p>Click the button to try to invoke a method that doesn't exist on the server.</p>
			<button onclick="Anthem_InvokePageMethod('MissingMethodName', null, function(result) { alert(result.error); }); return false">Invoke missing method</button>
		</form>
	</body>
</html>
<script runat="server">

void Page_Load()
{
	Anthem.Manager.Register(this);
}

</script>
