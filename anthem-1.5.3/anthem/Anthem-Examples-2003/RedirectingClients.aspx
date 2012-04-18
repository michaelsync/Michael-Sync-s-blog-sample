<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<script runat="server">
	void button1_Click(object sender, EventArgs e)
	{
        // Note that you can add multiple scripts.
		Anthem.Manager.AddScriptForClientSideEval("alert('Bye!')");
		Anthem.Manager.AddScriptForClientSideEval("window.location = 'http://anthem-dot-net.sourceforge.net/';");
	}
    void button2_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://anthem-dot-net.sourceforge.net/");
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>RedirectingClients</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>
                There are two ways to 
				redirect clients to a new page. 1) add javascript to the callback
                response to change the window location, or 2) use the ASP.NET Response.Redirect()
                method. If you use Response.Redirect(), Anthem will catch it and convert it into
                javascript that changes the window location.</p>
			<h2>Example</h2>
			<anthem:Button id="button1" runat="server" Text="Redirect Me Using Javascript!" OnClick="button1_Click" /><br />
			<anthem:Button id="button2" runat="server" Text="Redirect Me Using Response.Redirect()!" OnClick="button2_Click" />
            <h2>
                Steps</h2>
			<ol>
				<li>
					<p>Add an <code>anthem:Button</code> control to the page with an event handler 
						specified in its <code>OnClick</code> attribute:</p>
					<pre>
<code><strong>&lt;anthem:Button id="button1" runat="server" Text="Redirect Me!" OnClick="button1_Click"
    /&gt;<br />
    &lt;anthem:Button id="button2" runat="server" Text="Redirect Me!" OnClick="button2_Click" /&gt;</strong></code></pre>
				<li>
					<p>Implement the event handler so that it calls <code>Anthem.Manager.AddScriptForClientSideEval</code>
                        or <code>Response.Redirect</code>:</p>
					<pre><code><strong>void button1_Click(object sender, EventArgs e)
{
	Anthem.Manager.AddScriptForClientSideEval("alert('Bye!')");
	Anthem.Manager.AddScriptForClientSideEval("window.location = 'http://anthem-dot-net.sourceforge.net/';");
}</strong></code>
<code><strong>void button2_Click(object sender, EventArgs e)
{
	Response.Redirect("http://anthem-dot-net.sourceforge.net");
}</strong></code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>You can add any JavaScript you want to via the <code>AddScriptForClientSideEval</code>
				method. Each script gets evaluated in the order it was added on the server once 
				it gets returned to the client.</p>
		</form>
	</body>
</html>
