<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    /// <summary>
    /// Note that you must add all controls to the page on *every* Load. And they
    /// must be added in exactly the same order each time or ASP.NET will not be
    /// able to match up the form values with the controls.
    /// </summary>
    void Page_Load()
    {
        for (int i = 1; i <= ButtonCount; ++i)
        {
            AddButton(i);
        }
    }

    /// <summary>
    /// Button count is stored in the ViewState so that we know exactly how 
    /// many buttons to recreate during Load even if the Session state is lost.
    /// </summary>
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

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Description</h2>
    <p>
        Anthem controls can be dynamically created and added to the page just like all other
        server-side controls. You need to watch out for the same things you watch out for
        when dynamically adding non-Anthem controls.</p>
    <h2>
        Example</h2>
    <p>
        Click the button to add new buttons to the page. Click any of the new buttons to
        update a label on the page. All of this is done using call backs.</p>
    <anthem:Button ID="add" runat="server" Text="Add Button" OnClick="add_Click" />
    <anthem:Panel ID="panel" runat="server" />
    <anthem:Label ID="label" runat="server" />
    <h2>
        Remarks</h2>
    <p>
        It's up to you to remember what controls you added to the page so that you can re-add
        them at the beginning of each call back. This is the same thing you'd have to do
        during post backs.</p>
    <p>
        If you expect your dynamic controls to fire events during call backs, you need to
        create them during or before the Load event.</p>
</asp:Content>
