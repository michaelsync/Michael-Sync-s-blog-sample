<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>

<script runat="server">

    protected void EditLabel2_Edit(object sender, EventArgs e)
    {
        Label1.Text = "Editing";
        Label1.UpdateAfterCallBack = true;
    }

    protected void EditLabel2_Save(object sender, EditLabelSaveEventArgs e)
    {
        Label1.Text = "Saved";
        Label1.UpdateAfterCallBack = true;
        System.Threading.Thread.Sleep(500);
    }
    
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    <script type="text/javascript">
        function pre() {
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
            loading.innerHTML = "busy...";
            document.body.appendChild(loading);
        }
        function post() {
            var loading = document.getElementById("loading");
            document.body.removeChild(loading);
        }
    </script>

    <h1>
        Edit Label Demo Page</h1>
    <p>
        The EditLabel is a Label that can be edited in place. If you are using Internet
        Explorer, then you can literally edit in place. If you are using any other browser,
        then the label is replaced by a textbox to allow editing and then back to a label
        when you are done editing.</p>
    <h2>
        Simple EditLabel</h2>
    <p>
        Just click on the label to start editing. Tab out (or click anywhere else on the
        page) to save your changes.</p>
    <p>
        <anthemext:EditLabel ID="EditLabel1" runat="server" Text="Click Me!" Font-Italic="True" />
    </p>
    <h2>
        EditLabel with Save and Edit Events</h2>
    <p>
        In this example, the Save and Edit events are handled on the server and used to
        update the status (which is an anthem:Label). The TextDuringCallBack option is used
        to display a simple wait message during the callbacks. The PreCallBackFunction and
        PostCallBackFunction are used to display a busy notification in the upper right
        corner of the page. The Save operation is artificially delayed to highlight the
        two notifications.</p>
    <p>
        <anthemext:EditLabel ID="EditLabel2" runat="server" OnEdit="EditLabel2_Edit" Text="Click Me!"
            OnSave="EditLabel2_Save" Font-Italic="True" TextDuringCallBack="Wait..." PreCallBackFunction="pre"
            PostCallBackFunction="post" />
        <asp:Label ID="Label2" runat="server" AssociatedControlID="EditLabel2" /><br />
        Status:
        <anthem:Label ID="Label1" runat="server"></anthem:Label></p>
</asp:Content>
