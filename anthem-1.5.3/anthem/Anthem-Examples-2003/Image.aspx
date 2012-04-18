<%@ Page Language="C#" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Example</h2>
        <anthem:Button ID="toggleImage" runat="server" Text="Toggle Image" OnClick="toggleImage_Click" />
        <br />
        <anthem:Image ID="image" runat="server" ImageUrl="button_run.gif" />
    </form>
</body>
</html>

<script runat="server">

    void toggleImage_Click(object sender, EventArgs e)
    {
        if (image.ImageUrl == "button_run.gif")
        {
            image.ImageUrl = "button_run-down.gif";
        }
        else
        {
            image.ImageUrl = "button_run.gif";
        }
        image.UpdateAfterCallBack = true;
    }

</script>

