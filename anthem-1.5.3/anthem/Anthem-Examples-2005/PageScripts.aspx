<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<script runat="server">

    protected void button1_Click(object sender, EventArgs e)
    {
        // Add a new button to the page for fun...this button will
        // call a javascript function that is also added to the page
        HtmlButton button = new HtmlButton();
        button.InnerText = "Now click me!";
        button.Attributes["onclick"] = "ThankYou();";
        placeholder1.Controls.Add(button);
        placeholder1.UpdateAfterCallBack = true;

        // Add a simple javascript function to the page
        string script = @"
function ThankYou() {
  alert('Thank you!');
}";
        Anthem.Manager.RegisterClientScriptBlock(typeof(Page), script, script, true);
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>
        Page Script Examples</h1>
    <p>
        When you click on the button two things will happen:</p>
    <ol>
        <li>A new button will be added to the page. When you click on the new button it will
            execute a javascript function called ThankYou(). The function is not yet part of
            the page (go ahead and check the page source).</li>
        <li>A new script block is registered with the page. The script block includes the ThankYou()
            function.</li>
    </ol>
    <p>
        If the button caused a postback, the new script would be part of the page when the
        postback response was rendered by the browser. But the button causes a callback,
        which does not normally add scripts to the page. So after registering the new script
        block, we set <code>Anthem.Manager.IncludePageScripts = true;</code>.</p>
    <anthem:Button ID="button1" runat="server" Text="Click Me!" OnClick="button1_Click" />
    <br />
    <anthem:PlaceHolder ID="placeholder1" runat="server" />
</asp:Content>
