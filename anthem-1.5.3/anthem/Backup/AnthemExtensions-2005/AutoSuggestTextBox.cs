using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Text;

using Anthem;
using ASP = System.Web.UI.WebControls;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

//#if V2 
//[assembly: WebResource("AnthemExtensions.AutoSuggestTextBox.js", "text/javascript")] 
//#endif 

namespace AnthemExtensions
{
    /// <summary> 
    /// The autosuggesttextbox works like a textbox but suggests values while typing. 
    /// Values are listed in a dropdownlist-like manner. 
    /// </summary> 
    [
    ParseChildren(true)
    ]
    public class AutoSuggestTextBox : ASP.TextBox, IUpdatableControl, ICallBackControl
    {
        private bool _allowEditing = true; // if AllowEditing is false, user will not be able to key a value that's not suggested 
        private Anthem.Label _errorLabel; // Label to hold error text 
        private string _errorLabelName; // if set, textbox will use the label named to write error message on AlloEdit 
        private const string parentTagName = "span";
        private object _dataSource;
        private DataView _dataView;

        // START added code by prs 21sep06 - adding data access support for any SQL or OLEDB database
        private string _connectionString = "";
        private string _appSettingsKey = "";
        private bool _isStoredProcedure = false;
        private string _commandText = "";
        private string _dataProvider = "SqlClient";
        // END added code by prs 21sep06

        // START added code by prs 19sep06 - public property that sets MinCharTypedBeforeSearch property in javascript
        // searching starts after having typed the indicated amount of characters
        private int _minCharTypedBeforeSearch = 1;
        // END added code by prs 19sep06

        // START added code by prs 21nov06 - public property that sets MaxDelayTimeBeforeSearch property in javascript
        // time is in msec and elapses before attempting to do any searches
        private int _delayTimeBeforeSearch = 1000;
        // END added code by prs 21nov06

        #region Properties

        public bool AllowEditing
        {
            get { return _allowEditing; }
            set { _allowEditing = value; }
        }

        public string ErrorLabelName
        {
            get { return _errorLabelName; }
            set { _errorLabelName = value; }
        }

        public object DataSource
        {
            set { _dataSource = value; }
        }

        // START added code by prs 19sep06 - public property that sets MinCharTypedBeforeSearch property in javascript
        public int MinCharTypedBeforeSearch
        {
            get { return _minCharTypedBeforeSearch; }
            set { _minCharTypedBeforeSearch = value; }
        }
        // END added code by prs 19sep06

        // START added code by prs 21sep06 - adding data access support for any SQL or OLEDB database
        [Bindable(true), Browsable(true), Category("Data")]
        public string AppSettingsKey
        {
            get { return _appSettingsKey; }
            set { _appSettingsKey = value; }
        }

        [Bindable(true), Browsable(true), Category("Data")]
        public bool IsStoredProcedure
        {
            get { return _isStoredProcedure; }
            set { _isStoredProcedure = value; }
        }

        [Bindable(true), Browsable(true), Category("Data")]
        public string CommandText
        {
            get { return _commandText; }
            set { _commandText = value; }
        }

        [Bindable(true), Browsable(true), Category("Data")]
        public string DataProvider
        {
            get { return _dataProvider; }
            set { _dataProvider = value; }
        }
        // END added code by prs 21sep06

        // START added code by prs 21nov06 - public property that sets MaxDelayTimeBeforeSearch property in javascript
        public int DelayTimeBeforeSearch
        {
            get { return _delayTimeBeforeSearch; }
            set { _delayTimeBeforeSearch = value; }
        }
        // END added code by prs 21nov06

        #endregion

        #region Events
        #endregion

        #region WebControl Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (_errorLabelName != null)
            {
                foreach (Control c in Page.Controls)
                {
                    foreach (Control childc in c.Controls)
                    {
                        if (childc.ID == _errorLabelName)
                        {
                            _errorLabel = (Anthem.Label)childc;
                        }
                    }
                }
            }
            Anthem.Manager.Register(this);
        }

#if V2
        public override void RenderControl(HtmlTextWriter writer)
        {
            base.Visible = true;
            base.RenderControl(writer);
        }
#endif

        public override bool Visible
        {
            get
            {
#if !V2 
bool DesignMode = this.Context == null; 
#endif
                return Anthem.Manager.GetControlVisible(this, ViewState, DesignMode);
            }
            set { Anthem.Manager.SetControlVisible(ViewState, value); }
        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            // START prs changed code - 11nov06
            // solving naming differences in .NET 1.1 / 2.0
            string uId = string.Empty;
#if V2
            uId = this.ClientID;
#else 
            uId = this.UniqueID; 
#endif
            // END prs changed code - 11nov06
            string newUid = uId.Replace(":", "_");
            string ifrId = newUid + "_Iframe";
            string divId = newUid + "_Div";
            string jsId = newUid + "_Js";

            Anthem.Manager.AddScriptAttribute(this, "AutoComplete", "off");
            Anthem.Manager.AddScriptAttribute(this, "onkeyup", jsId + ".OnKeyUp(event)");
            Anthem.Manager.AddScriptAttribute(this, "onblur", jsId + ".OnBlur(event)");
            base.AddAttributesToRender(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
#if !V2 
        bool DesignMode = this.Context == null; 
#endif

            if (!DesignMode)
            {
                string script = null;
                script = @"<script type=""text/javascript"">
//![CDATA[ 
//begin browser detection script 
var detect = navigator.userAgent.toLowerCase(); 
var OS,browser,version,total,thestring; 

// START code added by prs - 13nov06
var arrResult; 
var aStr; 
var arrIDs;
// END code added by prs - 13nov06

// START code added by prs - 26nov06
var timer; 
var objthis;
// END code added by prs - 26nov06

if (checkIt('konqueror')) 
{ 
  browser = 'Konqueror'; 
  OS = 'Linux'; 
} 
else if (checkIt('safari')) browser = 'Safari'; 
else if (checkIt('omniweb')) browser = 'OmniWeb'; 
else if (checkIt('opera')) browser = 'Opera'; 
else if (checkIt('webtv')) browser = 'WebTV'; 
else if (checkIt('icab')) browser = 'iCab'; 
else if (checkIt('msie')) browser = 'Internet Explorer'; 
else if (!checkIt('compatible')) 
{ 
  browser = 'Netscape Navigator'; 
	version = detect.charAt(8);
} 
else browser = 'An unknown browser'; 

if (!version) version = detect.charAt(place + thestring.length); 

if (!OS) 
{ 
  if (checkIt('linux')) OS = 'Linux'; 
  else if (checkIt('x11')) OS = 'Unix'; 
  else if (checkIt('mac')) OS = 'Mac'; 
  else if (checkIt('win')) OS = 'Windows'; 
  else OS = 'an unknown operating system'; 
} 

function checkIt(string) 
{ 
  place = detect.indexOf(string) + 1; 
  thestring = string; 
  return place; 
} 
// end browser detection script 

function ASTextBox(ASTextBoxId, DivId, IfrId, minCharTypedBeforeSearch, delayTimeBeforeSearch) 
{ 
  //debugger; 
  // initialize member variables 
  var oThis = this; 
  this.tboxID = ASTextBoxId; 
  this.divID = DivId; 
  this.ifrId = IfrId; 
  var self = this; 
  
  // added code by prs 19sep06
  this.MinCharTypedBeforeSearch = minCharTypedBeforeSearch;
  
  // added code by prs 21nov06
  this.DelayTimeBeforeSearch = delayTimeBeforeSearch;

  // store the names - if anthem updates the control, we'll need new object references 
  var oText = document.getElementById(ASTextBoxId); 
  var oDiv = document.getElementById(DivId); 
  var oIfr = document.getElementById(IfrId); 

  selectedElem = 0; 
  var txtArray = new Array(); 
  this.txtArray = txtArray; 
  lastGoodText = ''; 

  this.TextBox = oText; 
  this.Div = oDiv; 
  this.iFrame = oIfr; 

  oText.parent = document.getElementById(ASTextBoxId).parentNode; 

  // attach handlers to the ASTextBox 
  oText.ASTextBox = this; 
  oText.onkeyup = ASTextBox.prototype.OnKeyUp; 
  oText.onblur = ASTextBox.prototype.OnBlur; 
  oDiv.style.display = 'none'; 
  oDiv.style.position = 'absolute'; 
  oDiv.style.zIndex = 2; 
  oDiv.style.borderColor = 'black'; 
  oDiv.style.borderStyle = 'solid'; 
  oDiv.style.backgroundColor = 'white'; 
  oDiv.style.width = 'auto'; 

  oIfr.style.display = 'none'; 
  oIfr.style.position = 'absolute'; 
  oIfr.style.zIndex = 2; 
  oIfr.style.backgroundColor = 'white'; 
} 

// added property by prs 19sep06 - MinCharTypedBeforeSearch 
// 1 <= value <= n (value < 1 will work like value == 1)
ASTextBox.prototype.MinCharTypedBeforeSearch = 1; 

// added property by prs 13nov06 - DropDownID 
// reflects the current selected ID
ASTextBox.prototype.DropDownID = '' 

// added property by prs 13nov06 - DropDownValue 
// reflects the current selected Value
ASTextBox.prototype.DropDownValue = ''; 

// added property by prs 13nov06 - DropDownWidth
// sets the width of the dropdown div
ASTextBox.prototype.DropDownWidth = 150;

// added property by prs 13nov06 - DropDownValue 
// sets the amount of columns (not implemented yet - 
// for now it is 2 columns fixed with 1st column=ID and 2nd column=Value)
ASTextBox.prototype.DropDownColumns = 2; 

// added property by prs 21nov06 - DelayTimeBeforeSearch
// time in msec that elapses before an attempt to search is done
ASTextBox.prototype.DelayTimeBeforeSearch = 1000; 

ASTextBox.prototype.DoAutoSuggest = false; 

ASTextBox.prototype.ListItemClass = 'ListItem'; 

ASTextBox.prototype.ListItemHoverClass = 'ListItemHover'; 

ASTextBox.prototype.OnBlur = function() 
{ 
  if (this.TextBox_Blur) 
  { 
    this.TextBox_Blur(); 
  } 
  else 
  { 
    this.ASTextBox.TextBox_Blur(); 
  } 
} 

ASTextBox.prototype.TextBox_Blur = function() 
{ 
  this.Div.style.display='none'; 
  this.iFrame.style.display='none'; 
} 

// ASTextBox OnKeyUp 
ASTextBox.prototype.OnKeyUp = function(oEvent) 
{ 
  //check for the proper location of the event object 
  if (!oEvent) 
  { 
    oEvent = window.event; 
  } 
  if (this.TextBox_KeyUp) 
  { 
    this.TextBox_KeyUp(oEvent); 
  } 
  else 
  { 
    this.ASTextBox.TextBox_KeyUp(oEvent); 
  } 
} 

ASTextBox.prototype.TextBox_KeyUp = function(oEvent) 
{ 
  if (!oEvent) 
  { 
    oEvent = window.event; 
  } 
  var iKeyCode = oEvent.keyCode; 

  // get the refs again, in case anthem has updated the control and destroyed the original objects 
  this.TextBox = document.getElementById(this.tboxID); 
  this.Div = document.getElementById(this.divID); 
  this.iFrame = document.getElementById(this.ifrId); 

  // backspace 
  if( iKeyCode == 8 ) 
  { 
    this.DoAutoSuggest = false; 
    // JCM change the last good text to reflect the current text 
    this.lastGoodText = this.TextBox.value.substring(0, this.TextBox.value.length -1); 
  } 
  // JCM down arrow is 40 
  else if (iKeyCode == 40 && selectedElem < this.txtArray.length -1 ) 
  { 
    this.txtArray[selectedElem].style.backgroundColor = 'white'; 
    this.txtArray[selectedElem].style.color = 'black'; 
    selectedElem += 1; 
    this.txtArray[selectedElem].style.backgroundColor = 'black'; 
    this.txtArray[selectedElem].style.color = 'white'; 
    this.TextBox.value = this.txtArray[selectedElem].innerHTML.replace('&amp;', '&'); 
    return; 
  } 
  // JCM up arrow is 38 
  else if (iKeyCode == 38 && selectedElem > 0) 
  { 
    this.txtArray[selectedElem].style.backgroundColor = 'white'; 
    this.txtArray[selectedElem].style.color = 'black'; 
    selectedElem -= 1; 
    this.txtArray[selectedElem].style.backgroundColor = 'black'; 
    this.txtArray[selectedElem].style.color = 'white'; 
    this.TextBox.value = this.txtArray[selectedElem].innerHTML.replace('&amp;', '&'); 
    return; 
  } 
  // escape key 
  else if (iKeyCode == 27) 
  { 
    this.TextBox.value = ''; 
    this.OnBlur(oEvent); 
    return; 
  } 
  // non-letter 
  else if (iKeyCode < 32 || (iKeyCode >= 33 && iKeyCode <= 46) || 
  (iKeyCode >= 112 && iKeyCode <= 123)) 
  { 
    return; 
  } 
  else 
  { 
    this.DoAutoSuggest = true; 
  } 

  var txt = this.TextBox.value; 

  if( txt.length > 1 )
  {
    clearTimeout(timer);
  }
  
  // START added code by prs 03dec06 - adding a delay before suggesting starts
  //if( txt.length > 0 ) 
  // code added by prs 19sep06 - added min chars to type before search / suggest
  /*
  if ( txt.length > (this.MinCharTypedBeforeSearch -1) )
  { 
    var argsArray = new Array(); 
    argsArray[0] = this; 
    argsArray[1] = txt; 
    var result = Anthem_InvokeControlMethod(this.TextBox.id, 'OnKeyUp', [], this.TextBoxCallbackComplete, argsArray) 
  } 
  else 
  { 
    this.Div.innerHTML = ''; 
    this.Div.style.display = 'none'; 
    this.iFrame.style.display = 'none'; 
  }
  */
  str = 'StartSuggest(\'' + txt + '\',' + this.MinCharTypedBeforeSearch + ');';
  //alert(str);
  objthis = this; // make textbox object globally available
  timer = setTimeout(str,this.DelayTimeBeforeSearch);
  // END added code by prs 03dec06
} 

// START added code by prs 03dec06 - this starts the search
function StartSuggest(txt, minCharTypedBeforeSearch)
{ 
  if ( txt.length > (minCharTypedBeforeSearch -1) )
  { 
    var argsArray = new Array(); 
    argsArray[0] = objthis; // take global object
    argsArray[1] = txt; 
    var result = Anthem_InvokeControlMethod(objthis.TextBox.id, 'OnKeyUp', [], objthis.TextBoxCallbackComplete, argsArray) 
  } 
  else 
  { 
    objthis.Div.innerHTML = ''; 
    objthis.Div.style.display = 'none'; 
    objthis.iFrame.style.display = 'none'; 
  }
}
// END added code by prs 03dec06

ASTextBox.prototype.TextBoxCallbackComplete = 
function(result, argsArray) 
{ 
  var tbox = argsArray[0]; 
  var oldVal = argsArray[1]; 
  var responseText = result.value; 

  // START code added by prs - 13nov06
  arrResult = result.value.split('|'); 
  // END code added by prs - 13nov06
  
  // JCM 11/23/2005 there is a possible race condition from mult key presses. 
  // This attempts to ignore any calls finishing after another call.
  if (tbox.TextBox.value != oldVal) { 
    return; 
  } 

  tbox.txtArray.length = 0; 
  selectedElem = 0; 
  while ( tbox.Div.hasChildNodes() ) 
    tbox.Div.removeChild(tbox.Div.firstChild); 

  // align the drop down div 
  var c = GetCoords(tbox.TextBox); 
  var n = tbox.TextBox.style.pixelHeight; 
  if( !n ) 
  { 
    n = 25; 
  } 
  else 
  { 
    n += 2; 
  } 
  if (browser == 'Internet Explorer') 
  { 
    tbox.Div.style.left = c.x; 
    tbox.Div.style.top = c.y + n; 

    tbox.iFrame.style.left = c.x; 
    tbox.iFrame.style.top = c.y + n; 
  } 

  // get all the matching strings from the server response 
  if(responseText.length > 0 ) // if length == 0 no suggestions 
  { 
    //var aStr = responseText.split('\n'); 
    // START code added by prs - 13nov06
    arrIDs = arrResult[0].split('\n');
    aStr = arrResult[1].split('\n'); 
    // END code added by prs - 13nov06

    // add each string to the popup-div 
    var i, n = aStr.length; 

    for ( i = 0; i < n; i++ ) 
    { 
      var oDiv = document.createElement('div'); 
      // START added code by prs - 11nov06
      var aColumns;
      // END added code by prs - 11nov06

      try 
      { 
        oDiv.innerHTML = aStr[i]; 
      } 
      catch(e) 
      { 
        return; 
      } 

      tbox.Div.appendChild(oDiv); 
      // START added code by prs - 11nov06
      tbox.Div.style.width = tbox.DropDownWidth + 'px';
      // END added code by prs - 11nov06
      tbox.txtArray[i] = oDiv; 

      oDiv.noWrap = true; 
      oDiv.style.width = '100%'; 
      oDiv.style.display = 'block'; 
      if ( i == 0 ) 
      { 
        oDiv.style.backgroundColor = 'black'; 
        oDiv.style.color = 'white'; 
      } 
      else 
      { 
        oDiv.style.backgroundColor = 'white'; 
        oDiv.style.color = 'black'; 
      } 
      
      oDiv.onmousedown = ASTextBox.prototype.Div_MouseDown; 
      oDiv.onmouseover = ASTextBox.prototype.Div_MouseOver; 
      oDiv.onmouseout = ASTextBox.prototype.Div_MouseOut; 
      oDiv.ASTextBox = tbox; 
    } 

    tbox.Div.style.display = 'block'; 
    
    if (browser == 'Internet Explorer') 
    { 
      tbox.iFrame.style.width = tbox.Div.offsetWidth; 
      tbox.iFrame.style.height = tbox.Div.offsetHeight; 
      tbox.iFrame.top = tbox.Div.style.top; 
      tbox.iFrame.left = tbox.Div.style.left; 
      tbox.iFrame.style.zIndex = tbox.Div.style.zIndex - 1; 
      tbox.iFrame.style.border = 0; 
      tbox.iFrame.style.display = 'block'; 
      //tbox.iFrame.style.cursor = 'hand';
    } 
    
    if( tbox.DoAutoSuggest == true ) 
    { 
      tbox.AutoSuggest( aStr ); 
    } 
  } 
  else // no suggestions - 
  { 
    if (tbox.AllowEditing == 'False') 
    { 
      if (tbox.lastGoodText && tbox.lastGoodText.length > 0 ) 
      { 
        tbox.TextBox.value = tbox.lastGoodText; 
      } 
      else 
      { 
        tbox.TextBox.value = ''; 
      } 
    } 
    tbox.Div.innerHTML = ''; 
    tbox.Div.style.display='none'; 
    tbox.iFrame.style.display='none'; 
  } 
} 

ASTextBox.prototype.Div_MouseDown = function() 
{ 
  this.ASTextBox.TextBox.value = this.innerHTML; 
  // START added code by prs - 13nov06
  // Set here the DropDownID and DropDownValue properties
  for ( k = 0; k < arrIDs.length; k++ ) 
  { 
    if ( aStr[k] == this.innerHTML )
        this.ASTextBox.DropDownID = arrIDs[k];
  }
  this.ASTextBox.DropDownValue = this.innerHTML;
  // END added code by prs - 13nov06
} 

ASTextBox.prototype.Div_MouseOver = function() 
{ 
  this.ASTextBox.txtArray[selectedElem].style.backgroundColor = 'white'; 
  this.ASTextBox.txtArray[selectedElem].style.color = 'black'; 
  this.style.backgroundColor = 'black'; 
  this.style.color = 'white'; 
  //this.style.cursor = 'default';
} 

ASTextBox.prototype.Div_MouseOut = function() 
{ 
  this.style.backgroundColor = 'white'; 
  this.style.color = 'black'; 
} 

ASTextBox.prototype.AutoSuggest = function(aSuggestions) //array 
{ 
  if (aSuggestions.length > 0) 
  { 
    this.TypeAhead(aSuggestions[0]); 
  } 
} 

ASTextBox.prototype.TypeAhead = function( sSuggestion) // string 
{ 
  if( this.TextBox.createTextRange || this.TextBox.setSelectionRange) 
  { 
    var iLen = this.TextBox.value.length; 
    this.lastGoodText = this.TextBox.value; // JCM store the last successful text in case AllowEditing is false and an invalid val is keyed 
    this.TextBox.value = sSuggestion; 
    // JCM store the last suggested text in case AllowEditing is false and an invalid val is keyed 
    this.SelectRange(iLen, sSuggestion.length); 
  } 
} 

ASTextBox.prototype.SelectRange = function (iStart, iLength) //int, int 
{ 
  //use text ranges for Internet Explorer 
  if (this.TextBox.createTextRange) 
  { 
    var oRange = this.TextBox.createTextRange(); 
    oRange.moveStart('character', iStart); 
    oRange.moveEnd('character', iLength - this.TextBox.value.length); 
    oRange.select(); 
    //use setSelectionRange() for Mozilla 
  } 
  else if (this.TextBox.setSelectionRange) 
  { 
    this.TextBox.setSelectionRange(iStart, iLength); 
  } 
} 

function GetCoords(obj) // object 
{ 
  var newObj = new Object(); 
  newObj.x = obj.offsetLeft; 
  newObj.y = obj.offsetTop; 
  theParent = obj.offsetParent; 
  while(theParent != null) 
  { 
    newObj.y += theParent.offsetTop; 
    newObj.x += theParent.offsetLeft; 
    theParent = theParent.offsetParent; 
  } 
  return newObj; 
} 
//]]> 
</script>";
#if V2
                Page.ClientScript.RegisterClientScriptBlock(typeof(AutoSuggestTextBox), script, script, false);
                //Anthem.Manager.RegisterClientScriptBlock(typeof(AutoSuggestTextBox), script, script, false);
#else 
                Page.RegisterClientScriptBlock(script, script); 
                //Anthem.Manager.RegisterClientScriptBlock(script, script); 
#endif
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string uId = string.Empty;
#if V2
            uId = this.ClientID;
#else 
            uId = this.UniqueID; 
#endif
            string newUid = uId.Replace(":", "_");
            string ifrId = newUid + "_Iframe";
            string divId = newUid + "_Div";
            string jsId = newUid + "_Js";
            string allowEd = _allowEditing.ToString();
            string minCharTypedBeforeSearch = _minCharTypedBeforeSearch.ToString();
            string delayTimeBeforeSearch = _delayTimeBeforeSearch.ToString();

#if !V2 
bool DesignMode = this.Context == null; 
#endif

            StringBuilder acScript = new StringBuilder();
            /*
            acScript.Append("<script type=\"text/javascript\">");
            acScript.AppendFormat("var {0} = new ASTextBox('{1}','{2}','{4}','{5}'); {0}.AllowEditing = '{3}';", jsId, newUid, divId, allowEd, ifrId, minCharsTyped);
            acScript.Append("</script>");
            */
            // START added code by prs 21nov06
            // added
            acScript.Append("<script type=\"text/javascript\">");
            acScript.AppendFormat("var {0} = new ASTextBox('{1}','{2}','{4}','{5}','{6}'); {0}.AllowEditing = '{3}';", jsId, newUid, divId, allowEd, ifrId, minCharTypedBeforeSearch, delayTimeBeforeSearch);
            acScript.Append("</script>");
            // END added code by prs 21nov06

#if V2
            Page.ClientScript.RegisterStartupScript(typeof(AutoSuggestTextBox), newUid, acScript.ToString());
            //Anthem.Manager.RegisterStartupScript(typeof(AutoSuggestTextBox), newUid, acScript.ToString());
#else
            Page.RegisterStartupScript(newUid, acScript.ToString());
            //Anthem.Manager.RegisterStartupScript(newUid, acScript.ToString());
#endif

            if (Visible)
            {
                if (!DesignMode)
                {
                    // parentTagName must be defined as a private const string field in this class. 
                    Anthem.Manager.WriteBeginControlMarker(writer, parentTagName, this);
                }
                base.Render(writer);
                if (!DesignMode)
                {
                    Anthem.Manager.WriteEndControlMarker(writer, parentTagName, this);
                    writer.Write(String.Format("<iframe id={1}></iframe><div id={0}></div>", divId, ifrId));
                }
            }

        }

        public override bool AutoPostBack
        {
            get { return false; }
        }

        #endregion

        #region ICallBackControl Members

        [DefaultValue("")]
        public string CallBackCancelledFunction
        {
            get
            {
                if (null == ViewState["CallBackCancelledFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["CallBackCancelledFunction"];
            }
            set { ViewState["CallBackCancelledFunction"] = value; }
        }

        [DefaultValue(true)]
        public bool EnableCallBack
        {
            get
            {
                if (ViewState["EnableCallBack"] == null)
                    return true;
                else
                    return (bool)ViewState["EnableCallBack"];
            }
            set
            {
                ViewState["EnableCallBack"] = value;
            }
        }

        [DefaultValue(true)]
        public bool EnabledDuringCallBack
        {
            get
            {
                if (null == ViewState["EnabledDuringCallBack"])
                    return true;
                else
                    return (bool)ViewState["EnabledDuringCallBack"];
            }
            set { ViewState["EnabledDuringCallBack"] = value; }
        }

        [DefaultValue("")]
        public string PostCallBackFunction
        {
            get
            {
                if (null == ViewState["PostCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PostCallBackFunction"];
            }
            set { ViewState["PostCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public string PreCallBackFunction
        {
            get
            {
                if (null == ViewState["PreCallBackFunction"])
                    return string.Empty;
                else
                    return (string)ViewState["PreCallBackFunction"];
            }
            set { ViewState["PreCallBackFunction"] = value; }
        }

        [DefaultValue("")]
        public string TextDuringCallBack
        {
            get
            {
                if (null == ViewState["TextDuringCallBack"])
                    return string.Empty;
                else
                    return (string)ViewState["TextDuringCallBack"];
            }
            set { ViewState["TextDuringCallBack"] = value; }
        }

        #endregion

        #region IPostBackEventHandler Members
        #endregion

        #region Anthem Methods

        [Anthem.Method]
        public String OnKeyUp()
        {
            // START code added by prs - 13nov06
            StringBuilder sbid = new StringBuilder();
            // END code added by prs - 13nov06
            StringBuilder sb = new StringBuilder();

            // This method should assign a DataView to _dataView which contains all rows to be displayed in the drop down 
#if V2
            if (this._commandText == "" || _dataSource is ASP.ObjectDataSource)
                BindData("name LIKE '" + this.Text + "*'");
            else
                BindData(this.Text);
#else
			if (this._commandText == "")
				BindData("name LIKE '" + this.Text + "*'");
			else
				BindData(this.Text);
#endif

            for (int i = 0; i < _dataView.Count; i++)
            {
                //sb.Append(_dataView[i][0]);
                // START code added by prs - 13nov06
                // with multiple columns, the second column is the one containing the search strings
                sbid.Append(_dataView[i][0]);
                sb.Append(_dataView[i][1]);
                sbid.Append("\n");
                // END code added by prs - 13nov06
                sb.Append("\n");
            }

            // Remove trailing '\n' 
            // START code added by prs - 13nov06
            if (sbid.Length > 1)
            {
                sbid.Remove(sbid.Length - 1, 1);
                if (ErrorLabelName != null)
                {
                    _errorLabel.Text = "";
                }
            }
            else if (!AllowEditing)
            {
                if (ErrorLabelName != null)
                {
                    _errorLabel.Text = "You must choose a value from the dropdown";
                }
            }
            // END code added by prs - 13nov06
            if (sb.Length > 1)
            {
                sb.Remove(sb.Length - 1, 1);
                if (ErrorLabelName != null)
                {
                    _errorLabel.Text = "";
                }
            }
            else if (!AllowEditing)
            {
                if (ErrorLabelName != null)
                {
                    _errorLabel.Text = "You must choose a value from the dropdown";
                }
            }
            //return sb.ToString();
            // START code added by prs - 13nov06
            return sbid.ToString() + "|" + sb.ToString();
            // END code added by prs - 13nov06
        }

        #endregion

        #region Public Methods

        public AutoSuggestTextBox()
        {
        }

        public void BindData(string filter)
        {
            // added code by prs 03oct06
#if V2
            if (this._commandText != "" && !(_dataSource is ASP.ObjectDataSource))
                GetData(filter);
#else
			if (this._commandText != "")
				GetData(filter);
#endif
            DataSet ds = new DataSet();
            DataView dv = new DataView();

#if !V2
			ds = (DataSet)_dataSource;
			dv = ds.Tables[0].DefaultView;
#endif

#if V2
            if (_dataSource is ASP.ObjectDataSource)
                dv = (DataView)((ASP.ObjectDataSource)_dataSource).Select();
            else if (_dataSource is DataSet)
            {
                ds = (DataSet)_dataSource;
                dv = ds.Tables[0].DefaultView;
            }
            else
                throw new ArgumentException("AutoSuggestTextBox.DataSource must be a DataSet or an ObjectDataSource with a Select method that returns a DataView.");
#endif

#if V2
            if (this._commandText == "" || _dataSource is ASP.ObjectDataSource)
                dv.RowFilter = filter;
#else
			if (this._commandText == "")
				dv.RowFilter = filter;
#endif
            _dataView = dv;
            // END added code by prs 03oct06
        }

        private void GetData(string filter)
        {
            // create a unique cache key based on Request path and filter term
            //string cacheKey = this.Page.Request.FilePath + filter;
            // START added code by prs 12nov06
            // this will make the cache key unique to the control so multiple can be used on one page
            string cacheKey = this.Page.Request.FilePath + this.ID + filter;
            // END added code by prs 12nov06
            // is this data already in the Cache?
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                this.DataSource = (DataSet)HttpContext.Current.Cache[cacheKey];
                return;
            }
            // data is not cached, go get it and store in cache-
            else
            {
#if V2
                this._connectionString = System.Web.Configuration.WebConfigurationManager.AppSettings[this._appSettingsKey];
#else
                this._connectionString = ConfigurationSettings.AppSettings[this._appSettingsKey];
#endif
                OleDbConnection conn = new OleDbConnection(this._connectionString);
                OleDbCommand cmd = new OleDbCommand(this._commandText, conn);
                if (this._isStoredProcedure)
                {
                    cmd.Parameters.Add(new OleDbParameter("@filter", filter));
                    cmd.CommandType = CommandType.StoredProcedure;
                }
                else
                {
                    cmd.CommandText = cmd.CommandText.Replace("@filter", filter);
                    cmd.CommandType = CommandType.Text;
                }
                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    this._dataSource = ds;
                    if (ds.Tables[0].Rows.Count > 0)
                        HttpContext.Current.Cache[cacheKey] = ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        #endregion

        #region Private Methods
        #endregion

        #region IUpdatableControl Members

        [DefaultValue(false)]
        public bool AutoUpdateAfterCallBack
        {
            get
            {
                if (ViewState["AutoUpdateAfterCallBack"] == null)
                    return false;
                else
                    return (bool)ViewState["AutoUpdateAfterCallBack"];
            }
            set
            {
                if (value) UpdateAfterCallBack = true;
                ViewState["AutoUpdateAfterCallBack"] = value;
            }
        }

        private bool _updateAfterCallBack = false;

        [Browsable(false)]
        public bool UpdateAfterCallBack
        {
            get { return _updateAfterCallBack; }
            set { _updateAfterCallBack = value; }
        }

        [
        Category("Misc"),
        Description("Fires before the control is rendered with updated values.")
        ]
        public event EventHandler PreUpdate
        {
            add { Events.AddHandler(EventPreUpdateKey, value); }
            remove { Events.RemoveHandler(EventPreUpdateKey, value); }
        }
        private static readonly object EventPreUpdateKey = new object();

        public virtual void OnPreUpdate()
        {
            EventHandler EditHandler = (EventHandler)Events[EventPreUpdateKey];
            if (EditHandler != null)
                EditHandler(this, EventArgs.Empty);
        }

        #endregion
    }
}
