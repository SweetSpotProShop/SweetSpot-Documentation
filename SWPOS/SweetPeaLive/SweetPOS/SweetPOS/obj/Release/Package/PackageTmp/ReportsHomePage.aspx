<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsHomePage.aspx.cs" Inherits="SweetPOS.ReportsHomePage" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
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
<%--<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Reports">
        <div style="text-align: left">
            <asp:Label ID="lblReportAccess" runat="server" Visible="false" ForeColor="Red" Text="You are not authorized to view reports" />
            <asp:Label ID="lblDateSelection" runat="server" Visible="false" Text="Select a date" />
        </div>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnCashOutReport">
            <h2>Reports Selection</h2>
            <br />
            <asp:Label ID="lblStoreLocation" runat="server" Text="Select Location:" />
            <asp:DropDownList ID="ddlStoreLocation" runat="server" AutoPostBack="True" Visible="true" DataTextField="varStoreName" DataValueField="intStoreLocationID" />
            <hr />
            <%--Start Calendar--%>
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell Width="33%">
                        <asp:Calendar ID="calStartDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
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
                    <asp:TableCell Width="33%">
                        <asp:Calendar ID="calEndDate" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" 
                            DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="182px" Width="200px" >
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
                </asp:TableRow>
            </asp:Table>
            <hr />
            <div>
                <asp:Table runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnCashOutReport" runat="server" Text="CashOut Report" Width="200px" OnClick="btnCashOutReport_Click" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber2" runat="server" Text="Report Number 2" Width="200px"/>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber3" runat="server" Text="Report Number 3" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber4" runat="server" Text="Report Number 4" Width="200px" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber5" runat="server" Text="Report Number 5" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber6" runat="server" Text="Report Number 6" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber7" runat="server" Text="Report Number 7" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber8" runat="server" Text="Report Number 8" Width="200px" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber9" runat="server" Text="Report Number 9" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber10" runat="server" Text="Report Number 10" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber11" runat="server" Text="Report Number 11" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber12" runat="server" Text="Report Number 12" Width="200px" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber13" runat="server" Text="Report Number 13" Width="200px" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnReportNumber14" runat="server" Text="Report Number 14" Width="200px" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </div>
        </asp:Panel>
    </div>
</asp:Content>