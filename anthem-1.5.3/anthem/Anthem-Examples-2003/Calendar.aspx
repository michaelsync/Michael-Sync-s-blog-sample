<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void cal_SelectionChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
    }

    protected void cal_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        System.Threading.Thread.Sleep(500);
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
	</head>
	<body>
		<form id="form1" runat="server">
			<h2>Example</h2>
			<p>There is an artificial delay in each event so you can see the TextDuringCallBack.</p>
			<anthem:Calendar id="cal" runat="server"
			    TextDuringCallBack="&lt;img src=&quot;tiny_red.gif&quot; border=0 /&gt;"
			    OnSelectionChanged="cal_SelectionChanged"
			    OnVisibleMonthChanged="cal_VisibleMonthChanged" />
		</form>
	</body>
</html>
