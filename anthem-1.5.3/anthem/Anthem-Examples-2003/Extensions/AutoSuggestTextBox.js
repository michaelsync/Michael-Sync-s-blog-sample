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
ASTextBox.prototype.DropDownID = ""; 

// added property by prs 13nov06 - DropDownValue 
// reflects the current selected Value
ASTextBox.prototype.DropDownValue = ""; 

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
  //str = "StartSuggest('" + txt + "'," + this.MinCharTypedBeforeSearch + ");";
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
