<%@ Page Language="C#" %>
<%@ Register TagPrefix="uc1" TagName="UserControl" Src="UserControl.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>InvokeUserControlMethod</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p><code>Anthem_InvokeControlMethod</code> can be used to invoke methods on user 
				controls, too.</p>
			<p>In this example, we create a user control and add it to the page twice. The user 
				control calls back to itself using <code>Anthem_InvokeControlMethod</code>.</p>
			<h2>Example</h2>
			<uc1:UserControl id="UserControl1" runat="server"></uc1:UserControl>
			<uc1:UserControl id="Usercontrol2" runat="server"></uc1:UserControl>
			<h2>Steps</h2>
			<ol>
				<li>
					<p>Create a user control (as a .ascx file) and add some HTML to it, a button, and a 
						span element. The button will trigger a call back to the user control on the 
						server. The span will have its contents updated so we can see that a change did 
						occur from clicking the button:</p>
					<pre><code><strong>&lt;%@ Control Language="C#" %&gt;
&lt;h3&gt;UserControl&lt;/h3&gt;
&lt;button onclick="UpdateUserControl()" type="button"&gt;Update&lt;/button&gt;
&lt;span id="result"&gt;&lt;/span&gt;</strong></code></pre>
				<li>
					<p>The <code>UpdateUserControl</code> function doesn't exist so we have to inject 
						into the page hosting the control. We can do that when the user control fires 
						its <code>Load</code> event. While we're in <code>Page_Load</code>, let's 
						register the control with <code>Anthem.Manager</code>:</p>
					<pre><code><strong>&lt;script runat="server"&gt;
	void Page_Load()
	{
		Anthem.Manager.Register(this);

		Page.RegisterClientScriptBlock(
			GetType().FullName,
			@"&lt;script language='javascript' type='text/javascript'&gt;
function UpdateUserControl(userControlID, spanID) {
	Anthem_InvokeControlMethod(
		userControlID,
		'GetNow',
		[],
		function(result) {
			document.getElementById(spanID).innerHTML = result.value;
		}
	);
}
&lt;" + "/script&gt;"
		);
	}
&lt;/script&gt;</strong></code></pre>
				<li>
					<p>Take a closer look at that <code>UpdateUserControl</code> function. It's 
						requiring two IDs to be passed in as arguments. The first is the ID of the user 
						control we want to invoke the method on and the second is the ID of the span we 
						want to update:</p>
					<pre><code>function UpdateUserControl(<strong>userControlID, spanID</strong>) {
	Anthem_InvokeControlMethod(
		<strong>userControlID</strong>,
		'GetNow',
		[],
		function(result) {
			document.getElementById(<strong>spanID</strong>).innerHTML = result.value;
		}
	);
}</code></pre>
				<li>
					<p>In order to get those two IDs into the <code>UpdateUserControl</code> function, 
						we need to modify the <code>button</code> element with the <code>onclick</code> 
						attribute to pass them in. Getting the user control's ID is easy since we can 
						use the <code>ClientID</code> property:</p>
					<pre><code>&lt;button onclick="UpdateUserControl(<strong>'&lt;%= ClientID %&gt;'</strong>)" type="button"&gt;Update&lt;/button&gt;</code></pre>
				<li>
					<p>We want something like <code>ClientID</code> on the span element, too. To get 
						that, we can make the span element a server-side control:</p>
					<pre><code>&lt;span id="result" <strong>runat="server"</strong>&gt;&lt;/span&gt;</code></pre>
				<li>
					<p>Adding that attribute to the <code>span</code> element gives our user control a 
						field called <code>result</code>. That field has a <code>ClientID</code> property 
						which we can access inside the <code>button</code> element's <code>onclick</code>
						attribute:</p>
					<pre><code> &lt;button onclick="UpdateUserControl('&lt;%= ClientID %&gt;', <strong>'&lt;%= result.ClientID %&gt;'</strong>)" type="button"&gt;Update&lt;/button&gt;</code></pre>
				<li>
					<p>The only thing we're missing is the <code>GetNow</code> method on the user 
						control:</p>
					<pre><code><strong>[Anthem.Method]
public DateTime GetNow()
{
	return DateTime.Now;
}</strong></code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>Be careful with your quotes! Look closely at the steps that added &lt;%= %&gt; 
				blocks of code to the <code>UpdateUserControl</code> function call to see 
				what's quoted and what's being evaluated.</p>
		</form>
	</body>
</HTML>
