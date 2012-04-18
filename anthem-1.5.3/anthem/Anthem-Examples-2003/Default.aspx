<%@ Page Language="C#" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Default</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h1>Anthem.NET</h1>
			<p>Read an <a href="Overview.aspx">overview</a> about why Anthem exists and how it 
				works.</p>
			<p>Anthem requires zero <a href="Configuration.aspx">configuration</a> to use but 
				there is one setting you might want to add to your web.config file if you're still 
				using ASP.NET 1.1.</p>
			<h2>Examples</h2>
			<p>To learn how to use Anthem, try reading the following examples in the order 
				listed:</p>
			<ol>
				<li><a href="ButtonsAndLabels.aspx">Introduction using buttons and labels</a></li>
				<li><a href="MoreButtonProperties.aspx">More button properties</a></li>
				<li><a href="LinkButtons.aspx">Using link buttons</a></li>
				<li><a href="ImageButtons.aspx">Using image buttons</a></li>
				<li><a href="DropDownLists.aspx">Using drop down lists</a></li>
				<li><a href="PanelsAndVisibility.aspx">Toggling visibility on panels</a></li>
				<li><a href="InvokePageMethod.aspx">Invoking methods on pages</a></li>
				<li><a href="InvokeCustomControlMethod.aspx">Invoking methods on custom controls</a></li>
				<li><a href="InvokeUserControlMethod.aspx">Invoking methods on user controls</a></li>
				<li><a href="DynamicAnthemControls.aspx">Dynamically adding Anthem controls to the page</a></li>
				<li><a href="DynamicUserControls.aspx">Dynamically loading user controls with Anthem controls on them</a></li>
				<li><a href="UnhandledExceptions.aspx">What happens when an exception is not caught on the page?</a></li>
				<li><a href="RedirectingClientsOnErrors.aspx">Redirecting clients from Page_Error</a></li>
				<li><a href="MissingMethod.aspx">Invoking missing methods</a></li>
				<li><a href="MissingControl.aspx">Invoking methods on missing controls</a></li>
				<li><a href="DataGrid.aspx">Using data grids</a></li>
				<li><a href="DataList.aspx">Using data lists</a></li>
				<li><a href="Repeater.aspx">Using repeaters</a></li>
				<li><a href="RepeatersAndViewState.aspx">Using repeaters with disabled view state</a></li>
				<li><a href="ReturningDataSets.aspx">Returning data sets</a></li>
				<li><a href="RedirectingClients.aspx">Redirecting clients during call backs</a></li>
				<li><a href="InvokeClientSideFunctionAfterCallBack.aspx">Invoking client-side JavaScript functions after a call back returns</a></li>
				<li><a href="Validation.aspx">Integration with validation controls</a></li>
				<li><a href="ServerTransferExample.aspx">Support for server transfer</a></li>
				<li><a href="PreAndPostCallBackFunctions.aspx">Executing JavaScript on every call back</a></li>
				<li><a href="ListBox.aspx">Using list boxes</a></li>
				<li><a href="Image.aspx">Using images</a></li>
				<li><a href="CheckBox.aspx">Using check boxes</a></li>
				<li><a href="Table.aspx">Populating tables</a></li>
				<li><a href="DataBindDuringCallBack.aspx">Data binding during call backs</a></li>
				<li><a href="Calendar.aspx">Using calendar controls</a></li>
				<li><a href="RadioButtonList.aspx">Using radio buttons</a></li>
				<li><a href="Timer.aspx">Make call backs at regular intervals with the timer control</a></li>
				<li><a href="ConcurrentCallBacks.aspx">Making concurrent call backs</a></li>
				<li><a href="ResponseEncoding.aspx">Unicode support (requires Japanese language pack)</a></li>
				<li><a href="MemoryLeaks.aspx">Test to see if Anthem leaks memory when using XMLHttpRequest</a></li>
				<li><a href="MemoryLeaksWithUpdates.aspx">Another test to see if Anthem leaks memory when updating controls on the page</a></li>
				<li><a href="FileDownload.aspx">Test file download with InvokePageMethod</a></li>
				<li><a href="DropDownListsPrePost.aspx">DropDownList Pre and Post Callback Features</a></li>
				<li><a href="UpdatePanel.aspx">UpdatePanel converts postbacks into callbacks</a></li>
				<li><a href="TextBox.aspx">Using the TextBox control</a></li>
				<li><a href="PageScripts.aspx">Dynamically adding scripts to the page</a></li>	
				<li><a href="InvokeCustomControl.aspx">Invoke a custom control</a></li>							
				<li><a href="PreUpdate.aspx">Add a client script during PreUpdate event</a></li>							
				<li><a href="HyperLink.aspx">HyperLink Example</a></li>
				<li><a href="Extensions/Default.aspx">Anthem Extensions Examples</a></li>
				<li><a href="FileUpload.aspx">Uploading a file using Anthem controls</a></li>							
				<% if (Environment.Version.Major >= 2) { %>
				<li><a href="Default2.aspx">Examples specific to ASP.NET 2.0</a></li>
				<% } %>
			</ol>
		</form>
	</body>
</html>
