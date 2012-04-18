<%@ Page Language="C#" MasterPageFile="~/Sample.master" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" Runat="Server">
<p>The Anthem.NET Core Library is a collection of controls that inherit from corresponding
    controls in System.Web.UI.HtmlControls and System.Web.UI.WebControls. Every control
    in the Core Library implements the Anthem.IUpdatableControl interface. This interface
    adds an important feature to every control in the Core Library: you can update the
    appearance of the control during a callback (i.e. change the text, make it visible,
    etc.).</p>
    <p>
        Controls that normally cause postbacks also implement the Anthem.ICallBack interface.
        This interface converts the postback into a callback. Controls that implement the
        Anthem.ICallBack interface include Button, and CheckBox if AutoCallBack=true.</p>
    <p>
        Controls that can contain other controls implement the Anthem.ICallBackContainer
        interface. This interface converts each child control postback into a callback.
        Controls that implement the Anthem.ICallBackContainer interface include DataGrid,
        GridView and Panel.</p>
    <p>
        In most cases converting an existing page that uses ASP.NET controls into a page
        that uses Anthem controls is trivial. For example, you can replace:</p>
    <pre>
&lt;asp:Button id="button" runat="server" text="Click" OnClick="button_Click" &nbsp;/&gt;</pre>
    <p>
        with</p>
    <pre>
&lt;anthem:Button id="button" runat="server" text="Click" OnClick="button_Click" /&gt;</pre>
    <p>
        The &lt;anthem:Button&gt; supports all of the same properties that &lt;asp:Button&gt;
        supports. The only difference between these two controls is that the first will
        make a postback when you click on the button, while the Anthem version will make
        a callback.</p>
</asp:Content>

