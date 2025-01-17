<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="SettingsHomePage.aspx.cs" Inherits="SweetPOS.SettingsHomePage" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="SettingsPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="Settings">
		<%--REMEMBER TO SET DEFAULT BUTTON--%>
		<asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnEmployeeSearch">
			<h2>Manage Settings</h2>
			<hr />
			<asp:Table ID="tblSettings" runat="server">
				<asp:TableRow>
					<asp:TableCell>
						<asp:Button ID="btnVendorManagement" runat="server" Text="Vendor Management" OnClick="btnVendorManagement_Click" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<hr />
			<h2>Employee Management</h2>
			<hr />
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
					<%--Enter search text to find matching Employees information--%>
					<asp:Table ID="tblEmployee" runat="server">
						<asp:TableRow>
							<asp:TableCell>
								<asp:TextBox ID="txtSearch" runat="server" AutoComplete="off" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnEmployeeSearch" runat="server" Width="150" Text="Employee Search" OnClick="btnEmployeeSearch_Click" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnAddNewEmployee" runat="server" Width="150" Text="Add New Employee" OnClick="btnAddNewEmployee_Click" />
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					<asp:UpdateProgress ID="UpdateProgress1" runat="server">
						<ProgressTemplate>
							<div>
								<img src="Images/ajax-loader.gif" />
							</div>
						</ProgressTemplate>
					</asp:UpdateProgress>
					<hr />
					<asp:GridView ID="grdEmployeesSearched" AutoGenerateColumns="false" runat="server" OnRowCommand="grdEmployeesSearched_RowCommand">
						<Columns>
							<asp:TemplateField HeaderText="View Profile">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnViewEmployee" CommandName="ViewProfile" CommandArgument='<%#Eval("intEmployeeID") %>' Text="View Profile" runat="server" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Employee Number">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("intEmployeeID") %>' ID="key" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Employee Name">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varEmployeeFullName") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Employee Address">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varAddress") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Phone Number">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varPhoneNumber") %>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="City">
								<ItemTemplate>
									<asp:Label runat="server" Text='<%#Eval("varCityName") %>' />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							No current employee data, please search for a employee
						</EmptyDataTemplate>
					</asp:GridView>
				</ContentTemplate>
			</asp:UpdatePanel>
			<br />
			<hr />
			<h2>Taxes</h2>
			<hr />
			<div>
				<asp:Table runat="server">
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblProvince" runat="server" Text="Province:" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:DropDownList ID="ddlProvince" runat="server" AutoPostBack="true" DataTextField="varProvinceName" DataValueField="intProvinceID"
								OnSelectedIndexChanged="ddlProvince_SelectedIndexChanged" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblTax" runat="server" Text="Tax:" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:DropDownList ID="ddlTax" runat="server" AutoPostBack="true" DataTextField="varTaxName" DataValueField="intTaxTypeID"
								OnSelectedIndexChanged="ddlTax_SelectedIndexChanged" OnPreRender="ddlTax_SelectedIndexChanged" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="lblCurrentDate" runat="server" Text="" Visible="false" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblCurrentTaxRate" runat="server" Text="Current Rate:" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="lblNewTaxRate" runat="server" Text="New Rate:" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:Label ID="lblImplimentationDate" runat="server" Text="As of:" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblCurrentDisplay" runat="server" Text="" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtNewTaxRate" runat="server" AutoComplete="off" />
						</asp:TableCell>
						<asp:TableCell>
							<asp:TextBox ID="txtImplimentationDate" runat="server" Text="" AutoComplete="off" />
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell>
						</asp:TableCell>
						<asp:TableCell>
							<asp:Button ID="btnSaveTheTax" Text="Set New Tax Rate" runat="server" OnClick="btnSaveTheTax_Click" />
						</asp:TableCell>
						<asp:TableCell>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</div>
			<br />
			<hr />
			<h2>Create New Brand</h2>
			<hr />
			<div>
				<asp:Table runat="server" GridLines="Both" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black">
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblBrand" runat="server" Text="Brand" />
							<div>
								<asp:TextBox ID="txtNewBrandName" runat="server" AutoComplete="off" placeholder="Brand Name" />
							</div>
							<div>
								<asp:TextBox ID="txtNewBrandConfirmName" runat="server" AutoComplete="off" placeholder="Confirm Brand" />
							</div>
							<asp:Button ID="btnAddBrand" runat="server" Width="150" Text="Add Brand" OnClick="btnAddBrand_Click" />
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</div>
			<br />
			<hr />
			<div id="divImport">
				<asp:Label ID="lblImportFiles" runat="server" Text="Import Files From Excel" Visible="false" />
				<asp:Table ID="tblImports" runat="server" GridLines="Both" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black" Visible="false">
					<asp:TableRow>
						<asp:TableCell>
							<asp:Label ID="lblImportInventory" runat="server" Text="Import Inventory" />
							<div>
								<asp:FileUpload ID="fupInventorySheet" runat="server" />
							</div>
							<asp:Button ID="btnImportInventory" runat="server" Width="150" Text="Import Inventory" OnClick="btnImportInventory_Click" />
						</asp:TableCell>
						<asp:TableCell>
                            <asp:Label ID="lblImportCustomer" runat="server" Text="Import Customer" />
                            <div>
                                <asp:FileUpload ID="fupCustomerSheet" runat="server" />
                            </div>
                            <asp:Button ID="btnImportCustomer" runat="server" Width="150" Text="Import Customer" OnClick="btnImportCustomer_Click" />
                        </asp:TableCell>
					</asp:TableRow>
				</asp:Table>
			</div>
			<%-- <br />
            <hr />
            <h2>Export Files To Excel</h2>
            <hr />
            <asp:Button ID="btnExportInvnetory" runat="server" Width="150" Text="Export Invnetory" OnClick="btnExportInventory_Click" />
            The receipts should really have selection date or range
            <asp:Button ID="btnExportReceipt" runat="server" Width="150" Text="Export Receipt" OnClick="btnExportReceipt_Click" />
            <asp:Button ID="btnExportCustomerEmail" runat="server" Width="150" Text="Export Customer Email" OnClick="btnExportCustomerEmail_Click" />--%>
		</asp:Panel>
	</div>
</asp:Content>