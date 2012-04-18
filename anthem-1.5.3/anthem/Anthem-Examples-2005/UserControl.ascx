<%@ Control Language="C#" %>
<h3>UserControl</h3>
<button onclick="UpdateUserControl('<%= ClientID %>', '<%= result.ClientID %>')" type="button">Update</button>
<span id="result" runat="server">?</span>
<script runat="server">
    void Page_Load()
    {
        Anthem.Manager.Register(this);

        Page.ClientScript.RegisterClientScriptBlock(
            GetType(),
            "script",
            @"<script language='javascript' type='text/javascript'>
function UpdateUserControl(userControlID, spanID) {
    Anthem_InvokeControlMethod(
        userControlID,
        'GetNow',
        [],
        function(result) {
            document.getElementById(spanID).innerHTML = result.value;
        }
    );
}
<" + "/script>"
        );
    }
    
    [Anthem.Method]
    public DateTime GetNow()
    {
        return DateTime.Now;
    }

</script>
