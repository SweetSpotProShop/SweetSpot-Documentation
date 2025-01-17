<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="SalesHomePage.aspx.cs" Inherits="SweetPOS.SalesHomePage" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style type="text/css">
        .auto-style1 {
            position: relative;
            left: 300px;
            top: -10px;
            width: 207px;
            height: 228px;
        }
    </style>
</asp:Content>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="salesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Sales">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnQuickSale">
            <h2>Sales</h2>
            <hr />
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnQuickSale" runat="server" Width="150" Text="Quick Sale" OnClick="btnQuickSale_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnProcessReturn" runat="server" Width="150" Text="Process Return" OnClick="btnProcessReturn_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnReceiptSearch" runat="server" Text="Search for Receipts" OnClick="btnReceiptSearch_Click" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnCashManagement" runat="server" Text="Cash Management" OnClick="btnCashManagement_Click" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <%--<div class="divider" />--%>
            <hr />
            <h2>Current Open Sales</h2>
            <hr />
            <div>
                <asp:GridView ID="grdCurrentOpenSales" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdCurrentOpenSales_RowCommand" RowStyle-HorizontalAlign="Center" >
                    <Columns>
                        <asp:TemplateField HeaderText="Resume Sale">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbReceiptID" runat="server" CommandName="ResumeSale" CommandArgument='<%#Eval("intReceiptID")%>' Text='<%#Eval("varReceiptNumber") + "-" + Eval("intReceiptSubNumber") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receipt Date">
                            <ItemTemplate>
                                <asp:Label ID="lblReceiptCreationDate" runat="server" Text='<%#Eval("dtmReceiptCreationDate","{0: MM/dd/yy}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer Name">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("varCustomerFullName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Discount">
                            <ItemTemplate>
                                <asp:Label ID="lblDiscountTotal" runat="server" Text='<%#Eval("fltDiscountTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trade In">
                            <ItemTemplate>
                                <asp:Label ID="lblTradeInTotal" runat="server" Text='<%#Eval("fltTradeInTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal">
                            <ItemTemplate>
                                <asp:Label ID="lblSubtotal" runat="server" Text='<%#Eval("fltSubTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GST">
                            <ItemTemplate>
                                <asp:Label ID="lblGSTTotal" runat="server" Text='<%#Eval("fltGovernmentTax","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PST">
                            <ItemTemplate>
                                <asp:Label ID="lblPSTTotal" runat="server" Text='<%#Eval("fltProvincialTax","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total">
                            <ItemTemplate>
                                <asp:Label ID="lblReceiptTotal" runat="server" Text='<%#Eval("fltReceiptTotal","{0:C}") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name">
                            <ItemTemplate>
                                <asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("varEmployeeFullName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                                <asp:Label ID="lblTransactionType" runat="server" Text='<%#Eval("varTransactionName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CustomerID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("intCustomerID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
						<asp:TemplateField HeaderText="Type ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTransactionTypeID" runat="server" Text='<%#Eval("intTransactionTypeID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <hr />
        </asp:Panel>
    </div>
</asp:Content>