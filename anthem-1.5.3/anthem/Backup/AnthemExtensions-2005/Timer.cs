using System;
using System.IO;
using System.Web;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;
using System.Drawing;
using System.ComponentModel;
using System.Collections.Specialized;
using Anthem;

#if V2
[assembly: WebResource("AnthemExtensions.Timer.js", "text/javascript")]
#endif

namespace AnthemExtensions {
    public enum TimerState {
        Started,
        Stopped,
        Paused
    }

    [DefaultEvent("Elapsed")]
    public class Timer : Anthem.Label, IPostBackEventHandler, ICallBackControl {
        private static readonly object EventElapsed;
        private int interval;
        private bool loop;
        private bool showCountDown;
        private string onClientElapsed;
        private TimerState timerState;
        private bool finalSecondsBlink;
        private int finalSecondsThreshold;
        private ASP.Style finalSecondsStyle;

        #region ctor
        public Timer() {
            this.interval = 10;
            this.loop = true;
            this.showCountDown = true;
            this.onClientElapsed = "";
            this.finalSecondsBlink = false;
            this.finalSecondsThreshold = 0;
            this.timerState = TimerState.Started;
        }

        static Timer() {
            EventElapsed = new object();
        }

        #endregion

        #region Overrides
        protected override void OnInit(EventArgs e) {
#if V2
            base.Page.RegisterRequiresControlState(this);
#endif
            Utils.RegisterScriptInclude(this, "AnthemExtensions.Timer.js");
            base.OnInit(e);
        }

        //function AnthemTimer(id, label, showCountDown, countDownTimeFinal, blinkFinal, styleFinal, loopTimer, callbackTimer, onClientElapsed, serverMethod)
        protected override void OnPreRender(EventArgs e) {
            base.OnPreRender(e);
            if (this.Enabled) {
#if V2
                string jsFunc = (this.EnableCallBack ? Manager.GetCallbackEventReference(this, "elapsed", false, null, this.TextDuringCallBack) :
                                                       base.Page.ClientScript.GetPostBackEventReference(this, "elapsed", false));
#else
                string jsFunc = (this.EnableCallBack ? Manager.GetCallbackEventReference(this, "elapsed", false, null, null) :
                                                       base.Page.GetPostBackEventReference(this, "elapsed"));
#endif
                jsFunc = jsFunc.Replace("this", this.ClientID); // this will equate to the timer object, that would be wrong, change to the clientID of the label
                string finalStyleString = Utils.GetStyleString(this.finalSecondsStyle);
                if (finalStyleString == "")
                    finalStyleString = "\"\"";
                string register = String.Format("var {0}_timer = new AnthemTimer(\"{0}_timer\", \"{0}\", {1}, {2}, {3}, {4}, \"{5}\", {6}, {7}, {8});{0}_timer.Start();",
                                                this.ClientID, // 0
                                                Utils.ToStringBool(this.ShowCountDown), // 1
                                                this.finalSecondsThreshold, // 2
                                                Utils.ToStringBool(this.finalSecondsBlink), // 3
                                                finalStyleString, // 4
                                                Utils.ToStringBool(this.Loop), // 5
                                                (this.IntervalSeconds * 1000), // 6
                                                Utils.ToJavascriptFunction(this.OnClientElapsed, "this"), // 7
                                                Utils.ToJavascriptFunction(jsFunc)); // 8
                Utils.RegisterScript(this, this.ClientID, register);
            }
        }

#if V2
        protected override object SaveControlState() {
            return new object[] { 
                base.SaveControlState(),
#else
        protected override object SaveViewState() {
            return new object[] { 
                base.SaveViewState(),
#endif
                this.interval,
                this.loop,
                this.showCountDown,
                this.onClientElapsed,
                this.timerState
            };
        }

#if V2
        protected override void LoadControlState(object savedState) {
            object[] array = (object[])savedState;
            base.LoadControlState(array[0]);
#else
        protected override void LoadViewState(object savedState) {
            object[] array = (object[])savedState;
            base.LoadViewState(array[0]);
#endif
            this.interval = (int)array[1];
            this.loop = (bool)array[2];
            this.showCountDown = (bool)array[3];
            this.onClientElapsed = (string)array[4];
            this.timerState = (TimerState)array[5];
        }

        #endregion

        #region Methods
        protected virtual void OnElapsed(EventArgs e) {
            EventHandler elapsedHandler = (EventHandler)base.Events[Timer.EventElapsed];
            if (elapsedHandler != null)
                elapsedHandler(this, e);
            if (!this.Loop && this.State != TimerState.Stopped)
                this.Stop();
        }

        public void Reset() {
            this.SendCommand("Reset");
        }

        public void Pause() {
            this.timerState = TimerState.Paused;
            this.SendCommand("Pause");
        }

        public void Resume() {
            this.timerState = TimerState.Started;
            this.SendCommand("Resume");
        }

        public void Stop() {
            this.timerState = TimerState.Stopped;
            this.SendCommand("Stop");
        }

        public void Start() {
            this.timerState = TimerState.Started;
            this.SendCommand("Start");
        }

        private void SendCommand(string cmd) {
            string script = String.Format("{0}_timer.{1}();", this.ClientID, cmd);
            Utils.SendScript(this, script);
        }

        #endregion

        #region Properties
        public event EventHandler Elapsed {
            add { base.Events.AddHandler(Timer.EventElapsed, value); }
            remove { base.Events.RemoveHandler(Timer.EventElapsed, value); }
        }

        [Browsable(false)]
        public TimerState State {
            get { return this.timerState; }
        }

        [DefaultValue(true)]
        [Description("Tells the timer to continue looping after the callback has been fired")]
        public bool Loop {
            get { return this.loop; }
            set { this.loop = value; }
        }

        [DefaultValue(10)]
        [Description("Number of seconds before firing callback event")]
        public int IntervalSeconds {
            get { return this.interval; }
            set { this.interval = value; }
        }

        [DefaultValue(true)]
        [Description("")]
#if V2
        [Themeable(true)]
#endif
        public bool ShowCountDown {
            get { return this.showCountDown; }
            set { this.showCountDown = value; }
        }

        public override string Text {
            get { return base.Text; }
            set { }
        }

        [DefaultValue("")]
        [Description("Javascript function to call before firing the anthem callback")]
        public string OnClientElapsed {
            get { return this.onClientElapsed; }
            set { this.onClientElapsed = value; }
        }

#if V2
        [Category("Styles")]
        [Description("Style to switch to once final seconds threshold is reached")]
        [NotifyParentProperty(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [Themeable(true)]
        public ASP.Style FinalSecondsStyle {
            get {
                if (this.finalSecondsStyle == null) {
                    this.finalSecondsStyle = new ASP.Style();
                    if (base.IsTrackingViewState)
                        ((IStateManager)this.finalSecondsStyle).TrackViewState();
                }
                return this.finalSecondsStyle;
            }
        }

        [DefaultValue(false)]
        [Description("Once final seconds threshold is reached, this enables blinking of the countdown text (if showing)")]
        [Themeable(true)]
        public bool FinalSecondsBlink {
            get { return this.finalSecondsBlink; }
            set { this.finalSecondsBlink = value; }
        }

        [DefaultValue(0)]
        [Description("Number of seconds before the timer elapses that the final seconds styles & blinking is applied (if counter is showing)")]
        [Themeable(true)]
        public int FinalSecondsThreshold {
            get { return this.finalSecondsThreshold; }
            set { this.finalSecondsThreshold = value; }
        }
#endif
        #endregion

        #region ICallBackControl implementation
        [DefaultValue("")]
        public string CallBackCancelledFunction {
            get {
                if (null == ViewState["CallBackCancelledFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["CallBackCancelledFunction"];
            }
            set { ViewState["CallBackCancelledFunction"] = value; }
        }

        [DefaultValue(true)]
        public bool EnableCallBack {
            get {
                if (ViewState["EnableCallBack"] == null)
                    return true;
                else
                    return (bool)ViewState["EnableCallBack"];
            }
            set {
                ViewState["EnableCallBack"] = value;
            }
        }

        [DefaultValue(true)]
        public bool EnabledDuringCallBack {
            get {
                if (null == ViewState["EnabledDuringCallBack"])
                    return true;
                else
                    return (bool)ViewState["EnabledDuringCallBack"];
            }
            set { ViewState["EnabledDuringCallBack"] = value; }
        }

        [DefaultValue("")]
        public string PostCallBackFunction {
            get {
                if (null == ViewState["PostCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PostCallBackFunction"];
            }
            set { ViewState["PostCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public string PreCallBackFunction {
            get {
                if (null == ViewState["PreCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PreCallBackFunction"];
            }
            set { ViewState["PreCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public string TextDuringCallBack {
            get {
                if (null == ViewState["TextDuringCallBack"])
                    return string.Empty;
                else
                    return (string)ViewState["TextDuringCallBack"];
            }
            set { ViewState["TextDuringCallBack"] = value; }
        }

        #endregion

        #region IPostBackEventHandler Members
        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument) {
            if (eventArgument == "elapsed") 
                this.OnElapsed(EventArgs.Empty);
        }

        #endregion
    }
}
