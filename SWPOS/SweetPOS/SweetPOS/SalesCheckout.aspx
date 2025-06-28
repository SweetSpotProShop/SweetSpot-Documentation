﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="SalesCheckout.aspx.cs" Inherits="SweetPOS.SalesCheckout" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            height: 152px;
        }
    </style>
</asp:Content>
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
<asp:Content ID="CheckoutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h3>Transaction Details</h3>
    <div>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="mopCash">
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2" CssClass="auto-style1">
                        <asp:Table runat="server">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" style="text-align: center">Methods of Payment</asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopAmericanExpress" runat="server" Enabled="false" Text="American Express" OnClick="mopAmericanExpress_Click" Width="163px" OnClientClick="return confirm('Confirm American Express');" CausesValidation="false" />
                                </asp:TableCell>
                                <asp:TableCell>
	                                <asp:Button ID="mopCash" runat="server" Enabled="false" Text="Cash" OnClick="mopCash_Click" Width="163px" OnClientClick="return confirm('Cash');" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
							<asp:TableRow>
                                <asp:TableCell>
									<asp:Button ID="mopCheque" runat="server" Enabled="false" Text="Cheque" OnClick="mopCheque_Click" Width="163px" OnClientClick="return confirm('Confirm Cheque');" CausesValidation="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopDebit" runat="server" Enabled="false" Text="Debit" OnClick="mopDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" CausesValidation="false" />                                    
                                </asp:TableCell>
                            </asp:TableRow>
							<asp:TableRow>
                                <asp:TableCell>
									<asp:Button ID="mopDiscover" runat="server" Enabled="false" Text="Discover" OnClick="mopDiscover_Click" Width="163px" OnClientClick="return confirm('Confirm Discover');" CausesValidation="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopGiftCard" runat="server" Enabled="false" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopMastercard" runat="server" Enabled="false" Text="Mastercard" OnClick="mopMastercard_Click" Width="163px" OnClientClick="return confirm('Confirm Mastercard');" CausesValidation="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopVisa" runat="server" Enabled="false" Text="Visa" OnClick="mopVisa_Click" Width="163px" OnClientClick="return confirm('Confirm Visa');" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
							<asp:TableRow>
                                <asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
                                    <asp:Button ID="mopChargeToAccount" runat="server" Enabled="false" Text="Charge To Account" OnClick="mopChargeToAccount_Click" Width="163px" OnClientClick="return confirm('Confirm Charge To Account');" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <hr />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblAmountPaid" runat="server" Text="Owing:" Width="163px" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtAmountPaying" runat="server" AutoComplete="off" Width="159px" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2" CssClass="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInCart" runat="server" Text="Total In Cart:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInCartAmount" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInDiscounts" runat="server" Text="Total Discounts:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTotalInDiscountsAmount" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblTradeIns" runat="server" Text="Trade-Ins:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblTradeInsAmount" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <%--<asp:Label ID="lblShipping" runat="server" Text="Shipping:" />--%>
                                </asp:TableCell>
                                <asp:TableCell>
                                    <%--<asp:Label ID="lblShippingAmount" runat="server" />--%>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblSubTotal" runat="server" Text="Subtotal:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblSubTotalAmount" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernment" runat="server" Text="GST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernmentAmount" runat="server" Visible="false" Text="0" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnRemoveGov" runat="server" Text="Remove GST" Width="163px" Visible="false" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincial" runat="server" Text="PST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincialAmount" runat="server" Visible="false" Text="0" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="btnRemoveProv" runat="server" Text="Remove PST" Width="163px" Visible="false" CausesValidation="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblBalance" runat="server" Text="Balance Due:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblBalanceAmount" runat="server" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:GridView ID="gvCurrentMOPs" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="gvCurrentMOPs_RowCommand" RowStyle-HorizontalAlign="Center" >
                            <Columns>
                                <asp:TemplateField HeaderText="Remove">
                                    <ItemTemplate>
                                        <asp:LinkButton Text="Remove MOP" runat="server" CommandArgument='<%# Eval("intPaymentID") %>' OnClientClick="return confirm('Are you sure you want to remove this Method of Payment?');" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="Payment Type" />
                                <asp:BoundField DataField="fltAmountPaid" ReadOnly="true" HeaderText="Amount Paid" DataFormatString="{0:C}" />
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingBalanceDue" runat="server" Text="Remaining Balance Due" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingBalanceDueDisplay" runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnCancelSale" runat="server" Text="Void Transaction" OnClick="btnCancelSale_Click" Width="163px" CausesValidation="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnExitSale" runat="server" Text="Hold Sale" OnClick="btnExitSale_Click" Width="163px" CausesValidation="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <%--<asp:Button ID="btnLayaway" runat="server" Text="Layaway" OnClick="btnLayaway_Click" Width="163px" CausesValidation="false" Visible="false" />--%>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReturnToCart" runat="server" Text="Sales Cart" OnClick="btnReturnToCart_Click" Width="163px" CausesValidation="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnFinalize" runat="server" Text="Process Sale" OnClick="btnFinalize_Click" Width="163px" CausesValidation="true" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblConfirmEmployee" runat="server" Text="Enter Employee Passcode:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtEmployeePasscode" runat="server" AutoComplete="off" TextMode="Password" />
                    </asp:TableCell>
                    <asp:TableCell>
                         <asp:RequiredFieldValidator ID="valEmployeePasscode" runat="server" ForeColor="red" ErrorMessage="Must Enter Passcode" ControlToValidate="txtEmployeePasscode" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                Comments: <br /><asp:TextBox ID="txtComments" runat="server" AutoComplete="off" TextMode="MultiLine" />
                        <asp:HiddenField ID="hdnTender" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnChange" runat="server" value="0" />
            </p>
        </asp:Panel>
    </div>
    <script>
        function userInput() {
            var r = confirm('Confirm Cash');
            if (r) {
                var given = prompt("Change Calculator", "");
                var change = Number(document.getElementById('<%=txtAmountPaying.ClientID %>').value) - given;
                var give = String(change.toFixed(2));
                if (change < 0) {
                    alert("Change: " + give);
                }
                document.getElementById('<%=hdnTender.ClientID %>').value = given;
                document.getElementById('<%=hdnChange.ClientID %>').value = give;
            }
        }
    </script>
</asp:Content>