<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>DataBindDuringCallBack</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Clicking the button will trigger a call back that creates some fake data, binds 
				the grid to that data, and updates the grid on the client page.</p>
			<anthem:Button id="button" runat="server" Text="Populate DataGrid" OnClick="button_Click" />
			<br />
			<anthem:DataGrid id="grid" runat="server" />
		</form>
	</body>
</html>
<script runat="server">

	void button_Click(object sender, EventArgs e)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("foo", typeof(string));
		dt.Columns.Add("bar", typeof(string));
		dt.Columns.Add("baz", typeof(string));
		dt.Rows.Add(new object[] { "foo1", "bar1", "baz1" });
		dt.Rows.Add(new object[] { "foo2", "bar2", "baz2" });
		dt.Rows.Add(new object[] { "foo3", "bar3", "baz3" });
		grid.DataSource = dt;
		grid.DataBind();
		grid.UpdateAfterCallBack = true;
	}

</script>
