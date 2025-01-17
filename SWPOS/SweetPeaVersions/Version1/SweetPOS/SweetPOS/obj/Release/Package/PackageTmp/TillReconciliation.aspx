<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="TillReconciliation.aspx.cs" Inherits="SweetPOS.TillReconciliation" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="TillReconciliationPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
		function CalculateTotal() {
			var dblHundred = document.getElementById("txtHundredDollarBills").value;
			var dblFifty = document.getElementById("txtFiftyDollarBills").value;
			var dblTwenty = document.getElementById("txtTwentyDollarBills").value;
			var dblTen = document.getElementById("txtTenDollarBills").value;
			var dblFive = document.getElementById("txtFiveDollarBills").value;
			var dblToonie = document.getElementById("txtToonieCoins").value;
			var dblLoonie = document.getElementById("txtLoonieCoins").value;
			var dblQuarter = document.getElementById("txtQuarterCoins").value;
			var dblDime = document.getElementById("txtDimeCoins").value;
			var dblNickel = document.getElementById("txtNickelCoins").value;
			var dblCountedCash = (dblHundred * 100) + (dblFifty * 50) + (dblTwenty * 20) + (dblTen * 10)
				+ (dblFive * 5) + (dblToonie * 2) + (dblLoonie * 1) + (dblQuarter * 0.25) + (dblDime * 0.10)
				+ (dblNickel * 0.05);
			document.getElementById('lblCountedCashTotal').textContent = dblCountedCash;

		}
	</script>
	<link href="MainStyleSheet.css" rel="stylesheet" type="text/css" />
	<div id="TillCashout" class="yesPrint">
		<div id="print">
			<h2>Till Cashout</h2>
			<hr />
			<%--Payment Breakdown--%>
			<div class="divTillCashout">
				<asp:Label ID="lblTillCashoutDate" Font-Bold="true" runat="server" />
				<hr />
				<h3>Enter A Count of Each Item</h3>
				<asp:Table ID="tblTillCashout" runat="server" GridLines="Both" CssClass="ReconciliatoinTable">
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblHundredDollarBills" Text="$100" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtHundredDollarBills" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revHundredDollarBills"
								ControlToValidate="txtHundredDollarBills"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblToonieCoins" Text="$2" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtToonieCoins" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revToonieCoins"
								ControlToValidate="txtToonieCoins"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblFiftyDollarBills" Text="$50" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtFiftyDollarBills" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revFiftyDollarBills"
								ControlToValidate="txtFiftyDollarBills"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblLoonieCoins" Text="$1" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtLoonieCoins" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revLoonieCoins"
								ControlToValidate="txtLoonieCoins"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblTwentyDollarBills" Text="$20" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtTwentyDollarBills" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revTwentyDollarBills"
								ControlToValidate="txtTwentyDollarBills"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblQuarterCoins" Text="$0.25" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtQuarterCoins" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revQuarterCoins"
								ControlToValidate="txtQuarterCoins"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblTenDollarBills" Text="$10" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtTenDollarBills" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revTenDollarBills"
								ControlToValidate="txtTenDollarBills"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblDimeCoins" Text="$0.10" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtDimeCoins" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revDimeCoins"
								ControlToValidate="txtDimeCoins"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblFiveDollarBills" Text="$5" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtFiveDollarBills" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revFiveDollarBills"
								ControlToValidate="txtFiveDollarBills"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label runat="server" ID="lblNickelCoins" Text="$0.05" Width="80" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtNickelCoins" Text="0" runat="server" AutoComplete="off" Width="30" />
							<asp:RegularExpressionValidator ID="revNickelCoins"
								ControlToValidate="txtNickelCoins"
								ValidationExpression="[-+]?([0-9]*)"
								Display="Static"
								EnableClientScript="true"
								ErrorMessage="Requires a whole number"
								runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</div>
			<br />
			<asp:Button ID="btnProcessCashCount" runat="server" Text="Calculate Over/Short" OnClick="btnProcessCashCount_Click" />
			<br />
			<hr />
			<div class="yesPrint" id="summary_header">
				<h3>Cash Summary</h3>
			</div>
			<div class="yesPrint" id="summary">
				<asp:Table ID="tblCashSummary" runat="server" GridLines="none" CellSpacing="10">
					<asp:TableRow>
						<asp:TableCell Text="Counted Cash Total:" />
						<asp:TableCell>
							<asp:Label ID="lblCountedCashTotal" CssClass="Underline" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Text="Expected Cash Total:" />
						<asp:TableCell>
							<asp:Label ID="lblExpectedCashTotal" CssClass="Underline" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Text="Drawer Float:" />
						<asp:TableCell>
							<asp:Label ID="lblDrawerFloat" CssClass="Underline" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell Text="Till Cash Drop:" />
						<asp:TableCell>
							<asp:Label ID="lblTillCashDrop" CssClass="Underline" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblOverShort" runat="server" Text="Over/Short:" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="lblOverShortDisplay" CssClass="Underline2" runat="server" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
				<hr />
			</div>
			<asp:Button class="noPrint" ID="btnProcessTillCashout" runat="server" Text="Process Till Cashout" Width="200px" OnClick="btnProcessTillCashout_Click" Enabled="false" />
			<asp:Button class="noPrint" ID="btnPrint" runat="server" Text="Print Report" Width="200px" Enabled="false" OnClientClick="CallPrint('print');" />
		</div>
	</div>
</asp:Content>