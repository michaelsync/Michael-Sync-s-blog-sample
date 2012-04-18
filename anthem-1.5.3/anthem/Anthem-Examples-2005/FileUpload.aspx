<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (this.FileUpload1.HasFile)
        {
            this.Label1.Text = string.Format("File \"{0}\" uploaded ({1} bytes).",
                System.IO.Path.GetFileName(this.FileUpload1.FileName),
                this.FileUpload1.FileBytes.Length);
        }
        else
        {
            this.Label1.Text = "No file was uploaded.";
        }
        this.Label1.UpdateAfterCallBack = true;

        if (this.FileUpload2.Visible)
        {
            if (this.FileUpload2.HasFile)
            {
                this.Label2.Text = string.Format("File \"{0}\" uploaded ({1} bytes).",
                    System.IO.Path.GetFileName(this.FileUpload2.FileName),
                    this.FileUpload2.FileBytes.Length);
            }
            else
            {
                this.Label2.Text = "No file was uploaded";
            }
            this.Label2.UpdateAfterCallBack = true;
        }
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        this.Button2.Visible = false;
        this.Button2.UpdateAfterCallBack = true;
        this.FileUpload2.Visible = true;
        this.FileUpload2.UpdateAfterCallBack = true;
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <script type="text/javascript">
      function Anthem_Error(result) {
        alert(result.error);
      }
    </script>

    <p>This page demonstrates uploading a file with an Anthem button.</p>
    <p>
      If you are using Anthem on a web site running ASP.NET 1.1 or above, you can use the 
      <a href="http://msdn2.microsoft.com/system.web.ui.htmlcontrols.htmlinputfile.aspx">System.Web.UI.HtmlControls.HtmlInputFile</a> 
      control (e.g. <code>&lt;input type="file" runat="server"/&gt;</code>) to select the file to upload. 
      See the Microsoft knowledgebase articles <a href="http://support.microsoft.com/kb/323245">KB323245</a> or 
      <a href="http://support.microsoft.com/kb/323246">KB323246</a> for details on how to work with the 
      <a href="http://msdn2.microsoft.com/system.web.ui.htmlcontrols.htmlinputfile.aspx">HtmlInputFile</a>
      control via code.
    </p>
    <p>
      If you are using Anthem on a web site running ASP.NET 2.0 or above, you can use the 
      <a href="http://msdn2.microsoft.com/system.web.ui.webcontrols.fileupload.aspx">System.Web.UI.WebControls.FileUpload</a>
      control (e.g. <code>&lt;ASP:FileUpload runat="server"/&gt;</code>)
      or Anthem.FileUpload control (e.g. <code>&lt;anthem:FileUpload runat="server"/&gt;</code>) 
      to select the file to upload. 
    </p>
    <div>
      <fieldset>
        <legend>File Upload Control</legend>
        <anthem:FileUpload ID="FileUpload1" runat="server" /><br />
        <anthem:Button ID="Button2" runat="server" Text="+" ToolTip="Click to upload multiple files." OnClick="Button2_Click" />
        <anthem:FileUpload ID="FileUpload2" runat="server" Visible="false" />
      </fieldset>
      <p>
        <anthem:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Anthem Upload Button" /><br />
        <anthem:Label ID="Label1" runat="server" Text=""/><br />
        <anthem:Label ID="Label2" runat="server" Text=""/>
      </p>
    </div>
</asp:Content>
