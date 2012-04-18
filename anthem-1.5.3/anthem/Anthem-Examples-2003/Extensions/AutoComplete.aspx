<%@ Register TagPrefix="MY" TagName="MyAutoComplete" Src="~/AutoComplete.ascx" %>
<%@ Register TagPrefix="AnthemExtensions" Namespace="AnthemExtensions" Assembly="AnthemExtensions" %>
<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" 
    "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<HTML>
  <HEAD>
    <title>YUI AutoComplete Sample</title>
    <script runat="server">
  [Anthem.Method]
  protected string[][] acpage_Search(string sQuery)
  {
      ArrayList result = new ArrayList();
      result.Add(new string[] { sQuery + "a", "page" });
      result.Add(new string[] { sQuery + "b", "page" });
      result.Add(new string[] { sQuery + "c", "page" });
      return (string[][])result.ToArray(typeof(string[]));
  }
    </script>
    <style type="text/css">
        FIELDSET { MARGIN: 1em }
        .container { MARGIN: 1em; WIDTH: 10em; POSITION: relative }
        .textbox { WIDTH: 95%; POSITION: absolute }
        .results { WIDTH: 95%; POSITION: absolute; TOP: 1.3em }
        .results .yui-ac-content { BORDER-RIGHT: #404040 1px solid; BORDER-TOP: #404040 1px solid; Z-INDEX: 9050; BACKGROUND: #fff; OVERFLOW-X: hidden; OVERFLOW: auto; BORDER-LEFT: #404040 1px solid; WIDTH: 100%; BORDER-BOTTOM: #404040 1px solid; POSITION: absolute; HEIGHT: 11em }
        .results .yui-ac-shadow { Z-INDEX: 9049; BACKGROUND: #a0a0a0; MARGIN: 0.3em; WIDTH: 100%; POSITION: absolute }
        .results UL { PADDING-RIGHT: 0px; PADDING-LEFT: 0px; PADDING-BOTTOM: 5px; MARGIN: 0px; WIDTH: 100%; PADDING-TOP: 5px }
        .results LI { PADDING-RIGHT: 5px; PADDING-LEFT: 5px; PADDING-BOTTOM: 0px; CURSOR: default; PADDING-TOP: 0px; WHITE-SPACE: nowrap }
        .results LI.yui-ac-highlight { BACKGROUND: #ff0 }
        </style>
    <script type="text/javascript">
        function formatResult(oResultItem, sQuery) {
            return oResultItem[0] + ' (' + oResultItem[1] + ')';
        }
    </script>
  </HEAD>
  <body>
    <form id="form1" runat="server">
      <h1>YuiAutoComplete Sample</h1>
      <p>This page demonstrates the YuiAutoComplete control in the AnthemExtensions 
        library. YuiAutoComplete is a wrapper for the <a href="http://developer.yahoo.com/yui/autocomplete/">
          YUI AutoComplete</a> control. AutoComplete is one of the many widgets 
        availalbe in the <a href="http://developer.yahoo.com/yui/">Yahoo! User Interface 
          Library</a>. YuiAutoComplete exposes all of the properties of YUI 
        AutoComplete and generates the javascript to load the libraries, initialize the 
        component, and connect YUI AutoComplete with Anthem.</p>
      <p>You start implementing the YuiAutoComplete control by selecting a TextBox to 
        monitor and defining a Search event handler. Then load the page in a browser. 
        When you start typing in the TextBox, the YUI AutoComplete control will use 
        Anthem to call the Search event handler on the server. The results are 
        displayed in a list which YUI AutoComplete monitors. If you select one of the 
        items in the list, the corresponding value will be entered in the TextBox for 
        you.</p>
      <p>The YUI AutoComplete javascript libraries are <a href="http://developer.yahoo.com/yui/articles/hosting/">
          hosted by Yahoo</a>, so you do not need to add the scripts to your site.</p>
      <p>There are&nbsp;2 instances of the YuiAutoComplete control on this page: one in 
        the Page, and one in a User Control. This is to demonstrate that you can use 
        the YuiAutoComplete control in&nbsp;both of these contexts.</p>
      <table border="0" cellpadding="0" cellspacing="0">
        <tr>
          <td valign="top">
            <fieldset>
              <legend>Page</legend>
              <div class="container">
                <asp:TextBox ID="textpage" runat="server" CssClass="textbox" /><br>
                <AnthemExtensions:YuiAutoComplete ID="acpage" runat="server" TextBox="textpage" OnSearch="acpage_Search" formatResult="formatResult"
                  CssClass="results" useShadow="true" />
              </div>
            </fieldset>
          </td>
          <td valign="top">
            <MY:MyAutoComplete ID="myac" runat="server" />
          </td>
        </tr>
      </table>
    </form>
  </body>
</HTML>
