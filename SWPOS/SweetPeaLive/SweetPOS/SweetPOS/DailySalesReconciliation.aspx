<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="DailySalesReconciliation.aspx.cs" Inherits="SweetPOS.DailySalesReconciliation" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>

<asp:Content ID="DailySalesReconciliationPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <script>
        function printReport(printable) {
            window.print();
        }
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
    <link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
    <div id="Reconciliation" class="yesPrint">
        <div id="print">
            <h2>Daily Sales Reconciliation</h2>
            <hr />
            <%--Payment Breakdown--%>

            <div class="divReconciliation">
                <asp:Label ID="lblReconciliationDate" Font-Bold="true" runat="server" />
                <hr />
                <h3>Balancing</h3>
                <br />
                <asp:Table ID="tblReconciliation" runat="server" GridLines="Both" CssClass="ReconciliatoinTable">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="9">
                            <asp:Label runat="server" ID="lblSales" Text="Recorded Sales" />
                        </asp:TableCell>
                    </asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesAmericanExpress" Text="American Express" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesCash" Text="Cash" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesCheque" Text="Cheque" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesDebit" Text="Debit" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesDiscover" Text="Discover" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesGiftCard" Text="Gift Card" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesMastercard" Text="Mastercard" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesTradeIn" Text="Trade-In" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesVisa" Text="Visa" Width="80" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesAmericanExpressDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesCashDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesChequeDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesDebitDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesDiscoverDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesGiftCardDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesMastercardDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesTradeInDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblRecordedSalesVisaDisplay" Text="$0.00" Width="80" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell ColumnSpan="6">
                            <asp:Label runat="server" ID="lblReceiptTallies" Text="Receipt Tallies" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesAmericanExpress" Text="American Express" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesCash" Text="Cash" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesCheque" Text="Cheque" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesDebit" Text="Debit" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesDiscover" Text="Discover" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesGiftCard" Text="Gift Card" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesMastercard" Text="Mastercard" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesTradeIn" Text="Trade-In" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label runat="server" ID="lblReceiptTalliesVisa" Text="Visa" Width="80" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesAmericanExpress" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revAmericanExpress"
                                ControlToValidate="txtReceiptTalliesAmericanExpress"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:Label ID="lblReceiptTalliesCashManagement" runat="server" Width="80" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesCheque" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revCheque"
                                ControlToValidate="txtReceiptTalliesCheque"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesDebit" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revDebit"
                                ControlToValidate="txtReceiptTalliesDebit"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesDiscover" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revDiscover"
                                ControlToValidate="txtReceiptTalliesDiscover"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesGiftCard" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revGiftCard"
                                ControlToValidate="txtReceiptTalliesGiftCard"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesMastercard" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revMastercard"
                                ControlToValidate="txtReceiptTalliesMastercard"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesTradeIn" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revTradeIn"
                                ControlToValidate="txtReceiptTalliesTradeIn"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
						<asp:TableCell>
                            <asp:TextBox ID="txtReceiptTalliesVisa" Text="0.00" runat="server" AutoComplete="off" Width="80" />
                            <asp:RegularExpressionValidator ID="revVisa"
                                ControlToValidate="txtReceiptTalliesVisa"
                                ValidationExpression="[$]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnCalculate" runat="server" Text="Calculate" Width="100px" OnClick="btnCalculate_Click" CausesValidation="True"/>
                        </asp:TableCell>
						<asp:TableCell ColumnSpan="5">
                            <asp:Button ID="btnClear" runat="server" Width="90px" Text="Clear" OnClick="btnClear_Click" />
                        </asp:TableCell>
					</asp:TableRow>
                </asp:Table>
            </div>
			<br />
            <hr />
            <div class="yesPrint" id="summary_header">
                <h3>Summary</h3>
            </div>
			<div class="yesPrint" id="summary">
                <asp:Table ID="tblSumm" runat="server" GridLines="none" CellSpacing="10">
                    <asp:TableRow>
						<asp:TableCell Text="Cash Purchases:" />
						<asp:TableCell>
                            <asp:Label ID="lblCashPurchasesDislay" CssClass="Underline" runat="server" />
                        </asp:TableCell>
						<asp:TableCell Text="Pre Tax Sales Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblPreTaxSalesTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
                    </asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Text="Counted Cash Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblReceiptTallyTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
						<asp:TableCell Text="Government Tax Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblGovernmentTaxTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Text="Recorded Sales Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblRecordedSalesTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
						<asp:TableCell Text="Provincial Tax Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblProvincialTaxTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell>
							<asp:Label ID="lblOverShort" runat="server" Text="Over(Short):" />
                        </asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="lblOverShortDisplay" CssClass="Underline2" runat="server" />
                        </asp:TableCell>
						<asp:TableCell Text="Sales and Tax Total:" />
                        <asp:TableCell>
                            <asp:Label ID="lblSalesAndTaxTotal" CssClass="Underline" runat="server" />
                        </asp:TableCell>
					</asp:TableRow>
                </asp:Table>
				<br />
                <hr />
            </div>
            <asp:Button class="noPrint" ID="btnProcessSalesReconciliation" runat="server" Text="Process Sales Reconciliation" Width="200px" OnClick="btnProcessSalesReconciliation_Click" />
            <asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" Enabled="false" OnClientClick="CallPrint('print');" />
        </div>
    </div>
</asp:Content>
