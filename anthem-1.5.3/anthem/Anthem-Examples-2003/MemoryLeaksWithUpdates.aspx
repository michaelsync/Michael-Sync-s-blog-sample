<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
	</head>
	<body>
		<form id="form1" runat="server">
			<p>The timer on this page ticks every second. A label on the page does get modified 
				to show how many requests have been made since starting.</p>
			<p>With this test, memory usage initally increases as expected. Eventually, the 
				increase starts to taper off. You can see several requests go by with no 
				increase and sometimes even small decreases in memory usage.</p>
			<p>Unfortunately, the increases are bigger and more frequent than the decreases 
				which, to be perfectly honest, scares me.</p>
			<p>It's possible that IE has some sort of garbage collection process going on here 
				and it doesn't reclaim memory until a certain threshold is reached.</p>
			<anthem:Timer ID="timer" runat="server" Enabled="true" Interval="1000" OnTick="timer_Tick" />
			<anthem:Label ID="label" runat="server" Text="0" />
		</form>
	</body>
</html>
<script runat="server">

    void timer_Tick(object sender, EventArgs e)
    {
		label.Text = (int.Parse(label.Text) + 1).ToString();
		label.UpdateAfterCallBack = true;
    }

</script>
