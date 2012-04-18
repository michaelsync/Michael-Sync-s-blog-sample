<%@ Page Language="C#" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>DynamicUserControls</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>The user control you see below this paragraph is being dynamically loaded and 
				inserted into a PlaceHolder control.</p>
			<asp:PlaceHolder ID="place" Runat="server" />
		</form>
	</body>
</html>
<script runat="server">

void Page_Load()
{
	place.Controls.Add(LoadControl("UserControlWithCheckBox.ascx"));
}

</script>
