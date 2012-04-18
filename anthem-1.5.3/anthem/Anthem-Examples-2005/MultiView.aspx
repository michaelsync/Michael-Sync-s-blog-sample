<%@ Page Language="C#" MasterPageFile="~/Sample.master" %>

<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContentPlaceHolder">
    <div>
        <anthem:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <anthem:View ID="View1" runat="server">
                View 1
                <anthem:Button ID="View1_Next" runat="server" Text="Next View" CommandName="NextView" />
                <anthem:Button ID="View1_Last" runat="server" Text="Last View" CommandName="SwitchViewByID"
                    CommandArgument="View3" />
            </anthem:View>
            <anthem:View ID="View2" runat="server">
                View 2
                <anthem:Button ID="View2_First" runat="server" Text="First View" CommandName="SwitchViewByID"
                    CommandArgument="View1" />
                <anthem:Button ID="View2_Prev" runat="server" Text="Previous View" CommandName="PrevView" />
                <anthem:Button ID="View2_Next" runat="server" Text="Next View" CommandName="NextView" />
                <anthem:Button ID="View2_Last" runat="server" Text="Last View" CommandName="SwitchViewByID"
                    CommandArgument="View3" />
            </anthem:View>
            <anthem:View ID="View3" runat="server">
                View 3
                <anthem:Button ID="View3_First" runat="server" Text="First View" CommandName="SwitchViewByID"
                    CommandArgument="View1" />
                <anthem:Button ID="View3_Prev" runat="server" Text="Previous View" CommandName="PrevView" />
            </anthem:View>
        </anthem:MultiView>
    </div>
</asp:Content>
