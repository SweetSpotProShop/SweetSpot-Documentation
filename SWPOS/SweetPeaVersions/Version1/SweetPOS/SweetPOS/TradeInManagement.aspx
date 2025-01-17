<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="TradeInManagement.aspx.cs" Inherits="SweetPOS.TradeInManagement" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="IndividualPageContent" runat="server">
	<h2>Trade In Management</h2>
	<%--REMEMBER TO SET DEFAULT BUTTON--%>
	<br />
	<asp:DropDownList ID="ddlStoreLocation" runat="server" AutoPostBack="true" DataTextField="varStoreName" DataValueField="intStoreLocationID" Enabled="false" />
	<hr />
	<asp:GridView ID="grdUnProcessedTradeIns" runat="server" AutoGenerateColumns="False" Width="100%" RowStyle-HorizontalAlign="Center" 
		OnRowCommand="grdUnProcessedTradeIns_RowCommand">
		<Columns>
			<asp:TemplateField HeaderText="Process">
				<ItemTemplate>
					<asp:LinkButton ID="btnProcessTradeIN" CommandArgument='<%#Eval("intPurchasedInventoryID") %>' runat="server" Text="Process" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:BoundField DataField="varSku" ReadOnly="true" HeaderText="Sku" />
			<asp:BoundField DataField="intItemQuantity" ReadOnly="true" HeaderText="Item Quantity" />
			<asp:BoundField DataField="fltCost" ReadOnly="true" HeaderText="Item Cost" DataFormatString="{0:C}" />
			<asp:BoundField DataField="varItemDescription" ReadOnly="true" HeaderText="Description" />
			<asp:TemplateField HeaderText="Process Action">
				<ItemTemplate>
					<asp:RadioButtonList ID="rblTradeInProcessAction" runat="server">
					    <asp:ListItem Text="Add To Inventory" Value="1" />
						<asp:ListItem Text="Remove From Process" Value="2" />
						<asp:ListItem Text="Used For Parts" Value="3" />
					</asp:RadioButtonList>
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>
</asp:Content>