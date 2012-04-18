<%@ Page Language="C#" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Anthem Panel Demo</title>
    <style type="text/css">
        fieldset { margin: 0.5em }
    </style>
    <script type="text/javascript">
		function Anthem_PreCallBack() {
			var loading = document.createElement("div");
			loading.id = "loading";
			loading.style.color = "black";
			loading.style.backgroundColor = "red";
			loading.style.paddingLeft = "5px";
			loading.style.paddingRight = "5px";
			loading.style.position = "absolute";
			loading.style.right = "10px";
			loading.style.top = "10px";
			loading.style.zIndex = "9999";
			loading.innerHTML = "working...";
			document.body.appendChild(loading);
		}
		function Anthem_PostCallBack() {
			var loading = document.getElementById("loading");
			document.body.removeChild(loading);
		}
	</script>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Anthem Panel Demo</h1>
        <p>There are only two Anthem controls on this page: the Anthem.Panel (blue border) that surrounds all the other controls, and an Anthem.Button in the bottom
            right corner to demonstrate that you can embed Anthem control inside of an Anthem.Panel. When AddCallBacks is true, the Anthem.Panel will
            convert all of the child controls to use callbacks instead of postbacks. The Anthem.Panel
            also has Pre- and PostCallBackFunctions defined to display a message while the callback
            is being processed on the server. Note that these are inherited by all child controls. Callbacks are generally very fast. You may need
            to turn on the server delay in order to see the message.</p>
        <asp:CheckBox ID="addCallBacks" runat="server" Text="AddCallBacks" Checked="true" ToolTip="If AddCallBacks is true, the Anthem.Panel will convert all child ASP.NET WebControls to use callbacks." />
        <asp:CheckBox ID="sleep" runat="server" Text="Use server delay" ToolTip="Use this option if you want to see the Pre/PostCallBack functions" />
        <anthem:Panel ID="UpdatePanel1" runat="server"
            AutoUpdateAfterCallBack="true" 
            BorderColor="#8080FF" 
            BorderStyle="Solid" 
            BorderWidth="1px"
            PreCallBackFunction="Anthem_PreCallBack"
            PostCallBackFunction="Anthem_PostCallBack"
            Width="100%">
            <fieldset>
                <legend>ASP:DataGrid</legend>
                <asp:DataGrid ID="grid" runat="server" AutoGenerateColumns="False" DataKeyField="ID"
                    Width="100%" AllowSorting="True" AllowPaging="True" PageSize="2" OnEditCommand="dataGrid1_EditCommand"
                    OnUpdateCommand="dataGrid1_UpdateCommand" OnCancelCommand="dataGrid1_CancelCommand"
                    OnDeleteCommand="dataGrid1_DeleteCommand" OnSortCommand="dataGrid1_SortCommand"
                    OnPageIndexChanged="dataGrid1_PageIndexChanged">
                    <Columns>
                        <asp:TemplateColumn SortExpression="foo" HeaderText="Foo">
                            <ItemTemplate>
                                <b>
                                    <%# DataBinder.Eval(Container.DataItem, "foo") %>
                                </b>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="_foo" Text='<%# DataBinder.Eval(Container.DataItem, "foo") %>' runat="server" />
                            </EditItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn DataField="bar" SortExpression="bar" HeaderText="Bar"></asp:BoundColumn>
                        <asp:BoundColumn DataField="baz" SortExpression="baz" HeaderText="Baz"></asp:BoundColumn>
                        <asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel"
                            EditText="Edit"></asp:EditCommandColumn>
                        <asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete"></asp:ButtonColumn>
                    </Columns>
                </asp:DataGrid>
            </fieldset>
            
            <div style="width: 49%; float: left;">
            
                <fieldset>
                    <legend>ASP:Button + ASP:RequiredFieldValidator</legend>
                    <asp:TextBox ID="textbox" runat="server" Text="sample" ValidationGroup="One" />
                    <asp:Button ID="button2" runat="server" CausesValidation="true" Text="ASP:Button"
                        OnClick="button2_Click" ValidationGroup="One" />
                    <br />
                    <asp:Label ID="textboxlabel" runat="server" Text="" ForeColor="Red" />
                    <asp:RequiredFieldValidator ID="textbox1required" runat="server" ControlToValidate="textbox"
                        EnableClientScript="true" Text="You must enter a value" ValidationGroup="One" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:CheckBox</legend>
                    <asp:CheckBox ID="checkbox" runat="server" Text="Check me!" OnCheckedChanged="checkbox1_CheckedChanged" AutoPostBack="True" ValidationGroup="Two" />
                    <asp:Label ID="checkboxlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:CheckBoxList</legend>
                    <asp:CheckBoxList ID="checkboxlist" runat="server" AutoPostBack="True" OnSelectedIndexChanged="checkboxlist1_SelectedIndexChanged" RepeatDirection="Horizontal" RepeatLayout="Flow" ValidationGroup="Three" >
                        <asp:ListItem>Red</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                        <asp:ListItem>Blue</asp:ListItem>
                    </asp:CheckBoxList>
                    <asp:Label ID="checkboxlistlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:DataList</legend>
                    <asp:DataList ID="datalist" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" OnItemCommand="datalist_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="repeatLinkButton" runat="server" CommandArgument="<%# ((ListItem)Container.DataItem).Value %>">
                                <%# ((ListItem)Container.DataItem).Text %>
                            </asp:LinkButton>                        
                        </ItemTemplate>
                        <SeparatorTemplate>
                            ::
                        </SeparatorTemplate>
                    </asp:DataList>
                    <asp:Label ID="datalistlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:DropDownList</legend>
                    <asp:DropDownList ID="dropdownlist" runat="server" Width="127px"
                        AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" ValidationGroup="Four">
                        <asp:ListItem>Select One</asp:ListItem>
                        <asp:ListItem>Red</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                        <asp:ListItem>Blue</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="dropdownlistlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:ListBox</legend>
                    <div style="float:left">
                        <asp:ListBox ID="listbox" runat="server" AutoPostBack="True" OnSelectedIndexChanged="listbox_SelectedIndexChanged" Rows="3" SelectionMode="Multiple" ValidationGroup="Five" >
                            <asp:ListItem>Red</asp:ListItem>
                            <asp:ListItem>Green</asp:ListItem>
                            <asp:ListItem>Blue</asp:ListItem>
                            <asp:ListItem></asp:ListItem>
                        </asp:ListBox>
                    </div>
                    <asp:Label ID="listboxlabel" runat="server" ForeColor="Red" />
                </fieldset>

            </div>
            
            <div style="width: 49%; float: right">
                
                <fieldset>
                    <legend>ASP:RadioButton</legend>
                    <asp:RadioButton ID="radiobutton1" runat="server" AutoPostBack="True" GroupName="radiobutton" OnCheckedChanged="radiobutton_CheckedChanged" Text="On" ValidationGroup="Six" />
                    <asp:RadioButton ID="radiobutton2" runat="server" AutoPostBack="True" GroupName="radiobutton" Text="Off" OnCheckedChanged="radiobutton_CheckedChanged" ValidationGroup="Six" />
                    <asp:Label ID="radiobuttonlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:RadioButtonList</legend>
                    <asp:RadioButtonList ID="radiobuttonlist" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="radiobuttonlist_SelectedIndexChanged" AutoPostBack="True" RepeatLayout="Flow" ValidationGroup="Seven" >
                        <asp:ListItem>Red</asp:ListItem>
                        <asp:ListItem>Green</asp:ListItem>
                        <asp:ListItem>Blue</asp:ListItem>
                    </asp:RadioButtonList>
                    <asp:Label ID="radiobuttonlistlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:Repeater</legend>
                    <asp:Repeater ID="repeater" runat="server" OnItemCommand="repeater_ItemCommand">
                        <ItemTemplate>
                            <asp:LinkButton ID="repeatLinkButton" runat="server" CommandArgument="<%# ((ListItem)Container.DataItem).Value %>">
                                <%# ((ListItem)Container.DataItem).Text %>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            :: 
                        </SeparatorTemplate>
                    </asp:Repeater>
                    <asp:Label ID="repeaterlabel" runat="server" ForeColor="Red" />
                </fieldset>
                
                <fieldset>
                    <legend>ASP:TextBox</legend>
                    Type something then tab out:
                    <asp:TextBox ID="textbox2" runat="server" OnTextChanged="textbox2_TextChanged" AutoPostBack="True" ValidationGroup="Eight" /><br />
                    <asp:Label ID="textboxlabel2" runat="server" ForeColor="Red" />
                </fieldset>

                <fieldset>
                    <legend>Anthem:Button</legend>
                    <p>
                        If you want a particular control in the UpdatePanel to use a normal post back (e.g.
                        to start a file download), then use the equivalent Anthem control and set the EnableCallBack
                        property to false. For example, this Anthem.Button will initially perform a postback.
                        Each time you click on the button it switches EnableCallBack.</p>
                    <anthem:Button ID="button" runat="server" EnableCallBack="False" OnClick="Button1_Click"
                        Text="PostBack" CausesValidation="True" ValidationGroup="Nine" />
                    <anthem:Label ID="buttonlabel" runat="server" ForeColor="Red" />
                </fieldset>

            </div>
            
            <div style="clear:both"></div>

        </anthem:Panel>
    </form>
</body>
</html>

<script runat="server">

    void Page_Load()
    {
        grid.DataSource = DataView1;
        buttonlabel.Text = string.Empty;

        ListItemCollection items = new ListItemCollection();
        items.Add(new ListItem("Red"));
        items.Add(new ListItem("Green"));
        items.Add(new ListItem("Blue"));

        datalist.DataSource = items;
        datalist.DataBind();
        
        repeater.DataSource = items;
        repeater.DataBind();

        if (Anthem.Manager.IsCallBack && sleep.Checked)
            System.Threading.Thread.Sleep(500);
        UpdatePanel1.AddCallBacks = addCallBacks.Checked;
    }

    void Page_PreRender()
    {
        DataBind();
    }

    DataSet TestData
    {
        get
        {
            DataSet ds = Session["PanelTestData"] as DataSet;
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
                dt.Rows.Add(new object[] { 1, "a", "k", "j" });
                dt.Rows.Add(new object[] { 2, "l", "b", "i" });
                dt.Rows.Add(new object[] { 3, "m", "o", "c" });
                dt.Rows.Add(new object[] { 4, "n", "d", "h" });
                dt.Rows.Add(new object[] { 5, "e", "f", "g" });
                Session["PanelTestData"] = ds;
            }
            return ds;
        }
    }

    DataView DataView1
    {
        get
        {
            DataView dv = Session["PanelDataView1"] as DataView;
            if (dv == null)
            {
                dv = new DataView(TestData.Tables[0]);
                Session["PanelDataView1"] = dv;
            }
            return dv;
        }
    }

    void dataGrid1_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        grid.EditItemIndex = e.Item.ItemIndex;
    }

    void dataGrid1_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        DataRow row = TestData.Tables[0].Rows.Find(grid.DataKeys[e.Item.ItemIndex]);
        row["foo"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[0].FindControl("_foo")).Text;
        row["bar"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Text;
        row["baz"] = ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Text;
        grid.EditItemIndex = -1;
    }

    void dataGrid1_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        grid.EditItemIndex = -1;
    }

    void dataGrid1_DeleteCommand(object source, DataGridCommandEventArgs e)
    {
        TestData.Tables[0].Rows.Remove(TestData.Tables[0].Rows.Find(grid.DataKeys[e.Item.ItemIndex]));

        // If the row that was just deleted was being edited, turn
        // editing off.
        if (grid.EditItemIndex == e.Item.ItemIndex)
            grid.EditItemIndex = -1;

        if (grid.CurrentPageIndex > 0 && TestData.Tables[0].Rows.Count <= (grid.CurrentPageIndex * grid.PageSize))
            --grid.CurrentPageIndex;
    }

    void dataGrid1_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        // Alternate sorting between ascending and descending.
        if (((DataView)grid.DataSource).Sort != e.SortExpression)
            ((DataView)grid.DataSource).Sort = e.SortExpression;
        else
            ((DataView)grid.DataSource).Sort += " DESC";
        grid.EditItemIndex = -1;
    }

    void dataGrid1_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
    {
        grid.CurrentPageIndex = e.NewPageIndex;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            buttonlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:");
            if (button.EnableCallBack)
                button.Text = "PostBack";
            else
                button.Text = "CallBack";
            button.EnableCallBack = !button.EnableCallBack;
        }
    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (dropdownlist.SelectedValue == string.Empty)
            dropdownlistlabel.Text = string.Empty;
        else
            dropdownlistlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + "You picked " + dropdownlist.SelectedValue;
    }

    protected void button2_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
            textboxlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + "You entered " + HttpUtility.HtmlEncode(textbox.Text);
        else
            textboxlabel.Text = "";
    }

    protected void checkbox1_CheckedChanged(object sender, EventArgs e)
    {
        checkboxlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + (checkbox.Checked ? "Checked" : "Not Checked");
    }

    protected void checkboxlist1_SelectedIndexChanged(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append((Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked ");
        if (checkboxlist.SelectedItem == null)
            sb.Length = 0;
        else
        {
            foreach (ListItem item in checkboxlist.Items)
            {
                if (item.Selected)
                    sb.Append(item.Value + ", ");
            }
            sb.Length = sb.Length - 2;
        }
        checkboxlistlabel.Text = sb.ToString();
    }

    protected void listbox_SelectedIndexChanged(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append((Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked ");
        if (listbox.SelectedItem == null)
            sb.Length = 0;
        else
        {
            foreach (ListItem item in listbox.Items)
            {
                if (item.Selected)
                    sb.Append(item.Value + ", ");
            }
            sb.Length = sb.Length - 2;
        }
        listboxlabel.Text = sb.ToString();
    }

    protected void radiobutton_CheckedChanged(object sender, EventArgs e)
    {
        if (radiobutton1.Checked)
            radiobuttonlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked " + radiobutton1.Text;
        else if (radiobutton2.Checked)
            radiobuttonlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked " + radiobutton2.Text;
        else
            radiobuttonlabel.Text = string.Empty;
    }

    protected void radiobuttonlist_SelectedIndexChanged(object sender, EventArgs e)
    {
        radiobuttonlistlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked " + radiobuttonlist.SelectedValue;
    }

    protected void repeater_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        repeaterlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked " + e.CommandArgument;
    }

    protected void textbox2_TextChanged(object sender, EventArgs e)
    {
        textboxlabel2.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You entered: " + HttpUtility.HtmlEncode(textbox2.Text);
    }

    protected void datalist_ItemCommand(object source, DataListCommandEventArgs e)
    {
        datalistlabel.Text = (Anthem.Manager.IsCallBack ? "[CallBack]:" : "[PostBack]:") + " You picked " + e.CommandArgument;
    }
</script>

