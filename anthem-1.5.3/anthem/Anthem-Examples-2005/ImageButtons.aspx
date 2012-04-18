<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>

<script runat="server">
    void imageButton_Click(object sender, ImageClickEventArgs e)
    {
        coordinates.Text = string.Format("x={0}, y={1}", e.X, e.Y);
        coordinates.UpdateAfterCallBack = true;
        System.Threading.Thread.Sleep(500);
    }

    protected string[] MyItems = { string.Empty };

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            DataBind();
    }
</script>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>
        Description</h1>
    <p>
        The <code>anthem:ImageButton</code> control works just like the <code>anthem:Button</code>
        control. Instead of having <code>Text</code> and <code>TextDuringCallBack</code>
        properties, the <code>anthem:ImageButton</code> control has <code>ImageUrl</code>
        and <code>ImageUrlDuringCallBack</code> properties.</p>
    <p>
        The image buttons in this example sleeps for two seconds in the <code>Click</code>
        handler so we can see the effect of the <code>ImageUrlDuringCallBack</code> property.</p>
    <p>
        The first two buttons are <code>anthem:ImageButton</code>'s and the second is an
        <code>asp:ImageButton</code> so we can verify that the client coordinates are accurate.</p>
    <h2>
        Example</h2>
    <!-- The samples are inside of a Repeater (a NamingContainer) so that they get assigned
			     complicated ids. This is to verify that the click coordinates are working. -->
    <asp:Repeater ID="samples" runat="server" DataSource="<%# MyItems %>">
        <ItemTemplate>
            <anthem:ImageButton ID="imageButton" runat="server" ImageUrl="button_run.gif" ImageUrlDuringCallBack="button_run-down.gif"
                OnClick="imageButton_Click" />
            <anthem:ImageButton ID="imageButton1" runat="server" ImageUrl="button_run.gif" TextDuringCallBack="wait"
                OnClick="imageButton_Click" />
            <asp:ImageButton ID="imageButton2" runat="server" ImageUrl="~/button_run.gif" OnClick="imageButton_Click" />
        </ItemTemplate>
    </asp:Repeater>
    <div>
        Coordinates:
        <anthem:Label ID="coordinates" runat="server" />
    </div>
</asp:Content>
