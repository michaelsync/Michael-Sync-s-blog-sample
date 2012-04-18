<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        button1.Text = DropDownList1.SelectedValue;
        button1.UpdateAfterCallBack = true;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Create the javascript functions that will be called before and after the callback
        string script = @"
function DropDownList1_PreCallBack(list) {
    if (!confirm('Are you sure you want to perform this call back?')) {
	    return false;
    }
    document.getElementById('" + this.button1.ClientID + @"').disabled = true;
}

function DropDownList1_PostCallBack(list) {
    document.getElementById('" + this.button1.ClientID + @"').disabled = false;
}

function DropDownList1_CallBackCanceled(list) {
    alert('Callback canceled.');
}";
        this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), script, script, true);
        
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

        You can invoke methods both before and after the callback. In this example, the
        pre-callback function asks you to confirm the callback. If you cancel the callback,
        the CallBackCancelledFunction is called.<br />
        <br />
        <anthem:DropDownList ID="DropDownList1" runat="server" 
            AutoCallBack="True"
            PreCallBackFunction="DropDownList1_PreCallBack"
            PostCallBackFunction="DropDownList1_PostCallBack" 
            OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged"
            CallBackCancelledFunction="DropDownList1_CallBackCanceled"
            TextDuringCallBack="working...">
            <asp:ListItem Value="Click Me!">Select One</asp:ListItem>
            <asp:ListItem>Audi</asp:ListItem>
            <asp:ListItem>Mercedes</asp:ListItem>
            <asp:ListItem>Ford</asp:ListItem>
        </anthem:DropDownList>
        <asp:Label ID="DropDownList1Label" runat="server" AssociatedControlID="DropDownList1" />
        <br />
        <br />
        <anthem:Button ID="button1" runat="server" Text="Click Me!" />
</asp:Content>
