<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsHomePage.aspx.cs" Inherits="SweetPOS.ReturnsHomePage" %>
<asp:Content ID="Content1" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            left: 300px;
            top: -10px;
            width: 207px;
            height: 228px;
        }
        </style>
</asp:Content>
<%-- <asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="returnsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Returns">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnReceiptSearch">
            <h2>Locate Receipt for Return</h2>
            <hr />
            <asp:Table ID="tblReceiptSearch" runat="server" >
                <asp:TableRow>
                    <asp:TableCell RowSpan="3" Width="20%">
                        <asp:Calendar ID="calSearchDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
                            DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="184px" Width="200px" >
                            <DayHeaderStyle BackColor="#5FD367" Font-Bold="True" Font-Size="7pt" />
                            <NextPrevStyle VerticalAlign="Bottom" />
                            <OtherMonthDayStyle ForeColor="#808080" />
                            <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
                            <SelectorStyle BackColor="#CCCCCC" />
                            <TitleStyle BackColor="#005555" BorderColor="Black" Font-Bold="True" />
                            <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <WeekendDayStyle BackColor="#FFFFCC" />
                        </asp:Calendar>
                    </asp:TableCell>
                    <asp:TableCell Width="20%">
                        <asp:Label ID="lblReceiptSearch" runat="server" Text="Enter Receipt Number, Customer Name, <br /> or Customer Phone Number:" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="txtReceiptSearch" runat="server" AutoComplete="off" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnReceiptSearch" runat="server" Width="150" Text="Search" OnClick="btnReceiptSearch_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <div>
                <asp:GridView ID="grdReceiptSelection" runat="server" RowStyle-HorizontalAlign="Center" AutoGenerateColumns="false" 
                    Width="100%" OnRowCommand="grdReceiptSelection_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Receipt Number">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbreceiptID" runat="server" CommandName="returnReceipt" CommandArgument='<%#Eval("intReceiptID")%>' 
                                    Text='<%#Eval("varReceiptNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receipt Date">
                            <ItemTemplate>
                                <asp:Label ID="lblReceiptDate" runat="server" Text='<%#Eval("dtmReceiptCompletionDate","{0: MM/dd/yy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("varCustomerFullName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblAmountPaid" runat="server" Text='<%#Eval("fltAmountPaid","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Store">
                            <ItemTemplate>
                                <asp:Label ID="lblStoreLocation" runat="server" Text='<%#Eval("varStoreName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>