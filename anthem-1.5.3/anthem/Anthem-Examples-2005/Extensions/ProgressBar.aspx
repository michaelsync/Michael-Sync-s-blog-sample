<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>

<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        Anthem.Manager.Register(this);

        // you can set these properties in the control definition (below) also
        // but here is where you can override those set by the properties (at design time)
        this.ProgressBar1.BackGroundColor = "yellow"; // example - default is 'red' but set 
        // to 'green' in property definition 
        // below but you can set it here to 'yellow'
        this.ProgressBar1.CallBackInterval = 200;
        // barwidth should be x times 50 with a minimum of 50 and up to 250 (otherwise it defaults to 100)
        this.ProgressBar1.BarWidth = 150;
        // multipler should be (BarWidth / 50), otherwise it will default to 2 (100 / 2)
        this.ProgressBar1.Multiplier = 3;
    }

    [Anthem.Method]
    public int UpdateCounter(int iCounter)
    {
        System.Threading.Thread.Sleep(100);
        iCounter++;
        return iCounter;
    }
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h1>
        ProgressBar Demo Page</h1>
    <p>
        The ProgressBar is a control that is able to report real-time progress of a server-side
        process's progress. This works as follows: a server-side method is executed repetitively
        and returns the current progress of the work that is done on the server. This could
        be a very slow database query, or heavy data computing, for example. The example
        is tested in IE 6.0+ and FF 1.x+ on PC, and FF 1.x+ and Safari 1.2/2.0 on Mac.</p>
    <h2>
        ProgressBar</h2>
    <p>
        Just click on the button and the progressBar will start to fill. Note that the status
        line is also used to reflect that work is being done on the server.</p>
    <div>
        <anthemext:ProgressBar ID="ProgressBar1" runat="server" 
            BackGroundColor="green" 
            AutoUpdateAfterCallBack="False"
            Width="110px">
        </anthemext:ProgressBar><br />
        <input id="btnGetDataHTML" type="button" value="Get Data" onclick="populateTable();" />
    </div>
</asp:Content>
