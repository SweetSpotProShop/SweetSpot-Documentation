<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="CashManagementHomePage.aspx.cs" Inherits="SweetPOS.CashManagementHomePage" %>

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

<asp:Content ID="salesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Sales">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnProcessTillCashOut">
            <h2>Cash Management</h2>
            <hr />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnProcessTillCashOut" runat="server" Text="Close Till Drawer" OnClick="btnProcessTillCashOut_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnProcessDailyReconciliation" runat="server" Width="150" Text="Daily Reconciliation" OnClick="btnProcessDailyReconciliation_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnFinalizeReconciliation" runat="server" Text="Finalize Reconciliation" OnClick="btnFinalizeReconciliation_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
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
                </asp:TableRow>
            </asp:Table>

            <%--<div class="divider" />--%>
            <hr />

			<h2>Open Tills</h2>
            <hr />
            <div>
				<%-- OnRowDataBound="grdCurrentOpenTills_RowDataBound"
					OnRowCommand="grdCurrentOpenTills_RowCommand"  --%>
                <asp:GridView ID="grdCurrentOpenTills" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:TemplateField HeaderText="Till State">
                            <ItemTemplate>
                                <asp:Button ID="btnUnprocess" runat="server" CommandArgument='<%#Eval("intTillCashoutID")%>' Text="Unprocess" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Till #">
                            <ItemTemplate>
                                <asp:Label ID="lblTillNumber" runat="server" Text='<%#Eval("intTillNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
					    <asp:TemplateField HeaderText="Counted Cash">
                            <ItemTemplate>
                                <asp:Label ID="lblCountedCash" runat="server" Text='<%#Eval("fltCountedTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expected Cash">
                            <ItemTemplate>
                                <asp:Label ID="lblExpectedCash" runat="server" Text='<%#Eval("fltCashDrawerTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Float">
                            <ItemTemplate>
                                <asp:Label ID="lblCashDrawerFloat" runat="server" Text='<%#Eval("fltCashDrawerFloat","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Drop">
                            <ItemTemplate>
                                <asp:Label ID="lblCashDrawerCashDrop" runat="server" Text='<%#Eval("fltCashDrawerCashDrop","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Over/Short">
                            <ItemTemplate>
                                <asp:Label ID="lblOverShort" runat="server" Text='<%#(Convert.ToDouble(Eval("fltCountedTotal")) - Convert.ToDouble(Eval("fltCashDrawerTotal"))).ToString("C")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Processed" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="chkIsProcessed" runat="server" Text='<%#Eval("bitIsProcessed") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />

            <h2>Closed Tills</h2>
            <hr />
            <div>
                <asp:GridView ID="grdCurrentClosedTills" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDataBound="grdCurrentClosedTills_RowDataBound"
					OnRowCommand="grdCurrentClosedTills_RowCommand" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:TemplateField HeaderText="Till State">
                            <ItemTemplate>
                                <asp:Button ID="btnUnprocess" runat="server" CommandArgument='<%#Eval("intTillCashoutID")%>' Text="Unprocess" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Till #">
                            <ItemTemplate>
                                <asp:Label ID="lblTillNumber" runat="server" Text='<%#Eval("intTillNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
					    <asp:TemplateField HeaderText="Counted Cash">
                            <ItemTemplate>
                                <asp:Label ID="lblCountedCash" runat="server" Text='<%#Eval("fltCountedTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Expected Cash">
                            <ItemTemplate>
                                <asp:Label ID="lblExpectedCash" runat="server" Text='<%#Eval("fltCashDrawerTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Float">
                            <ItemTemplate>
                                <asp:Label ID="lblCashDrawerFloat" runat="server" Text='<%#Eval("fltCashDrawerFloat","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Drop">
                            <ItemTemplate>
                                <asp:Label ID="lblCashDrawerCashDrop" runat="server" Text='<%#Eval("fltCashDrawerCashDrop","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Over/Short">
                            <ItemTemplate>
                                <asp:Label ID="lblOverShort" runat="server" Text='<%#(Convert.ToDouble(Eval("fltCountedTotal")) - Convert.ToDouble(Eval("fltCashDrawerTotal"))).ToString("C")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Processed" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="chkIsProcessed" runat="server" Text='<%#Eval("bitIsProcessed") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>
