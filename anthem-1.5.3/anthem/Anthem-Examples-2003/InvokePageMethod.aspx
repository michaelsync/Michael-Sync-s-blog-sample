<%@ Page Language="C#" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>InvokePageMethod</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>Anthem allows you to invoke methods on your server-side pages via client-side 
				JavaScript.</p>
			<p>In this example, you'll invoke a simple <code>Add</code> method defined on your 
				server-side page from the client.</p>
			<h2>Example</h2>
			<script runat="server">
				void Page_Load()
				{
					Anthem.Manager.Register(this);
				}

				[Anthem.Method]
				public int Add(int a, int b)
				{
					return a + b;
				}
			</script>
			<input id="a" size="3" value="1"> <input id="b" size="3" value="2"> <button onclick="DoAdd(); return false;" type="button">
				Add</button> <input id="c" size="6">
			<script language="javascript" type="text/javascript">
				function DoAdd() {
					Anthem_InvokePageMethod(
						'Add',
						[document.getElementById('a').value, document.getElementById('b').value],
						function(result) {
							document.getElementById('c').value = result.value;
						}
					);
				}
			</script>
			<h2>Steps</h2>
			<ol>
				<li>
					<p>Add a public method called <code>Add</code> to your page so that it takes in two 
						integers and returns their sum:</p>
					<pre><code><strong>public int Add(int a, int b)
{
    return a + b;
}</strong></code></pre>
				<li>
					<p>Apply the <code>Anthem.Method</code> attribute to the <code>Add</code> method. 
						Without this, it can't be invoked from clients:</p>
					<pre><code><strong>[Anthem.Method]</strong>
public int Add(int a, int b)
{
    return a + b;
}</code></pre>
				<li>
					<p>Register the page with the Anthem manager when the page fires its <code>Load</code>
						event:</p>
					<pre><code><strong>void Page_Load()
{
	Anthem.Manager.Register(this);
}</strong></code></pre>
				<li>
					<p>Add three input controls and a button to trigger the call back to your page:</p>
					<pre><code><strong>&lt;input id="a" size="3" value="1"&gt;
&lt;input id="b" size="3" value="2"&gt;
&lt;button onclick="DoAdd(); return false;" type="button"&gt;Add&lt;/button&gt;
&lt;input id="c" size="6"&gt;</strong></code></pre>
				<li>
					<p>The button is invoking a client-side function called <code>DoAdd</code>. That 
						function needs to be defined on the page so that it invokes the server-side <code>Add</code>
						method:</p>
					<pre><code><strong>&lt;script language="javascript" type="text/javascript"&gt;
    function DoAdd() {
        Anthem_InvokePageMethod(
            'Add',
            [document.getElementById('a').value, document.getElementById('b').value],
            function(result) {
                document.getElementById('c').value = result.value;
            }
        );
    }
&lt;/script&gt;</strong></code></pre>
				<li>
					<p>The first argument to <code>Anthem_InvokePageMethod</code> needs to be the name 
						of the method you want to invoke:</p>
					<pre><code>Anthem_InvokePageMethod(
    <strong>'Add',</strong>
    [document.getElementById('a').value, document.getElementById('b').value],
    function(result) {
        document.getElementById('c').value = result.value;
    }
);</code></pre>
				<li>
					<p>The second argument is the array of parameters for that method:</p>
					<pre><code>Anthem_InvokePageMethod(
    'Add',
    <strong>[document.getElementById('a').value, document.getElementById('b').value],</strong>
    function(result) {
        document.getElementById('c').value = result.value;
    }
);</code></pre>
				<li>
					<p>The third argument is a function that will get invoked here on the client when 
						the server-side call back completes:</p>
					<pre><code>Anthem_InvokePageMethod(
    'Add',
    [document.getElementById('a').value, document.getElementById('b').value,
    <strong>function(result) {
        document.getElementById('c').value = result.value;
    }</strong>
);</code></pre>
				<li>
					<p>The argument to the client-side call back function is a result object. It has a <code>
							value</code> and an <code>error</code> property. If an error occurred on 
						the server, <code>value</code> will be null and <code>error</code> won't be:</p>
					<pre><code>Anthem_InvokePageMethod(
    'Add',
    [document.getElementById('a').value, document.getElementById('b').value,
    function(<strong>result</strong>) {
        document.getElementById('c').value = <strong>result.value</strong>;
    }
);</code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>Don't forget to make your method public and put the <code>Anthem.Method</code> attribute 
				on it!</p>
			<p>The conversion from strings to the integers (or whatever the type of the 
				parameters is on the server) happens on the server. The types of parameters 
				that are supported is limited to the common "primitive" types (like strings, 
				integers, doubles, and single-dimensional arrays of those types).</p>
			<p>The supported return value types is slightly richer with support for <code>DataSet</code>-related 
				objects and limited automatic support for other custom types.</p>
			<p>The client-side call back function can be defined anonymously as the above 
				example demonstrates.</p>
		</form>
	</body>
</HTML>
