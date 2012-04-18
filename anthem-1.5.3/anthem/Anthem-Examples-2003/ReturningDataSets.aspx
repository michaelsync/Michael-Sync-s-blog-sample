<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>ReturningDataSets</title>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>Invoking methods on pages and controls has special support for returning <code>DataSet</code>,
				<code>DataTable</code>, and <code>DataRow</code> objects.</p>
			<p>In this example, a <code>DataSet</code> is returned from a method on the page 
				and the rows in its only table is added to a drop down list.</p>
			<h2>Example</h2>
			<script runat="server">
				void Page_Load()
				{
					Anthem.Manager.Register(this);
				}
				
				[Anthem.Method]
				public DataSet GetData()
				{
					DataSet ds = new DataSet();
					DataTable dt = new DataTable("MyTable");
					ds.Tables.Add(dt);
					dt.Columns.Add("id", typeof(int));
					dt.Columns.Add("name", typeof(string));
					dt.Rows.Add(new object[] { 1, "foo" });
					dt.Rows.Add(new object[] { 2, "bar" });
					dt.Rows.Add(new object[] { 3, "baz" });
					return ds;
				}
			</script>
			<select id="data">
			</select>
			<button id="button" onclick="GetData(); return false;" type="button">Click Me!</button>
			<script language="javascript" type="text/javascript">
				function GetData() {
					Anthem_InvokePageMethod(
						'GetData',
						[],
						function (result) {
							var table = result.value.Tables['MyTable'];
							var select = document.getElementById('data');
							for (var i = 0; i < table.Rows.length; ++i) {
								var row = table.Rows[i];
								var option = document.createElement('option');
								option.value = row['id'];
								option.text = row['name'];
								if (window.navigator.appName.toLowerCase().indexOf("microsoft") > -1) {
									select.add(option);
								} else {
									select.add(option, null);
								}
							}
						}
					);
				}
			</script>
			<h2>Steps</h2>
			<ol>
				<li>
					<p>Add a method to your page that returns data. Don't forget to make it public and 
						add the <code>Anthem.Method</code> attribute to it:</p>
					<pre><code><strong>[Anthem.Method]
public DataSet GetData()
{
	DataSet ds = new DataSet();
	DataTable dt = new DataTable("MyTable");
	ds.Tables.Add(dt);
	dt.Columns.Add("id", typeof(int));
	dt.Columns.Add("name", typeof(string));
	dt.Rows.Add(new object[] { 1, "foo" });
	dt.Rows.Add(new object[] { 2, "bar" });
	dt.Rows.Add(new object[] { 3, "baz" });
	return ds;
}</strong></code></pre>
				<li>
					<p>Add a client-side <code>select</code> and <code>button</code> to your page:</p>
					<pre><code><strong>&lt;select id="data"&gt;
&lt;/select&gt;
&lt;button id="button" onclick="GetData(); return false;"&gt;Click Me!&lt;/button&gt;</strong></code></pre>
				<li>
					<p>The <code>GetData</code> function is a client-side, JavaScript function that 
						invokes the method on the page. Add that function in a client-side script 
						block:</p>
					<pre><code><strong>&lt;script language="javascript" type="text/javascript"&gt;
    function GetData() {
        Anthem_InvokePageMethod(
            'GetData',
            [],
            function (result) {
                var table = result.value.Tables['MyTable'];
                var select = document.getElementById('data');
                for (var i = 0; i &lt; table.Rows.length; ++i) {
                    var row = table.Rows[i];
                    var option = document.createElement('option');
                    option.value = row['id'];
                    option.text = row['name'];
                    if (window.navigator.appName.toLowerCase().indexOf("microsoft") &gt; -1) {
                        select.add(option);
                    } else {
                        select.add(option, null);
                    }
                }
            }
        );
    }
&lt;/script&gt;</strong></code></pre>
				<li>
					<p>The result's <code>value</code> property is a <code>DataSet</code>. Notice how 
						it has a <code>Tables</code> property and how the tables in that keyed 
						collection have a <code>Rows</code> property. The row objects are keyed by column name:</p>
					<pre><code>&lt;script language="javascript" type="text/javascript"&gt;
    function GetData() {
        Anthem_InvokePageMethod(
            'GetData',
            [],
            function (result) {
                <strong>var table = result.value.Tables['MyTable'];</strong>
                var select = document.getElementById('data');
                for (var i = 0; i &lt; <strong>table.Rows.length</strong>; ++i) {
                    var row = <strong>table.Rows[i]</strong>;
                    var option = document.createElement('option');
                    option.value = <strong>row['id']</strong>;
                    option.text = <strong>row['name']</strong>;
                    if (window.navigator.appName.toLowerCase().indexOf("microsoft") &gt; -1) {
                        select.add(option);
                    } else {
                        select.add(option, null);
                    }
                }
            }
        );
    }
&lt;/script&gt;</code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>The "API" for <code>DataSet</code>, <code>DataTable</code>, and <code>DataRow</code>
				on the client tries to emulate the API on the server. It's not perfect, but if 
				you follow this example, you should be able to get most of what you need done.</p>
		</form>
	</body>
</HTML>
