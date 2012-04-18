<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PanelsAndVisibility</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>The <code>anthem:Panel</code> control isn't really much more than a fancy <code>div</code>. 
				It's mostly useful as a container for other controls (even non-Anthem controls) 
				which can be toggled on and off during a call back.</p>
			<h2>Example</h2>
			<p>Click the button to toggle the visibilty of the two panels.</p>
			<anthem:Panel id="panel1" runat="server" Visible="true" BorderStyle="Solid">
				<p>This is <code>Panel 1</code>.</p>
				<p>This panel's <code>Visible</code> property is initially set to <code>true</code> 
					(which is the default).</p>
				<p>Click the button and watch it disappear. Click it again and watch it reappear.</p>
				<p>There's another panel next to this one with its <code>Visible</code> property 
					initially set to <code>false</code>.</p>
			</anthem:Panel>
			<anthem:Panel id="panel2" runat="server" Visible="false" BorderStyle="Solid">
				<p>This is <code>Panel 2</code>.</p>
				<p>This panel's <code>Visible</code> property is initially set to <code>false</code>.</p>
			</anthem:Panel>
			<br />
			<anthem:Button id="button" runat="server" Text="Click Me!" OnClick="button_Click" />
			<script runat="server">

void button_Click(object sender, EventArgs e)
{
	panel1.Visible = !panel1.Visible;
	panel2.Visible = !panel2.Visible;

	panel1.UpdateAfterCallBack = true;
	panel2.UpdateAfterCallBack = true;
}

			</script>
			<h2>Steps</h2>
			<ol>
				<li>
					<p>The <code>anthem:Panel</code> control works just like the <code>asp:Panel</code> 
						control except that it can be modified during a call back. Add an <code>anthem:Panel</code>
						control to your page and set its <code>Visible</code> property to whatever it 
						needs to be:</p>
					<pre><code><strong>&lt;anthem:Panel id="panel1" runat="server" Visible="false"&gt;This panel is not initially visible.&lt;/anthem:Panel&gt;</strong></code></pre>
				</li>
				<li>
					<p>During any call back, you can toggle the value of the <code>Visible</code> property. 
						Be sure to set the <code>UpdateAfterCallBack</code> property to true or the 
						change won't be reflected in the client-side page:</p>
					<pre><code><strong>&lt;script runat="server"&gt;

void button_Click(object sender, EventArgs e)
{
	panel1.Visible = !panel1.Visible;
	panel1.UpdateAfterCallBack = true;
}

&lt;/script&gt;</strong></code></pre>
			</ol>
			<h2>Remarks</h2>
			<p>It's possible to use <code>anthem:Panel</code> controls to dynamically update 
				non-Anthem controls during a call back. Just put those controls inside an <code>anthem:Panel</code>
				control and set <code>UpdateAfterCallBack</code> to true on the panel. This 
				won't work if those controls try to inject any JavaScript into the page, 
				however.</p>
		</form>
	</body>
</HTML>
