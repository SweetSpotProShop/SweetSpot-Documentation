<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SweetPea.aspx.cs" Inherits="SweetPOS.SweetPea" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Table runat="server" Width="80%" HorizontalAlign="Center">
                <asp:TableRow>
                    <asp:TableCell Width =" 100%" HorizontalAlign="Center" ColumnSpan="2">
                        <asp:Image ID="imgPOSLOGO" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width =" 100%" HorizontalAlign="Center" ColumnSpan="2">
                        <h1>Sweet Pea Point Of Sale Manager</h1>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width =" 100%" ColumnSpan="2">
                        <hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right" Width="45%">
                        <asp:Label ID="lblUserName" runat="server" Text="Business Number" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox ID="txtUserName" AutoComplete="off" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Right">
                        <asp:Label ID="lblPassword" runat="server" Text="Access Code" />
                    </asp:TableCell>
                    <asp:TableCell HorizontalAlign="Left">
                        <asp:TextBox ID="txtPassword" runat="server" AutoComplete="off" TextMode="Password" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="100%" HorizontalAlign="Center" ColumnSpan="2">
                        <asp:Button ID="btnSystemLogin" runat="server" OnClick="btnSystemLogin_Click" Text="System Login" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width =" 100%" HorizontalAlign="Center" ColumnSpan="2">
                        <asp:Label ID="lblErrorOccurred" runat="server" Visible="true" ForeColor="Red"
                            Text="There is a problem with your setup information. Some possible causes are: your account is inactive, you have exceeded you licencing limit, or installation on an unauthorized machine. Please email S and G Applications at support@sandgapplications.com. Please include your Client number and any information you believe may be relevent. If this is an emergency contact us directly at 306-526-9039" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width =" 100%" HorizontalAlign="Center" ColumnSpan="2">
                        <h4>Powered by S and G Applications Inc.</h4>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </form>
</body>
</html>