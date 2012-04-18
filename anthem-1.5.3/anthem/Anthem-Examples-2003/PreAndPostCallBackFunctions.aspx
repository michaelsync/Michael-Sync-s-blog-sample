<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>PreAndPostCallBackFunctions</title>
		<script language="javascript">
		function Anthem_PreCallBack() {
			if (!confirm("Are you sure you want to get the current time?")) {
				return false;
			}
			var loading = document.createElement("div");
			loading.id = "loading";
			loading.style.color = "black";
			loading.style.backgroundColor = "red";
			loading.style.paddingLeft = "5px";
			loading.style.paddingRight = "5px";
			loading.style.position = "absolute";
			loading.style.right = "10px";
			loading.style.top = "10px";
			loading.style.zIndex = "9999";
			loading.innerHTML = "Loading...";
			document.body.appendChild(loading);
		}
		function Anthem_CallBackCancelled() {
			alert("Your call back was cancelled!");
		}
		function Anthem_PostCallBack() {
			var loading = document.getElementById("loading");
			document.body.removeChild(loading);
		}
		</script>
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>There are three special functions that you can define to hook into Anthem's call 
				back process: <code>Anthem_PreCallBack</code>, <code>Anthem_PostCallBack</code>, 
				and <code>Anthem_CallBackCancelled</code>.</p>
			<h2>Example</h2>
			<p>Click the following button to get the current time:</p>
			<anthem:Button id="button" runat="server" Text="Click Me!" OnClick="button_Click" />
			<anthem:Label id="label" runat="server">?</anthem:Label>
			<p>You should be prompted to make sure you really want to get the current time. If 
				you say OK, you should see a red "Loading..." message in the upper right corner 
				of your page (similar to what Gmail does). I put a call to <code>Thread.Sleep</code>
				in the call back method to make sure you see it. When the call back completes 
				the "Loading..." message should go away.</p>
			<p>If you click Cancel when prompted, you won't see that message but you will see 
				an alert telling you that you cancelled the call back.</p>
			<h2>Remarks</h2>
			<p>Before a call back is made to the server, a function called <code>Anthem_PreCallBack</code>
				on the client page is invoked if it exists. If it returns false, the call back 
				is cancelled.</p>
			<p>If the call back is cancelled, a function called <code>Anthem_CallBackCancelled</code>
				is invoked if it exists.</p>
			<p>If the call back was not cancelled, a function called <code>Anthem_PostCallBack</code>
				is invoked if it exists.</p>
			<p>View source to see how these three JavaScript functions are implemented on this 
				page.</p>
		</form>
	</body>
</HTML>
<script runat="server">

		void button_Click(object sender, System.EventArgs e)
		{
			System.Threading.Thread.Sleep(2000);
			label.Text = DateTime.Now.ToString();
			label.UpdateAfterCallBack = true;
		}

</script>
