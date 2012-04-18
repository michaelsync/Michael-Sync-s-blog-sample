<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register Assembly="AnthemExtensions" Namespace="AnthemExtensions" TagPrefix="anthemext" %>

<script runat="server">
    protected void txt_TextChanged(object sender, EventArgs e)
    {
        Anthem.Manager.AddScriptForClientSideEval("alert('Textbox callback fired' + ' - textbox value = " + ((Anthem.TextBox)sender).Text + "')");

    }    
</script>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <h2>
        Example</h2>
    <p>
        Click the TextBox control. It fires a TextChanged event after changing text<br />
        and changing the focus (using the Tab key) and this causes a callback to occur.</p>
    <anthemext:TextBox ID="txt" AutoCallBack="true" OnTextChanged="txt_TextChanged" runat="server">
                <FocusStyle BackColor="AliceBlue"></FocusStyle>
    </anthemext:TextBox>
</asp:Content>
