<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>

<script runat="server">

    protected void cal_SelectionChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
    }

    protected void cal_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        System.Threading.Thread.Sleep(500);
    }
</script>

<asp:Content ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	<h2>Example</h2>
	<p>There is an artificial delay in each event so you can see the TextDuringCallBack.</p>
	<anthem:Calendar id="cal" runat="server"
	    TextDuringCallBack="&lt;img src=&quot;tiny_red.gif&quot; border=0 /&gt;"
	    OnSelectionChanged="cal_SelectionChanged"
	    OnVisibleMonthChanged="cal_VisibleMonthChanged" />
</asp:Content>