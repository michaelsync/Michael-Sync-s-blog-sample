<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register TagPrefix="cc1" Namespace="Anthem.Examples" Assembly="Anthem.Examples" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Invoke Custom Control</title>
	</head>
	<body>
		<form id="form1" runat="server">
		    <h1>Invoke Custom Control</h1>
		    <p>This is an example of a simple custom control that has a Click event. The source code for the control
		    is in App_Code/CustomControl2.cs.</p>
			<cc1:CustomControl2 ID="CustomControl2" runat="server" Text="Click Me!" />
		</form>
	</body>
</html>
