<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register TagPrefix="cc1" Namespace="Anthem.Examples" Assembly="__code" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>Description</h2>
			<p>Anthem supports invoking methods on controls as well as on pages.</p>
			<p>In this example, you'll be adding two instances of the same custom control to 
				the page. Those controls will be invoking methods on themselves when they're 
				clicked.</p>
			<h2>Example</h2>
			<cc1:CustomControl id="CustomControl1" runat="server" />
			<br />
			<cc1:CustomControl id="CustomControl2" runat="server" />
			<h2>Steps</h2>
			<ol>
				<li>
					<p>First, implement your custom control by creating a new class that derives from <code>
							System.Web.UI.Control</code>:</p>
					<pre><code><strong>using System;
using System.Web.UI;

namespace Anthem.Examples
{
	public class CustomControl : Control
	{
	}
}</strong></code></pre></li>
				<li>
					<p>Make the control render itself by overriding its <code>Render</code> method. Do 
						something simple like output the current time in a <code>span</code> element:</p>
					<pre><code><strong>protected override void Render(HtmlTextWriter writer)
{
	writer.RenderBeginTag("span");
	writer.Write(DateTime.Now);
	writer.RenderEndTag();
}</strong></code></pre></li>
				<li>
					<p>When the <code>span</code> gets clicked, we want the time to update itself by 
						invoking a method on the control. To do this, add an <code>id</code> and an <code>onclick</code>
						to the <code>span</code> element. The <code>onclick</code> attribute will 
						invoke the JavaScript function that does all the work by passing in a reference 
						to the <code>span</code> element that got clicked:</p>
					<pre><code>protected override void Render(HtmlTextWriter writer)
{
	<strong>writer.AddAttribute("id", ClientID);
	writer.AddAttribute("onclick", "GetNow(this)");</strong>
	writer.RenderBeginTag("span");
	writer.Write(DateTime.Now);
	writer.RenderEndTag();
}</code></pre></li>
				<li>
					<p>The <code>GetNow</code> function doesn't normally exist on our pages so we have 
						to make sure it gets added to all pages that host this kind of control. 
						Override the control's <code>OnLoad</code> method to inject that script into 
						the page. While you're in <code>OnLoad</code>, be sure to register the control 
						with <code>Anthem.Manager</code>:</p>
					<pre><code><strong>protected override void OnLoad(EventArgs e)
{
	base.OnLoad(e);

	Anthem.Manager.Register(this);

	Page.RegisterClientScriptBlock(
		typeof(CustomControl).FullName,
		@"&lt;script language='javascript' type='text/javascript'&gt;
function GetNow(control) {
	Anthem_InvokeControlMethod(
		control.id,
		'GetNow',
		[],
		function (result) {
			control.innerHTML = result.value;
		}
	);
}
&lt;/script&gt;");
}</strong></code></pre>
				</li>
				<li>
					<p>Looking at that client-side function, we can see that it's trying to invoke a 
						method called <code>GetNow</code> on the control. Add that to the custom 
						control class and make sure that it's public and has the <code>Anthem.Method</code>
						attribute on it:</p>
					<pre><code><strong>[Anthem.Method]
public DateTime GetNow()
{
	return DateTime.Now;
}</strong></code></pre>
				</li>
				<li>
					<p>Unlike <code>Anthem_InvokePageMethod</code>, the first argument to <code>Anthem_InvokeControlMethod</code>
						is the ID of the control you want to invoke the method on because there could 
						be more than one instance of a control on a page. Remember how we passed in <code>this</code>
						to the <code>GetNow</code> function? That argument is called <code>control</code>
						inside <code>GetNow</code> and is used to grab the ID of the control we need to 
						invoke the method on.</p>
					<pre><code>function GetNow(<strong>control</strong>) {
	Anthem_InvokeControlMethod(
		<strong>control.id</strong>,
		'GetNow',
		[],
		function (result) {
			control.innerHTML = result.value;
		}
	);
}</code></pre>
				</li>
				<li>
					<p>The second argument to <code>Anthem_InvokeControlMethod</code> is the name of 
						the method on the control you want to invoke:</p>
					<pre><code>function GetNow(control) {
	Anthem_InvokeControlMethod(
		control.id,
		<strong>'GetNow',</strong>
		[],
		function (result) {
			control.innerHTML = result.value;
		}
	);
}</code></pre>
				</li>
				<li>
					<p>The third argument is an array of parameters to pass in to the method. The <code>GetNow</code>
						method on the control takes in no parameters so we're passing in an empty 
						array:</p>
					<pre><code>function GetNow(control) {
	Anthem_InvokeControlMethod(
		control.id,
		'GetNow',
		<strong>[]</strong>,
		function (result) {
			control.innerHTML = result.value;
		}
	);
}</code></pre>
				</li>
				<li>
					<p>Finally, <code>Anthem_InvokeControlMethod</code> takes in a function to call 
						back when the server-side call back "returns":</p>
					<pre><code>function GetNow(control) {
	Anthem_InvokeControlMethod(
		control.id,
		'GetNow',
		[],
		<strong>function (result) {
			control.innerHTML = result.value;
		}</strong>
	);
}</code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>Can you figure out why the format of the <code>DateTime</code> changes after you 
				click one of the custom controls for the first time?</p>
</asp:Content>
