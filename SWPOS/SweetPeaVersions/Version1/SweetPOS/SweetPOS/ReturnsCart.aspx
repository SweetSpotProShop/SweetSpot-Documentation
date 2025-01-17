﻿<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="ReturnsCart.aspx.cs" Inherits="SweetPOS.ReturnsCart" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>--%>
<asp:Content ID="NonActive" ContentPlaceHolderID="SPMaster" runat="server">
    <style>
        .costDetail {
            display: none;
        }
        .cost:hover .costDetail {
            display: block;
            position: absolute;
            text-align: left;
            max-width: 300px;
            max-height: 300px;
            overflow: auto;
            background-color: #fff;
            border: 2px solid #bbb;
            padding: 3px;
        }
    </style>
    <style>
        .priceDetail {
            display: none;
        }
        .price:hover .priceDetail {
            display: block;
            position: absolute;
            text-align: left;
            max-width: 300px;
            max-height: 300px;
            overflow: auto;
            background-color: #fff;
            border: 2px solid #bbb;
            padding: 3px;
        }
    </style>
    <div id="divMainMenu">
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
<asp:Content ID="ReturnsCartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="ReturnCart">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnProceedToReturnCheckout">
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
            <asp:Label ID="lblCustomerDisplay" runat="server" Text="" />
            <br />
            <br />
            <div style="text-align: right">
                <asp:Label ID="lblReceiptNumber" runat="server" Text="Receipt No:" />
                <asp:Label ID="lblReceiptNumberDisplay" runat="server" />
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:" />
                <asp:Label ID="lblDateDisplay" runat="server" Text="" />
                <hr />
            </div>
            <h3>Available Items</h3>
            <hr />
            <asp:Label ID="lblInvalidQuantity" runat="server" Visible="false" Text="Invalid Quantity Entered" ForeColor="Red" />
            <asp:GridView ID="grdReceiptItems" runat="server" AutoGenerateColumns="false" OnRowCommand="grdReceiptItems_RowCommand" RowStyle-HorizontalAlign="Center" >
                <Columns>
                    <asp:TemplateField HeaderText="Return Item">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbReturnItem" Text="Return Item" CommandArgument='<%#Eval("intInventoryID") %>' runat="server" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <div>
                                <asp:Label ID="lblQuantitySold" runat="server" Text='<%#Eval("intItemQuantity") %>' />
                            </div>
                            <div>
                                <asp:TextBox ID="txtQuantityToReturn" runat="server" AutoComplete="off" placeholder="Enter Quantity To Return" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
                    <asp:TemplateField HeaderText="Paid">
                        <ItemTemplate>
                            <%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")))-(Convert.ToDouble(Eval("fltItemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Discount Applied" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbIsPercentageDiscount" Checked='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
                            <div id="divReturnAmountDiscount" class="txt" runat="server">
                                <asp:Label ID="lblReturnAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Non-Stocked Product" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsNonStockedProduct" Checked='<%# Eval("bitIsNonStockedProduct") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Regular Product" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIsRegularProduct" Checked='<%# Eval("bitIsRegularProduct") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount to Refund">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReturnAmount" AutoComplete="off" Text='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) == false ? ((Convert.ToDouble(Eval("fltItemPrice")))-(Convert.ToDouble(Eval("fltItemDiscount")))).ToString("C") : ((Convert.ToDouble(Eval("fltItemPrice")) - ((Convert.ToDouble(Eval("fltItemDiscount")) / 100) * Convert.ToDouble(Eval("fltItemPrice"))))).ToString("C") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <h3>Return Cart</h3>
            <hr />
            <asp:GridView ID="grdReturningItems" runat="server" AutoGenerateColumns="false" OnRowCommand="grdReturningItems_RowCommand" RowStyle-HorizontalAlign="Center" >
                <Columns>
                    <asp:TemplateField HeaderText="Cancel Return">
                        <ItemTemplate>
                            <asp:LinkButton ID="lkbCancelItem" Text="Cancel Return" CommandArgument='<%#Eval("intInventoryID") %>' runat="server" CausesValidation="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
                    <asp:BoundField DataField="intItemQuantity" ReadOnly="true" HeaderText="Quantity" />
                    <asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
                    <asp:BoundField DataField="fltItemRefund" ReadOnly="true" HeaderText="Refund Amount" DataFormatString="{0:C}" />
                    <asp:TemplateField HeaderText="Discount Applied" Visible="false">
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbIsPercentageDiscountReturn" Checked='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
                            <div id="divRIReturnAmountDiscount" class="txt" runat="server">
                                <asp:Label ID="lblRIReturnAmountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Non-Stocked Product" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblIsNonStockedProductReturn" Text='<%# Eval("bitIsNonStockedProduct") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Regular Product" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblIsRegularProductReturn" Text='<%# Eval("bitIsRegularProduct") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblReturnSubtotal" runat="server" Text="Return Subtotal:" />
            <asp:Label ID="lblReturnSubtotalDisplay" runat="server" Text="" />
            <hr />
            <asp:Button ID="btnCancelReturn" runat="server" Text="Void Transaction" OnClick="btnCancelReturn_Click" CausesValidation="false" />
            <asp:Button ID="btnProceedToReturnCheckout" runat="server" Text="Checkout" OnClick="btnProceedToReturnCheckout_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>