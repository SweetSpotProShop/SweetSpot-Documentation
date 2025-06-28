using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class CreateCompanyDatabase
    {
        DatabaseCalls DBC = new DatabaseCalls();
        
        public void CreateAllTableData(int businessNumber)
        {
            //Create general tables
            createCustomer(businessNumber);
            createBrand(businessNumber);
            createStoreLocation(businessNumber);
            createJobCode(businessNumber);
            createVendorSupplier(businessNumber);
            createEmployee(businessNumber);
            createMethodOfPayment(businessNumber);
            createEHSJYNLKHFNKLUCLFJ(businessNumber);
            createInventory(businessNumber);
            createVendorSupplierProduct(businessNumber);

            //Licence Tables
            createLicence(businessNumber);

            //Receipt tables
            createReceipt(businessNumber);
            createReceiptItem(businessNumber);
            createReceiptPayment(businessNumber);
            createReceiptItemTaxes(businessNumber);
            createReceiptCurrent(businessNumber);
            createReceiptItemCurrent(businessNumber);
            createReceiptPaymentCurrent(businessNumber);
            createReceiptItemTaxesCurrent(businessNumber);
            createReceiptVoidCancel(businessNumber);
            createReceiptItemVoidCancel(businessNumber);
            createReceiptPaymentVoidCancel(businessNumber);
            createReceiptItemTaxesVoidCancel(businessNumber);

            //Invoice tables
            createInvoice(businessNumber);
            createInvoiceItem(businessNumber);
            createInvoicePayment(businessNumber);
            createInvoiceCurrent(businessNumber);
            createInvoiceItemCurrent(businessNumber);
            createInvoicePaymentCurrent(businessNumber);
            createInvoiceVoidCancel(businessNumber);
            createInvoiceItemVoidCancel(businessNumber);
            createInvoicePaymentVoidCancel(businessNumber);

            //PurchaseOrder tables
            createPurchaseOrder(businessNumber);
            createPurchaseOrderItem(businessNumber);
            createPurchaseOrderItemTaxes(businessNumber);
            createPurchaseOrderCurrent(businessNumber);
            createPurchaseOrderItemCurrent(businessNumber);
            createPurchaseOrderItemTaxesCurrent(businessNumber);
            createPurchaseOrderVoidCancel(businessNumber);
            createPurchaseOrderItemVoidCancel(businessNumber);
            createPurchaseOrderItemTaxesVoidCancel(businessNumber);
            createPurchasedInventory(businessNumber);

            //Stored Incrementing Numbers
            createStoredReceiptNumber(businessNumber);
            createStoredTradeInNumber(businessNumber);
            createStoredInvoiceNumber(businessNumber);
            createStoredPurchaseOrderNumber(businessNumber);
            createStoredInventorySku(businessNumber);

            //Per Location tables
            createGuestCustomerPerLocation(businessNumber);
            createTradeInSkuPerLocation(businessNumber);

            //Per Inventory tabkes
            createTaxTypePerInventoryItem(businessNumber);

            //Cash tables 
            createSalesReconciliation(businessNumber);
            createTillCashout(businessNumber);

            //Error tables
            createError(businessNumber);

            //Set Tables as initialized
            setInitializationOfBusiness(businessNumber);
            InsertIntoDefaultTables(businessNumber);
        }

        //Create all the required tables for operation then set bitIsBusinessInitialized = 1
        private void setInitializationOfBusiness(int businessNumber)
        {
            string sqlCmd = "UPDATE tblBusinessIdentification SET bitIsBusinessInitialized "
                + "= 1 WHERE intBusinessNumber = @intBusinessNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", businessNumber }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertIntoDefaultTables(int businessNumber)
        {
            //Brand = Trade In only
            CreateTradeInBrand(businessNumber);
            //JobCode = SuDo Only
            CreateAdminJobCode(businessNumber);
        }
        private void CreateTradeInBrand(int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Brand VALUES('Trade In')";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateAdminJobCode(int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "JobCode VALUES('SuDo')";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //General Tables
        private void createCustomer(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Customer(intCustomerID INT NOT NULL "
                + "IDENTITY(1, 1) PRIMARY KEY, varFirstName VARCHAR(50) NOT NULL, varLastName VARCHAR(50) NOT NULL, "
                + "varAddress VARCHAR(100) NOT NULL, varHomePhone VARCHAR(20) NOT NULL, varMobilePhone VARCHAR(20) NOT "
                + "NULL, varEmailAddress VARCHAR(100) NOT NULL, varCityName VARCHAR(100) NOT NULL, intProvinceID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tblProvince(intProvinceID), intCountryID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tblCountry(intCountryID), varPostalCode VARCHAR(10) NOT NULL, dtmCreationDate DATE NOT "
                + "NULL, bitAllowMarketing BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createBrand(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Brand(intBrandID INT NOT NULL "
                + "IDENTITY(1,1) PRIMARY KEY, varBrandName VARCHAR(50) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createStoreLocation(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, varStoreName VARCHAR(100) NOT NULL, varPhoneNumber "
                + "VARCHAR(20) NOT NULL, varAddress VARCHAR(100) NOT NULL, varEmailAddress VARCHAR(100) NOT NULL, "
                + "varCityName VARCHAR(100) NOT NULL, intProvinceID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tblProvince(intProvinceID), intCountryID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tblCountry(intCountryID), varPostalCode VARCHAR(10) NOT NULL, varTaxNumber VARCHAR(20) NOT NULL, "
                + "varStoreCode VARCHAR(4) NOT NULL, bitIsRetailLocation BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createJobCode(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "JobCode(intJobCodeID INT NOT NULL "
                + "IDENTITY(1,1) PRIMARY KEY, varJobDescription VARCHAR(50) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createVendorSupplier(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "VendorSupplier(intVendorSupplierID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, varVendorSupplierName VARCHAR(50) NOT NULL, varAddress "
                + "VARCHAR(50) NOT NULL, varMainPhoneNumber VARCHAR(20) NOT NULL, varFaxNumber VARCHAR(20) NOT NULL, "
                + "varEmailAddress VARCHAR(100) NOT NULL, varCityName VARCHAR(100) NOT NULL, intProvinceID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tblProvince(intProvinceID), intCountryID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tblCountry(intCountryID), dtmCreationDate DATE NOT NULL, varPostalCode VARCHAR(10) NOT NULL, "
                + "varVendorSupplierCode VARCHAR(6) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createEmployee(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID INT NOT NULL "
                + "IDENTITY(1,1) PRIMARY KEY, varFirstName VARCHAR(50) NOT NULL, varLastName VARCHAR(50) NOT NULL, "
                + "intJobCodeID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "JobCode(intJobCodeID), varAddress VARCHAR(100) NOT NULL, varHomePhone VARCHAR(20) NOT NULL, "
                + "varMobilePhone VARCHAR(20) NOT NULL, varEmailAddress VARCHAR(100) NOT NULL, varCityName VARCHAR(100) "
                + "NOT NULL, intProvinceID INT NOT NULL FOREIGN KEY REFERENCES tblProvince(intProvinceID), intCountryID "
                + "INT NOT NULL FOREIGN KEY REFERENCES tblCountry(intCountryID), dtmCreationDate DATE NOT NULL, "
                + "varPostalCode VARCHAR(10) NOT NULL, bitIsEmployeeActive BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createMethodOfPayment(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "MethodOfPayment(intMethodOfPaymentID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, varMethodOfPaymentName "
                + "VARCHAR(50) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createEHSJYNLKHFNKLUCLFJ(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "EHSJYNLKHFNKLUCLFJ(intEmployeeID "
                + "INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Employee(intEmployeeID), intEmployeeEFSHJEMVIF INT NOT NULL, PRIMARY KEY(intEmployeeID, "
                + "intEmployeeEFSHJEMVIF));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInventory(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID INT NOT "
                + "NULL IDENTITY(1,1) PRIMARY KEY, varSku VARCHAR(25) NOT NULL, intBrandID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Brand(intBrandID), varModelName VARCHAR(50) NOT "
                + "NULL, varDescription VARCHAR(MAX) NOT NULL, intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), varUPCcode VARCHAR(25) "
                + "NOT NULL, intQuantity INT NOT NULL, fltPrice FLOAT NOT NULL, fltAverageCost FLOAT NOT NULL, "
                + "bitIsNonStockedProduct BIT NOT NULL, bitIsRegularProduct BIT NOT NULL, bitIsUsedProduct BIT NOT "
                + "NULL, bitIsActiveProduct BIT NOT NULL, dtmCreationDate DATE NOT NULL, varAdditionalInformation "
                + "VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createVendorSupplierProduct(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "VendorSupplierProduct(intVendorSupplierProductID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intVendorSupplierID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "VendorSupplier(intVendorSupplierID), intInventoryID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), varVendorSupplierProductCode "
                + "VARCHAR(25) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Receipt Tables
        private void createReceipt(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Receipt(intReceiptID INT NOT NULL "
                + "PRIMARY KEY, intReceiptGroupID INT NOT NULL, varReceiptNumber VARCHAR(25) NOT NULL, "
                + "intReceiptSubNumber INT NOT NULL, dtmReceiptCreationDate DATE NOT NULL, dtmReceiptCreationTime "
                + "TIME(0) NOT NULL, dtmReceiptCompletionDate DATE NOT NULL, dtmReceiptCompletionTime TIME(0) NOT NULL, "
                + "intCustomerID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Customer(intCustomerID), intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), "
                + "intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, fltCartTotal FLOAT NOT NULL, fltDiscountTotal "
                + "FLOAT NOT NULL, fltTradeInTotal FLOAT NOT NULL, fltShippingTotal FLOAT NOT NULL, "
                + "intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), "
                + "varReceiptComments VARCHAR(MAX) NOT NULL, fltTenderedAmount FLOAT NOT NULL, fltChangeAmount FLOAT "
                + "NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItem(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItem(intReceiptItemID INT "
                + "NOT NULL PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Receipt(intReceiptID), intInventoryID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), intItemQuantity INT "
                + "NOT NULL, fltItemAverageCostAtSale FLOAT NOT NULL, fltItemPrice FLOAT NOT NULL, fltItemDiscount "
                + "FLOAT NOT NULL, fltItemRefund FLOAT NOT NULL, bitIsPercentageDiscount BIT NOT NULL, "
                + "bitIsNonStockedProduct BIT NOT NULL, bitIsRegularProduct BIT NOT NULL, bitIsTradeIn BIT NOT NULL, "
                + "varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptPayment(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptPayment(intPaymentID INT NOT "
                + "NULL PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Receipt(intReceiptID), intMethodOfPaymentID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), fltAmountPaid FLOAT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItemTaxes(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemTaxes(intReceiptItemID "
                + "INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "ReceiptItem(intReceiptItemID), intTaxTypeID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, bitIsTaxCharged BIT NOT NULL, PRIMARY "
                + "KEY(intReceiptItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptCurrent(intReceiptID INT NOT "
                + "NULL IDENTITY(1,1) PRIMARY KEY, intReceiptGroupID INT NOT NULL, varReceiptNumber VARCHAR(25) NOT "
                + "NULL, intReceiptSubNumber INT NOT NULL, dtmReceiptCreationDate DATE NOT NULL, dtmReceiptCreationTime "
                + "TIME(0) NOT NULL, intCustomerID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Customer(intCustomerID), intEmployeeID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, fltCartTotal "
                + "FLOAT NOT NULL, fltDiscountTotal FLOAT NOT NULL, fltTradeInTotal FLOAT NOT NULL, fltShippingTotal "
                + "FLOAT NOT NULL, intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tblTransactionType(intTransactionTypeID), varReceiptComments VARCHAR(MAX) NOT NULL, "
                + "fltTenderedAmount FLOAT NOT NULL, fltChangeAmount FLOAT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItemCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemCurrent(intReceiptItemID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN KEY REFERENCES tbl"
                + Convert.ToInt32(businessNumber) + "ReceiptCurrent(intReceiptID), intInventoryID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), intItemQuantity "
                + "INT NOT NULL, fltItemAverageCostAtSale FLOAT NOT NULL, fltItemPrice FLOAT NOT NULL, fltItemDiscount "
                + "FLOAT NOT NULL, fltItemRefund FLOAT NOT NULL, bitIsPercentageDiscount BIT NOT NULL, "
                + "bitIsNonStockedProduct BIT NOT NULL, bitIsRegularProduct BIT NOT NULL, bitIsTradeIn BIT NOT NULL, "
                + "varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptPaymentCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptPaymentCurrent(intPaymentID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "ReceiptCurrent(intReceiptID), intMethodOfPaymentID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), fltAmountPaid FLOAT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItemTaxesCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "ReceiptItemTaxesCurrent(intReceiptItemID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "ReceiptItemCurrent(intReceiptItemID), intTaxTypeID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, bitIsTaxCharged BIT "
                + "NOT NULL, PRIMARY KEY(intReceiptItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptVoidCancel(intReceiptID INT "
                + "NOT NULL PRIMARY KEY, intReceiptGroupID INT NOT NULL, varReceiptNumber VARCHAR(25) NOT NULL, "
                + "intReceiptSubNumber INT NOT NULL, dtmReceiptCreationDate DATE NOT NULL, dtmReceiptCreationTime "
                + "TIME(0) NOT NULL, dtmReceiptVoidCancelDate DATE NOT NULL, dtmReceiptVoidCancelTime TIME(0) NOT NULL, "
                + "intCustomerID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Customer(intCustomerID), intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), "
                + "intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, fltCartTotal FLOAT NOT NULL, fltDiscountTotal "
                + "FLOAT NOT NULL, fltTradeInTotal FLOAT NOT NULL, fltShippingTotal FLOAT NOT NULL, "
                + "intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), "
                + "varReceiptComments VARCHAR(MAX) NOT NULL, fltTenderedAmount FLOAT NOT NULL, fltChangeAmount FLOAT "
                + "NOT NULL, bitReceiptVoided BIT NOT NULL, bitReceiptCancelled BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "ReceiptItemVoidCancel(intReceiptItemID INT NOT NULL PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "ReceiptVoidCancel(intReceiptID), "
                + "intInventoryID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Inventory(intInventoryID), intItemQuantity INT NOT NULL, fltItemAverageCostAtSale FLOAT NOT NULL, "
                + "fltItemPrice FLOAT NOT NULL, fltItemDiscount FLOAT NOT NULL, fltItemRefund FLOAT NOT NULL, "
                + "bitIsPercentageDiscount BIT NOT NULL, bitIsNonStockedProduct BIT NOT NULL, bitIsRegularProduct BIT "
                + "NOT NULL, bitIsTradeIn BIT NOT NULL, varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptPaymentVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "ReceiptPaymentVoidCancel(intPaymentID INT NOT NULL PRIMARY KEY, intReceiptID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "ReceiptVoidCancel(intReceiptID), "
                + "intMethodOfPaymentID INT NOT NULL FOREIGN KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), "
                + "fltAmountPaid FLOAT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createReceiptItemTaxesVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "ReceiptItemTaxesVoidCancel(intReceiptItemID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "ReceiptItemVoidCancel(intReceiptItemID), intTaxTypeID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, bitIsTaxCharged BIT "
                + "NOT NULL, PRIMARY KEY(intReceiptItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Invoice Tables
        private void createInvoice(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Invoice(intInvoiceID INT NOT NULL "
                + "PRIMARY KEY, varInvoiceNumber VARCHAR(25) NOT NULL, dtmInvoiceCreationDate DATE NOT NULL, "
                + "dtmInvoiceCreationTime TIME(0) NOT NULL, dtmInvoiceCompletionDate DATE NOT NULL, "
                + "dtmInvoiceCompletionTime TIME(0) NOT NULL, intCustomerID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Customer(intCustomerID), intEmployeeID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, "
                + "intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), "
                + "varInvoiceComments VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoiceItem(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceItem(intInvoiceItemID INT "
                + "NOT NULL PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Invoice(intInvoiceID), intInventoryID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), varItemSku "
                + "VARCHAR(25) NOT NULL, intItemQuantity INT NOT NULL, fltItemCost FLOAT NOT NULL, varItemDescription "
                + "VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoicePayment(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoicePayment(intPaymentID INT NOT "
                + "NULL PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Invoice(intInvoiceID), intMethodOfPaymentID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), fltAmountReceived FLOAT NOT NULL, "
                + "intChequeNumber INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoiceCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceCurrent(intInvoiceID INT NOT "
                + "NULL IDENTITY(1,1) PRIMARY KEY, varInvoiceNumber VARCHAR(25) NOT NULL, dtmInvoiceCreationDate DATE "
                + "NOT NULL, dtmInvoiceCreationTime TIME(0) NOT NULL, intCustomerID INT NOT NULL FOREIGN KEY REFERENCES "
                + "tbl" + Convert.ToInt32(businessNumber) + "Customer(intCustomerID), intEmployeeID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, "
                + "intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), "
                + "varInvoiceComments VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoiceItemCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceItemCurrent(intInvoiceItemID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN KEY REFERENCES tbl"
                + Convert.ToInt32(businessNumber) + "InvoiceCurrent(intInvoiceID), intInventoryID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), varItemSku "
                + "VARCHAR(25) NOT NULL, intItemQuantity INT NOT NULL, fltItemCost FLOAT NOT NULL, varItemDescription "
                + "VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoicePaymentCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoicePaymentCurrent(intPaymentID "
                + "INT NOT NULL IDENTITY(1,1) PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "InvoiceCurrent(intInvoiceID), intMethodOfPaymentID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), fltAmountReceived FLOAT NOT NULL, "
                + "intChequeNumber INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoiceVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceVoidCancel(intInvoiceID INT "
                + "NOT NULL PRIMARY KEY, varInvoiceNumber VARCHAR(25) NOT NULL, dtmInvoiceCreationDate DATE NOT NULL, "
                + "dtmInvoiceCreationTime TIME(0) NOT NULL, dtmInvoiceVoidCancelDate DATE NOT NULL, "
                + "dtmInvoiceVoidCancelTime TIME(0) NOT NULL, intCustomerID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Customer(intCustomerID), intEmployeeID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), fltCostTotal FLOAT NOT NULL, "
                + "intTransactionTypeID INT NOT NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), "
                + "varInvoiceComments VARCHAR(MAX) NOT NULL, bitIsInvoiceVoided BIT NOT NULL, bitIsInvoiceCancelled BIT "
                + "NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoiceItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "InvoiceItemVoidCancel(intInvoiceItemID INT NOT NULL PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "InvoiceVoidCancel(intInvoiceID), "
                + "intInventoryID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber)
                + "Inventory(intInventoryID), varItemSku VARCHAR(25) NOT NULL, intItemQuantity INT NOT NULL, "
                + "fltItemCost FLOAT NOT NULL, varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createInvoicePaymentVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "InvoicePaymentVoidCancel(intPaymentID INT NOT NULL PRIMARY KEY, intInvoiceID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "InvoiceVoidCancel(intInvoiceID), "
                + "intMethodOfPaymentID INT NOT NULL FOREIGN KEY REFERENCES tblMethodOfPayment(intMethodOfPaymentID), "
                + "fltAmountReceived FLOAT NOT NULL, intChequeNumber INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Purchase Order Tables
        private void createPurchaseOrder(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrder(intPurchaseOrderID "
                + "INT NOT NULL PRIMARY KEY, intPurchaseOrderGroupID INT NOT NULL, varPurchaseOrderNumber VARCHAR(25) "
                + "NOT NULL, dtmPurchaseOrderCreationDate DATE NOT NULL, dtmPurchaseOrderCreationTime TIME(0) NOT NULL, "
                + "dtmPurchaseOrderCompletionDate DATE NOT NULL, dtmPurchaseOrderCompletionTime TIME(0) NOT NULL, "
                + "intVendorSupplierID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber)
                + "VendorSupplier(intVendorSupplierID), intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), "
                + "intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Licence(intTerminalID), fltCostSubTotal FLOAT NOT NULL, fltGSTTotal FLOAT NOT NULL, fltPSTTotal "
                + "FLOAT NOT NULL, bitGSTCharged BIT NOT NULL, bitPSTCharged BIT NOT NULL, intTransactionTypeID INT NOT "
                + "NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), varPurchaseOrderComments "
                + "VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItem(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderItem(intPurchaseOrderItemID INT NOT NULL PRIMARY KEY, intPurchaseOrderID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrder(intPurchaseOrderID), "
                + "intVendorSupplierProductID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber)
                + "VendorSupplierProduct(intVendorSupplierProductID), intPurchaseOrderQuantity INT NOT NULL, "
                + "intReceivedQuantity INT NOT NULL, fltPurchaseOrderCost FLOAT NOT NULL, fltReceivedCost FLOAT NOT "
                + "NULL, varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItemTaxes(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderItemTaxes(intPurchaseOrderItemID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "PurchaseOrderItem(intPurchaseOrderItemID), intTaxTypeID INT NOT "
                + "NULL FOREIGN KEY REFERENCES tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, bitIsTaxCharged "
                + "BIT NOT NULL, PRIMARY KEY(intPurchaseOrderItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderCurrent(intPurchaseOrderID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intPurchaseOrderGroupID INT NOT NULL, varPurchaseOrderNumber VARCHAR(25) NOT NULL, "
                + "dtmPurchaseOrderCreationDate DATE NOT NULL, dtmPurchaseOrderCreationTime TIME(0) NOT NULL, "
                + "intVendorSupplierID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "VendorSupplier(intVendorSupplierID), intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl"
                + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), intStoreLocationID INT NOT NULL FOREIGN "
                + "KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), "
                + "intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Licence(intTerminalID), fltCostSubTotal FLOAT NOT NULL, fltGSTTotal FLOAT NOT NULL, fltPSTTotal "
                + "FLOAT NOT NULL, bitGSTCharged BIT NOT NULL, bitPSTCharged BIT NOT NULL, intTransactionTypeID INT NOT "
                + "NULL FOREIGN KEY REFERENCES tblTransactionType(intTransactionTypeID), varPurchaseOrderComments "
                + "VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItemCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderItemCurrent(intPurchaseOrderItemID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intPurchaseOrderID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderCurrent(intPurchaseOrderID), intVendorSupplierProductID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "VendorSupplierProduct(intVendorSupplierProductID), intPurchaseOrderQuantity INT NOT NULL, "
                + "intReceivedQuantity INT NOT NULL, fltPurchaseOrderCost FLOAT NOT NULL, fltReceivedCost FLOAT NOT "
                + "NULL, varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItemTaxesCurrent(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderItemTaxesCurrent(intPurchaseOrderItemID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "PurchaseOrderItemCurrent(intPurchaseOrderItemID), intTaxTypeID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, "
                + "bitIsTaxCharged BIT NOT NULL, PRIMARY KEY(intPurchaseOrderItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderVoidCancel(intPurchaseOrderID INT NOT NULL PRIMARY KEY, intPurcahseOrderGroupID INT NOT "
                + "NULL, varPurchaseOrderNumber VARCHAR(25) NOT NULL, dtmPurchaseOrderCreationDate DATE NOT NULL, "
                + "dtmPurchaseOrderCreationTime TIME(0) NOT NULL, dtmPurchaseOrderVoidCancelDate DATE NOT NULL, "
                + "dtmPurchaseOrderVoidCancelTime TIME(0) NOT NULL, intVendorSupplierID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) + "VendorSupplier(intVendorSupplierID), "
                + "intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "Employee(intEmployeeID), intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), intTerminalID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), "
                + "fltCostSubTotal FLOAT NOT NULL, fltGSTTotal FLOAT NOT NULL, fltPSTTotal FLOAT NOT NULL, "
                + "bitGSTCharged BIT NOT NULL, bitPSTCharged BIT NOT NULL, varPurchaseOrderComments VARCHAR(MAX) NOT "
                + "NULL, bitPOVoided BIT NOT NULL, bitPOCancelled BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "PurchaseOrderItemVoidCancel(intPurchaseOrderItemID INT NOT NULL PRIMARY KEY, intPurchaseOrderID INT "
                + "NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderVoidCancel(intPurchaseOrderID), intVendorSupplierProductID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "VendorSupplierProduct(intVendorSupplierProductID), intPurchaseOrderQuantity INT NOT NULL, "
                + "intReceivedQuantity INT NOT NULL, fltPurchaseOrderCost FLOAT NOT NULL, fltReceivedCost FLOAT NOT "
                + "NULL, varItemDescription VARCHAR(MAX) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchaseOrderItemTaxesVoidCancel(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchaseOrderItemTaxesVoidCancel(intPurchaseOrderItemID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "PurchaseOrderItemVoidCancel(intPurchaseOrderItemID), intTaxTypeID "
                + "INT NOT NULL FOREIGN KEY REFERENCES tblTaxType(intTaxTypeID), fltTaxAmount FLOAT NOT NULL, "
                + "bitIsTaxCharged BIT NOT NULL, PRIMARY KEY(intPurchaseOrderItemID, intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createPurchasedInventory(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "PurchasedInventory(intPurchasedInventoryID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), intInventoryID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), varSku VARCHAR(25) NOT NULL, "
                + "intItemQuantity INT NOT NULL, fltCost FLOAT NOT NULL, varItemDescription VARCHAR(MAX) NOT NULL, "
                + "intProcessedInventoryID INT NOT NULL, bitIsProcessedIntoInventory BIT NOT NULL, "
                + "bitDoNotShowToProcess BIT NOT NULL, bitIsUsedForParts BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Stored Incrementing Numbers
        private void createStoredReceiptNumber(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "StoredReceiptNumber(intReceiptStoreLocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), varStoreCodeReceipt VARCHAR(10) NOT NULL, intReceiptNumberSystem "
                + "INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createStoredTradeInNumber(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "StoredTradeInNumber(intTradeInStoreLocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), varStoreCodeTradeIn VARCHAR(10) NOT NULL, intTradeInNumberSystem "
                + "INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createStoredInvoiceNumber(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "StoredInvoiceNumber(intInvoiceStoreLocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), varStoreCodeInvoice VARCHAR(10) NOT NULL, intInvoiceNumberSystem "
                + "INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createStoredPurchaseOrderNumber(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "StoredPurchaseOrderNumber(intPurchaseOrderStoreLocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), varStoreCodePurchaseOrder VARCHAR(10) NOT NULL, "
                + "intPurchaseOrderNumberSystem INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createStoredInventorySku(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "StoredInventorySku(intInventoryStoreLocationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) 
                + "StoreLocation(intStoreLocationID), varStoreCodeInventory VARCHAR(10) NOT NULL, "
                + "intInventoryNumberSystem INT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Per Location Tables
        private void createGuestCustomerPerLocation(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "GuestCustomerPerLocation(intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), intCustomerID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Customer(intCustomerID), PRIMARY "
                + "KEY(intStoreLocationID, intCustomerID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createTradeInSkuPerLocation(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber)
                + "TradeInSkuPerLocation(intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), intInventoryID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), PRIMARY "
                + "KEY(intStoreLocationID, intInventoryID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Per Inventory Tables
        private void createTaxTypePerInventoryItem(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "TaxTypePerInventoryItem(intInventoryID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Inventory(intInventoryID), intTaxTypeID INT NOT NULL FOREIGN KEY "
                + "REFERENCES tblTaxType(intTaxTypeID), bitChargeTax BIT NOT NULL, PRIMARY KEY(intInventoryID, "
                + "intTaxTypeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Sales Cash Tables
        private void createSalesReconciliation(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) 
                + "SalesReconciliation(intSalesReconciliationID INT NOT NULL IDENTITY(1,1) PRIMARY KEY, "
                + "dtmSalesReconciliationDate DATE NOT NULL, dtmSalesReconciliationProcessedDate DATE NOT NULL, "
                + "dtmSalesReconciliationProcessedTime TIME(0) NOT NULL, fltAmericanExpressSales FLOAT NOT NULL, "
                + "fltCashSales FLOAT NOT NULL, fltChequeSales FLOAT NOT NULL, fltDebitSales FLOAT NOT NULL, "
                + "fltDiscoverSales FLOAT NOT NULL, fltGiftCardSales FLOAT NOT NULL, fltMasterCardSales FLOAT NOT "
                + "NULL, fltTradeInSales FLOAT NOT NULL, fltVisaSales FLOAT NOT NULL, fltAmericanExpressCounted FLOAT "
                + "NOT NULL, fltCashCounted FLOAT NOT NULL, fltChequeCounted FLOAT NOT NULL, fltDebitCounted FLOAT NOT "
                + "NULL, fltDiscoverCounted FLOAT NOT NULL, fltGiftCardCounted FLOAT NOT NULL, fltMasterCardCounted "
                + "FLOAT NOT NULL, fltTradeInCounted FLOAT NOT NULL, fltVisaCounted FLOAT NOT NULL, fltPreTaxSalesTotal "
                + "FLOAT NOT NULL, fltGovernmentTaxTotal FLOAT NOT NULL, fltProvincialTaxTotal FLOAT NOT NULL, "
                + "fltOverShort FLOAT NOT NULL, fltCashPurchases FLOAT NOT NULL, bitIsFinalized BIT NOT NULL, "
                + "bitIsProcessed BIT NOT NULL, intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), intEmployeeID INT NOT NULL "
                + "FOREIGN KEY REFERENCES tbl" + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID));";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void createTillCashout(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "TillCashout(intTillCashoutID INT "
                + "NOT NULL IDENTITY(1,1) PRIMARY KEY, intTerminalID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Licence(intTerminalID), dtmTillCashoutDate DATE NOT NULL, "
                + "dtmTillCashoutProcessedDate DATE NOT NULL, dtmTillCashoutProcessedTime TIME(0) NOT NULL, "
                + "intHundredDollarBillCount INT NOT NULL, intFiftyDollarBillCount INT NOT NULL, "
                + "intTwentyDollarBillCount INT NOT NULL, intTenDollarBillCount INT NOT NULL, intFiveDollarBillCount "
                + "INT NOT NULL, intToonieCoinCount INT NOT NULL, intLoonieCoinCount INT NOT NULL, intQuarterCoinCount "
                + "INT NOT NULL, intDimeCoinCount INT NOT NULL, intNickelCoinCount INT NOT NULL, fltCashDrawerTotal "
                + "FLOAT NOT NULL, fltCountedTotal FLOAT NOT NULL, fltCashDrawerFloat FLOAT NOT NULL, "
                + "fltCashDrawerCashDrop FLOAT NOT NULL, bitIsProcessed BIT NOT NULL, bitIsFinalized BIT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Error/Tracking Tables
        private void createError(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Error(intErrorTrackingID INT NOT "
                + "NULL IDENTITY(1,1) PRIMARY KEY, intEmployeeID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "Employee(intEmployeeID), dtmErrorDate DATE NOT NULL, dtmErrorTime "
                + "TIME(0) NOT NULL, varErrorPageOccurrence VARCHAR(MAX) NOT NULL, varErrorMethodOccurrence "
                + "VARCHAR(MAX) NOT NULL, intErrorCode INT NOT NULL, varErrorMessage VARCHAR(100) NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        //Licence Tables
        private void createLicence(object businessNumber)
        {
            string sqlCmd = "CREATE TABLE tbl" + Convert.ToInt32(businessNumber) + "Licence(intTerminalID INT NOT NULL "
                + "IDENTITY(1,1) PRIMARY KEY, intStoreLocationID INT NOT NULL FOREIGN KEY REFERENCES tbl" 
                + Convert.ToInt32(businessNumber) + "StoreLocation(intStoreLocationID), intTillNumber INT NOT NULL, "
                + "intLicenceID INT NOT NULL FOREIGN KEY REFERENCES tblIssuingLicences(intLicenceID), "
                + "fltDrawerFloatAmount FLOAT NOT NULL);";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}