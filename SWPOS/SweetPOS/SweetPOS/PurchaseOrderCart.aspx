<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchaseOrderCart.aspx.cs" Inherits="SweetPOS.PurchaseOrderCart" %>
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
	<%--<div id="divMainImage">
		<img src="Images/CompanyBanner.png" />
	</div>--%>
	<link rel="stylesheet" type="text/css" href="CSS/MainStyleSheet.css" />
</asp:Content>
<asp:Content ID="PurchaseOrderCartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="PurchaseOrderCart">
		<asp:panel id="pnlDefaultButton" runat="server" defaultbutton="btnSavePurchaseForProcessing">
			<asp:Label ID="lblVendor" runat="server" Text="Vendor:" />
			<asp:TextBox ID="txtVendorDisplay" runat="server" Text="" Enable="false" />
			<br />
			<br />
			<div style="text-align: right">
				<asp:Label ID="lblPurchaseOrderNumber" runat="server" Text="PO No:" />
				<asp:Label ID="lblPurchaseOrderNumberDisplay" runat="server" />
				<br />
				<asp:Label ID="lblDate" runat="server" Text="Date:" />
				<asp:Label ID="lblDateDisplay" runat="server" Text="" />
				<hr />
			</div>
			<h3>Available Items</h3>
			<hr />
			<asp:GridView ID="grdVendorSupplierItems" runat="server" AutoGenerateColumns="false" OnRowCommand="grdVendorSupplierItems_RowCommand" RowStyle-HorizontalAlign="Center">
				<Columns>
					<asp:TemplateField HeaderText="Add To PO">
						<ItemTemplate>
							<asp:LinkButton ID="lkbAddItemToPO" Text="Add Item" CommandArgument='<%#Eval("intVendorSupplierProductID") %>' runat="server" CausesValidation="false" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
					<asp:BoundField DataField="varVendorSku" ReadOnly="true" HeaderText="Vendor SKU" />
					<asp:TemplateField HeaderText="Quantity">
						<ItemTemplate>
							<div>
								<asp:TextBox ID="txtQuantity" runat="server" AutoComplete="off" placeholder="Enter Quantity" />
							</div>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
					<asp:BoundField DataField="fltPurchaseOrderCost" ReadOnly="true" HeaderText="Cost" DataFormatString="{0:C}" />
				</Columns>
			</asp:GridView>
			<hr />
			<h3>Purchase Order Cart</h3>
			<hr />
			<asp:GridView ID="grdPurchaseOrderItems" runat="server" AutoGenerateColumns="false" OnRowCommand="grdPurchaseOrderItems_RowCommand" RowStyle-HorizontalAlign="Center">
				<Columns>
					<asp:TemplateField HeaderText="Remove">
						<ItemTemplate>
							<asp:LinkButton ID="lkbRemoveItem" Text="Remove" CommandArgument='<%#Eval("intPurchaseOrderItemID") %>' runat="server" CausesValidation="false" />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
					<asp:BoundField DataField="varVendorSku" ReadOnly="true" HeaderText="Vendor SKU" />
					<asp:BoundField DataField="intPurchaseOrderQuantity" ReadOnly="true" HeaderText="Quantity" />
					<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
					<asp:BoundField DataField="fltPurchaseOrderCost" ReadOnly="true" HeaderText="Cost" DataFormatString="{0:C}" />
				</Columns>
			</asp:GridView>
			<hr />
			<asp:Table ID="tblPurchaseOrderTotals" runat="server">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblGSTTotal" runat="server" Text="GST Total:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblGSTTotalDisplay" runat="server" Text="" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:RadioButtonList ID="rdbGSTIncorp" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" 
							OnSelectedIndexChanged="rdbGSTIncorp_SelectedIndexChanged">
							<asp:ListItem Text="Add In" runat="server" Value="1" />
							<asp:ListItem Text="Remove" runat="server" Value="0" />
						</asp:RadioButtonList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblPSTTotal" runat="server" Text="PST Total:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblPSTTotalDisplay" runat="server" Text="" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:RadioButtonList ID="rdbPSTIncorp" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
							OnSelectedIndexChanged="rdbPSTIncorp_SelectedIndexChanged">
							<asp:ListItem Text="Add In" runat="server" Value="1" />
							<asp:ListItem Text="Remove" runat="server" Value="0" />
						</asp:RadioButtonList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblPurchaseOrderSubtotal" runat="server" Text="Purchase Order Subtotal:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblPurchaseOrderSubtotalDisplay" runat="server" Text="" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<hr />
			<asp:Button ID="btnCancelPurchaseOrder" runat="server" Text="Void Purchase Order" OnClick="btnCancelPurchaseOrder_Click" CausesValidation="false" />
			<asp:Button ID="btnSavePurchaseForProcessing" runat="server" Text="Save Purchase Order" OnClick="btnSavePurchaseForProcessing_Click" CausesValidation="false" />
		</asp:panel>
	</div>
</asp:Content>