function AnthemTimer(id, label, showCountDown, countDownTimeFinal, blinkFinal, styleFinal, loopTimer, callbackTimer, onClientElapsed, serverMethod) {
    this._type = "timer";
	this._id = id;
	this._blinkFinal = blinkFinal;
	this._styleFinal = styleFinal;
	this._countDownTimeFinal = countDownTimeFinal;
	this._control = label;
    this._showCountDown = showCountDown;
    this._loopTimer = loopTimer;
    this._callbackTimer = callbackTimer;
    this._serverMethod = serverMethod;    
    this._onClientElapsed = onClientElapsed;    
    this._countLabel = document.getElementById(label);
    this._styleOrig = (this._countLabel == null ? null : this._countLabel.style);
    this.TimeRemaining = 0;
    this.State = "stopped";    
    this.Start = function() {
        this.State = "started";
	    this.TimeRemaining = this._callbackTimer;
        this._updateLabel();
        window.setTimeout(this._id + ".Tick()", 1000);
    }
	this.Resume = function() {
	    this.State = "started";
	    window.setTimeout(this._id + ".Tick()", 1000);
	}
    this.Pause = function() {
        this.State = "paused";
    }
    this.Stop = function() {
        this.State = "stopped";
    }    
    this.Reset = function() {
        this.TimeRemaining = this._callbackTimer;
    }
    this._updateLabel = function() {
        if (this._showCountDown && this._countLabel != null) {
            var i = (this.TimeRemaining / 1000)
            this._countLabel.innerText = i;
            /*if (this._countDownTimeFinal > 0 && this._finalStyle != "") {
                if (i > this._countDownTimeFinal) {
                    //this._countLabel.style = this._styleOrig; // blinking will be turned off
                }
                else {                    
                    this._countLabel.style = this._styleFinal;
                    this._blink();
                }
            }*/
        }
    }
    /*this._blink = function() {
        if (this._blinkFinal) {
            if (document.all)
                this._countLabel.style.textDecoration = this._countLabel.style.textDecoration + " blink";
            else {
                // don't know yet
            }
        }
    }*/
    this.Tick = function() {
        if (this.State == "started") {
            this.TimeRemaining -= 1000;
            this._updateLabel();    
            if (this.TimeRemaining > 0)
                window.setTimeout(this._id + ".Tick()", 1000);
            else {
                try {
                    if (this._onClientElapsed != null) 
                        this._onClientElapsed(this);
                }
                catch (e) {
                    alert("There was a problem running your client elapsed javascript function.  Error : " + e.message);
                }
                try {
                    this._serverMethod();
                }
                catch (e) {
                    alert("There was a problem with the timer callback.  Exception : " + e.message);
                }
                if (!this._loopTimer) 
                    this.State = "stopped";
                else {
                    this.Reset();
                    window.setTimeout(this._id + ".Tick()", 1000);
                }
            }
        }
    }
}