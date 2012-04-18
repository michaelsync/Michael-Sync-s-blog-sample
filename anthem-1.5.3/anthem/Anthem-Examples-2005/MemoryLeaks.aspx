<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<title>Untitled Page</title>
	</head>
	<body>
		<form id="form1" runat="server">
			<p>The timer on this page ticks every second. The page does not get modified. This 
				should demonstrate that Anthem is not leaking memory with its usage of 
				XMLHttpRequest.</p>
			<p>With the first few requests, you will see memory usage increase. After a bit, 
				memory stops increasing. There are occasional bumps both up and down but they 
				might be occurring due to other factors inside IE that we know nothing about.</p>
			<anthem:Timer ID="timer" runat="server" Enabled="true" Interval="1000" OnTick="timer_Tick" />
		</form>
	</body>
</html>
<script runat="server">

    void timer_Tick(object sender, EventArgs e)
    {
    }

</script>
