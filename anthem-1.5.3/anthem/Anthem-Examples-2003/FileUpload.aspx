<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %> 
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        if ( (this.FileUpload1.PostedFile != null) && (this.FileUpload1.PostedFile.ContentLength > 0) )
        {
            this.Label1.Text = string.Format("File \"{0}\" uploaded ({1} bytes).", 
                System.IO.Path.GetFileName(this.FileUpload1.PostedFile.FileName), 
                this.FileUpload1.PostedFile.ContentLength);
        }
        else
        {
            this.Label1.Text = "No file was uploaded.";
        }
        this.Label1.UpdateAfterCallBack = true;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File Upload Example</title>
    <script type="text/javascript">
      function Anthem_Error(result) {
        alert(result.error);
      }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
        <input type="file" id="FileUpload1" runat="server" /><br />
      </fieldset>
      <p>
        <anthem:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Anthem Upload Button" /><br />
        <anthem:Label ID="Label1" runat="server" Text=""/>
      </p>
    </div>
    </form>
</body>
</html>
