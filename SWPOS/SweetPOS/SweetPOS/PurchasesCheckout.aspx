<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchasesCheckout.aspx.cs" Inherits="SweetPOS.PurchasesCheckout" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
    <div id="divMainMenuLocked">
        <ul>
            <li><a>HOME</a></li>
            <li><a>CUSTOMERS</a></li>
            <li><a>SALES</a></li>
            <li><a>INVENTORY</a></li>
            <li><a>REPORTS</a></li>
            <li><a>SETTINGS</a></li>
        </ul>
    </div>
    <div id="divMainImage">
        <img src="Images/CompanyBanner.png" />
    </div>
    <link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="PurchaseCheckOutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h3>Transaction Details</h3>
    <div>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnCash">
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2" CssClass="auto-style1">
                        <asp:Table runat="server" >
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" style="text-align: center">Methods of Payment</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="btnCash" runat="server" Text="Cash" OnClick="btnCash_Click" Width="163px" OnClientClick="return confirm('Confirm Cash');" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnCheque" runat="server" Text="Cheque" OnClick="btnCheque_Click" Width="163px" OnClientClick="return confirm('Confirm Cheque');" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="btnDebit" runat="server" Text="Debit" OnClick="btnDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnGiftCard" runat="server" Text="Gift Card" OnClick="btnGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <hr />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblPurchaseAmount" runat="server" Text="Purchase Amount:" Width="163px" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtPurchaseAmount" runat="server" Width="159px" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2" CssClass="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblChequeNumber" runat="server" Text="Enter Cheque Number:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtChequeNumber" runat="server" AutoComplete="off" Text="0000" />
                                    <asp:RegularExpressionValidator ID="revChequeNumber"
                                        ControlToValidate="txtChequeNumber"
                                        ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                        Display="Static"
                                        EnableClientScript="true"
                                        ErrorMessage="Requires a number"
                                        runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalPurchase" runat="server" Text="Total Purchases:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalPurchaseAmount" runat="server" Text="" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:GridView ID="gvCurrentPayment" runat="server" AutoGenerateColumns="false" Width="100%" OnRowDeleting="OnRowDeleting" HorizontalAlign="Center" >
                            <Columns>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton Text="Remove Payment" runat="server" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to remove this Method of Payment?');" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="Payment Type" />
                                <asp:BoundField DataField="fltAmountReceived" ReadOnly="true" HeaderText="Amount Paid Out" DataFormatString="{0:C}" />
                                <asp:BoundField DataField="intChequeNumber" ReadOnly="true" HeaderText="Cheque Number" />
                                <asp:TemplateField HeaderText="Table ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentID" Text='<%#Eval("intPaymentID") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingPurchaseDue" runat="server" Text="Remaining Purchase Due" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingPurchaseDueDisplay" runat="server" Text="" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnCancelPurchase" runat="server" Text="Cancel Purchase" OnClick="btnCancelPurchase_Click" Width="163px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReturnToPurchaseCart" runat="server" Text="Return To Purchases" OnClick="btnReturnToPurchaseCart_Click" Width="163px" />
                    </asp:TableCell>
					<asp:TableCell>
						<asp:Button ID="btnSavePurchase" runat="server" Text="Save Purchase" OnClick="btnSavePurchase_Click" CausesValidation="false" Width="163px" />
					</asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnFinalizePurchase" runat="server" Text="Process Purchase" OnClick="btnFinalizePurchase_Click" Width="163px" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                Comments: <br />
                <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" />
            </p>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            height: 152px;
        }
    </style>
</asp:Content>