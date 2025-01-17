<%@ Page Title="" Language="C#" MasterPageFile="~/SweetPOSMasterPage.Master" AutoEventWireup="true" CodeBehind="PurchasesCart.aspx.cs" Inherits="SweetPOS.PurchasesCart" %>
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
<asp:Content ID="PurchasesPageContent" ContentPlaceHolderID="IndividualPageContent" runat="server">
    <div id="PurchaseCart">
        <asp:Panel ID="pnlDefaultButton" runat="server" DefaultButton="btnAddPurchase" >
            <asp:Label ID="lblCustomer" runat="server" Text="Customer Name:" />
            <asp:TextBox ID="txtCustomer" runat="server" AutoComplete="off" Enabled="false" />
            <asp:Button ID="btnCustomerSelect" runat="server" Text="Change Customer" OnClick="btnCustomerSelect_Click" Visible="false" CausesValidation="false" />
            <div>
                <br />
                <div>
                    <asp:GridView ID="grdCustomersSearched" runat="server" AutoGenerateColumns="false" ShowFooter="true" 
                        OnRowCommand="grdCustomersSearched_RowCommand" AllowPaging="True" PageSize="5" 
                        OnPageIndexChanging="grdCustomersSearched_PageIndexChanging" >
                        <Columns>
                            <asp:TemplateField HeaderText="Switch Customer">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnSwitchCustomer" CommandName="SwitchCustomer" CommandArgument='<%#Eval("intCustomerId") %>' Text="Switch Customer" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Button ID="btnAddCustomer" runat="server" Text="Add Customer" OnClick="btnAddCustomer_Click" />
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
                            <asp:TemplateField HeaderText="Phone Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" Text='<%#Eval("varHomePhone") %>' />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtHomePhone" runat="server" AutoComplete="off" placeholder="Home Phone Number" ToolTip="Home Phone Number" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email Address" >
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
            <div style="text-align: right">
                <asp:Label ID="lblInvoiceNumber" runat="server" Text="Invoice #:" />
                <asp:Label ID="lblInvoiceNumberDisplay" runat="server" />
                <br />
                <asp:Label ID="lblDate" runat="server" Text="Date:" />
                <asp:Label ID="lblDateDisplay" runat="server" />
                <hr />
            </div>
            <h3>Purchases</h3>
            <asp:Button ID="btnAddPurchase" runat="server" Text="Add Purchase" OnClick="btnAddPurchase_Click" />
            <hr />
            <asp:GridView ID="grdPurchasedItems" runat="server" AutoGenerateColumns="false" Style="margin-right: 0px" 
                OnRowEditing="OnRowEditing" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="OnRowCanceling" >
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Edit Item" >
                        <ItemTemplate>
                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton Text="Update" runat="server" CommandName="Update" CausesValidation="false" />
                            <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel" CausesValidation="false" />
							<asp:Label ID="lblInvoiceItemID" runat="server" Text='<%#Eval("intInvoiceItemID")%>' Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="varItemSku" ReadOnly="true" HeaderStyle-Width="20%" HeaderText="SKU" />
					<asp:BoundField DataField="varItemDescription" HeaderStyle-Width="20%" HeaderText="Description" />
                    <asp:BoundField DataField="fltItemCost" HeaderStyle-Width="20%" HeaderText="Cost" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <hr />
            <asp:Label ID="lblPurchaseAmount" runat="server" Text="Purchase Amount:" />
            <asp:Label ID="lblPurchaseAmountDisplay" runat="server" Text="" />
            <hr />
            <asp:Button ID="btnCancelPurchase" runat="server" Text="Cancel Purchase" OnClick="btnCancelPurchase_Click" CausesValidation="false" />
			<asp:Button ID="btnSavePurchase" runat="server" Text="Save Purchase" OnClick="btnSavePurchase_Click" CausesValidation="false" />
            <asp:Button ID="btnProceedToPayOut" runat="server" Text="Proceed to Pay Out" OnClick="btnProceedToPayOut_Click" CausesValidation="false" />
        </asp:Panel>
    </div>
</asp:Content>