<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>

<script runat="server">

    protected void cal_SelectionChanged(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);
        this.Label1.Text = cal.SelectedDate.ToString();
    }

    protected void cal_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
    {
        System.Threading.Thread.Sleep(500);
    }
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>
        Extended Calendar Demo Page</h1>
    <p>The Calendar control in the AnthemExtensions library has a MouseOver property that is applied to each day.</p>
    <p>
        There is an artificial delay in each event so you can see the TextDuringCallBack.</p>
    <anthemext:Calendar ID="cal" runat="server" TextDuringCallBack="&lt;img src=&quot;tiny_red.gif&quot; border=0 /&gt;"
        OnSelectionChanged="cal_SelectionChanged" OnVisibleMonthChanged="cal_VisibleMonthChanged">
        <MouseOverStyle BackColor="Green" />
        <DayStyle BackColor="White" />
    </anthemext:Calendar>
    <br />
    <anthem:Label ID="Label1" runat="server" AutoUpdateAfterCallBack="True" BorderColor="Gray"
        BorderStyle="Solid" BorderWidth="1px" UpdateAfterCallBack="True" Width="257px"></anthem:Label>
</asp:Content>
