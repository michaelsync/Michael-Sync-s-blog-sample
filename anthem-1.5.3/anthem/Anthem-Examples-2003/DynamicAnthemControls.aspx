<%@ Page Language="C#" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title>DynamicAnthemControls</title>
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<h2>Description</h2>
			<p>Anthem controls can be dynamically created and added to the page just like all 
				other server-side controls. You need to watch out for the same things you watch 
				out for when dynamically adding non-Anthem controls.</p>
			<h2>Example</h2>
			<p>Click the button to add new buttons to the page. Click any of the new buttons to 
				update a label on the page. All of this is done using call backs.</p>
			<anthem:Button id="add" runat="server" Text="Add Button" OnClick="add_Click" />
			<anthem:Panel id="panel" runat="server" />
			<anthem:Label id="label" runat="server" />
			<h2>Remarks</h2>
			<p>It's up to you to remember what controls you added to the page so that you can 
				re-add them at the beginning of each call back. This is the same thing you'd 
				have to do during post backs.</p>
			<p>If you expect your dynamic controls to fire events during call backs, you need 
				to create them during or before the Load event.</p>
		</form>
	</body>
</html>
<script runat="server">

void Page_Load()
{
	for (int i = 1; i <= ButtonCount; ++i)
	{
		AddButton(i);
	}
}

private int ButtonCount
{
	get
	{
		int buttonCount = 0;
		if (ViewState["buttonCount"] != null)
		{
			buttonCount = (int)ViewState["buttonCount"];
		}
		return buttonCount;
	}
	
	set
	{
		ViewState["buttonCount"] = value;
	}
}

private void AddButton(int i)
{
	Anthem.Button newButton = new Anthem.Button();
	newButton.ID = "button" + i;
	newButton.Text = "Button " + i;
	newButton.Click += new EventHandler(dynamicButton_Click);
	panel.Controls.Add(newButton);
}

void add_Click(object sender, EventArgs e)
{
	ButtonCount += 1;
	AddButton(ButtonCount);
	panel.UpdateAfterCallBack = true;
}

void dynamicButton_Click(object sender, EventArgs e)
{
	Anthem.Button button = (Anthem.Button)sender;
	label.Text = "You clicked " + button.Text + "!";
	label.UpdateAfterCallBack = true;
}

</script>
