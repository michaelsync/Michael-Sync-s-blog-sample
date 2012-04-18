<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void button1_Click(object sender, EventArgs e)
    {
        HtmlButton button = new HtmlButton();
        button.InnerText = "Now click me!";
        button.Attributes["onclick"] = "ThankYou();";
        placeholder1.Controls.Add(button);
        placeholder1.UpdateAfterCallBack = true;
        
        string script = @"<script type=""text/javascript"">
function ThankYou() {
  alert('Thank you!');
}
</" + "script>";
#if V2
        Anthem.Manager.RegisterClientScriptBlock(typeof(Page), script, script);
#else
        Anthem.Manager.RegisterClientScriptBlock(script, script);
#endif
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Page Scripts</title>
	</head>
	<body>
		<form id="form1" runat="server">
		    <h1>Page Script Examples</h1>
		    <p>When you click on the button two things will happen:</p>
		    <ol>
		        <li>A new button will be added to the page. When you click on the new button it will
		        execute a javascript function called ThankYou(). The function is not yet part of the 
		        page (go ahead and check the page source).</li>
		        <li>A new script block is registered with the page. The script block includes the
		        ThankYou() function.</li>
		    </ol>
		    <p>If the button caused a postback, the new script would be part of the page when the 
		    postback response was rendered by the browser. But the button causes a callback, which
		    does not normally add scripts to the page. So after registering the new script block,
		    we set <code>Anthem.Manager.IncludePageScripts = true;</code>.</p>
		    <anthem:Button ID="button1" runat="server" Text="Click Me!" OnClick="button1_Click" />
            <br />
		    <anthem:PlaceHolder ID="placeholder1" runat="server" />
		</form>
	</body>
</html>
