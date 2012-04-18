<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<script runat="server">

    void button_Click(object sender, EventArgs e)
    {
        label.Text = DateTime.Now.ToString();
        //label.UpdateAfterCallBack = true;
    }

</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <p>
        This example will introduce you to Anthem and show how similar it is to "normal"
        ASP.NET development.</p>
    <p>
        You're going to add an <code>anthem:Button</code> and an <code>anthem:Label</code>
        control to a page. Clicking the button will modify the label to contain the current
        date and time.</p>
    <h2>
        Example</h2>
    <p>
        Click the button to update the label next to it with the current time.</p>
    <anthem:Button ID="button" runat="server" Text="Click Me!" OnClick="button_Click" />
    <anthem:Label ID="label" runat="server" />
    <h2>
        Steps</h2>
    <ol>
        <li>
            <p>
                Add a <code>Register</code> directive to the top of your page:</p>
            <pre><code><strong>&lt;%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %&gt;</strong></code></pre>
            <li>
                <p>
                    Add an <code>anthem:Button</code> control to your page:</p>
                <pre><code><strong>&lt;anthem:Button id="button" runat="server" Text="Click Me!" /&gt;</strong></code></pre>
                <li>
                    <p>
                        Add an <code>anthem:Label</code> control to your page:</p>
                    <pre><code><strong>&lt;anthem:Label id="label" runat="server" /&gt;</strong></code></pre>
                    <li>
                        <p>
                            Add a handler for the button's <code>Click</code> event either by double-clicking
                            on the button in the designer or by adding an <code>OnClick</code> attribute to
                            the button's tag:</p>
                        <pre><code>&lt;anthem:Button id="button" runat="server" Text="Click Me!" <strong>OnClick="button_Click"</strong>
                            /&gt;</code></pre>
                        <li>
                            <p>
                                Implement the handler in your code behind class or in a server-side script block
                                in your page. Set the <code>Text</code> property like a normal <code>Label</code>
                                control but make sure you set the <code>UpdateAfterCallBack</code> property to true
                                or you won't see the change reflected on the page:</p>
<pre><code><strong>&lt;script runat="server"&gt;

void button_Click(object sender, EventArgs e) 
{ 
    label.Text = DateTime.Now.ToString(); 
    label.UpdateAfterCallBack = true; 
} 
    
&lt;/script&gt;</strong></code></pre>
                        </li>
    </ol>
    <h2>
        Remarks</h2>
    <p>
        As you can see, an <code>anthem:Button</code> control is just like an <code>asp:Button</code>
        control. It has the same <code>Text</code> property, the same <code>Click</code>
        event, and everything else the <code>asp:Button</code> control has. This is because
        the <code>Anthem.Button</code> class derives from the <code>System.Web.UI.WebControls.Button</code>
        class.</p>
    <p>
        Likewise, the <code>anthem:Label</code> control is just like the <code>asp:Label</code>
        control. The only difference visible in this example is the setting of the <code>UpdateAfterCallBack</code>
        property. If that wasn't done, the Anthem manager would not have "returned" the
        new HTML for that label. Each control that you want updated on the client page needs
        to have its <code>UpdateAfterCallBack</code> property set to true. Alternatively,
        you can set <code>AutoUpdateAfterCallBack</code> to true on the element's tag in
        your .aspx file to have it always update itself after a call back (even if it doesn't
        need to).</p>
</asp:Content>
