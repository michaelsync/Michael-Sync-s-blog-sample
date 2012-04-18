using System;
using System.ComponentModel;
using System.Web.UI;
using ASP = System.Web.UI.WebControls;

namespace Anthem
{
    /// <summary>
    /// Creates an updatable timer control.
    /// </summary>
    public class Timer : Control //, IUpdatableControl
    {
        #region Declarations

        // Tracks whether the client timeout script should be added to the page.
        private bool _addTimeoutScript = false;

        #endregion

        #region Unique Anthem control code

        /// <summary>
        /// True if the timer is enabled on the client. The timer will continue to call the
        /// DoTick method while it is enabled.
        /// </summary>
        [DefaultValue(false)]
        public bool Enabled
        {
            get
            {
                if (null == ViewState["Enabled"])
                    return false;
                else
                    return (bool)ViewState["Enabled"];
            }
            set
            {
                ViewState["Enabled"] = value;
            }
        }

        /// <summary>
        /// The number of milliseconds between each call to DoTick.
        /// </summary>
        [DefaultValue(1000)]
        public int Interval
        {
            get
            {
                if (null == ViewState["Interval"])
                    return 1000;
                else
                    return (int)ViewState["Interval"];
            }
            set { ViewState["Interval"] = value; }
        }

        /// <summary>
        /// This method is called by the Timer control each time the timer expires.
        /// </summary>
        [Anthem.Method]
        public void DoTick()
        {
            OnTick(EventArgs.Empty);
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer()
        {
            this.Enabled = true;
            _addTimeoutScript = true;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            this.Enabled = false;
            _addTimeoutScript = true;
        }

        /// <summary>
        /// Occurs when the timer expires.
        /// </summary>
        public event EventHandler Tick;

        /// <summary>
        /// Raises the <see cref="Tick"/> event.
        /// </summary>
        protected virtual void OnTick(EventArgs e)
        {
            if (Enabled && (Tick != null))
            {
                _addTimeoutScript = true;
                Tick(this, e);
            }
        }

        /// <summary>
        /// Registers the client side script for the control.
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Anthem.Manager.IsCallBack)
            {
                // Always register the var that will track the timer.
                string script = string.Format("<script type=\"text/javascript\">var {0}_timer = null;</script>", this.ClientID);
#if V2
                Page.ClientScript.RegisterStartupScript(this.GetType(), script, script, false);
#else
                Page.RegisterStartupScript(script, script);
#endif

                // For the initial page load, register the timer script so that it loads after
                // the page is finished loading.
                if (Enabled)
                {
                    script = string.Format("<script type=\"text/javascript\">{0}</script>", GetSetTimeoutScript());
#if V2
                    Page.ClientScript.RegisterStartupScript(this.GetType(), script, script, false);
#else
                    Page.RegisterStartupScript(script, script);
#endif
                }
            }
            else if (_addTimeoutScript)
            {
                // If this is a callback, add a script to call the timer again or to stop the timer.
                if (Enabled)
                {
                    Anthem.Manager.AddScriptForClientSideEval(GetSetTimeoutScript());
                }
                else
                {
                    Anthem.Manager.AddScriptForClientSideEval(GetClearTimeoutScript());
                }

            }
        }
        
        /// <summary>
        /// Visible is inherited from Control, but ignored by Timer.
        /// </summary>
        [Browsable(false)]
        public override bool Visible
        {
            get { return true; }
            set {}
        }

        #endregion

        #region Common Anthem control code

        // Note that the Timer control only needs to register. Since it is not
        // rendered in the traditional sense, the other common code is not needed.

        /// <summary>
        /// Raises the <see cref="System.Web.UI.Control.Load"/> event and registers the control
        /// with <see cref="Anthem.Manager"/>.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/>.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Anthem.Manager.Register(this);
        }

        #endregion

        #region Private Methods

        private string GetSetTimeoutScript()
        {
            return string.Format("{0} {1}_timer = setTimeout(function() {{ Anthem_InvokeControlMethod('{1}', 'DoTick', [], function() {{ }}, null) }}, {2})",
                GetClearTimeoutScript(),
                ClientID,
                Interval
            );
        }

        private string GetClearTimeoutScript()
        {
            return string.Format("if (typeof({0}_timer) != 'undefined' && {0}_timer != null) {{ clearTimeout({0}_timer); {0}_timer = null; }}", ClientID);
        }

        #endregion
    }
}
