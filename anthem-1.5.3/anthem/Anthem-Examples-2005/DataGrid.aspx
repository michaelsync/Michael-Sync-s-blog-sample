<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

	void Page_Load()
	{
		dataGrid1.DataSource = DataView1;
	}

	void Page_PreRender()
	{
		DataBind();
	}

	DataSet TestData
	{
		get
		{
			DataSet ds = Session["TestData"] as DataSet;
			if (ds == null)
			{
				ds = new DataSet();
				DataTable dt = new DataTable();
				ds.Tables.Add(dt);
				dt.Columns.Add("id", typeof(int));
				dt.PrimaryKey = new DataColumn[] { dt.Columns["id"] };
				dt.Columns.Add("foo", typeof(string));
				dt.Columns.Add("bar", typeof(string));
				dt.Columns.Add("baz", typeof(string));
				dt.Rows.Add(new object[] {1, "a", "k", "j"});
				dt.Rows.Add(new object[] {2, "l", "b", "i"});
				dt.Rows.Add(new object[] {3, "m", "o", "c"});
				dt.Rows.Add(new object[] {4, "n", "d", "h"});
				dt.Rows.Add(new object[] {5, "e", "f", "g"});
				Session["TestData"] = ds;
			}
			return ds;
		}
	}

	DataView DataView1
	{
		get
		{
			DataView dv = Session["DataView1"] as DataView;
			if (dv == null)
			{
				dv = new DataView(TestData.Tables[0]);
				Session["DataView1"] = dv;
			}
			return dv;
		}
	}

	void dataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		grid.EditItemIndex = e.Item.ItemIndex;
		grid.UpdateAfterCallBack = true;
	}

	void dataGrid1_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		DataRow row = TestData.Tables[0].Rows.Find(grid.DataKeys[e.Item.ItemIndex]);
		row["foo"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[0].FindControl("_foo")).Text;
		row["bar"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Text;
		row["baz"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text;
		grid.EditItemIndex = -1;
		grid.UpdateAfterCallBack = true;
	}

	void dataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		grid.EditItemIndex = -1;
		grid.UpdateAfterCallBack = true;
	}

	void dataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		TestData.Tables[0].Rows.Remove(TestData.Tables[0].Rows.Find(grid.DataKeys[e.Item.ItemIndex]));

		// If the row that was just deleted was being edited, turn
		// editing off.
		if (grid.EditItemIndex == e.Item.ItemIndex)
		{
			grid.EditItemIndex = -1;
		}
		
		if (grid.CurrentPageIndex > 0 && TestData.Tables[0].Rows.Count <= (grid.CurrentPageIndex * grid.PageSize))
		{
			--grid.CurrentPageIndex;
		}
		
		grid.UpdateAfterCallBack = true;
	}

	void dataGrid1_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		// Alternate sorting between ascending and descending.
		if (((DataView)grid.DataSource).Sort != e.SortExpression)
		{
			((DataView)grid.DataSource).Sort = e.SortExpression;
		}
		else
		{
			((DataView)grid.DataSource).Sort += " DESC";
		}
		grid.EditItemIndex = -1;
		grid.UpdateAfterCallBack = true;
	}

	void dataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
	{
		Anthem.DataGrid grid = (Anthem.DataGrid)source;
		grid.CurrentPageIndex = e.NewPageIndex;
		grid.UpdateAfterCallBack = true;
	}

</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
			<h2>Example</h2>
			<anthem:DataGrid id="dataGrid1" runat="server" AutoGenerateColumns="False" DataKeyField="ID" Width="100%"
				AllowSorting="True" AllowPaging="True" PageSize="2" OnEditCommand="dataGrid1_EditCommand" OnUpdateCommand="dataGrid1_UpdateCommand"
				OnCancelCommand="dataGrid1_CancelCommand" OnDeleteCommand="dataGrid1_DeleteCommand" OnSortCommand="dataGrid1_SortCommand"
				OnPageIndexChanged="dataGrid1_PageIndexChanged">
				<Columns>
					<asp:TemplateColumn SortExpression="foo" HeaderText="Foo">
						<ItemTemplate>
							<b>
								<%# DataBinder.Eval(Container.DataItem, "foo") %>
							</b>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:TextBox id=_foo Text='<%# DataBinder.Eval(Container.DataItem, "foo") %>' Runat="server" />
						</EditItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="bar" SortExpression="bar" HeaderText="Bar"></asp:BoundColumn>
					<asp:BoundColumn DataField="baz" SortExpression="baz" HeaderText="Baz"></asp:BoundColumn>
					<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
					<asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete"></asp:ButtonColumn>
				</Columns>
			</anthem:DataGrid>
</asp:Content>
