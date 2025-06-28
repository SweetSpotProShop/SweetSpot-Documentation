<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="InventoryAddNew.aspx.cs" Inherits="SweetPOS.InventoryAddNew" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SPMaster" runat="server">
</asp:Content>--%>
<asp:Content ID="InventoryAddNewPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="NewInventory">
        <%--REMEMBER TO SET DEFAULT BUTTON--%>
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnSaveItem">
            <%--Textboxes and Labels for user to enter inventory info--%>
            <h2>New Inventory Item</h2>
            <br />
            <asp:Table ID="Table1" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKU" runat="server" Text="SKU:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblSKUDisplay" runat="server" Text="" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:Label ID="lblAverageCost" runat="server" Text="Average Cost:" />
                    </asp:TableCell>
                    <asp:TableCell Width="25%">
                        <asp:TextBox ID="txtAverageCost" runat="server" AutoComplete="off" Enabled="false" Text="0" />
                        <asp:RegularExpressionValidator ID="revCost"
                                ControlToValidate="txtAverageCost"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblBrand" runat="server" Text="Brand Name:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlBrand" runat="server" AutoPostBack="false" Enabled="false" DataTextField="varBrandName" DataValueField="intBrandID" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblPrice" runat="server" Text="Price:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtPrice" runat="server" AutoComplete="off" Enabled="false" Text="0" />
                        <asp:RegularExpressionValidator ID="revPrice"
                                ControlToValidate="txtPrice"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblQuantity" runat="server" Text="Quantity:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtQuantity" runat="server" AutoComplete="off" Enabled="false" Text="0" />
                        <asp:RegularExpressionValidator ID="revQuantity"
                                ControlToValidate="txtQuantity"
                                ValidationExpression="[-+]?([0-9]*\.[0-9]+|[0-9]+)"
                                Display="Static"
                                EnableClientScript="true"
                                ErrorMessage="Requires a number"
                                runat="server" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblStoreLocation" runat="server" Text="Store Location:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:DropDownList ID="ddlStoreLocation" runat="server" AutoPostBack="false" Enabled="false" DataTextField="varStoreName" DataValueField="intStoreLocationID" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
						<hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblCreationDate" runat="server" Text="Creation Date:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtCreationDate" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Label ID="lblModelName" runat="server" Text="Model:" Visible="true" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtModelName" runat="server" AutoComplete="off" AutoPostBack="false" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblUPCcode" runat="server" Text="UPC Code:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="txtUPCcode" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RadioButtonList ID="rdbIsRegularProduct" runat="server" Enabled="false">
                            <asp:ListItem Text="Item is Regular Product" Value="0" />
                            <asp:ListItem Text="Item is Non-Stocked Product" Value="1" />
                        </asp:RadioButtonList>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Label ID="lblDescription" runat="server" Text="Description:" />
                    </asp:TableCell>
                    <asp:TableCell ColumnSpan="3">
                        <asp:TextBox ID="txtDescription" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="2">
                        <asp:Label ID="lblAdditionalInformation" runat="server" Text="Additional Information:" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="chkUsedProduct" runat="server" Text="Used Product" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
                        <asp:TextBox Height="30px" Width="100%" ID="txtAdditionalInformation" runat="server" AutoComplete="off" Enabled="false" />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell ColumnSpan="4">
						<hr />
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:Button ID="btnAddItem" runat="server" Text="Add Item" OnClick="btnAddItem_Click" Visible="false" />
                        <asp:Button ID="btnEditItem" runat="server" Text="Edit Item" OnClick="btnEditItem_Click" Visible="true" />
                        <asp:Button ID="btnSaveItem" runat="server" Text="Save Changes" OnClick="btnSaveItem_Click" Visible="false" />
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnBackToSearch" runat="server" Text="Exit Item" OnClick="btnBackToSearch_Click" Visible="true" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" Visible="false" CausesValidation="false"/>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:Button ID="btnCreateSimilar" runat="server" Text="Create Similar" OnClick="btnCreateSimilar_Click" Visible="true" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </asp:Panel>
    </div>
</asp:Content>
