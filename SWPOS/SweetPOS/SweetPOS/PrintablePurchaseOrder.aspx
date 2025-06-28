<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintablePurchaseOrder.aspx.cs" Inherits="SweetPOS.PrintablePurchaseOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
    <style media="print">
        .noPrint {
            display: none;
        }
        .yesPrint {
            display: inline-block !important;
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
<asp:Content ID="printablePurchaseOrderDisplay" ContentPlaceHolderID="IndividualPageContent" runat="server">
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
                    <div id="Purchase Order" class="yesPrint">
                        <b>Purchase Order: 
                        <asp:Label ID="lblPurchaseOrderNumber" runat="server" Text="" />
                        <br />
                        Date:
                        <asp:Label ID="lblPurchaseOrderCompletionDate" runat="server" Text="" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblPurchaseOrderCompletionTime" runat="server" Text="" /></b>
                    </div>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <hr />
        <div id="finalPurchaseOrder" class="yesPrint">
            <asp:Table ID="tblPartiesInvolved" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <h3><asp:Label ID="lblVendorName" runat="server" /></h3>
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <h3><asp:Label ID="lblStoreName" runat="server" /></h3>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblVendorAddress" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblAddress" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblVendorPostalCode" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblPostalCode" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell CssClass="leftSide">
                        <asp:Label ID="lblVendorPhoneNumber" runat="server" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSide">
                        <asp:Label ID="lblPhoneNumber" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <hr />
            <h3>Expected Items</h3>
            <asp:GridView ID="grdExpectedItemsList" runat="server" CellPadding="4" Width="100%" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="lblExpectedItemSku" Text='<%#Eval("varSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Vendor SKU">
                        <ItemTemplate>
                            <asp:Label ID="lblExpectedVendorItemSku" Text='<%#Eval("varVendorSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblExpectedItemDescription" Text='<%# Eval("varItemDescription")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost">
                        <ItemTemplate>
							<asp:Label ID="lblExpectedItemCost" Text='<%# (Eval("fltPurchaseOrderCost","{0:C}")).ToString() %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblExpectedItemQuantity" Text='<%#Eval("intPurchaseOrderQuantity")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Extended Cost">
                        <ItemTemplate>
							<asp:Label ID="lblExpectedItemExtendedCost" Text='<%# ((Convert.ToDouble(Eval("fltPurchaseOrderCost")) * Convert.ToDouble(Eval("intPurchaseOrderQuantity")))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <h3>Received Items</h3>
            <asp:GridView ID="grdReceivedItems" runat="server" CellPadding="4" Width="100%" AutoGenerateColumns="False" RowStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:TemplateField HeaderText="SKU #">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedItemSku" Text='<%#Eval("varSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
					<asp:TemplateField HeaderText="Vendor SKU">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedVendorItemSku" Text='<%#Eval("varVendorSku")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedItemDescription" Text='<%# Eval("varItemDescription")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cost">
                        <ItemTemplate>
							<asp:Label ID="lblReceivedItemCost" Text='<%# (Eval("fltReceivedCost","{0:C}")).ToString() %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblReceivedItemQuantity" Text='<%#Eval("intReceivedQuantity")%>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Extended Cost">
                        <ItemTemplate>
							<asp:Label ID="lblReceivedItemExtendedCost" Text='<%# ((Convert.ToDouble(Eval("fltReceivedCost")) * Convert.ToDouble(Eval("intReceivedQuantity")))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
        </div>
        <div id="paymentDetails" class="yesPrint">
            <h3>Cost Details</h3>
            <asp:Table ID="tblSummary" runat="server" Width="70%">
                <asp:TableRow>
                    <asp:TableCell CssClass="leftFirst">
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
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
                    </asp:TableCell>
                    <asp:TableCell CssClass="leftSecond">
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                        <asp:Label ID="lblPST" runat="server" Text="PST:" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
                        <asp:Label ID="lblPSTDisplay" runat="server" DataFormatString="{0:C}" />
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightFirst">
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
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
                    </asp:TableCell>
                    <asp:TableCell CssClass="rightSecond">
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