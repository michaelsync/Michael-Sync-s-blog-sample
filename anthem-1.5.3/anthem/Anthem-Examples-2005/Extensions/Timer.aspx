<%@ Page Language="C#" %>

<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    protected void Button_Click(object sender, EventArgs e) {
        Anthem.Button btn = (Anthem.Button)sender;
        switch (btn.CommandArgument) {
            case "start" : 
                if (timer.State == TimerState.Started)
                    this.SendMsg("Timer is already started");
                timer.Start(); 
                break;
                
            case "stop" : 
                if (timer.State == TimerState.Stopped)
                    this.SendMsg("Timer is already stopped");
                timer.Stop(); 
                break;
                
            case "pause"  : 
                switch (timer.State) {
                    case TimerState.Stopped : this.SendMsg("Timer is stopped already"); break;
                    case TimerState.Paused  : this.SendMsg("Timer is already paused"); break;
                }
                timer.Pause(); 
                break;
                
            case "resume" : 
                switch (timer.State) {
                    case TimerState.Started : this.SendMsg("Timer is already started"); break;
                    case TimerState.Stopped : this.SendMsg("Timer is stopped"); break;
                }                
                timer.Resume(); 
                break;
                
            case "reset"  : 
                timer.Reset();                 
                break; 
        }
    }

    private void SendMsg(string msg) {
        Anthem.Manager.AddScriptForClientSideEval("alert('" + msg + "');");
    }

    protected void btnPostBack_Click(object sender, EventArgs e) {
    }
        
    protected void timer_Elapsed(object sender, EventArgs e) {
        Anthem.Manager.AddScriptForClientSideEval("alert('Saying hi from server side code on timer elapsed')");
    }

    protected void cbLoop_CheckedChanged(object sender, EventArgs e) {
        timer.Loop = cbLoop.Checked;    
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Timer Improved</title>
</head>
<body>
    <script type="text/javascript" language="javascript">
        function timerElapsed(sender) {
            alert("timer elapsed client script running.  ID : " + sender.id);
        }
    </script>
    <form id="form1" runat="server">
    <asp:Button ID="btnPostBack" Text="Do Postback" OnClick="btnPostBack_Click" runat="server" />
    <anthem:Debugger id="debugger" runat="Server" />
        <anthem:CheckBox ID="cbLoop" AutoCallBack="true" Checked="true" Text="Loop" runat="server" OnCheckedChanged="cbLoop_CheckedChanged" />
        <anthem:Button ID="Button1" Text="Start" CommandArgument="start" CausesValidation="false" runat="server" OnClick="Button_Click" />
        <anthem:Button ID="Button5" Text="Stop" CommandArgument="stop" CausesValidation="false" runat="server" OnClick="Button_Click" />
        <anthem:Button ID="Button2" Text="Pause" CommandArgument="pause" CausesValidation="false" runat="server" OnClick="Button_Click" />
        <anthem:Button ID="Button3" Text="Resume" CommandArgument="resume" CausesValidation="false" runat="server" OnClick="Button_Click" />
        <anthem:Button ID="Button4" Text="Reset" CommandArgument="reset" CausesValidation="false" runat="server" OnClick="Button_Click" />
        <anthemext:Timer ID="timer" OnClientElapsed="timerElapsed" TextDuringCallBack="in callback" IntervalSeconds="10" FinalSecondsBlink="false" FinalSecondsThreshold="5" runat="server" OnElapsed="timer_Elapsed">
            <FinalSecondsStyle ForeColor="Red" />
        </anthemext:Timer>
    </form>
</body>
</html>
