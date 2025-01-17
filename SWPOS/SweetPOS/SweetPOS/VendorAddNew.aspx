<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="VendorAddNew.aspx.cs" Inherits="SweetPOS.VendorAddNew" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="VendorAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="NewVendor">
		<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveVendor">
			<%--Textboxes and Labels for user to enter vendor info--%>
			<h2>Vendor Management</h2>
			<asp:Table ID="Table1" runat="server" Width="100%">
				<asp:TableRow>
					<asp:TableCell Width="25%">
						<asp:Label ID="lblVendorSupplierName" runat="server" Text="Vendor Name:" />
					</asp:TableCell>
					<asp:TableCell Width="25%">
						<asp:TextBox ID="txtVendorSupplierName" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
					</asp:TableCell>
					<asp:TableCell Width="25%">
						<asp:Label ID="lblVendorSupplierCode" runat="server" Text="Vendor Code:" />
					</asp:TableCell>
					<asp:TableCell Width="25%">
						<asp:TextBox ID="txtVendorSupplierCode" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" ViewStateMode="Enabled" Enabled="false" />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell>
						<asp:RequiredFieldValidator ID="valVendorSupplierName" runat="server" ForeColor="red" ErrorMessage="Must enter a Vendor Name" ControlToValidate="txtVendorSupplierName" />
					</asp:TableCell>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell>
						<asp:RequiredFieldValidator ID="valVendorSupplierCode" runat="server" ForeColor="red" ErrorMessage="Must enter a Vendor Code" ControlToValidate="txtVendorSupplierCode" />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblMainPhoneNumber" runat="server" Text="Main Phone Number:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtMainPhoneNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblFaxNumber" runat="server" Text="Fax Number:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtFaxNumber" runat="server" AutoComplete="off" ValidateRequestMode="Enabled" Enabled="false" />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblEmailAddress" runat="server" Text="Email:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtEmailAddress" runat="server" AutoComplete="off" Enabled="false" />
					</asp:TableCell>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell>

					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell ColumnSpan="4">
                        <hr />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblAddress" runat="server" Text="Address:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtAddress" runat="server" AutoComplete="off" Enabled="false" />
					</asp:TableCell>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell>
						<asp:CheckBox ID="chkIsActive" runat="server" Text="Vendor is Active" Enabled="false" />
 					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell>
					</asp:TableCell>
					<asp:TableCell ColumnSpan="2">
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblCity" runat="server" Text="City:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtCity" runat="server" AutoComplete="off" Enabled="false" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblPostalCode" runat="server" Text="PostalCode:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:TextBox ID="txtPostalCode" runat="server" AutoComplete="off" Enabled="false" />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Label ID="lblProvince" runat="server" Text="Province:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="ddlProvince" AutoPostBack="true" runat="server" Enabled="false" DataTextField="varProvinceName" DataValueField="intProvinceID" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Label ID="lblCountry" runat="server" Text="Country:" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Enabled="false" DataTextField="varCountryName" DataValueField="intCountryID" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell ColumnSpan="4">
                        <hr />
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						<asp:Button ID="btnAddVendor" runat="server" Text="Add Vendor" OnClick="btnAddVendor_Click" Visible="false" CausesValidation="true" />
						<asp:Button ID="btnEditVendor" runat="server" Text="Edit Vendor" OnClick="btnEditVendor_Click" Visible="true" CausesValidation="false" />
						<asp:Button ID="btnSaveVendor" runat="server" Text="Save Changes" OnClick="btnSaveVendor_Click" Visible="false" CausesValidation="true" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Button ID="btnCreatePurchaseOrder" runat="server" Text="Create PO" OnClick="btnCreatePurchaseOrder_Click" Visible="true" CausesValidation="false" />
					</asp:TableCell>
					<asp:TableCell>
						<asp:Button ID="btnBackToSearch" runat="server" Text="Exit Vendor" OnClick="btnBackToSearch_Click" Visible="true" CausesValidation="false" />
						<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" CausesValidation="false" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<hr />
			<%--Gridview for the invoices--%>
			<asp:Table ID="tblSuppliedInventoryAndPO" runat="server" Width="100%">
				<asp:TableRow Width="100%">
					<asp:TableCell Width="50%">
						<h2>Vendor Supplied Inventory</h2>
						<div>
							<asp:GridView ID="grdVendorSuppliedInventory" runat="server" AutoGenerateColumns="false" Width="100%" 
								OnRowCommand="grdVendorSuppliedInventory_RowCommand" RowStyle-HorizontalAlign="Center"
								OnRowDataBound="grdVendorSuppliedInventory_RowDataBound" ShowFooter="true">
								<Columns>
									<asp:TemplateField HeaderText="View Item">
										<ItemTemplate>
											<asp:LinkButton ID="lkbInventoryID" runat="server" CommandName="viewInventory" CommandArgument='<%#Eval("intInventoryID")%>' Text='<%#Eval("varSku")%>' />
										</ItemTemplate>
										<FooterTemplate>
											<asp:Button ID="btnAddInventoryToVendor" runat="server" Text="Set Inventory" CommandName="addInventoryToVendor" />
										</FooterTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Vendor Sku">
										<ItemTemplate>
											<asp:Label ID="lblVendorSupplierProductCode" runat="server" Text='<%#Eval("varVendorSupplierProductCode") %>' />
										</ItemTemplate>
										<FooterTemplate>
											<asp:TextBox ID="txtVendorSupplierProductCode" runat="server" Enabled="false" />
										</FooterTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Description">
										<ItemTemplate>
											<asp:Label ID="lblDescription" runat="server" Text='<%#Eval("varDescription") %>' />
										</ItemTemplate>
										<FooterTemplate>
											<asp:DropDownList ID="ddlInventoryList" runat="server" DataTextField="varDescription" DataValueField="intInventoryID" Enabled="false" />
										</FooterTemplate>
									</asp:TemplateField>
									<asp:TemplateField>
										<ItemTemplate>
											<asp:Button ID="btnRemoveFromVendor" runat="server" Text="Remove" CommandName="removeFromVendor" CommandArgument='<%#Eval("intInventoryID")%>' />
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</asp:TableCell>
					<asp:TableCell Width="50%">
						<h2>Purchase Orders</h2>
						<div>
							<asp:GridView ID="grdReceivedPurchaseOrders" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="grdReceivedPurchaseOrders_RowCommand" RowStyle-HorizontalAlign="Center">
								<Columns>
									<asp:TemplateField HeaderText="View PO">
										<ItemTemplate>
											<asp:LinkButton ID="lkbViewPurchaseOrder" runat="server" CommandName="ViewPurchaseOrder" CommandArgument='<%#Eval("intPurchaseOrderID")%>' Text='<%#Eval("varPurchaseOrderNumber") %>' />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Received Date">
										<ItemTemplate>
											<asp:Label ID="lblPurchaseOrderCompletionDate" runat="server" Text='<%#Eval("dtmPurchaseOrderCompletionDate","{0: MM/dd/yy}") %>' />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Cost">
										<ItemTemplate>
											<asp:Label ID="lblCostSubTotal" runat="server" Text='<%#Eval("fltCostSubTotal","{0:C}") %>' />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Employee">
										<ItemTemplate>
											<asp:Label ID="lblEmployeeName" runat="server" Text='<%#Eval("employee.varLastName") + ", " + Eval("employee.varFirstName") %>' />
										</ItemTemplate>
									</asp:TemplateField>
								</Columns>
							</asp:GridView>
						</div>
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		</asp:Panel>
	</div>
</asp:Content>
