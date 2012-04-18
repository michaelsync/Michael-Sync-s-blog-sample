<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

	void dropDownList1_SelectedIndexChanged(object sender, EventArgs e)
	{
		dropDownList2.Items.Clear();
		dropDownList3.Items.Clear();

		if (dropDownList1.SelectedIndex > 0)
		{
			dropDownList2.Items.Add(dropDownList1.SelectedValue + ".1");
			dropDownList2.Items.Add(dropDownList1.SelectedValue + ".2");
			dropDownList2.Items.Add(dropDownList1.SelectedValue + ".3");
		}

		dropDownList2.UpdateAfterCallBack = true;
		dropDownList3.UpdateAfterCallBack = true;
	}

	void dropDownList2_SelectedIndexChanged(object sender, EventArgs e)
	{
		dropDownList3.Items.Clear();

		dropDownList3.Items.Add(dropDownList2.SelectedValue + ".1");
		dropDownList3.Items.Add(dropDownList2.SelectedValue + ".2");
		dropDownList3.Items.Add(dropDownList2.SelectedValue + ".3");

		dropDownList3.UpdateAfterCallBack = true;
	}

</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
			<h2>Description</h2>
			<p>The <code>anthem:DropDownList</code> control can be populated inside a call back 
				and can also trigger call backs.</p>
			<p>In this example, you'll add three <code>anthem:DropDownList</code> controls to 
				the page so that selecting a value in the first will populate the second and 
				selecting a value in the second will populate the third.</p>
			<h2>Example</h2>
			<anthem:DropDownList id="dropDownList1" runat="server" AutoCallBack="true" OnSelectedIndexChanged="dropDownList1_SelectedIndexChanged">
			    <Items>
		            <asp:ListItem />
		            <asp:ListItem>1</asp:ListItem>
		            <asp:ListItem>2</asp:ListItem>
		            <asp:ListItem>3</asp:ListItem>
		        </Items>
			</anthem:DropDownList>
			<anthem:DropDownList id="dropDownList2" runat="server" AutoCallBack="true" OnSelectedIndexChanged="dropDownList2_SelectedIndexChanged" />
			<anthem:DropDownList id="dropDownList3" runat="server" />
			<h2>Steps</h2>
			<ol>
				<li>
					<p>Add a <code>Register</code> directive to the top of your page:</p>
					<pre><code><strong>&lt;%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %&gt;</strong></code></pre>
				</li>
				<li>
					<p>Add an <code>anthem:DropDownList</code> control to your page containing three 
						items:</p>
					<pre><code><strong>&lt;anthem:DropDownList id="dropDownList1" runat="server"&gt;
	&lt;asp:ListItem Text="" /&gt;
	&lt;asp:ListItem&gt;1&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;2&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;3&lt;/asp:ListItem&gt;
&lt;/anthem:DropDownList&gt;</strong></code></pre>
                </li>
				<li>
					<p>Add two more <code>anthem:DropDownList</code> controls to the page without any 
						items:</p>
					<pre><code><strong>&lt;anthem:DropDownList id="dropDownList2" runat="server" /&gt;
&lt;anthem:DropDownList id="dropDownList3" runat="server" /&gt;</strong></code></pre>
                </li>
				<li>
					<p>Add <code>AutoCallBack</code> properties to the first two lists and set their 
						values to <code>true</code>:</p>
					<pre><code>&lt;anthem:DropDownList id="dropDownList1" runat="server" <strong>AutoCallBack="true"</strong>&gt;
	&lt;asp:ListItem Value="none" Text="" /&gt;
	&lt;asp:ListItem&gt;1&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;2&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;3&lt;/asp:ListItem&gt;
&lt;/anthem:DropDownList&gt;
&lt;anthem:DropDownList id="dropDownList2" runat="server" <strong>AutoCallBack="true"</strong> /&gt;</code></pre>
				</li>
				<li>
					<p>Add <code>SelectedIndexChanged</code> event handlers to the first two lists. 
						They can by double-clicking on the controls in the designer or adding <code>OnSelectedIndexChanged</code>
						attributes to the <code>anthem:DropDownList</code> tags:</p>
					<pre><code>&lt;anthem:DropDownList id="dropDownList1" runat="server" AutoCallBack="true"
	<strong>OnSelectedIndexChanged="dropDownList1_SelectedIndexChanged"</strong>
&gt;
	&lt;asp:ListItem Value="none" Text="" /&gt;
	&lt;asp:ListItem&gt;1&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;2&lt;/asp:ListItem&gt;
	&lt;asp:ListItem&gt;3&lt;/asp:ListItem&gt;
&lt;/anthem:DropDownList&gt;
&lt;anthem:DropDownList id="dropDownList2" runat="server" AutoCallBack="true"
	<strong>OnSelectedIndexChanged="dropDownList2_SelectedIndexChanged"</strong>
/&gt;</code></pre>
				</li>
				<li>
					<p>Implement the handlers for these events so that they populate the next list 
						after the list that fired the event:</p>
					<pre><code><strong>&lt;script runat="server"&gt;

void dropDownList1_SelectedIndexChanged(object sender, EventArgs e)
{
	dropDownList2.Items.Clear();
	dropDownList3.Items.Clear();

	if (dropDownList1.SelectedIndex &gt; 0)
	{
		dropDownList2.Items.Add(dropDownList1.SelectedValue + ".1");
		dropDownList2.Items.Add(dropDownList1.SelectedValue + ".2");
		dropDownList2.Items.Add(dropDownList1.SelectedValue + ".3");
	}

	dropDownList2.UpdateAfterCallBack = true;
	dropDownList3.UpdateAfterCallBack = true;
}

void dropDownList2_SelectedIndexChanged(object sender, EventArgs e)
{
	dropDownList3.Items.Clear();

	dropDownList3.Items.Add(dropDownList2.SelectedValue + ".1");
	dropDownList3.Items.Add(dropDownList2.SelectedValue + ".2");
	dropDownList3.Items.Add(dropDownList2.SelectedValue + ".3");

	dropDownList3.UpdateAfterCallBack = true;
}

&lt;/script&gt;</strong></code></pre>
				</li>
			</ol>
			<h2>Remarks</h2>
			<p>Please note that the <code>SelectedIndexChanged</code> event is the same event 
				as the "normal" <code>asp:DropDownList</code> control.</p>
			<p>The <code>AutoCallBack</code> property is the Anthem equivalent of the <code>AutoPostBack</code>
				property.</p>
</asp:Content>
