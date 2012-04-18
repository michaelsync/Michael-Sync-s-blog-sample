<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void Page_Load()
    {
        Anthem.Manager.Register(this);
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Description</h2>
    <p>
        Starting with Anthem 1.1.0, trying to invoke methods on controls that can't be found
        on the server will return an error of "CONTROLNOTFOUND".</p>
    <p>
        This is a developer error. It probably means one or more of the following:</p>
    <ul>
        <li>The control is being dynamically added to the page and you're not doing that during
            the call back</li>
        <li>You spelled the ID of the control wrong in the call to <code>Anthem_InvokeControlMethod</code></li>
        <li>You used the control's <code>ID</code> instead of its <code>ClientID</code> in the
            call to <code>Anthem_InvokeControlMethod</code> (this could be a problem when the
            control is contained by other controls)</li>
    </ul>
    <h2>
        Example</h2>
    <p>
        Click the button to try to invoke a method on a control that doesn't exist on the
        server.</p>
    <button onclick="Anthem_InvokeControlMethod('MissingControlID', 'MissingMethodName', null, function(result) { alert(result.error); }); return false">
        Invoke method on missing control</button>
</asp:Content>
