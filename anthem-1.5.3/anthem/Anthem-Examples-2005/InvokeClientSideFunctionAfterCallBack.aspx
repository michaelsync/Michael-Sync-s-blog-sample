<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("ClientSideFunction();");
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">

    <script type="text/javascript">
        function ClientSideFunction() {
            alert("Invoked!");
        }
    </script>

    <h2>
        Example</h2>
    <p>
        Click the button to invoke perform a call back to the server which will then invoke
        a client-side JavaScript function here on the client after the call back returns.</p>
    <anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />
</asp:Content>
