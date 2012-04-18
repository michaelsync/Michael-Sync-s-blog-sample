<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<script runat="server">

	void Page_Load()
	{
		Server.Transfer("ServerTransferDestination.aspx");
	}

</script>