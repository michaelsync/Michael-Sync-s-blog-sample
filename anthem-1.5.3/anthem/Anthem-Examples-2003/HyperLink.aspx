<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        HyperLink1.NavigateUrl = "http://anthem-dot-net.sourceforge.net";
        HyperLink1.Text = HyperLink1.NavigateUrl;
        HyperLink1.ToolTip = "Click me to redirect your browser to " + HyperLink1.NavigateUrl;
        HyperLink1.UpdateAfterCallBack = true;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>HyperLink Example</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>
        HyperLink Example</h1>
        Click on the button to update the HyperLink control with a new URL. Then click on
        the HyperLink control to link to redirect the page.<br />
        <br />
        <anthem:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Click Me!" /><br />
        <br />
        <anthem:HyperLink ID="HyperLink1" runat="server">HyperLink</anthem:HyperLink></div>
    </form>
</body>
</html>
