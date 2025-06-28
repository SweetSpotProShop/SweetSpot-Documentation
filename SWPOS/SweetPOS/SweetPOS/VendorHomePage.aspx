<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="VendorHomePage.aspx.cs" Inherits="SweetPOS.VendorHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="vendorHomePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="Vendor">
		<asp:Panel ID="vendorSearch" runat="server" DefaultButton="btnVendorSearch">
			<h2>Vendor Information</h2>
			<hr />
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
					<%--Enter search text to find matching customer information--%>
					<asp:Table ID="tblVendorRow" runat="server">
						<asp:TableRow>
							<asp:TableCell>
								<asp:TextBox ID="txtSearch" AutoComplete="off" runat="server" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnVendorSearch" runat="server" Width="150" Text="Vendor Search" OnClick="btnVendorSearch_Click" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnAddNewVendor" runat="server" Width="150" Text="Add New Vendor" OnClick="btnAddNewVendor_Click" />
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
					<asp:GridView ID="grdVendorsSearched" runat="server" AutoGenerateColumns="false" OnRowCommand="grdVendorsSearched_RowCommand" AllowPaging="True" 
						PageSize="25" OnPageIndexChanging="grdVendorsSearched_PageIndexChanging" RowStyle-HorizontalAlign="Center">
						<Columns>
							<asp:TemplateField HeaderText="View Profile">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnViewVendor" CommandName="ViewProfile" CommandArgument='<%#Eval("intVendorSupplierID")%>' Text="View Profile" runat="server" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Vendor Number">
								<ItemTemplate>
									<asp:Label ID="lblVendorSupplierID" runat="server" Text='<%#Eval("intVendorSupplierID")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Vendor Name">
								<ItemTemplate>
									<asp:Label ID="lblVendorSupplierName" runat="server" Text='<%#Eval("varVendorSupplierName")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Vendor Address">
								<ItemTemplate>
									<asp:Label ID="lblAddress" runat="server" Text='<%#Eval("varAddress")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Phone Number">
								<ItemTemplate>
									<asp:Label ID="lblPhoneNumbers" runat="server" Text='<%#Eval("varMainPhoneNumber")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Email Address">
								<ItemTemplate>
									<asp:Label ID="lblEmailAddress" runat="server" Text='<%#Eval("varEmailAddress")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="City">
								<ItemTemplate>
									<asp:Label ID="lblCityName" runat="server" Text='<%#Eval("varCityName")%>' />
								</ItemTemplate>
							</asp:TemplateField>
						</Columns>
						<EmptyDataTemplate>
							No current customer data, please search for a customer
						</EmptyDataTemplate>
					</asp:GridView>
				</ContentTemplate>
			</asp:UpdatePanel>
		</asp:Panel>
	</div>
</asp:Content>
