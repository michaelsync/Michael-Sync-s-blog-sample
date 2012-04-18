<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        object o = null;
        o.ToString();
    }

    void Page_Error()
    {
        Response.Redirect("http://anthem-dot-net.sourceforge.net/");
    }

</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <p>
        Click the button to trigger an unhandled exception on the server. This should redirect
        you to the Anthem home page.</p>
    <anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />
    <p>
        There is really nothing fancy here, just:</p>
<pre><code>
&lt;script runat="server"&gt;
    void Page_Error()
    { 
        Response.Redirect("http://anthem-dot-net.sourceforge.net/");
    }
&lt;/script&gt;
</code></pre>        
    <p>
        You could also do this with the Anthem_Error function but doing it in Page_Error
        lets you do server-side processing like logging the error.</p>
</asp:Content>
