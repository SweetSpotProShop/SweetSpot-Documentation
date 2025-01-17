<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsCheckout.aspx.cs" Inherits="SweetPOS.ReturnsCheckout" %>
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
<asp:Content ID="ReturnsCheckoutPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <h3>Transaction Details</h3>
    <div>
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="mopCash">
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2" CSSClass="auto-style1">
                        <asp:Table runat="server">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Style="text-align: center">Methods For Refund</asp:TableCell>
                            </asp:TableRow>
				             <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopAmericanExpress" runat="server" Text="American Express" OnClick="mopAmericanExpress_Click" Width="163px" OnClientClick="return confirm('Confirm American Express');" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell>
									<asp:Button ID="mopCash" runat="server" Text="Cash" OnClick="mopCash_Click" Width="163px" OnClientClick="return confirm('Confirm Cash');" Enabled="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
									<asp:Button ID="mopCheque" runat="server" Text="Cheque" OnClick="mopCheque_Click" Width="163px" OnClientClick="return confirm('Confirm Cheque');" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell>
									<asp:Button ID="mopDebit" runat="server" Text="Debit" OnClick="mopDebit_Click" Width="163px" OnClientClick="return confirm('Confirm Debit');" Enabled="false" />
                                </asp:TableCell>
                            </asp:TableRow>
							<asp:TableRow>
								<asp:TableCell>
                                    <asp:Button ID="mopDiscover" runat="server" Text="Discover" OnClick="mopDiscover_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Button ID="mopGiftCard" runat="server" Text="Gift Card" OnClick="mopGiftCard_Click" Width="163px" OnClientClick="return confirm('Confirm Gift Card');" Enabled="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Button ID="mopMastercard" runat="server" Text="Mastercard" OnClick="mopMastercard_Click" Width="163px" OnClientClick="return confirm('Confirm Mastercard');" Enabled="false" />
                                </asp:TableCell>
                                <asp:TableCell>
									<asp:Button ID="mopVisa" runat="server" Text="Visa" OnClick="mopVisa_Click" Width="163px" OnClientClick="return confirm('Confirm Visa');" Enabled="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2">
                                    <hr />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundAmount" runat="server" Text="Refund Amount:" Width="163px" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:TextBox ID="txtAmountRefunding" runat="server" AutoComplete="off" Width="159px" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2" CSSClass="auto-style1">
                        <asp:Table ID="tblTotals" runat="server">
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundSubTotal" runat="server" Text="Refund Subtotal:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundSubTotalAmount" runat="server" Text="" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernment" runat="server" Text="GST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblGovernmentAmount" runat="server" Text="" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincial" runat="server" Text="PST:" Visible="false" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblProvincialAmount" runat="server" Text="" Visible="false" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundBalance" runat="server" Text="Total Refund Amount:" />
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label ID="lblRefundBalanceAmount" runat="server" Text="" />
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
                                        <asp:LinkButton Text="Remove Refund Method" runat="server" CommandArgument='<%#Eval("intPaymentID") %>' OnClientClick="return confirm('Are you sure you want to remove this Method of Payment?');" CausesValidation="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="Refund Type" />
                                <asp:BoundField DataField="fltAmountPaid" ReadOnly="true" HeaderText="Refund Amount" DataFormatString="{0:C}" />
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingRefund" runat="server" Text="Remaining Refund:" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="2">
                        <hr />
                        <asp:Label ID="lblRemainingRefundDisplay" runat="server" Text="" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnCancelReturn" runat="server" Text="Void Transaction" OnClick="btnCancelReturn_Click" Width="163px" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReturnToCart" runat="server" Text="Return Cart" OnClick="btnReturnToCart_Click" Width="163px" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnFinalize" runat="server" Text="Process Refund" OnClick="btnFinalize_Click" Width="163px" />                        
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <p>
                Comments:
               <br />
                <asp:TextBox ID="txtComments" runat="server" AutoComplete="off" TextMode="MultiLine" />
            </p>
        </asp:Panel>
    </div>
</asp:Content>