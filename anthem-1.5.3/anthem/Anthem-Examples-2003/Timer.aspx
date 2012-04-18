<%@ Page Language="C#" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Timer Control Sample Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div>This timer is ticking every three seconds. The timer starts immediately when the page loads.<br />
            --&gt;<anthem:Label ID="label1" runat="server">Started</anthem:Label>&lt;--
            <anthem:Timer ID="timer1" runat="server" Enabled="true" Interval="3000" OnTick="timer1_Tick" />
            </div>
            <hr />
            <div>This timer will tick every two seconds. The timer will start when you click on Start Timer.<br />
            --&gt;<anthem:Label ID="label2" runat="server">Stopped</anthem:Label>&lt;--
            <anthem:Timer ID="timer2" runat="server" Enabled="false" Interval="2000" OnTick="timer2_Tick" />
            <br />
            <anthem:Button ID="stopTimer" runat="server" Text="Stop Timer" OnClick="stopTimer_Click" />
            <anthem:Button ID="startTimer" runat="server" Text="Start Timer" OnClick="startTimer_Click" />
            </div>
            <p>You can trigger other call backs to the server while the timer is ticking. This shouldn't affect the timer (much).</p>
            <anthem:Button ID="callBack" runat="server" Text="Call Back" OnClick="callBack_Click" /><br />
            <anthem:Label ID="callbacklabel" runat="server" />
        </div>
    </form>
</body>
</html>

<script runat="server">

    void timer1_Tick(object sender, EventArgs e)
    {
        label1.Text = DateTime.Now.ToString();
        label1.UpdateAfterCallBack = true;
    }
    
    void timer2_Tick(object sender, EventArgs e)
    {
        label2.Text = DateTime.Now.ToString();
        label2.UpdateAfterCallBack = true;
    }

    void stopTimer_Click(object sender, EventArgs e)
    {
        timer2.StopTimer();
        label2.Text = "Stopped";
        label2.UpdateAfterCallBack = true;
    }

    void startTimer_Click(object sender, EventArgs e)
    {
        timer2.StartTimer();
        label2.Text = "Started";
        label2.UpdateAfterCallBack = true;
    }

    protected void callBack_Click(object sender, EventArgs e)
    {
        callbacklabel.Text = DateTime.Now.ToString();
        callbacklabel.UpdateAfterCallBack = true;
    }
</script>

