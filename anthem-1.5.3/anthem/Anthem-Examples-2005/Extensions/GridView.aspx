<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<script runat="server">

    void resetButton_Click(object sender, EventArgs e)
    {
        DataSource.RemoveDataTable();
        gridView.DataBind();
        gridView.UpdateAfterCallBack = true;
    }

    protected void filterButton_Click(object sender, EventArgs e)
    {
        gridView.UpdateAfterCallBack = true;
    }

    protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
    {
        Anthem.GridView gv = sender as Anthem.GridView;
        string id = gv.SelectedRow.Cells[0].Text;
        Anthem.Manager.AddScriptForClientSideEval(
            string.Format(
                "alert('You selected the record with an ID of {0}!')",
                id
            )
        );
    }

    protected void DetailsView1_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
    {
        gridView.DataBind();
    }

    protected void DetailsView1_ItemDeleted(object sender, DetailsViewDeletedEventArgs e)
    {
        gridView.DataBind();
    }
    
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <style>
        .gridview {
	        font: 8pt Tahoma;
	        cursor: default;
	        background-color: white;
        }
        .gridview .header {
	        background-color: #DCDCDC;
	        text-align: left;
	        color: #61002B;
	        overflow: hidden;
        }
        .gridview .header a {
	        color: #61002B;
        }
        .gridview .headersorted {
            background-color: #C0C0C0;
        }
        .gridview .itemsorted {
            background-color: #E7E7E7;    
        }
        .gridview .item {
	        background-color: #F5F5F5;
        }
        .gridview .alternatingitem {
	        background-color: #F5F5F5;
        }
        .gridview .mouseoveritem {
	        background-color: #ECE7C9;
        }
        .gridview .selecteditem {
	        background-color: #F0F0F0;
	        font-weight: bold;
        }
        .gridview .edititem {
	        background-color: #F0F0F0;
        }
        .gridview .pager {
	        background-color: #E3E3E3;
	        font: 8pt Verdana;
        }    
    </style>

    <script type="text/javascript">
        function preDelete() {
            return confirm('Are you sure?');
        }
    </script>

    <p>The GridView control in the AnthemExtensions library includes several row-oriented properties
        and events.</p>
    <p>
        Use the <code>select</code> linkbutton to show up SelectView and FormView controls.
        These controls are however not yet associated with clicks on edit linkbuttons.</p>
    <asp:ObjectDataSource ID="DataSource1" runat="server" TypeName="DataSource" SelectMethod="Select"
        UpdateMethod="Update" DeleteMethod="Delete" FilterExpression="{0}">
        <FilterParameters>
            <asp:ControlParameter ControlID="filterTextBox" />
        </FilterParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource ID="DetailSource1" runat="server" TypeName="DetailSource" SelectMethod="Select"
        UpdateMethod="Update" DeleteMethod="Delete">
        <SelectParameters>
            <asp:ControlParameter ControlID="gridView" Name="id" PropertyName="SelectedValue"
                Type="int32" />
        </SelectParameters>
    </asp:ObjectDataSource>
    <anthemext:GridView ID="gridView" EnableViewState="True" runat="server" UseCoolPager="true"
        TotalRecordString="Total Errors :" AscImage="~/Extensions/sorted_asc.gif" DescImage="~/Extensions/sorted_desc.gif"
        RowClickEvent="Edit" HidePagerOnOnePage="true" CellSpacing="1" CssClass="gridview"
        GridLines="None" BorderWidth="0px" AutoGenerateColumns="False" DataSourceID="DataSource1"
        DataKeyNames="id" AllowSorting="True" AllowPaging="True" PageSize="5" CellPadding="4"
        ForeColor="#333333" OnSelectedIndexChanged="gridView_SelectedIndexChanged" Width="300px">
        <Columns>
            <asp:BoundField DataField="id" HeaderText="ID" SortExpression="id" ReadOnly="True" />
            <asp:BoundField DataField="a" HeaderText="A" SortExpression="a" />
            <asp:BoundField DataField="b" HeaderText="B" SortExpression="b" />
            <asp:CheckBoxField DataField="c" HeaderText="C" SortExpression="c" />
            <asp:TemplateField ShowHeader="False">
                <edititemtemplate>
                    <asp:LinkButton runat="server" Text="Update" CommandName="Update" CausesValidation="True" id="LinkButton1" />
                    <asp:LinkButton runat="server" Text="Cancel" CommandName="Cancel" CausesValidation="False" id="LinkButton2" />
                </edititemtemplate>
                <itemtemplate>
                    <asp:LinkButton runat="server" Text="Edit" CommandName="Edit" CausesValidation="False" id="LinkButton4" />
                    <anthem:LinkButton runat="server" Text="Delete" CommandName="Delete" CausesValidation="False" id="LinkButton3"
                        PreCallBackFunction="preDelete" />
                </itemtemplate>
            </asp:TemplateField>
            <asp:CommandField ShowSelectButton="True" />
        </Columns>
        <HeaderStyle Wrap="false" CssClass="header" />
        <RowStyle CssClass="item" />
        <MouseOverRowStyle CssClass="mouseoveritem" />
        <SortedColumnHeaderRowStyle CssClass="headersorted" />
        <SortedColumnRowStyle CssClass="itemsorted" />
        <AlternatingRowStyle CssClass="alternatingitem" />
        <SelectedRowStyle CssClass="selecteditem" />
        <EditRowStyle CssClass="edititem" />
        <EmptyDataRowStyle CssClass="item" />
        <PagerStyle CssClass="pager" />
    </anthemext:GridView>
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <anthem:DetailsView ID="DetailsView1" runat="server" AutoGenerateDeleteButton="True"
                    AutoGenerateEditButton="True" DataKeyNames="id" DataSourceID="DetailSource1"
                    HeaderText="DetailsView" OnItemUpdated="DetailsView1_ItemUpdated" OnItemDeleted="DetailsView1_ItemDeleted"
                    Width="300px">
                    <HeaderStyle BackColor="#2461BF" Font-Bold="True" ForeColor="White" />
                    <CommandRowStyle BackColor="#2461BF" ForeColor="White" />
                    <FieldHeaderStyle BackColor="#507CD1" />
                    <RowStyle BackColor="#EFF3FB" />
                </anthem:DetailsView>
            </td>
            <td valign="top">
                <anthem:FormView ID="FormView1" runat="server" DataKeyNames="id" DataSourceID="DetailSource1"
                    HeaderText="FormView" Width="300px">
                    <RowStyle BackColor="#EFF3FB" />
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <b>ID:</b></td>
                                <td>
                                    <%# Eval("id") %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>E:</b></td>
                                <td>
                                    <%# Eval("e") %>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <b>F:</b></td>
                                <td>
                                    <%# Eval("f") %>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <HeaderStyle BackColor="#2461BF" Font-Bold="True" ForeColor="White" />
                </anthem:FormView>
            </td>
        </tr>
    </table>
    <div>
        <p>
            Enter an expression to filter the data (something like <code>id &lt;= 5</code> or
            <code>not c</code>):</p>
        <asp:TextBox ID="filterTextBox" runat="server" />
        <anthem:Button ID="filterButton" runat="server" Text="Filter" OnClick="filterButton_Click" />
    </div>
    <div>
        <p>
            Click the Reset button to restore the data after updating/deleting.</p>
        <anthem:Button ID="resetButton" runat="server" Text="Reset" OnClick="resetButton_Click" />
    </div>
</asp:Content>
