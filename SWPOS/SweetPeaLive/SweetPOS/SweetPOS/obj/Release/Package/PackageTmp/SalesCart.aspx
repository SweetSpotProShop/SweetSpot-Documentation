<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="SalesCart.aspx.cs" Inherits="SweetPOS.SalesCart" %>
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
<asp:Content ID="SaleCartPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="SaleCart">
		<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnInventorySearch">
			<asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
			<asp:TextBox ID="txtCustomer" runat="server" AutoComplete="off" Enabled="false" />
			<asp:Button ID="btnCustomerSelect" runat="server" Text="Change Customer" OnClick="btnCustomerSelect_Click" Visible="false" CausesValidation="false" />
			<div>
				<br />
				<div>
					<asp:GridView ID="grdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true" OnRowCommand="grdCustomersSearched_RowCommand"
						AllowPaging="True" PageSize="5" OnPageIndexChanging="grdCustomersSearched_PageIndexChanging">
						<Columns>
							<asp:TemplateField HeaderText="Switch Customer">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("intCustomerID") %>' Text="Switch Customer" runat="server" />
								</ItemTemplate>
								<FooterTemplate>
									<asp:Button ID="btnAddNewCustomer" runat="server" Text="Add New Customer" OnClick="btnAddNewCustomer_Click" />
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Customer Name">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varLastName") + ", " + Eval("varFirstName") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtLastName" runat="server" AutoComplete="off" placeholder="Last Name" ToolTip="Last Name" />
									</div>
									<div>
										<asp:TextBox ID="txtFirstName" runat="server" AutoComplete="off" placeholder="First Name" ToolTip="First Name" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Home Phone Number">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varHomePhone") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtHomePhone" runat="server" AutoComplete="off" placeholder="Home Phone Number" ToolTip="Home Phone Number" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Email Address">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varEmailAddress") %>' />
								</ItemTemplate>
								<FooterTemplate>
									<div>
										<asp:TextBox ID="txtEmailAddress" runat="server" AutoComplete="off" placeholder="Email Address" ToolTip="Email Address" />
									</div>
									<div>
										<asp:CheckBox ID="chkAllowMarketing" runat="server" Text="Allow Marketing Enrollment" />
									</div>
								</FooterTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
				<br />
			</div>
			<%--//Radio button for InStore or Shipping--%>
			<%--     <asp:RadioButton ID="rdbInStorePurchase" runat="server" Text="In Store" Checked="True" Enabled="false" GroupName="rgSales" />
            <asp:RadioButton ID="rdbToBeShipped" runat="server" Text="To Be Shipped" GroupName="rgSales" Enabled="false" />
            <asp:Label ID="lblShipAmount" runat="server" Text="Amount:" />
            <asp:TextBox ID="txtShipAmount" runat="server" OnTextChanged="txtShipAmount_TextChanged" AutoPostBack="true" Text="0" />
            <asp:Label ID="lblShipWarning" runat="server" Visible="false" />--%>
			<div style="text-align: right">
				<asp:Label ID="lblReceiptNumber" runat="server" Text="Receipt #:" />
				<asp:Label ID="lblReceiptNumberDisplay" runat="server" />
				<br />
				<asp:Label ID="lblDate" runat="server" Text="Date:" />
				<asp:Label ID="lblDateDisplay" runat="server" />
				<hr />
			</div>
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
					<div>
						<asp:TextBox ID="txtInventorySearch" runat="server" AutoComplete="off" />
						<asp:Button ID="btnInventorySearch" runat="server" Width="150" Text="Inventory Search" OnClick="btnInventorySearch_Click" />
						<asp:Button ID="btnClearSearch" runat="server" Width="150" Text="Clear Search" OnClick="btnClearSearch_Click" />
						<asp:Button ID="btnRefreshCart" runat="server" Width="150" Text="Refresh Cart" OnClick="btnRefreshCart_Click" Visible="false" />
					</div>
					<asp:UpdateProgress ID="UpdateProgress1" runat="server">
						<ProgressTemplate>
							<div>
								<img src="Images/ajax-loader.gif" />
							</div>
						</ProgressTemplate>
					</asp:UpdateProgress>
					<hr />
					<asp:GridView ID="grdInventorySearched" runat="server" AutoGenerateColumns="False" OnRowCommand="grdInventorySearched_RowCommand" RowStyle-HorizontalAlign="Center">
						<Columns>
							<asp:TemplateField HeaderText="Add Item">
								<ItemTemplate>
									<asp:LinkButton Text="Add Item" runat="server" CommandName="AddItem" CommandArgument='<%#Eval("intInventoryID")%>' CausesValidation="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="varSku" HeaderText="SKU" />
							<asp:TemplateField HeaderText="In Stock">
								<ItemTemplate>
									<div>
										<asp:Label ID="lblAvailableQuantity" Text='<%#Eval("intItemQuantity")%>' runat="server" />
									</div>
									<div>
										<asp:TextBox ID="txtSaleQuantity" runat="server" AutoComplete="off" placeholder="Enter Quantity To Add" />
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Description">
								<ItemTemplate>
									<div>
										<asp:Label ID="Description" Text='<%#Eval("varItemDescription")%>' runat="server" />
									</div>
									<div>
										<asp:TextBox ID="txtTradeInDescription" runat="server" AutoComplete="off" placeholder="Enter Description" Visible="false" />
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
								<ItemTemplate>
									<div class='cost' id="divRollOverSearch" runat="server">
										<asp:Label ID="rollPrice" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
										<div id="divPriceConvert" class="costDetail" runat="server">
											<asp:Label ID="rollCost" runat="server" Text='<%# Convert.ToString(Eval("fltItemAverageCostAtSale","{0:C}")).Replace("\n","<br/>") %>' />
										</div>
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Discount">
								<ItemTemplate>
									<div>
										<asp:CheckBox ID="chkIsPercentDiscount" runat="server" Text="Discount by Percent" />
									</div>
									<div>
										<asp:TextBox ID="txtItemDiscount" runat="server" AutoComplete="off" placeholder="Enter Amount" />
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Trade In" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkTradeInSearch" Checked='<%# Eval("bitIsTradeIn") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Non-Stocked Product" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkNonStockedProduct" Checked='<%# Eval("bitIsNonStockedProduct") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="RegularProduct" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkRegularProduct" Checked='<%# Eval("bitIsRegularProduct") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<hr />
					<h3>Sale Cart</h3>
					<hr />
					<asp:Label ID="lblInvalidQuantity" runat="server" Visible="false" Text="Quantity Exceeds the Remaining Inventory" ForeColor="Red" />
					<asp:GridView ID="grdSaleCartItem" EmptyDataText=" No Records Found" runat="server" AutoGenerateColumns="false" Style="margin-right: 0px"
						RowStyle-HorizontalAlign="Center"
						OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="ORowCanceling" OnRowDeleting="OnRowDeleting">
						<Columns>
							<asp:TemplateField HeaderText="Remove Item">
								<ItemTemplate>
									<asp:LinkButton Text="Remove" runat="server" CommandName="Delete"
										OnClientClick="return confirm('Are you sure you want to delete?');" CausesValidation="false" />
									<asp:Label ID="lblInventoryID" runat="server" Text='<%#Eval("intInventoryID")%>' Visible="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Edit Item">
								<ItemTemplate>
									<asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" />
								</ItemTemplate>
								<EditItemTemplate>
									<asp:LinkButton Text="Update" runat="server" CommandName="Update" CausesValidation="false" />
									<asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
								</EditItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="SKU" />
							<asp:BoundField DataField="intItemQuantity" HeaderText="Quantity" />
							<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
							<asp:TemplateField HeaderText="Price" ItemStyle-Width="50px">
								<ItemTemplate>
									<div class='cost' id="divRollOverCart" runat="server">
										<asp:Label ID="lblItemPrice" runat="server" Text='<%#  (Eval("fltItemPrice","{0:C}")).ToString() %>' />
										<div id="divCostConvert" class="costDetail" runat="server">
											<asp:Label ID="lblItemCost" runat="server" Text='<%# Convert.ToString(Eval("fltItemAverageCostAtSale","{0:C}")).Replace("\n","<br/>") %>' />
										</div>
									</div>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Discount Amount">
								<ItemTemplate>
									<asp:CheckBox ID="ckbPercentageDisplay" Checked='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) %>' runat="server" Text="Discount by Percent" Enabled="false" />
									<div id="divAmountDisplay" class="txt" runat="server">
										<asp:Label ID="lblDiscountDisplay" runat="server" Text='<%# Eval("fltItemDiscount") %>' Enabled="false" />
									</div>
								</ItemTemplate>
								<EditItemTemplate>
									<asp:CheckBox ID="ckbPercentageEdit" Checked='<%# Convert.ToBoolean(Eval("bitIsPercentageDiscount")) %>' runat="server" Text="Discount by Percent" Enabled="true" />
									<div id="divAmountEdit" class="txt" runat="server">
										<asp:TextBox ID="txtDiscountEdit" runat="server" AutoComplete="off" Text='<%# Eval("fltItemDiscount") %>' Enabled="true" />
									</div>
								</EditItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Trade In" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkTradeIn" Checked='<%# Eval("bitIsTradeIn") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Non-Stocked Product" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkNonStockedProduct" Checked='<%# Eval("bitIsNonStockedProduct") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="RegularProduct" Visible="false">
								<ItemTemplate>
									<asp:CheckBox ID="chkRegularProduct" Checked='<%# Eval("bitIsRegularProduct") %>' runat="server" Enabled="false" />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
					<hr />
					<asp:Label ID="lblSubtotal" runat="server" Text="Subtotal:" />
					<asp:Label ID="lblSubtotalDisplay" runat="server" />
					<hr />
					<asp:Table runat="server">
						<asp:TableRow>
							<asp:TableCell>
								<asp:Button ID="btnCancelSale" runat="server" Text="Void Transaction" OnClick="btnCancelSale_Click" Width="163px" CausesValidation="false" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnExitSale" runat="server" Text="Hold Sale" OnClick="btnExitSale_Click" Width="163px" CausesValidation="false" />
							</asp:TableCell>
							<%--<asp:TableCell>
								<asp:Button ID="btnLayaway" runat="server" Text="Layaway" OnClick="btnLayaway_Click" Width="163px" CausesValidation="false" Visible="false" />
							</asp:TableCell>--%>
							<asp:TableCell>
								<asp:Button ID="btnProceedToCheckout" runat="server" Text="Checkout" OnClick="btnProceedToCheckout_Click" Width="163px" CausesValidation="false" />
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
	</div>
</asp:Content>