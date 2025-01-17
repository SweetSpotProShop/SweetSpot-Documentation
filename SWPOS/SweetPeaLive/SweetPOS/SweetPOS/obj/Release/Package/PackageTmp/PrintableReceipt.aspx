<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintableReceipt.aspx.cs" Inherits="SweetPOS.PrintableReceipt" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
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
    <div id="divMainMenu" class="noPrint">
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
<asp:Content ID="printableReceiptDisplay" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <script>
        function printReport(printable) {
            window.print();
        }
        function CallPrint(mainPage, disclaimer) {
            var prtContent = document.getElementById(mainPage);
            var prtDisclaimer = document.getElementById(disclaimer);
            var WinPrint = window.open('', '', 'letf=10,top=10,width="450",height="250",toolbar=1,scrollbars=1,status=0');
            WinPrint.document.write("<html><head><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"print\"><LINK rel=\"stylesheet\" type\"text/css\" href=\"css/print.css\" media=\"screen\"></head><body>");
            WinPrint.document.write(prtContent.innerHTML);
            WinPrint.document.write(prtDisclaimer.innerHTML);
            WinPrint.document.write("</body></html>");
            WinPrint.document.close();
            WinPrint.focus();
            WinPrint.print();
            WinPrint.close();
            return false;
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
                        <b>Receipt:
                        <asp:Label ID="lblReceiptNumber" runat="server" Text="" />
                        <br />
                        Tax Number:
                        <asp:Label ID="lblTaxNumber" runat="server" Text="" />
                        <br />
                        Date:
                        <asp:Label ID="lblReceiptCompletionDate" runat="server" Text="" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblReceiptCompletionTime" runat="server" Text="" /></b>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <hr />
        <div id="finalReceipt" class="yesPrint">
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
                        <asp:Label ID="lblCustomerAddress" runat="server" />
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
            <asp:GridView ID="grdItemsSoldList" runat="server" CellPadding="4" Width="100%" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="lblItemSku" Text='<%#Eval("varSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblItemDescription" Text='<%# Eval("varItemDescription")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Retail Price">
                        <ItemTemplate>
                            <%--<asp:Label ID="lblItemPrice" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Eval("fltItemPrice","{0:C}") : (Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice"))) - Convert.ToDouble(Eval("fltItemDiscount"))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C")) %>' runat="server" />--%>
							<asp:Label ID="lblItemPrice" Text='<%# (Eval("fltItemPrice","{0:C}")).ToString() %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discounts">
                        <ItemTemplate>
							<%--<asp:Label ID="lblItemDiscount" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? (Eval("fltItemDiscount","{0:C}")).ToString() : ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) + Convert.ToDouble(Eval("fltItemRefund"))) * Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") %>' runat="server" />--%>
							<asp:Label ID="lblItemDiscount" Text='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? (Eval("fltItemDiscount","{0:C}")).ToString() : ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblItemQuantity" Text='<%#Eval("intItemQuantity")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Sale Price">
                        <ItemTemplate>
                            <%--<asp:Label ID="lblItemSalePrice" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")))-(Convert.ToDouble(Eval("fltItemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C") : Eval("fltItemRefund", "{0:C}") %>' runat="server" />--%>
	                        <asp:Label ID="lblItemSalePrice" Text='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice"))) - (Convert.ToDouble(Eval("fltItemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Extended Price">
                        <ItemTemplate>
							<%--<asp:Label ID="lblItemExtendedPrice" Text='<%# Convert.ToInt32(Request.QueryString["inv"].Split(Convert.ToChar("-"))[1]) == 1 ? Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice"))-Convert.ToDouble(Eval("fltItemDiscount")))*Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))*Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") : (Convert.ToDouble(Eval("fltItemRefund")) * Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") %>' runat="server" />--%>
							<asp:Label ID="lblItemExtendedPrice" Text='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")) - Convert.ToDouble(Eval("fltItemDiscount"))) * Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice")))) * Convert.ToDouble(Eval("intItemQuantity"))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
        <div id="paymentDetails" class="yesPrint">
            <h3>Payment Details</h3>
            <asp:Table ID="tblSummary" runat="server" Width="70%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblDiscounts" runat="server" Text="Discounts:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblDiscountsDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblBlank" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblBlankDisplay" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblTradeIns" runat="server" Text="Trade-Ins:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblTradeInsDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblGST" runat="server" Text="GST:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblGSTDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblShipping" runat="server" Text="Shipping:" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblShippingDisplay" runat="server" Visible="false" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblPST" runat="server" Text="PST:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblPSTDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblTender" runat="server" Text="Tender:" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblTenderDisplay" runat="server" Visible="false" DataFormatString="{0:C}" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                        <asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                        <asp:Label ID="lblSubtotalDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblTotalPaid" runat="server" Text="Total Paid:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblTotalPaidDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblChange" runat="server" Text="Change:" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblChangeDisplay" runat="server" Visible="false" DataFormatString="{0:C}" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                <asp:GridView ID="grdReceiptPayment" runat="server" CellPadding="4" Width="70%" AutoGenerateColumns="false" RowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="Payment Type" />
                        <asp:BoundField DataField="fltAmountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:C}" />
                    </Columns>
                </asp:GridView>
            </p>
        </div>
    </div>
    <div class="noPrint">
        <%--added a cssclass here for testing--%>
        <asp:Button ID="btnPrint" CssClass="noPrint" runat="server" Text="Print" Width="100px" OnClientClick="CallPrint('print', 'disclaimer');" />
        <br />
        <asp:Button ID="btnHome" runat="server" Text="Home" Width="100px" OnClick="btnHome_Click" />
        <br />
    </div>
    <div id="disclaimer">
        <h6>
            <p>
                <%--<b>PLEASE NOTE: </b>
                All used equipment is sold as is and it is understood that its' condition
        and usability may reflect prior use. The Sweet Spot Discount Golf assumes no responsibility
        beyond the point of sale. 
                <b>ALL SALES FINAL</b>
                Thank you for shopping at the Sweet Spot.--%>
            </p>
        </h6>
    </div>
</asp:Content>