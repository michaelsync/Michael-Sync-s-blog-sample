<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register TagPrefix="uc" TagName="validation" Src="~/Validation.ascx" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div>
        <h1>
            Validation</h1>
        <p>
            ASP.NET supports both client-side and server-side validation. ASP.NET enables client-side
            validation by writing certain javascript into the page. Anthem enables client-side
            validation by injecting that same javascript into an existing page. Anthem removes
            client-side validation by removing critical pieces of that javascript.</p>
        <p>
            This page demonstrates several validation controls using both server-side and client-side
            validation. Note the following limitations in ASP.NET (don't blame Anthem):</p>
        <ul>
            <li>ASP.NET 1.0 and 1.1 only support client-side validation if you have Internet Explorer
                (you can create a &lt;browserCaps&gt; element block in your web.config to enable
                client-side validation with other browsers, but the validation scripts will probably
                not work).</li>
            <li>ASP.NET 1.0 and 1.1 ignore the ValidatonGroup parameter. Even though this page will
                run in ASP.NET 1.x, all the validators will fire for every validation.</li>
        </ul>
        <asp:Panel ID="panel" runat="server" BorderColor="LightSteelBlue" BorderStyle="Solid"
            BorderWidth="2px" Style="padding: 1em">
            <uc:validation id="val" runat="server" />
        </asp:Panel>
    </div>
</asp:Content>
