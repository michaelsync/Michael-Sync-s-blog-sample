<%@ Page Language="C#" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        button1.Text = DropDownList1.SelectedValue;
        button1.UpdateAfterCallBack = true;
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DropDownList Pre and Post Events</title>

    <script type="text/javascript">

	    function DropDownList1_PreCallBack(list) {
		    if (!confirm('Are you sure you want to perform this call back?')) {
			    return false;
		    }
		    document.getElementById('button1').disabled = true;
	    }

	    function DropDownList1_PostCallBack(list) {
		    document.getElementById('button1').disabled = false;
	    }
    	
	    function DropDownList1_CallBackCanceled(list) {
	        alert("Callback canceled.");
	    }
				
    </script>

</head>
<body>
    <form id="form1" runat="server">
        You can invoke methods both before and after the callback. In this example, the
        pre-callback function asks you to confirm the callback. If you cancel the callback,
        the CallBackCancelledFunction is called.<br />
        <br />
        <anthem:DropDownList ID="DropDownList1" runat="server" 
            AutoCallBack="True"
            PreCallBackFunction="DropDownList1_PreCallBack"
            PostCallBackFunction="DropDownList1_PostCallBack" 
            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
            CallBackCancelledFunction="DropDownList1_CallBackCanceled"
            TextDuringCallBack="working...">
            <asp:ListItem Value="Click Me!">Select One</asp:ListItem>
            <asp:ListItem>Audi</asp:ListItem>
            <asp:ListItem>Mercedes</asp:ListItem>
            <asp:ListItem>Ford</asp:ListItem>
        </anthem:DropDownList>
        <asp:Label ID="DropDownList1Label" runat="server" AssociatedControlID="DropDownList1" />
        <br />
        <br />
        <anthem:Button ID="button1" runat="server" Text="Click Me!" />
    </form>
</body>
</html>
