<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Repeater</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Example</h2>
			<p>Here is a repeater that outputs a list of strings. Click the button to update 
				the repeater with a reversed data source.</p>
			<anthem:Repeater id="repeater" runat="server">
				<HeaderTemplate>
					<ul>
				</HeaderTemplate>
				<ItemTemplate>
					<li>
						<%# Container.DataItem %>
					</li>
				</ItemTemplate>
				<FooterTemplate></ul></FooterTemplate>
			</anthem:Repeater>
			<br />
			<anthem:Button id="sort" runat="server" Text="Sort" OnClick="sort_Click" />
		</form>
	</body>
</html>
<script runat="server">

void Page_Load()
{
	string[] data = new string[] { "a", "b", "c", "d", "e" };
	repeater.DataSource = data;
}

void sort_Click(object sender, EventArgs e)
{
	bool reversed = ViewState["reversed"] != null ? (bool)ViewState["reversed"] : false;
	if (!reversed)
	{
		string[] data = repeater.DataSource as string[];
		Array.Reverse(data);
	}
	ViewState["reversed"] = !reversed;
}

void Page_PreRender()
{
	DataBind();
	repeater.UpdateAfterCallBack = true;
}

</script>
