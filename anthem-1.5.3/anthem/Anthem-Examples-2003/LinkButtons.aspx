<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LinkButtons</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h1>Description</h1>
			<p>The <code>anthem:LinkButton</code> control works just like the <code>anthem:Button</code>
				control.</p>
			<h2>Example</h2>
			<p>Click the button to see its text change while it waits for the call back to 
				return. The event handler on the server is purposely sleeping for two seconds 
				to make this obvious.</p>
			<anthem:LinkButton id="linkButton" runat="server" Text="Click Me!" TextDuringCallBack="Working..."
				OnClick="linkButton_Click" />
			<script runat="server">
				void linkButton_Click(object sender, EventArgs e)
				{
					System.Threading.Thread.Sleep(2000);
				}
			</script>
			<h2>Remarks</h2>
			<p>Its not easy to "disable" a link. So don't click the link while it's waiting for 
				a call back to return or you'll confuse yourself. Support for automatically 
				disabling controls like these will come in a later version of Anthem.</p>
		</form>
	</body>
</HTML>
