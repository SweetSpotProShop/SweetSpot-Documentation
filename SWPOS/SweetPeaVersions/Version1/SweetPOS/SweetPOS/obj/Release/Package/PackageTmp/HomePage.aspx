<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="SweetPOS.HomePage" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<h2>Today's Transactions</h2>
	<%--REMEMBER TO SET DEFAULT BUTTON--%>
	<asp:Label ID="lblStoreLocation" runat="server" Text="Location:" />
	<asp:DropDownList ID="ddlStoreLocation" runat="server" AutoPostBack="true" DataTextField="varStoreName" DataValueField="intStoreLocationID" Enabled="false" />
	<div style="text-align: right">
		<asp:Label ID="lblUserAccess" runat="server" Visible="false" Text="User Access" />
	</div>
	<hr />
	<asp:GridView ID="grdSameDaySales" runat="server" AutoGenerateColumns="False" Width="100%" ShowFooter="true"
		OnRowDataBound="grdSameDaySales_RowDataBound" OnDataBound="grdSameDaySales_DataBound"
		RowStyle-HorizontalAlign="Center" OnRowCommand="grdSameDaySales_RowCommand">
		<Columns>
			<asp:TemplateField HeaderText="Receipt Number">
				<ItemTemplate>
					<asp:LinkButton ID="lbtnReceiptNumber" runat="server" Text='<%#Eval("varReceiptNumber") + "-" + Eval("intReceiptSubNumber") %>'
						CommandArgument='<%#Eval("intReceiptID") %>' />
				</ItemTemplate>
				<FooterTemplate>
					<asp:Label ID="lblTotals" runat="server" Text="Totals:" />
				</FooterTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="varCustomerFullName" ReadOnly="true" HeaderText="Customer" />
			<asp:BoundField DataField="varEmployeeFullName" ReadOnly="true" HeaderText="Employee" />
			<asp:BoundField DataField="fltDiscountTotal" ReadOnly="true" HeaderText="Discount" DataFormatString="{0:C}" />
			<asp:BoundField DataField="fltTradeInTotal" ReadOnly="true" HeaderText="Trade In" DataFormatString="{0:C}" />
			<asp:BoundField DataField="fltSubTotal" ReadOnly="true" HeaderText="Subtotal" DataFormatString="{0:C}" />
			<asp:BoundField DataField="fltGovernmentTax" ReadOnly="true" HeaderText="Government Tax" DataFormatString="{0:C}" />
			<asp:BoundField DataField="fltProvincialTax" ReadOnly="true" HeaderText="Provincial Tax" DataFormatString="{0:C}" />
			<asp:BoundField DataField="fltReceiptTotal" ReadOnly="true" HeaderText="Balance Paid" DataFormatString="{0:C}" />
			<asp:BoundField DataField="varMethodOfPaymentName" ReadOnly="true" HeaderText="MOP Type" />
			<asp:BoundField DataField="fltAmountPaid" ReadOnly="true" HeaderText="MOP Amount" DataFormatString="{0:C}" />
			<asp:TemplateField HeaderText="Transaction Type" Visible="false">
				<ItemTemplate>
					<asp:TextBox ID="txtTransactionTypeID" runat="server" Text='<%#Eval("intTransactionTypeID")%>' />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>
