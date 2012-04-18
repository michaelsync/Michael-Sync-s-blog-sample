<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Test File Download with InvokePageMethod</title>
    <script type="text/javascript">

    function test_Anthem_InvokePageMethod()
    {
	    Anthem_InvokePageMethod(
		    'GetStringFromPageMethod',
		    [],
		    function(result){
			    alert(result.value);
		    }
	    ); 
    }

    </script>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Test File Download</h2>
			<p style="border: 1px solid gray; padding: .5em 1em;">
			<strong>Test that __EVENTTARGET is being set properly (see bug #1429412):</strong>
			<br/>
			<asp:LinkButton id="linkbutton" runat="server" 
			    Text="Call Anthem_InvokePageMethod()">
			</asp:linkbutton>
			&lt;-- click this button <em>before</em> and <em>after</em> downloading one of the files below.
			You should see the same message both times.
			</p>
			<asp:Repeater ID="FileList" runat="server" OnItemCommand="FileList_ItemCommand">
			    <HeaderTemplate>
			    <h4>Files</h4>
			        <ul>
			    </HeaderTemplate>
			    <ItemTemplate>
			        <li>
			            <%# ((ListItem)Container.DataItem).Text %>
			            <asp:LinkButton ID="LinkButton1" runat="server" Text="download" 
			                CommandArgument="<%# ((ListItem)Container.DataItem).Value %>">
			            </asp:LinkButton>
			        </li>
			    </ItemTemplate>
			    <FooterTemplate>
			        </ul>
			    </FooterTemplate>
			</asp:Repeater>
			
		</form>
	</body>
</html>

<script runat="server">
    
    public override void DataBind()
    {
        string filepath = this.MapPath(".");
        ListItemCollection files = new ListItemCollection();
        foreach (string filename in System.IO.Directory.GetFiles(filepath, "*.aspx"))
        {
            ListItem file = new ListItem(System.IO.Path.GetFileName(filename), filename);
            files.Add(file);
        }
        FileList.DataSource = files;
        FileList.DataBind();
        base.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Anthem.Manager.Register(this);
		    linkbutton.Attributes.Add("onclick", "test_Anthem_InvokePageMethod(); return false;");
    
        if (!IsPostBack)
            this.DataBind();
    }

    protected void FileList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string filename = (string)e.CommandArgument;
        System.IO.FileInfo fileinfo = new System.IO.FileInfo(filename);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filename));
        Response.AppendHeader("Content-Length", fileinfo.Length.ToString());
        Response.WriteFile(filename);
        Response.Flush();
        Response.End();
    }

	  [Anthem.Method]
	  public string GetStringFromPageMethod () 
	  {
		  return "This string was successfully returned from a server-side Page method.";
	  }
    
</script>
