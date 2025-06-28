<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderHomePage.aspx.cs" Inherits="SweetPOS.PurchaseOrderHomePage" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="PurchaseOrderHomePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="Inventory">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnProcessTradeIn">
            <h2>Purchase Management</h2>
            <hr />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:Table runat="server">
                        <asp:TableRow>
							<asp:TableCell>
                                <asp:Button ID="btnProcessTradeIn" runat="server" Width="150" Text="Process Trade Ins" OnClick="btnProcessTradeIn_Click" />
                            </asp:TableCell>
							<asp:TableCell Width="15%">
                            </asp:TableCell>
                            <asp:TableCell Width="25%">
                                <asp:Label ID="lblPurchaseOrderNumber" runat="server" Text="Enter Invoice / PO#:" />
                            </asp:TableCell>
							<asp:TableCell>
								<asp:Label ID="lblSelectVendor" runat="server" Text="Select Vendor:" />
                            </asp:TableCell>
							<asp:TableCell>
                            </asp:TableCell>
                        </asp:TableRow>
						<asp:TableRow>
							<asp:TableCell>
								<asp:Button ID="btnBulkPurchase" runat="server" Width="150" Text="Bulk Purchase" OnClick="btnBulkPurchase_Click" />
							</asp:TableCell>
							<asp:TableCell>
                            </asp:TableCell>
							<asp:TableCell>
                                <asp:TextBox ID="txtNewPurchaseOrderNumber" runat="server" AutoComplete="off" placeHolder="Leave Blank to Auto-Create" Width="95%" />
                            </asp:TableCell>
							<asp:TableCell>
                                <asp:DropDownList ID="ddlVendor" runat="server" Width="150" DataTextField="varVendorSupplierName" DataValueField="intVendorSupplierID" />
                            </asp:TableCell>
							<asp:TableCell>
                                <asp:Button ID="btnCreatePO" runat="server" Width="150" Text="Create PO" OnClick="btnCreatePO_Click" />
                            </asp:TableCell>
						</asp:TableRow>
                    </asp:Table>
                    <hr />
					<h2>Open Bulk Purchases</h2>
                    <asp:GridView ID="grdOpenBulkPurchases" runat="server" AutoGenerateColumns="false" CellPadding="3" OnRowCommand="grdOpenBulkPurchases_RowCommand" >
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Update">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnUpdateBulkPurchase" CommandName="update" CommandArgument='<%#Eval("intInvoiceID") %>' Text="Update" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
							<asp:BoundField DataField="dtmInvoiceCreationDate" HeaderText="Created Date" ItemStyle-HorizontalAlign="Center" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
							<asp:BoundField DataField="varInvoiceNumber" HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="varCustomerName" HeaderText="Customer" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="varEmployeeName" HeaderText="Employee" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="fltCostTotal" HeaderText="Invoice Cost" ItemStyle-HorizontalAlign="Center" ReadOnly="true" DataFormatString="{0:C2}" />
							<asp:BoundField DataField="varInvoiceComments" HeaderText="Comments" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnDeleteBulkPurchase" CommandName="delete" CommandArgument='<%#Eval("intInvoiceID") %>' Text="Delete" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No current Bulk Purchases
                        </EmptyDataTemplate>
                    </asp:GridView>
					<hr />
					<h2>Purchase Orders to Receive</h2>
                    <asp:GridView ID="grdOpenPurchaseOrders" runat="server" AutoGenerateColumns="false" CellPadding="3" OnRowCommand="grdOpenPurchaseOrders_RowCommand" >
                        <Columns>
                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Update">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnUpdatePurchaseOrder" CommandName="update" CommandArgument='<%#Eval("intPurchaseOrderID") %>' Text="Update" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
							<asp:BoundField DataField="dtmPurchaseOrderCreationDate" HeaderText="Created Date" ItemStyle-HorizontalAlign="Center" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy}" />
							<asp:BoundField DataField="varPurchaseOrderNumber" HeaderText="PO Number" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="varVendorSupplierName" HeaderText="Vendor" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="varEmployeeName" HeaderText="Employee" ItemStyle-HorizontalAlign="Center" ReadOnly="true" />
							<asp:BoundField DataField="fltCostSubTotal" HeaderText="PO Cost" ItemStyle-HorizontalAlign="Center" ReadOnly="true" DataFormatString="{0:C2}" />
					        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Receive">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnReceivePurchaseOrder" CommandName="receive" CommandArgument='<%#Eval("intPurchaseOrderID") %>' Text="Receive" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            No current Purchase Orders
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
</asp:Content>