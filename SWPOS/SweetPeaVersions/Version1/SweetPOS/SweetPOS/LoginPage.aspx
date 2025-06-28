<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="LoginPage.aspx.cs" Inherits="SweetPOS.LoginPage" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
    <div id="divMainMenu">
        <ul>
            <li><a>HOME</a></li>
            <li><a>CUSTOMERS</a></li>
            <li><a>SALES</a></li>
            <li><a>INVENTORY</a></li>
            <li><a>REPORTS</a></li>
            <li><a>SETTINGS</a></li>
        </ul>
    </div>
    <div id="divMainImage">
        <img src="Images/CompanyBanner.png" />
    </div>
    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="IndividualPageContent" runat="server">
        <div id="divLoginPage">
        <asp:Panel ID="pnlLoginPanel" runat="server" DefaultButton="btnLogin">
            <asp:Table ID="tblLogin" runat="server" class="auto-style1" >
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblLogin" runat="server" Font-Bold="true" Text="Login Form" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblPassword" runat="server" Text="Password:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEFSHJEMVIFEntry" runat="server" AutoComplete="off" TextMode="Password" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RequiredFieldValidator ID="rfvTxtEFSHJEMVIFEntry" runat="server" ControlToValidate="txtEFSHJEMVIFEntry" ErrorMessage="Please enter a password" ForeColor="Red" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell />
                    <asp:TableCell>
                        <asp:Button ID="btnLogin" runat="server" Text="Log In" OnClick="btnLogin_Click" Width="100%" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>
