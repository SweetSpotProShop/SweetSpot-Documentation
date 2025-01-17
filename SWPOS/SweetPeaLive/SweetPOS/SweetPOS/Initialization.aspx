<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Initialization.aspx.cs" Inherits="SweetPOS.Initialization" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="NewStoreLocation">
            <asp:Panel ID="pnlLocationSetup" runat="server" DefaultButton="btnSaveStoreLocation" Visible="false">
                <%--Textboxes and Labels for user to enter customer info--%>
                <h2>Location Management</h2>
                <asp:Table ID="Table1" runat="server" Width="80%" HorizontalAlign="Center">
                    <asp:TableRow>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblStoreLocationName" runat="server" Text="Store Name:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
							<asp:DropDownList ID="ddlStoreLocationName" runat="server" AutoPostBack="true" DataTextField="varStoreName" 
								DataValueField="intStoreLocationID" OnSelectedIndexChanged="ddlStoreLocationName_SelectedIndexChanged" />
							<asp:TextBox ID="txtStoreLocationName" runat="server" AutoComplete="off" Enabled="false" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblStoreCode" runat="server" Text="Store Code:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:TextBox ID="txtStoreCode" runat="server" AutoComplete="off" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblPhoneNumber" runat="server" Text="Phone Number:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPhoneNumber" runat="server" AutoComplete="off" Enabled="false" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblTaxNumber" runat="server" Text="Tax Number:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtTaxNumber" runat="server" AutoComplete="off" Enabled="false" />
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
                            <asp:CheckBox ID="chkIsRetailLocation" runat="server" Text="Retail Location?" Enabled="false" />
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
                            <asp:DropDownList ID="ddlProvince" AutoPostBack="true" runat="server" Enabled="false" 
								DataTextField="varProvinceName" DataValueField="intProvinceID" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblCountry" runat="server" Text="Country:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="true" Enabled="false" 
								DataTextField="varCountryName" DataValueField="intCountryID" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                        <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnSaveStoreLocation" runat="server" Text="Next ->" OnClick="btnSaveStoreLocation_Click" CausesValidation="true" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </div>
        <div id="NewTerminalInformation">
            <asp:Panel ID="pnlTerminalSetup" runat="server" DefaultButton="btnSaveTerminalInformation" Visible="false">
                <%--Textboxes and Labels for user to enter customer info--%>
                <h2>Terminal Setup</h2>
                <asp:Table ID="tblTerminalInformation" runat="server" HorizontalAlign="Center" Width="80%">
                    <asp:TableRow>
						<asp:TableCell Width="25%">
                            <asp:Label ID="lblTillNumber" runat="server" Text="Till Number:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
							<asp:DropDownList ID="ddlTillNumber" runat="server" AutoPostBack="true" DataTextField="intTillNumber" 
								DataValueField="intTerminalID" OnSelectedIndexChanged="ddlTillNumber_SelectedIndexChanged" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Label ID="lblLicenceNumber" runat="server" Text="Licence Number:" />
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:TextBox ID="txtLicenceNumber" runat="server" AutoComplete="off" Enabled="false" />
                        </asp:TableCell>                        
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblDrawerFloat" runat="server" Text="Drawer Float:" />
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtDrawerFloat" runat="server" AutoComplete="off" Enabled="false" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
							<%--<asp:Button ID="btnRemoveTables" runat="server" Text="Remove Tables" OnClick="btnRemoveTables_Click" />--%>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnSaveTerminalInformation" runat="server" Text="Next ->" OnClick="btnSaveTerminalInformation_Click" CausesValidation="true" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
        </div>
        <div>
<%--            <asp:Panel ID="pnlTaxesSetup" runat="server" DefaultButton="btnTaxesSetup" Visible="false">
                <h2>Taxes Setup</h2>
                <asp:GridView ID="grdTaxesSetup" runat="server" AutoGenerateColumns="false" Width="80%" HorizontalAlign="Center" RowStyle-HorizontalAlign="Center">
                    <Columns>
                        <asp:TemplateField HeaderText="Tax ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxID" runat="server" Text="Tax ID" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxName" runat="server" Text="Tax Name" Enabled="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effective Date">
                            <ItemTemplate>
                                <asp:Label ID="lblEffectiveDate" runat="server" Text="Effective Date" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax Rate">
                            <ItemTemplate>
                                <asp:Label ID="lblTaxRate" runat="server" Text="Tax Rate" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available Tax">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkAvailableTax" runat="server" Text="Is Tax Available" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Table ID="tblTaxesSetup" runat="server" HorizontalAlign="Center" Width="80%">
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="25%">
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                        </asp:TableCell>
                        <asp:TableCell Width="25%">
                            <asp:Button ID="btnSaveTaxesInformation" runat="server" Text="Next ->" OnClick="btnTaxesInfromation_Click" CausesValidation="true" />
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell ColumnSpan="4">
                            <hr />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>--%>
        </div>
    </form>
</body>
</html>
