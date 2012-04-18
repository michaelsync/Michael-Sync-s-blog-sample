<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        object o = null;
        o.ToString();
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <p>
        Click the button to throw an unhandled exception on the server. You should see a
        message box appear telling you there was a <code>NullReferenceException</code> on
        the server.</p>
    <anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />

    <script type="text/javascript">
				function Anthem_Error(result) {
					alert('Anthem_Error was invoked with the following error message: ' + result.error);
				}
    </script>

    <p>
        The message box appeared because of the custom <code>Anthem_Error</code> function
        defined on this page (view source to see it). Whenever attempting to do a call back
        returns an error, <code>Anthem_Error</code> gets invoked, if it exists.</p>
</asp:Content>
