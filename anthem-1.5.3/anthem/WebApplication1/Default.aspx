<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="WebApplication1._Default" %>
    <%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
    
    <script runat="server">
        protected void btnSubmit_Click(object sender, EventArgs e) {
            lblError.Visible = true;
            lblError.Text = "Fuck it!! " + DateTime.Now.ToString();
            lblError.UpdateAfterCallBack = true;
        }
    </script>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to ASP.NET!
    </h2>
    <p>
        To learn more about ASP.NET visit <a href="http://www.asp.net" title="ASP.NET Website">www.asp.net</a>.

        <%--<anthem:Label ID="lblError" runat="server" AutoUpdateAfterCallBack="True" ForeColor="Red"
                UpdateAfterCallBack="True" Visible="true" Text="F"></anthem:Label>--%>
                <anthem:Label id="lblError" runat="server" Text="Right here" Visible="true"/>
    </p>
    <p>
        You can also find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.
             <anthem:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                 />
    </p>
</asp:Content>
