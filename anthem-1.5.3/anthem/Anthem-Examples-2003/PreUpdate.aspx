<%@ Page Language="C#" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Button1_Click(object sender, EventArgs e)
    {
        Label1.Text = "Whop!";
        Label1.UpdateAfterCallBack = true;
    }

    protected void Label1_PreUpdate(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("alert('whopped!');");
    }

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>PreUpdate Event Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        The PreUpdate event is raised before the Anthem control is rendered, if UpdateAfterCallBack
        is true. In this sample the anthem:Label PreUdate event handler uses Anthem.Manager.AddScriptForClientSideEval
        to run a script after the callback.<br />
        <br />
        <anthem:Button ID="Button1" runat="server" Text="Click Me!" OnClick="Button1_Click" /><br />
        <anthem:Label ID="Label1" runat="server" OnPreUpdate="Label1_PreUpdate">Click the button.</anthem:Label>
    </div>
    </form>
</body>
</html>
