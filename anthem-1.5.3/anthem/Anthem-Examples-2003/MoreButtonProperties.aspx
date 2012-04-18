<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>MoreButtonProperties</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>The <code>anthem:Button</code> control has some properties that are not found on <code>
					asp:Button</code>. This example shows how to use them.</p>
			<p>After clicking a button, you might want to give the user some feedback to let 
				him or her know that it might take a while to handle that button click. To do 
				that, you can change the text on the button and also disable it for the 
				duration of the call back. There are two special properties on the <code>anthem:Button</code>
				control that allow you to do this.</p>
			<p>Additionally, you might want to disable other controls on the page or do other 
				work. The <code>anthem:Button</code> control will execute functions before and 
				after the call back. It's even possible to cancel call backs using these 
				functions.</p>
			<h2>Example</h2>
			<p>Click the first button to see get prompted to confirm making the call back. The 
				first button's text will change and will be disabled during the call back. The 
				other button will also disable itself while the call back is in progress.</p>
			<anthem:Button id="button1" runat="server" Text="Click Me!" TextDuringCallBack="Working..." EnabledDuringCallBack="false"
				PreCallBackFunction="button1_PreCallBack" PostCallBackFunction="button1_PostCallBack" CallBackCancelledFunction="button1_CallBackCancelled"
				OnClick="button1_Click" />
			<anthem:Button id="button2" runat="server" Text="Another Button" />
			<script language="javascript" type="text/javascript">

				function button1_PreCallBack(button) {
					if (!confirm('Are you sure you want to perform this call back?')) {
						return false;
					}
					document.getElementById('button2').disabled = true;
				}
	
				function button1_PostCallBack(button) {
					document.getElementById('button2').disabled = false;
				}
				
				function button1_CallBackCancelled(button) {
					alert('Your call back was cancelled!');
				}

			</script>
			<script runat="server">

				void button1_Click(object sender, EventArgs e)
				{
					System.Threading.Thread.Sleep(2000);
				}

			</script>
			<h2>Steps</h2>
			<ol>
				<li>
					<p>Add a <code>anthem:Button</code> control to your page:</p>
					<pre><code><strong>&lt;anthem:Button id="button1" runat="server" Text="Click Me!" /&gt;</strong></code></pre>
				<li>
					<p>Add a second <code>anthem:Button</code> control to your page. We'll disable this 
						button when the first button is clicked:</p>
					<pre><code><strong>&lt;anthem:Button id="button2" runat="server" Text="Another Button" /&gt;</strong></code></pre>
				<li>
					<p>Set the <code>TextDuringCallBack</code> and <code>EnabledDuringCallBack</code> properties 
						to the desired values:</p>
					<pre><code>&lt;anthem:Button id="button1" runat="server" Text="Click Me!"
	<strong>TextDuringCallBack="Working..." EnabledDuringCallBack="false"</strong> /&gt;</code></pre>
				<li>
					<p>To disable the second button when the first button gets clicked, add <code>PreCallBackFunction</code>
						and <code>PostCallBackFunction</code> properties to the first button's tag:</p>
					<pre><code>&lt;anthem:Button id="button1" runat="server" Text="Click Me!"
	TextDuringCallBack="Working..." EnabledDuringCallBack="false"
	<strong>PreCallBackFunction="button1_PreCallBack"
	PostCallBackFunction="button1_PostCallBack"</strong> /&gt;</code></pre>
				<li>
					<p>Those properties are the names of client-side JavaScript functions that get 
						invoked before and after the button calls back to the server when it gets 
						clicked. Add a client-side script block (with no <code>runat="server"</code> attribute) 
						to the page containing those functions:</p>
					<pre><code><strong>&lt;script language="javascript" type="text/javascript"&gt;

function button1_PreCallBack(button) {
	document.getElementById('button2').disabled = true;
}

function button1_PostCallBack(button) {
	document.getElementById('button2').disabled = false;
}

&lt;/script&gt;</strong></code></pre>
				<li>
					<p>To cancel call backs, return <code>false</code> from the <code>PreCallBackFunction</code>:</p>
					<pre><code><strong>function button1_PreCallBack(button) {
	if (!confirm('Are you sure you want to perform this call back?')) {
		return false;
	}
	document.getElementById('button2').disabled = true;
}</strong></code></pre>
				<li>
					<p>When a call back gets cancelled, the function specified with the <code>CallBackCancelledFunction</code>
						property gets invoked. Add that property to your button:</p>
					<pre><code>&lt;anthem:Button id="button1" runat="server" Text="Click Me!"
	TextDuringCallBack="Working..." EnabledDuringCallBack="false"
	PreCallBackFunction="button1_PreCallBack"
	PostCallBackFunction="button1_PostCallBack" 
	<strong>CallBackCancelledFunction="button1_CallBackCancelled"</strong> /&gt;</code></pre>
				<li>
					<p>Finally, add the <code>button1_CallBackCancelled</code> function (as client-side 
						JavaScript):</p>
					<pre><code><strong>function button1_CallBackCancelled(button) {
	alert('Your call back was cancelled!');
}</strong></code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>You can do anything you want in the functions specified with the <code>PreCallBackFunction</code>
				and <code>PostCallBackFunction</code> properties. The <code>TextDuringCallBack</code>
				and <code>EnabledDuringCallBack</code> features could have been done with the 
				functions but those will probably be so common, they have first-class support.</p>
			<p>Don't confuse the <code>PreCallBackFunction</code> and <code>PostCallBackFunction</code>
				properties with events. They're just JavaScript strings that get evaluated on 
				the client to some sort of callable object like a function. The property names 
				are suffixed with "Function" to make them look different than events much like 
				the <code>CustomValidator</code> control's <code>ClientValidationFunction</code>
				property.</p>
			<p>Note that the button that caused the call back is passed in to the client-side 
				functions. This way you could have multiple buttons on the page with the same 
				values for those properties.</p>
		</form>
	</body>
</HTML>
