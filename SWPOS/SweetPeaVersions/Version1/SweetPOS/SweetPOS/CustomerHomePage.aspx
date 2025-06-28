<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="CustomerHomePage.aspx.cs" Inherits="SweetPOS.CustomerHomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="customerHomePageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<div id="Customer">
		<asp:Panel ID="custSearch" runat="server" DefaultButton="btnCustomerSearch">
			<h2>Customer Information</h2>
			<hr />
			<asp:ScriptManager ID="ScriptManager1" runat="server">
			</asp:ScriptManager>
			<asp:UpdatePanel ID="UpdatePanel1" runat="server">
				<ContentTemplate>
					<%--Enter search text to find matching customer information--%>
					<asp:Table ID="tblCustomerRow" runat="server">
						<asp:TableRow>
							<asp:TableCell>
								<asp:TextBox ID="txtSearch" AutoComplete="off" runat="server" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnCustomerSearch" runat="server" Width="150" Text="Customer Search" OnClick="btnCustomerSearch_Click" />
							</asp:TableCell>
							<asp:TableCell>
								<asp:Button ID="btnAddNewCustomer" runat="server" Width="150" Text="Add New Customer" OnClick="btnAddNewCustomer_Click" />
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
					<asp:GridView ID="grdCustomersSearched" runat="server" AutoGenerateColumns="false" OnRowCommand="grdCustomersSearched_RowCommand" AllowPaging="True" PageSize="25" OnPageIndexChanging="grdCustomersSearched_PageIndexChanging" RowStyle-HorizontalAlign="Center">
						<Columns>
							<asp:TemplateField HeaderText="Sale">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnStartSale" CommandName="StartSale" CommandArgument='<%#Eval("intCustomerID")%>' Text="Start Sale" runat="server" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="View Profile">
								<ItemTemplate>
									<asp:LinkButton ID="lbtnViewCustomer" CommandName="ViewProfile" CommandArgument='<%#Eval("intCustomerID")%>' Text="View Profile" runat="server" />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Customer Number">
								<ItemTemplate>
									<asp:Label ID="lblCustomerID" runat="server" Text='<%#Eval("intCustomerID")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Customer Name">
								<ItemTemplate>
									<asp:Label ID="lblCustomerName" runat="server" Text='<%#Eval("varLastName") + ", " + Eval("varFirstName")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Customer Address">
								<ItemTemplate>
									<asp:Label ID="lblAddress" runat="server" Text='<%#Eval("varAddress")%>' />
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Phone Numbers">
								<ItemTemplate>
									<asp:Label ID="lblPhoneNumbers" runat="server" Text='<%#"H: " + Eval("varHomePhone") + ", M: " + Eval("varMobilePhone")%>' />
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
