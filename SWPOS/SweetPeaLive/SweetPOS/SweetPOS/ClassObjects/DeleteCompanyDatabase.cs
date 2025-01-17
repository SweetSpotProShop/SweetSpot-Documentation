using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class DeleteCompanyDatabase
    {
        //This class was created to drop all a companies table and remove them from the database.
        //This was only used for assistance in testing the creation of a buiness and their table
        //Running this to undo all the creates was easier than going in manually.
        //Should a business no longer require our services we would simply inactivate their licences
        DatabaseCalls DBC = new DatabaseCalls();
        //Used - Called from Initialization 1
        public void DELETEAllTableData(int businessNumber)
        {
            DELETEInitializationOfTerminal(businessNumber);
            DELETEInitializationOfBusiness(businessNumber);

            //Error tables
            DELETEError(businessNumber);
            //Cash tables
            DELETETillCashout(businessNumber);
            DELETESalesReconciliation(businessNumber);
            //Per Inventory tabkes
            DELETETaxTypePerInventoryItem(businessNumber);
            //Per Location tables
            DELETETradeInSkuPerLocation(businessNumber);
            DELETEGuestCustomerPerLocation(businessNumber);
            //Stored Incrementing Numbers
            DELETEStoredInventorySku(businessNumber);
            DELETEStoredPurchaseOrderNumber(businessNumber);
            DELETEStoredInvoiceNumber(businessNumber);
            DELETEStoredTradeInNumber(businessNumber);
            DELETEStoredReceiptNumber(businessNumber);
            //PurchaseOrder tables
            DELETEPurchasedInventory(businessNumber);
            DELETEPurchaseOrderItemTaxesVoidCancel(businessNumber);
            DELETEPurchaseOrderItemVoidCancel(businessNumber);
            DELETEPurchaseOrderVoidCancel(businessNumber);
            DELETEPurchaseOrderItemTaxesCurrent(businessNumber);
            DELETEPurchaseOrderItemCurrent(businessNumber);
            DELETEPurchaseOrderCurrent(businessNumber);
            DELETEPurchaseOrderItemTaxes(businessNumber);
            DELETEPurchaseOrderItem(businessNumber);
            DELETEPurchaseOrder(businessNumber);
            //Invoice tables
            DELETEInvoicePaymentVoidCancel(businessNumber);
            DELETEInvoiceItemVoidCancel(businessNumber);
            DELETEInvoiceVoidCancel(businessNumber);
            DELETEInvoicePaymentCurrent(businessNumber);
            DELETEInvoiceItemCurrent(businessNumber);
            DELETEInvoiceCurrent(businessNumber);
            DELETEInvoicePayment(businessNumber);
            DELETEInvoiceItem(businessNumber);
            DELETEInvoice(businessNumber);

            //Receipt tables
            DELETEReceiptItemTaxesVoidCancel(businessNumber);
            DELETEReceiptPaymentVoidCancel(businessNumber);
            DELETEReceiptItemVoidCancel(businessNumber);
            DELETEReceiptVoidCancel(businessNumber);
            DELETEReceiptItemTaxesCurrent(businessNumber);
            DELETEReceiptPaymentCurrent(businessNumber);
            DELETEReceiptItemCurrent(businessNumber);
            DELETEReceiptCurrent(businessNumber);
            DELETEReceiptItemTaxes(businessNumber);
            DELETEReceiptPayment(businessNumber);
            DELETEReceiptItem(businessNumber);
            DELETEReceipt(businessNumber);
            //Licence Tables
            DELETELicence(businessNumber);
            //Create general tables
            DELETEVendorSupplierProduct(businessNumber);
            DELETEInventory(businessNumber);
            DELETEEHSJYNLKHFNKLUCLFJ(businessNumber);
            DELETEMethodOfPayment(businessNumber);
            DELETEEmployee(businessNumber);
            DELETEVendorSupplier(businessNumber);
            DELETEJobCode(businessNumber);
            DELETEStoreLocation(businessNumber);
            DELETEBrand(businessNumber);
            DELETECustomer(businessNumber);
        }
        //De-Initialize Business Number
        private void DELETEInitializationOfTerminal(int businessNumber)
        {
            string sqlCmd = "UPDATE tblIssuingLicences SET bitLicenceInUse = 0, intBusinessNumber = 0 WHERE intBusinessNumber = @intBusinessNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", businessNumber }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //General Tables
        private void DELETECustomer(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Customer;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEBrand(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Brand;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEStoreLocation(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoreLocation;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEJobCode(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "JobCode;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEVendorSupplier(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "VendorSupplier;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEEmployee(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Employee;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEMethodOfPayment(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "MethodOfPayment;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEEHSJYNLKHFNKLUCLFJ(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "EHSJYNLKHFNKLUCLFJ;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInventory(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Inventory;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEVendorSupplierProduct(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "VendorSupplierProduct;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Receipt Tables
        private void DELETEReceipt(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Receipt;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItem(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItem;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemReturn(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemReturn;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptPayment(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptPayment;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemTaxes(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemTaxes;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptPaymentCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptPaymentCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemTaxesCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemTaxesCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptPaymentVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptPaymentVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEReceiptItemTaxesVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "ReceiptItemTaxesVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseTradeIn(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseTradeIn;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Invoice Tables
        private void DELETEInvoice(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Invoice;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoiceItem(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceItem;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoicePayment(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoicePayment;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoiceCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoiceItemCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceItemCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoicePaymentCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoicePaymentCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoiceVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoiceItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoiceItemVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInvoicePaymentVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "InvoicePaymentVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Purchase Order Tables
        private void DELETEPurchaseOrder(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrder;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItem(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItem;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItemTaxes(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItemTaxes;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItemCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItemCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItemTaxesCurrent(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItemTaxesCurrent;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItemVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItemVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchaseOrderItemTaxesVoidCancel(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchaseOrderItemTaxesVoidCancel;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEPurchasedInventory(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "PurchasedInventory;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Stored Incrementing Numbers
        private void DELETEStoredReceiptNumber(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoredReceiptNumber;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEStoredTradeInNumber(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoredTradeInNumber;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEStoredInvoiceNumber(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoredInvoiceNumber;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEStoredPurchaseOrderNumber(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoredPurchaseOrderNumber;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEStoredInventorySku(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "StoredInventorySku;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Per Location Tables
        private void DELETEGuestCustomerPerLocation(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "GuestCustomerPerLocation;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETETradeInSkuPerLocation(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "TradeInSkuPerLocation;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Per Inventory Tables
        private void DELETETaxTypePerInventoryItem(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "TaxTypePerInventoryItem;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Sales Cash Tables
        private void DELETESalesReconciliation(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "SalesReconciliation;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETETillCashout(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "TillCashout;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Error/Tracking Tables
        private void DELETEError(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Error;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Licence Tables
        private void DELETELicence(object businessNumber)
        {
            string sqlCmd = "DROP TABLE tbl" + Convert.ToInt32(businessNumber) + "Licence;";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void DELETEInitializationOfBusiness(int businessNumber)
        {
            string sqlCmd = "UPDATE tblBusinessIdentification SET bitIsBusinessInitialized "
                + "= 0 WHERE intBusinessNumber = @intBusinessNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", businessNumber }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}