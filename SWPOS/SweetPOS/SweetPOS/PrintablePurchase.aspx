<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintablePurchase.aspx.cs" Inherits="SweetPOS.PrintablePurchase" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
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
    <div id="divMainMenuLocked" class="noPrint">
        <ul>
            <li><a>HOME</a></li>
            <li><a>CUSTOMERS</a></li>
            <li><a>SALES</a></li>
            <li><a>INVENTORY</a></li>
            <li><a>REPORTS</a></li>
            <li><a>SETTINGS</a></li>
        </ul>
    </div>
    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="printableInvoiceDisplay" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <script>
        function printReport(printable) {
            window.print();
        }
    </script>
    <link rel="stylesheet" type="text/css" href="CSS/displayPrintableInvoice.css" />
    <div id="print">
		<asp:Table runat="server">
			<asp:TableRow>
				<asp:TableCell CssClass="leftSide">
					    <div id="divMainImage">
					       <img src="Images/combinedLogo.jpg" />
						</div>
				</asp:TableCell>
				<asp:TableCell>
					&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
				</asp:TableCell>
				<asp:TableCell CssClass="rightSide">
					<div id="Invoice" class="yesPrint">
						<b>Invoice:
						<asp:Label ID="lblInvoiceNumber" runat="server" />
						<br />
						Date:
						<asp:Label ID="lblInvoiceCompletionDate" runat="server" />
						&nbsp;&nbsp;&nbsp;
						<asp:Label ID="lblInvoiceCompletionTime" runat="server" /></b>
					</div>
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
        <hr />
        <div id="finalInvoice" class="yesPrint">
            <asp:Table ID="tblPartiesInvolved" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <h3><asp:Label ID="lblCustomerName" runat="server" /></h3>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <h3><asp:Label ID="lblStoreName" runat="server" /></h3>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblCustomerAddress" runat="server"  />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblAddress" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblCustomerPostalCode" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblPostalCode" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblCustomerPhoneNumber" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblPhoneNumber" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <asp:GridView ID="grdItemsPurchasedList" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Center" >
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="lblItemSku" Text='<%#Eval("varItemSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblItemDescription" Text='<%#Eval("varItemDescription")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblItemQuantity" Text='<%#Eval("intItemQuantity")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblItemCost" Text='<%# Eval("fltItemCost","{0:C}") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
        <div id="purchaseDetails" class="yesPrint">
            <h3>Purchase Details</h3>
            <asp:Table ID="tblSummary" runat="server" Width="70%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblSubtotalDisplay" runat="server" Text="" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblTotalPaid" runat="server" Text="Total Paid:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblTotalPaidDisplay" runat="server" Text="" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                <asp:GridView ID="grdMOPS" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="Payment Type" />
                        <asp:BoundField DataField="fltAmountReceived" ReadOnly="true" HeaderText="Amount Received" DataFormatString="{0:C}" />
                        <asp:BoundField DataField="intChequeNumber" ReadOnly="true" HeaderText="Cheque Number" />
                    </Columns>
                </asp:GridView>
            </p>
            <br />
            <div class="noPrint">
                <%--added a cssclass here for testing--%>
                <asp:Button ID="btnPrint" CssClass="noPrint" runat="server" Text="Print" Width="100px" OnClientClick="printReport()" />
                <br />
                <asp:Button ID="btnHome" runat="server" Text="Home" Width="100px" OnClick="btnHome_Click" />
                <br />
            </div>
        </div>
    </div>
</asp:Content>