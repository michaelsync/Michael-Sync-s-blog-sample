<%@ Page Language="C#" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" > 

<html>
  <head>
    <title>ServerTransferExample</title>
  </head>
  <body>
  </body>
</html>

<script runat="server">

	void Page_Load()
	{
		Server.Transfer("ServerTransferDestination.aspx");
	}

</script>