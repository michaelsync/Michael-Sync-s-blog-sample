<%@ Page Language="C#" %>

<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server"> 
    protected void Page_Load(object sender, EventArgs e)
    {
        AutoSuggestTextBox1.BackColor = System.Drawing.Color.Yellow;
        //AutoSuggestTextBox1.DataSource = TestData;
        // START added code by prs - 11nov06
        // multiple column dataset (ID - value pairs)
        AutoSuggestTextBox1.DataSource = TestData2Col;
        // END added code by prs - 11nov06

        AutoSuggestTextBox3.DataSource = ObjectDataSource1;
    }

    protected void button_Click(object sender, EventArgs e)
    {
        AutoSuggestTextBox1.UpdateAfterCallBack = true;
        AutoSuggestTextBox1.BackColor = System.Drawing.Color.Red;
    }

    System.Data.DataSet TestData
    {
        get
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            ds.Tables.Add(dt);
            dt.Columns.Add("name", typeof(string));
            dt.Rows.Add(new object[] { "Allen & Ampersand" });
            dt.Rows.Add(new object[] { "Allison" });
            dt.Rows.Add(new object[] { "Aniston" });
            dt.Rows.Add(new object[] { "Arjona" });
            dt.Rows.Add(new object[] { "Arkenstone" });
            dt.Rows.Add(new object[] { "Arkin" });
            dt.Rows.Add(new object[] { "Arnold" });
            dt.Rows.Add(new object[] { "Arthur" });
            dt.Rows.Add(new object[] { "Astin" });
            dt.Rows.Add(new object[] { "Bert" });
            dt.Rows.Add(new object[] { "Bjorn" });
            dt.Rows.Add(new object[] { "Bon" });
            dt.Rows.Add(new object[] { "Burt" });

            return ds;
        }
    }

    // START added code by prs - 11nov06
    System.Data.DataSet TestData2Col
    {
        get
        {
            System.Data.DataSet ds = new System.Data.DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            ds.Tables.Add(dt);
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Rows.Add(new object[] { 1, "Ampersand" });
            dt.Rows.Add(new object[] { 2, "Allison" });
            dt.Rows.Add(new object[] { 3, "Aniston" });
            dt.Rows.Add(new object[] { 4, "Arjona" });
            dt.Rows.Add(new object[] { 5, "Arkenstone" });
            dt.Rows.Add(new object[] { 6, "Arkin" });
            dt.Rows.Add(new object[] { 7, "Arnold" });
            dt.Rows.Add(new object[] { 8, "Arthur" });
            dt.Rows.Add(new object[] { 9, "Astin" });
            dt.Rows.Add(new object[] { 10, "Bert" });
            dt.Rows.Add(new object[] { 11, "Bjorn" });
            dt.Rows.Add(new object[] { 12, "Bon" });
            dt.Rows.Add(new object[] { 13, "Burt" });

            return ds;
        }
    }
    // END added code by prs - 11nov06
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>AutoSuggestTextBox Demo Page</title>
    
    <!-- Use the following line if you want to debug in VS.NET 2005.
         Don't forget to disable the js injection code in AutoSuggestTextBox.cs
         (search for Page.ClientScript.RegisterClientScriptBlock) -->
    <!-- <script type="text/javascript" src="AutoSuggestTextBox.js"></script> -->
    
    <!-- not used yet -->
    <script language="JavaScript" type="text/javascript">
        function disableEnterKey(e)
        {
             var key;
             if(window.event)
                  key = window.event.keyCode;     //IE
             else
                  key = e.which;     //firefox
             if(key == 13)
                  return false;
             else
                  return true;
        }
    </script>
    
    <!-- START Code added by prs - 14nov06 -->
    <script language="javascript" type="text/javascript">
        function setColumnIDandValue()
        {
            var divID = document.getElementById("divColumnID");
            var divValue = document.getElementById("divColumnValue");
            // the object AutoSuggestTextBox1_Js contains the autosuggest object
            // (it was created in the Render method in the .cs class by injecting javascript)
            divID.innerHTML = AutoSuggestTextBox1_Js.DropDownID;
            divValue.innerHTML = AutoSuggestTextBox1_Js.DropDownValue;
        }
    </script>
    <!-- END Code added by prs - 14nov06 -->
</head>
<body>
    <form id="form1" runat="server">
        <h1>AutoSuggestTextBox Demo Page</h1>
        <p>The AutoSuggestTextBox is a control derived from ASP.TextBox, and uses anthem functionality
           to add a suggest or search capability when the user types characters in it.
           It has a property to search after a mimimum of characters is typed in. It now also features
           a delay after which suggesting starts. This property (DelayTimeBeforeSearch) is 1000 msec. per 
           default but is set here to 2000 msec.</p>
        <h2>AutoSuggestTextBox</h2>
        <p>
            Just start typing an "a" or "A" (or "b" or "B") to see a result set shown in a dropdownlist-like fashion.
            You can use the arrow keys to scroll down and enter to make the final choice. Use 
            the backspace key to undo any typed character. Use the MinCharTypedBeforeSearch property to increase
            the amount of typed chars before suggesting starts.
        </p>
        <p>
            <anthemext:AutoSuggestTextBox ID="AutoSuggestTextBox1" runat="server" MinCharTypedBeforeSearch="1" DelayTimeBeforeSearch="2000" />
        </p>
        <!-- START Code added by prs - 14nov06 -->
        <p>
            The control now supports (fixed) two column support. The first column must contain the ID,<br />
            and the second column is the Value. After having a suggested value, pressing the button<br />
            shows the current ID and Value. These are made available as properties of the javascript object.<br />
            <input type="button" value="Get ID and Value" onclick="setColumnIDandValue()" /><br />
            ColumnID:&nbsp;</p>
        <div id="divColumnID" style="width: 89px; height: 17px"></div>
            ColumnValue:&nbsp;<div id="divColumnValue" style="width: 89px; height: 17px"></div>
        <!-- END Code added by prs - 14nov06 -->
        <p>
            You can change the backgroundcolor by clicking on the button below. Note that this button 
            click eventhandler gets executed also when you press the Enter key after having selected
            a value with the arrow keys and pressing Enter. To reproduce, just place the cursor in 
            the autosuggest textbox and hit Enter.
        </p>
        <p>
            <anthem:Button ID="btnChangeColor" runat="server" Text="Change Textbox Color" OnClick="button_Click" OnClientClick="disableEnterKey(event)" />
        </p>
        <h2>AutoSuggestTextBox with ObjectDataSource</h2>
        <p>
            As above, just start typing an "a" or "A" (or "b" or "B") to see a result set shown in a 
            dropdownlist-like fashion. 
            Here an ObjectDataSource is used to create the DataSet which is then kept in the cache object,
            and after that the filter you typed is applied.
        </p>
        <p>
            <anthemext:AutoSuggestTextBox 
            ID="AutoSuggestTextBox3" 
            runat="server" 
            MinCharTypedBeforeSearch="1" 
            AppSettingsKey="autoSuggestConnectionString" 
            CommandText="" /> 
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            SelectMethod="Select2" 
            TypeName="AutoSuggestDataSource" />
        </p>
        <h2>AutoSuggestTextBox with a DataSet obtained from a database</h2>
        <p>
            As above, just start typing an "b" or "B" (or any letter of the alfabet) to see a result set 
            shown in a dropdownlist-like fashion. 
            This time a connection is made to the local SQL database Northwind, assuming that SQL Server 
            2005 Express is installed. Make sure you modify the connection string in web.config before 
            trying out this example.
        </p>
        <p>
            <anthemext:AutoSuggestTextBox 
            ID="AutoSuggestTextBox2" 
            runat="server" 
            MinCharTypedBeforeSearch="1" 
            AppSettingsKey="autoSuggestConnectionString" 
            CommandText="select employeeid as ID, lastname +','+firstname as NAME FROM employees where Lastname like '@filter%'" /> 
        </p>
    </form>
</body>
</html>
