<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>Table</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<p>Table controls do not retain their state on the server across both call backs 
				and post backs. This is why you never see more than one row in this table no 
				matter how many times you click the button.</p>
			<anthem:Table id="table" runat="server">
				<asp:TableRow>
					<asp:TableHeaderCell>Column 1</asp:TableHeaderCell>
					<asp:TableHeaderCell>Column 2</asp:TableHeaderCell>
					<asp:TableHeaderCell>Column 3</asp:TableHeaderCell>
				</asp:TableRow>
			</anthem:Table>
			<br />
			<anthem:Button id="button" runat="server" Text="Add Row To Table" OnClick="button_Click" />
		</form>
	</body>
</html>
<script runat="server">

	void button_Click(object sender, EventArgs e)
	{
		TableRow row = new TableRow();
		TableCell cell1 = new TableCell();
		cell1.Text = "Cell 1";
		row.Cells.Add(cell1);
		TableCell cell2 = new TableCell();
		cell2.Text = "Cell 2";
		row.Cells.Add(cell2);
		TableCell cell3 = new TableCell();
		cell3.Text = "Cell 3";
		row.Cells.Add(cell3);
		table.Rows.Add(row);
		table.UpdateAfterCallBack = true;
	}

</script>
