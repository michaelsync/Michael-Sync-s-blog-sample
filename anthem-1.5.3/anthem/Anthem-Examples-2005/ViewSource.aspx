<%@ Page Language="C#" %>
<%@ Import Namespace="anrControls.SyntaxColoring" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Page_Load(object sender, EventArgs e)
    {
        string filename = this.Request["filename"];
        if (!string.IsNullOrEmpty(filename))
        {
            filename = HttpUtility.UrlDecode(filename);
            CodeHighlighter highlighter = CodeHighlighterFactory.GetHighlighter(filename);
            string contents = System.IO.File.ReadAllText(filename);
            contents = highlighter.ColorCode(contents, false);
            this.PlaceHolder1.Controls.Add(new LiteralControl(contents));
        }
    }
    
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <link type="text/css" href="colorcode.css" rel="stylesheet" media="all" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:PlaceHolder ID="PlaceHolder1" runat="server"/>
    </div>
    </form>
</body>
</html>
