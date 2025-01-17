<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="ReportsDailySalesReconciliation.aspx.cs" Inherits="SweetPOS.ReportsDailySalesReconciliation" %>
<asp:Content ID="ReportsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <style media="print">
        .noPrint {
            display: none;
            /*margin-left: 0;*/
        }
        .yesPrint {
            display: inline-block !important;
            /* margin-right:100px;
           float: right;*/
            margin-left: 10px !important;
        }
    </style>
    <script>
        function CallPrint(strid) {
            var prtContent = document.getElementById(strid);
            var WinPrint = window.open('', '', 'letf=10,top=10,width="450",height="250",toolbar=1,scrollbars=1,status=0');

            WinPrint.document.write("<html><head><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"print\"><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"screen\"></head><body>");

            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write("</body></html>");
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            return false;
        }
    </script>
    <div id="print">
        <h2>Sales Reconciliations Listed By Date</h2>
        <hr />
        <div>
            <asp:Label ID="lblReconciliationDates" runat="server" Font-Bold="true" />
        </div>
        <hr />
        <div>
            <asp:GridView ID="grdReconciliationDate" runat="server" AutoGenerateColumns="false" Width="100%" RowStyle-HorizontalAlign="Center" 
                OnRowCommand="grdReconciliationDate_RowCommand" OnRowDataBound="grdReconciliationDate_RowDataBound" >
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("dtmSalesReconciliationDate","{0:d}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            American Express
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesAmericanExpress" runat="server" Text='<%#Eval("fltAmericanExpressCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesAmericanExpress" runat="server" Text='<%#Eval("fltAmericanExpressSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblAmericanExpressBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltAmericanExpressCounted")) == Convert.ToDouble(Eval("fltAmericanExpressSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Cash
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesCash" runat="server" Text='<%# (Convert.ToDouble(Eval("fltCashCounted")) + Convert.ToDouble(Eval("fltCashPurchases"))).ToString("C") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesCash" runat="server" Text='<%# Eval("fltCashSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblCashBalance" runat="server" ForeColor="Green" Text='<%# (Convert.ToDouble(Eval("fltCashCounted")) + Convert.ToDouble(Eval("fltCashPurchases"))) == Convert.ToDouble(Eval("fltCashSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Cheque
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesCheque" runat="server" Text='<%#Eval("fltChequeCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesCheque" runat="server" Text='<%#Eval("fltChequeSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblChequeBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltChequeCounted")) == Convert.ToDouble(Eval("fltChequeSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Debit
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesDebit" runat="server" Text='<%#Eval("fltDebitCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesDebit" runat="server" Text='<%#Eval("fltDebitSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblDebitBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltDebitCounted")) == Convert.ToDouble(Eval("fltDebitSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Discover
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesDiscover" runat="server" Text='<%#Eval("fltDiscoverCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesDiscover" runat="server" Text='<%#Eval("fltDiscoverSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblDiscoverBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltDiscoverCounted")) == Convert.ToDouble(Eval("fltDiscoverSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Gift Card
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesGiftCard" runat="server" Text='<%#Eval("fltGiftCardCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesGiftCard" runat="server" Text='<%#Eval("fltGiftCardSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblGiftCardBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltGiftCardCounted")) == Convert.ToDouble(Eval("fltGiftCardSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            MasterCard
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesMastercard" runat="server" Text='<%#Eval("fltMastercardCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesMastercard" runat="server" Text='<%#Eval("fltMastercardSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblMastercardBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltMastercardCounted")) == Convert.ToDouble(Eval("fltMastercardSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Trade In
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesTradeIn" runat="server" Text='<%#Eval("fltTradeInCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesTradeIn" runat="server" Text='<%#Eval("fltTradeInSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblTradeInBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltTradeInCounted")) == Convert.ToDouble(Eval("fltTradeInSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Visa
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblReceiptTalliesVisa" runat="server" Text='<%#Eval("fltVisaCounted","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblRecordedSalesVisa" runat="server" Text='<%#Eval("fltVisaSales","{0:C}") %>' />
                            <br />
                            <asp:Label ID="lblVisaBalance" runat="server" ForeColor="Green" Text='<%# Convert.ToDouble(Eval("fltVisaCounted")) == Convert.ToDouble(Eval("fltVisaSales")) ? "Balanced" : "Discrepancy" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Over/Short">
                        <ItemTemplate>
                            <asp:Label ID="lblOverShort" runat="server" Text='<%#Eval("fltOverShort","{0:C}")%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Processed">
                        <ItemTemplate>
                            <asp:Label ID="lblProcessed" runat="server" Text='<%# Convert.ToDouble(Eval("bitIsProcessed")) == 1 ? "TRUE" : "FALSE" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Finalized">
                        <ItemTemplate>
                            <asp:Label ID="lblFinalized" runat="server" Text='<%# Convert.ToDouble(Eval("bitIsFinalized")) == 1 ? "TRUE" : "FALSE" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" CausesValidation="false" CommandName="EditDailyReconciliation" CommandArgument='<%#Eval("dtmSalesReconciliationDate") %>' />
                            <asp:Button ID="btnFinalize" runat="server" Text="Finalize" CausesValidation="false" CommandName="FinalizeReconciliation" CommandArgument='<%#Eval("intSalesReconciliationID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <hr />
    </div>
    <asp:Table runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" OnClientClick="CallPrint('print');" />
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button class="noPrint" ID="btnDownload" runat="server" Text="Download" Visible="true" Width="200px" OnClick="btnDownload_Click" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>