<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>RepeatersAndViewState</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Example</h2>
			<p>Here are two repeaters that output a list of strings. Click the button to 
				trigger a call back to update both repeaters. The first one has 
				EnableViewState="true". The second has EnableViewState="false". Data binding is 
				only done during the initial GET request.</p>
			<h3>Repeater 1</h3>
			<anthem:Repeater id="repeater1" runat="server" EnableViewState="true">
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
			<h3>Repeater 2</h3>
			<anthem:Repeater id="repeater2" runat="server" EnableViewState="false">
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
			<anthem:Button id="update" runat="server" Text="Update Repeaters" OnClick="update_Click" />
			<p>The moral of this story is that you have to repeatedly bind your data to your 
				repeaters during call backs if you try to update them with their view state 
				disabled.</p>
		</form>
	</body>
</html>
<script runat="server">

void Page_Load()
{
	if (!IsPostBack)
	{
		string[] data = new string[] { "a", "b", "c", "d", "e" };
		repeater1.DataSource = data;
		repeater2.DataSource = data;
		DataBind();
	}
}

void update_Click(object sender, EventArgs e)
{
	repeater1.UpdateAfterCallBack = true;
	repeater2.UpdateAfterCallBack = true;
}

</script>
