<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>
 <%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

    <script runat="server">
        protected void btnSubmit_Click(object sender, EventArgs e) {
            lblError.Visible = true;
            lblError.Text = "Fuck it!! " + DateTime.Now.ToString();
            
            lblError.UpdateAfterCallBack = true;
        }
    </script>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>
                <anthem:Label id="lblError" Text="F" runat="server" Visible="false"/>
    </p>
    <p>
        
             <anthem:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                 />
    </p>
    </div>
    </form>
</body>
</html>
