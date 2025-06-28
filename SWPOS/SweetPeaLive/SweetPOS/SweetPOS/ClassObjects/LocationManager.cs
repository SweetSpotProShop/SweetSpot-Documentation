using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class LocationManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private int GetStoreLocationID(string storeName, int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID FROM tbl" + businessNumber + "StoreLocation WHERE varStoreName = @varStoreName";
            object[][] parms =
            {
                new object[] { "@varStoreName", storeName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int GetProvinceID(string provinceName)
        {
            string sqlCmd = "SELECT intProvinceID FROM tblProvince WHERE varProvinceName = @varProvinceName";
            object[][] parms =
            {
                new object[] { "@varProvinceName", provinceName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int GetCountryID(string countryName)
        {
            string sqlCmd = "SELECT intCountryID FROM tblCountry WHERE varCountryName = @varCountryName";
            object[][] parms =
            {
                new object[] { "@varCountryName", countryName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int ReturnStoreLocationIDFromStoreName(string storeName, int businessNumber)
        {
            return GetStoreLocationID(storeName, businessNumber);
        }
        public int ReturnProvinceIDFromProvinceName(string provinceName)
        {
            return GetProvinceID(provinceName);
        }
        public int ReturnCountryIDFromCountryName(string countryName)
        {
            return GetCountryID(countryName);
        }

        private List<StoreLocation> ConvertFromDataTableToStoreLocation(DataTable dt)
        {
            List<StoreLocation> location = dt.AsEnumerable().Select(row =>
            new StoreLocation
            {
                intStoreLocationID = row.Field<int>("intStoreLocationID"),
                varStoreName = row.Field<string>("varStoreName"),
                varPhoneNumber = row.Field<string>("varPhoneNumber"),
                varAddress = row.Field<string>("varAddress"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                varTaxNumber = row.Field<string>("varTaxNumber"),
                varStoreCode = row.Field<string>("varStoreCode"),
                bitIsRetailLocation = row.Field<bool>("bitIsRetailLocation")
            }).ToList();
            return location;
        }
        private List<StoreLocation> CreateNewStoreLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber
                + "StoreLocation VALUES(@varStoreName, @varPhoneNumber, @varAddress, "
                + "@varEmailAddress, @varCityName, @intProvinceID, @intCountryID, "
                + "@varPostalCode, @varTaxNumber, @varStoreCode, @bitIsRetailLocation)";

            object[][] parms =
            {
                 new object[] { "@varStoreName", storeLocation.varStoreName },
                 new object[] { "@varPhoneNumber", storeLocation.varPhoneNumber },
                 new object[] { "@varAddress", storeLocation.varAddress },
                 new object[] { "@varEmailAddress", storeLocation.varEmailAddress },
                 new object[] { "@varCityName", storeLocation.varCityName },
                 new object[] { "@intProvinceID", storeLocation.intProvinceID },
                 new object[] { "@intCountryID", storeLocation.intCountryID },
                 new object[] { "@varPostalCode", storeLocation.varPostalCode },
                 new object[] { "@varTaxNumber", storeLocation.varTaxNumber },
                 new object[] { "@varStoreCode", storeLocation.varStoreCode },
                 new object[] { "@bitIsRetailLocation", storeLocation.bitIsRetailLocation }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnStoreLocationFromStoreLocationStats(parms, businessNumber);
        }
        private List<StoreLocation> ReturnStoreLocationFromStoreLocationStats(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, varStoreName, varPhoneNumber, varAddress, varEmailAddress, "
                + "varCityName, intProvinceID, intCountryID, varPostalCode, varTaxNumber, varStoreCode, "
                + "bitIsRetailLocation FROM tbl" + businessNumber + "StoreLocation WHERE "
                + "varStoreName = @varStoreName AND varPhoneNumber = @varPhoneNumber AND varAddress = "
                + "@varAddress AND varEmailAddress = @varEmailAddress AND varCityName = @varCityName AND "
                + "intProvinceID = @intProvinceID AND intCountryID = @intCountryID AND varPostalCode = "
                + "@varPostalCode AND varTaxNumber = @varTaxNumber AND varStoreCode = @varStoreCode AND "
                + "bitIsRetailLocation = @bitIsRetailLocation";
            return ConvertFromDataTableToStoreLocation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<StoreLocation> ReturnLocation(int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, varStoreName, varPhoneNumber, varAddress, varEmailAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode, varTaxNumber, varStoreCode, bitIsRetailLocation FROM tbl" + businessNumber + "StoreLocation "
                + "WHERE intStoreLocationID = @storeLocationID";

            object[][] parms =
            {
                 new object[] { "@storeLocationID", storeLocationID }
            };
            return ConvertFromDataTableToStoreLocation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<StoreLocation> ReturnLocationFromTerminalID(int terminalID, int businessNumber)
        {
            string sqlCmd = "SELECT SL.intStoreLocationID, varStoreName, varPhoneNumber, varAddress, varEmailAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode, varTaxNumber, varStoreCode, bitIsRetailLocation FROM tbl" + businessNumber + "StoreLocation SL "
                + "JOIN tbl" + businessNumber + "Licence L ON L.intStoreLocationID = SL.intStoreLocationID WHERE L.intTerminalID = @intTerminalID";

            object[][] parms =
            {
                 new object[] { "@intTerminalID", terminalID }
            };
            return ConvertFromDataTableToStoreLocation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<StoreLocation> AddNewStoreLocationToBusiness(StoreLocation storeLocation, int businessNumber)
        {
            return CreateNewStoreLocation(storeLocation, businessNumber);
        }

        public DataTable ReturnProvinceDropDown(int countryID)
        {
            string sqlCmd = "SELECT intProvinceID, varProvinceName FROM "
                + "tblProvince WHERE intCountryID = @intCountryID "
                + "ORDER BY varProvinceName";
            object[][] parms = { new object[] { "@intCountryID", countryID } };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnCountryDropDown()
        {
            string sqlCmd = "SELECT intCountryID, varCountryName FROM "
                + "tblCountry ORDER BY varCountryName";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnStoreLocationDropDown(int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, varStoreName FROM tbl" + businessNumber + "StoreLocation "
                + "WHERE bitIsRetailLocation = 1 ORDER BY varStoreName";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public string ReturnProvinceName(int provinceID)
        {
            string sqlCmd = "SELECT varProvinceName FROM tblProvince WHERE "
                + "intProvinceID = @intProvinceID";
            object[][] parms =
            {
                new object[] { "@intProvinceID", provinceID }
            };
            return DBC.MakeDatabaseCallToReturnString(sqlCmd, parms);
        }

        private void CreateStoredReceiptNumberForLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "StoredReceiptNumber VALUES(@intStoreLocationID, "
                + "@varStoreCodeReceipt, @intReceiptNumberSystem)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocation.intStoreLocationID },
                new object[] { "@varStoreCodeReceipt", storeLocation.varStoreCode + "RE" },
                new object[] { "@intReceiptNumberSystem", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateStoredTradeInNumberForLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "StoredTradeInNumber VALUES(@intStoreLocationID, "
                + "@varStoreCodeTradeIn, @intTradeInNumberSystem)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocation.intStoreLocationID },
                new object[] { "@varStoreCodeTradeIn", storeLocation.varStoreCode + "TR" },
                new object[] { "@intTradeInNumberSystem", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateStoredInvoiceNumberForLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "StoredInvoiceNumber VALUES(@intStoreLocationID, "
                + "@varStoreCodeInvoice, @intInvoiceNumberSystem)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocation.intStoreLocationID },
                new object[] { "@varStoreCodeInvoice", storeLocation.varStoreCode + "IN" },
                new object[] { "@intInvoiceNumberSystem", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateStoredPurchaseOrderNumberForLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "StoredPurchaseOrderNumber "
                + "VALUES(@intStoreLocationID, @varStoreCodePurchaseOrder, @intPurchaseOrderNumberSystem)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocation.intStoreLocationID },
                new object[] { "@varStoreCodePurchaseOrder", storeLocation.varStoreCode + "PO" },
                new object[] { "@intPurchaseOrderNumberSystem", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateStoredInventorySKUForLocation(StoreLocation storeLocation, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "StoredInventorySKU VALUES(@intStoreLocationID, "
                + "@varStoreCodeInventory, @intInventoryNumberSystem)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocation.intStoreLocationID },
                new object[] { "@varStoreCodeInventory", storeLocation.varStoreCode + "IT" },
                new object[] { "@intInventoryNumberSystem", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void ExecuteInsertQueryToConnectGuestToStore(int businessNumber, int storeLocationID, int customerID)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "GuestCustomerPerLocation "
                + "VALUES(@intStoreLocationID, @intCustomerID)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID },
                new object[] { "@intCustomerID", customerID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void ExecuteInsertQueryToConnectTradeInToStore(int businessNumber, int storeLocationID, int inventoryID)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "TradeInSkuPerLocation "
                + "VALUES(@intStoreLocationID, @intInventoryID)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID },
                new object[] { "@intInventoryID", inventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void CallStoreLocationCreationCode(StoreLocation storeLocation, int businessNumber)
        {
            //List<StoreLocation> curStoreLocation = CreateNewStoreLocation(storeLocation, businessNumber);
            CreateStoredReceiptNumberForLocation(storeLocation, businessNumber);
            CreateStoredTradeInNumberForLocation(storeLocation, businessNumber);
            CreateStoredInvoiceNumberForLocation(storeLocation, businessNumber);
            CreateStoredPurchaseOrderNumberForLocation(storeLocation, businessNumber);
            CreateStoredInventorySKUForLocation(storeLocation, businessNumber);
        }
        public void ConnectGuestCustomerToStoreLocation(int businessNumber, int storeLocationID, int customerID)
        {
            ExecuteInsertQueryToConnectGuestToStore(businessNumber, storeLocationID, customerID);
        }        
        public void ConnectTradeInInventoryToStoreLocation(int businessNumber, int storeLocationID, int inventoryID)
        {
            ExecuteInsertQueryToConnectTradeInToStore(businessNumber, storeLocationID, inventoryID);
        }
        
    }
}