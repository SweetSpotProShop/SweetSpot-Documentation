using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class InventoryManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Inventory> ConvertFromDataTableToInventory(DataTable dt, DateTime currentDate, int businessNumber)
        {
            LocationManager LM = new LocationManager();
            List<Inventory> inventory = dt.AsEnumerable().Select(row =>
            new Inventory
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intBrandID = row.Field<int>("intBrandID"),
                varModelName = row.Field<string>("varModelName"),
                varDescription = row.Field<string>("varDescription"),
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                varUPCcode = row.Field<string>("varUPCcode"),
                intQuantity = row.Field<int>("intQuantity"),
                fltPrice = row.Field<double>("fltPrice"),
                fltAverageCost = row.Field<double>("fltAverageCost"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsUsedProduct = row.Field<bool>("bitIsUsedProduct"),
                bitIsActiveProduct = row.Field<bool>("bitIsActiveProduct"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            foreach(Inventory i in inventory)
            {
                i.lstTaxTypePerInventoryItem = ReturnTaxTypePerInventoryItem(businessNumber, currentDate, i.storeLocation.intProvinceID, i.intInventoryID);
            }
            return inventory;
        }
        private List<Inventory> ConvertFromDataTableToInventoryList(DataTable dt, int businessNumber)
        {
            LocationManager LM = new LocationManager();
            List<Inventory> inventory = dt.AsEnumerable().Select(row =>
            new Inventory
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intBrandID = row.Field<int>("intBrandID"),
                varModelName = row.Field<string>("varModelName"),
                varDescription = row.Field<string>("varDescription"),
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                varUPCcode = row.Field<string>("varUPCcode"),
                intQuantity = row.Field<int>("intQuantity"),
                fltPrice = row.Field<double>("fltPrice"),
                fltAverageCost = row.Field<double>("fltAverageCost"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsUsedProduct = row.Field<bool>("bitIsUsedProduct"),
                bitIsActiveProduct = row.Field<bool>("bitIsActiveProduct"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                varAdditionalInformation = row.Field<string>("varAdditionalInformation")
            }).ToList();
            return inventory;
        }
        private List<Inventory> ReturnInventorySearchQueryString(string searchText, int activeInactive, int businessNumber)
        {
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string strActiveInactive = "";
            if(activeInactive == ReturnActiveInactiveItem("Active", businessNumber))
            {
                strActiveInactive = " AND bitIsActiveProduct = 1";
            }
            else if (activeInactive == ReturnActiveInactiveItem("Inactive", businessNumber))
            {
                strActiveInactive = " AND bitIsActiveProduct = 0";
            }
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, intStoreLocationID, "
                        + "varUPCcode, intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, bitIsRegularProduct, "
                        + "bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, varAdditionalInformation FROM tbl" + businessNumber 
                        + "Inventory WHERE ((intBrandID IN(SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName "
                        + "LIKE @parm1" + i + ") OR CONCAT(varSku, varModelName, varDescription, varUPCcode) LIKE @parm2" + i + ") "
                        + "AND intInventoryID NOT IN(SELECT intInventoryID FROM tbl" + businessNumber + "TradeInSkuPerLocation)) "
                        + "AND intInventoryID NOT IN (SELECT intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE "
                        + "bitIsProcessedIntoInventory = 0)" + strActiveInactive;
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, "
                        + "intStoreLocationID, varUPCcode, intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, "
                        + "bitIsRegularProduct, bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, varAdditionalInformation "
                        + "FROM tbl" + businessNumber + "Inventory WHERE ((intBrandID IN(SELECT intBrandID FROM tbl" + businessNumber
                        + "Brand WHERE varBrandName LIKE @parm1" + i + ") OR CONCAT(varSku, varModelName, varDescription, "
                        + "varUPCcode) LIKE @parm2" + i + ") AND intInventoryID NOT IN(SELECT intInventoryID FROM tbl"
                        + businessNumber + "TradeInSkuPerLocation)) AND intInventoryID NOT IN (SELECT intInventoryID FROM tbl" 
                        + businessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0)" + strActiveInactive + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
            }
            sqlCmd += " ORDER BY varDescription ASC";
            return ConvertFromDataTableToInventoryList(DBC.MakeDatabaseCallToReturnDataTableFromArrayListTwo(sqlCmd, parms, strText), businessNumber);
        }

        public List<Inventory> ReturnInventoryItem(int inventoryID, int businessNumber, DateTime currentDate)
        {
            string sqlCmd = "SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, intStoreLocationID, varUPCcode, "
                + "intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, bitIsRegularProduct, bitIsUsedProduct, bitIsActiveProduct, "
                + "dtmCreationDate, varAdditionalInformation FROM tbl" + businessNumber + "Inventory WHERE intInventoryID = @intInventoryID "
                + "AND intInventoryID NOT IN (SELECT intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE "
                + "bitIsProcessedIntoInventory = 0)";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", inventoryID }
            };
            return ConvertFromDataTableToInventory(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), currentDate, businessNumber);
        }
        public List<Inventory> ReturnInventoryFromSearchString(string searchText, int activeInactive, int businessNumber)
        {
            return ReturnInventorySearchQueryString(searchText, activeInactive, businessNumber);
        }

        private List<TaxTypePerInventoryItem> ReturnTaxTypePerInventoryItem(int businessNumber, DateTime currentDate, int provinceID, int inventoryID)
        {
            string sqlCmd = "SELECT intInventoryID, TTPII.intTaxTypeID, varTaxName, fltTaxRate, bitChargeTax FROM tbl" + businessNumber 
                + "TaxTypePerInventoryItem TTPII JOIN tblTaxType TT ON TT.intTaxTypeID = TTPII.intTaxTypeID JOIN tbl" + businessNumber 
                + "TaxRateByProvince TRBP ON TRBP.intTaxTypeID = TTPII.intTaxTypeID WHERE dtmTaxEffectiveDate <= @dtmCurrentDate AND "
                + "intProvinceID = @intProvinceID AND intInventoryID = @intInventoryID";

	    string sqlCmd2 = "SELECT sku, TR.taxID, taxName, taxRate, bitChargeTax FROM tbl_taxRate TR JOIN tbl_taxTypePerInventoryItem "
                + "TTPII ON TTPII.taxID = TR.taxID JOIN tbl_taxType TT ON TT.taxID = TTPII.taxID JOIN(SELECT taxID, MAX(taxDate) AS MTD "
                + "FROM tbl_taxRate WHERE taxDate <= @taxDate AND provStateID = @provStateID GROUP BY taxID) TRBP ON TRBP.taxID = "
                + "TTPII.taxID WHERE TR.taxDate = TRBP.MTD AND sku = @sku AND provStateID = @provStateID";


            object[][] parms =
            {
                new object[] { "@dtmCurrentDate", currentDate.ToShortDateString() },
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@intInventoryID", inventoryID }
            };

            return ConvertFromDataTableToTaxTypePerInventoryItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<TaxTypePerInventoryItem> ConvertFromDataTableToTaxTypePerInventoryItem(DataTable dt)
        {
            List<TaxTypePerInventoryItem> taxTypePerInventoryItem = dt.AsEnumerable().Select(row =>
            new TaxTypePerInventoryItem
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                varTaxName = row.Field<string>("varTaxName"),
                fltTaxRate = row.Field<double>("fltTaxRate"),
                bitChargeTax = row.Field<bool>("bitChargeTax")                
            }).ToList();
            return taxTypePerInventoryItem;
        }
        private DataTable CallForTradeInsToProcess(CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchasedInventoryID, intInventoryID, varSku, intItemQuantity, fltCost, varItemDescription, "
                + "intProcessedInventoryID, bitIsProcessedIntoInventory, bitDoNotShowToProcess, bitIsUsedForParts FROM tbl"
                + cu.terminal.intBusinessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0 AND "
                + "bitDoNotShowToProcess = 0 AND bitIsUsedForParts = 0 AND intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        private DataTable CallDropDownActiveInactive(int businessNumber)
        {
            string sqlCmd = "SELECT intActiveInactive, varActiveInactive FROM tbl" + businessNumber + "ActiveInactive "
                + "ORDER BY intActiveInactive";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnTradeInsForProcessing(CurrentUser cu)
        {
            return CallForTradeInsToProcess(cu);
        }
        public DataTable ReturnDropDownForBrand(int businessNumber)
        {
            string sqlCmd = "SELECT intBrandID, varBrandName FROM tbl" + businessNumber + "Brand ORDER BY varBrandName";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnDropDownForActiveInactive(int businessNumber)
        {
            return CallDropDownActiveInactive(businessNumber);
        }

        private int GetBrandID(string brandName, int businessNumber)
        {
            string sqlCmd = "SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName = @varBrandName";
            object[][] parms =
            {
                new object[] { "@varBrandName", brandName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int ReturnTradeInBrand(int businessNumber)
        {
            string sqlCmd = "SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName = 'Trade In'";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int InsertTradeInToInventoryTable(ReceiptItem receiptItem, DateTime createDateTime, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "Inventory VALUES(@varSku, @intBrandID, "
                + "@varModelName, @varDescription, @intStoreLocationID, @varUPCcode, @intQuantity, @fltPrice, @fltAverageCost, "
                + "@bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsUsedProduct, @bitIsActiveProduct, @dtmCreationDate, @varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@varSku", ReturnNextTradeInSKUNumberforLocation(cu.currentStoreLocation.intStoreLocationID, cu.terminal.intBusinessNumber) },
                new object[] { "@intBrandID", ReturnTradeInBrand(cu.terminal.intBusinessNumber) },
                new object[] { "@varModelName", "" },
                new object[] { "@varDescription", receiptItem.varItemDescription },
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@varUPCcode", "" },
                new object[] { "@intQuantity", receiptItem.intItemQuantity },
                new object[] { "@fltPrice", 0 },
                new object[] { "@fltAverageCost", receiptItem.fltItemPrice * -1 },
                new object[] { "@bitIsNonStockedProduct", receiptItem.bitIsNonStockedProduct },
                new object[] { "@bitIsRegularProduct", receiptItem.bitIsRegularProduct },
                new object[] { "@bitIsUsedProduct", receiptItem.bitIsTradeIn },
                new object[] { "@bitIsActiveProduct", false },
                new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@varAdditionalInformation", "" }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnInventoryIDFromSKU(parms, cu.terminal.intBusinessNumber);
        }
        private int InsertPurchaseTradeInToInventoryTable(InvoiceItem invoiceItem, DateTime createDateTime, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "Inventory VALUES(@varSku, @intBrandID, "
                + "@varModelName, @varDescription, @intStoreLocationID, @varUPCcode, @intQuantity, @fltPrice, @fltAverageCost, "
                + "@bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsUsedProduct, @bitIsActiveProduct, @dtmCreationDate, "
                + "@varAdditionalInformation)";

            object[][] parms =
            {
                new object[] { "@varSku", ReturnNextTradeInSKUNumberforLocation(cu.currentStoreLocation.intStoreLocationID, cu.terminal.intBusinessNumber) },
                new object[] { "@intBrandID", ReturnTradeInBrand(cu.terminal.intBusinessNumber) },
                new object[] { "@varModelName", "" },
                new object[] { "@varDescription", invoiceItem.varItemDescription },
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@varUPCcode", "" },
                new object[] { "@intQuantity", invoiceItem.intItemQuantity },
                new object[] { "@fltPrice", 0 },
                new object[] { "@fltAverageCost", invoiceItem.fltItemCost },
                new object[] { "@bitIsNonStockedProduct", false },
                new object[] { "@bitIsRegularProduct", true },
                new object[] { "@bitIsUsedProduct", true },
                new object[] { "@bitIsActiveProduct", false },
                new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@varAdditionalInformation", "" }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnInventoryIDFromSKU(parms, cu.terminal.intBusinessNumber);
        }
        private int ReturnActiveInactiveItem(string activeInactive, int businessNumber)
        {
            string sqlCmd = "SELECT intActiveInactive FROM tbl" + businessNumber + "ActiveInactive WHERE varActiveInactive = "
                + "@varActiveInactive";
            object[][] parms =
            {
                new object[] { "@varActiveInactive", activeInactive }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int ReturnBrandIDFromBrandName(string brandName, int businessNumber)
        {
            return GetBrandID(brandName, businessNumber);
        }
        public int AddNewInventoryItem(Inventory inventory, DateTime createDateTime, CurrentUser cu)
        {
            int newItem = AddNewInventory(inventory, createDateTime, cu.terminal.intBusinessNumber);
            SetTaxesForNewInventory(newItem, true, createDateTime, cu.terminal.intBusinessNumber);
            return newItem;
        }
        public int AddNewInventory(Inventory inventory, DateTime createDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Inventory VALUES(@varSku, @intBrandID, @varModelName, @varDescription, "
                + "@intStoreLocationID, @varUPCcode, @intQuantity, @fltPrice, @fltAverageCost, @bitIsNonStockedProduct, "
                + "@bitIsRegularProduct, @bitIsUsedProduct, @bitIsActiveProduct, @dtmCreationDate, @varAdditionalInformation)";
            object[][] parms =
            {
                 new object[] { "@varSku", ReturnNextSKUNumberforLocation(inventory.intStoreLocationID, businessNumber) },
                 new object[] { "@intBrandID", inventory.intBrandID },
                 new object[] { "@varModelName", inventory.varModelName },
                 new object[] { "@varDescription", inventory.varDescription },
                 new object[] { "@intStoreLocationID", inventory.intStoreLocationID },
                 new object[] { "@varUPCcode", inventory.varUPCcode },
                 new object[] { "@intQuantity", inventory.intQuantity },
                 new object[] { "@fltPrice", inventory.fltPrice },
                 new object[] { "@fltAverageCost", inventory.fltAverageCost },
                 new object[] { "@bitIsNonStockedProduct", inventory.bitIsNonStockedProduct },
                 new object[] { "@bitIsRegularProduct", inventory.bitIsRegularProduct },
                 new object[] { "@bitIsUsedProduct", inventory.bitIsUsedProduct },
                 new object[] { "@bitIsActiveProduct", inventory.bitIsActiveProduct },
                 new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                 new object[] { "@varAdditionalInformation", inventory.varAdditionalInformation }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnInventoryIDFromSKU(parms, businessNumber);
        }
        public int ReturnInventoryIDFromSKU(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT intInventoryID FROM tbl" + businessNumber + "Inventory WHERE varSku = @varSku AND intBrandID = "
                + "@intBrandID AND varModelName = @varModelName AND varDescription = @varDescription AND intStoreLocationID = "
                + "@intStoreLocationID AND varUPCcode = @varUPCcode AND intQuantity = @intQuantity AND fltPrice = @fltPrice AND "
                + "fltAverageCost = @fltAverageCost AND bitIsNonStockedProduct = @bitIsNonStockedProduct AND bitIsRegularProduct = "
                + "@bitIsRegularProduct AND bitIsUsedProduct = @bitIsUsedProduct AND bitIsActiveProduct = @bitIsActiveProduct AND "
                + "dtmCreationDate = @dtmCreationDate AND varAdditionalInformation = @varAdditionalInformation";
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        public int NewTradeIn(ReceiptItem receiptItem, DateTime createDateTime, CurrentUser cu)
        {
            int newTrade = InsertTradeInToInventoryTable(receiptItem, createDateTime, cu);
            SetTaxesForNewInventory(newTrade, false, createDateTime, cu.terminal.intBusinessNumber);
            return newTrade;
        }
        public int NewPurchaseTradeIn(InvoiceItem invoiceItem, DateTime createDateTime, CurrentUser cu)
        {
            int newTrade = InsertPurchaseTradeInToInventoryTable(invoiceItem, createDateTime, cu);
            SetTaxesForNewInventory(newTrade, false, createDateTime, cu.terminal.intBusinessNumber);
            return newTrade;
        }
        public int ReturnDefaultBrand(int businessNumber)
        {
            string sqlCmd = "SELECT MIN(intBrandID) FROM tbl" + businessNumber + "Brand";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        private bool UpdatePurchasedInventory(int purchasedInventoryID, int tradeInAction, CurrentUser cu)
        {
            bool added = false;
            bool noShow = false;
            bool parts = false;
            if (tradeInAction == 1)
            {
                added = true;
            }
            else if (tradeInAction == 2)
            {
                noShow = true;
            }
            else if (tradeInAction == 3)
            {
                parts = true;
            }

            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "PurchasedInventory SET bitIsProcessedIntoInventory "
                + "= @bitIsProcessedIntoInventory, bitDoNotShowToProcess = @bitDoNotShowToProcess, bitIsUsedForParts = @bitIsUsedForParts "
                + "WHERE intPurchasedInventoryID = @intPurchasedInventoryID";
            object[][] parms =
            {
                new object[] { "bitIsProcessedIntoInventory", added },
                new object[] { "bitDoNotShowToProcess", noShow },
                new object[] { "bitIsUsedForParts", parts },
                new object[] { "intPurchasedInventoryID", purchasedInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return added;
        }

        public string ReturnNextSKUNumberforLocation(int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, intInventoryNumberSystem, CONCAT(varStoreCodeInventory, CASE WHEN "
                + "LEN(CAST(intInventoryNumberSystem AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intInventoryNumberSystem "
                + "AS VARCHAR(6))),6) ELSE CAST(intInventoryNumberSystem AS VARCHAR(MAX)) END) AS varInventorySKU FROM tbl" 
                + businessNumber + "StoredInventorySKU WHERE intStoreLocationID = @intStoreLocationID";

            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            //Creates the new inventory sku
            CreateInventoryNumberForNextInventoryItem(DBC.MakeDatabaseCallToReturnSecondColumnAsInt(sqlCmd, parms) + 1, storeLocationID, businessNumber);
            //Returns the inventory sku for use on new item
            return DBC.MakeDatabaseCallToReturnThirdColumnAsString(sqlCmd, parms);
        }
        public string ReturnNextTradeInSKUNumberforLocation(int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, intTradeInNumberSystem, CONCAT(varStoreCodeTradeIn, CASE WHEN "
                + "LEN(CAST(intTradeInNumberSystem AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intTradeInNumberSystem "
                + "AS VARCHAR(6))),6) ELSE CAST(intTradeInNumberSystem AS VARCHAR(MAX)) END) AS varInventorySKU FROM tbl"
                + businessNumber + "StoredTradeInNumber WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            //Creates the new inventory sku
            CreateInventoryNumberForNextTradeInItem(DBC.MakeDatabaseCallToReturnSecondColumnAsInt(sqlCmd, parms) + 1, storeLocationID, businessNumber);
            //Returns the inventory sku for use on new item
            return DBC.MakeDatabaseCallToReturnThirdColumnAsString(sqlCmd, parms);
        }

        private void MakeTradeInAvailableForSale(int purchasedInventoryID, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "Inventory SET bitIsRegularProduct = 1, bitIsActiveProduct "
                + "= 1 FROM tbl" + cu.terminal.intBusinessNumber + "Inventory I JOIN tbl" + cu.terminal.intBusinessNumber
                + "PurchasedInventory PI ON PI.intInventoryID = I.intInventoryID WHERE PI.intPurchasedInventoryID = @intPurchasedInventoryID";
            object[][] parms =
            {
                new object[] { "@intPurchasedInventoryID", purchasedInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void SavingTradeInForProcessing(ReceiptItem item, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "PurchasedInventory VALUES (@intStoreLocationID, "
                + "@intInventoryID, @varSku, @intItemQuantity, @fltCost, @varItemDescription, @intProcessedInventoryID, "
                + "@bitIsProcessedIntoInventory, @bitDoNotShowToProcess, @bitIsUsedForParts)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@intInventoryID", item.intInventoryID },
                new object[] { "@varSku", item.varSku },
                new object[] { "@intItemQuantity", item.intItemQuantity },
                new object[] { "@fltCost", item.fltItemPrice * -1 },
                new object[] { "@varItemDescription", item.varItemDescription },
                new object[] { "@intProcessedInventoryID", item.intReceiptItemID },
                new object[] { "@bitIsProcessedIntoInventory", false },
                new object[] { "@bitDoNotShowToProcess", false },
                new object[] { "@bitIsUsedForParts", false }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }        
        private void SavingPurchasesForProcessing(InvoiceItem item, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "PurchasedInventory VALUES (@intStoreLocationID, "
                + "@intInventoryID, @varSku, @intItemQuantity, @fltCost, @varItemDescription, @intProcessedInventoryID, "
                + "@bitIsProcessedIntoInventory, @bitDoNotShowToProcess, @bitIsUsedForParts)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@intInventoryID", item.intInventoryID },
                new object[] { "@varSku", item.varItemSku },
                new object[] { "@intItemQuantity", item.intItemQuantity },
                new object[] { "@fltCost", item.fltItemCost },
                new object[] { "@varItemDescription", item.varItemDescription },
                new object[] { "@intProcessedInventoryID", item.intInvoiceItemID },
                new object[] { "@bitIsProcessedIntoInventory", false },
                new object[] { "@bitDoNotShowToProcess", false },
                new object[] { "@bitIsUsedForParts", false }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void SaveInventoryFromInvoiceItem(InvoiceItem invoiceItem, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "Inventory SET fltAverageCost = @fltAverageCost, "
                + "intQuantity = @intQuantity, varDescription = @varDescription WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", invoiceItem.intInventoryID },
                new object[] { "@fltAverageCost", invoiceItem.fltItemCost },
                new object[] { "@intQuantity", invoiceItem.intItemQuantity },
                new object[] { "@varDescription", invoiceItem.varItemDescription }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }                
        private void CreateInventoryNumberForNextInventoryItem(int intSku, int storeLocationID, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "StoredInventorySKU SET intInventoryNumberSystem "
                + "= @inventorySKU WHERE intStoreLocationID = @storeLocationID";

            object[][] parms =
            {
                new object[] { "@inventorySKU", intSku },
                new object[] { "@storeLocationID", storeLocationID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateInventoryNumberForNextTradeInItem(int intSku, int storeLocationID, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "StoredTradeInNumber SET intTradeInNumberSystem "
                + "= @inventorySKU WHERE intStoreLocationID = @storeLocationID";

            object[][] parms =
            {
                new object[] { "@inventorySKU", intSku },
                new object[] { "@storeLocationID", storeLocationID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void UpdateInventory(Inventory inventory, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "Inventory SET fltAverageCost = @fltAverageCost, intBrandID "
                + "= @intBrandID, fltPrice = @fltPrice, intQuantity = @intQuantity, intStoreLocationID = @intStoreLocationID, "
                + "varModelName = @varModelName, varUPCcode = @varUPCcode, bitIsRegularProduct = @bitIsRegularProduct, "
                + "bitIsNonStockedProduct = @bitIsNonStockedProduct, varDescription = @varDescription, bitIsUsedProduct = "
                + "@bitIsUsedProduct, bitIsActiveProduct = @bitIsActiveProduct, varAdditionalInformation = "
                + "@varAdditionalInformation WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventory.intInventoryID },
                new object[] { "@fltAverageCost", inventory.fltAverageCost },
                new object[] { "@intBrandID", inventory.intBrandID },
                new object[] { "@fltPrice", inventory.fltPrice },
                new object[] { "@intQuantity", inventory.intQuantity },
                new object[] { "@intStoreLocationID", inventory.intStoreLocationID },
                new object[] { "@varModelName", inventory.varModelName },
                new object[] { "@varUPCcode", inventory.varUPCcode },
                new object[] { "@bitIsRegularProduct", inventory.bitIsRegularProduct },
                new object[] { "@bitIsNonStockedProduct", inventory.bitIsNonStockedProduct },
                new object[] { "@varDescription", inventory.varDescription },
                new object[] { "@bitIsUsedProduct", inventory.bitIsUsedProduct },
                new object[] { "@bitIsActiveProduct", inventory.bitIsActiveProduct },
                new object[] { "@varAdditionalInformation", inventory.varAdditionalInformation }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void SetUpTradeInForProcessing(ReceiptItem item, CurrentUser cu)
        {
            SavingTradeInForProcessing(item, cu);
        }
        public void UpdateInventoryFromInvoiceItem(InvoiceItem invoiceItem, CurrentUser cu)
        {
            SaveInventoryFromInvoiceItem(invoiceItem, cu);
        }
        public void AddNewBrandName(string brandName, int businessNumber)
        {
            string sqlCmd = "IF EXISTS(SELECT TOP 1 intBrandID FROM tbl" + businessNumber + "Brand WHERE "
                + "varBrandName = @varBrandName) BEGIN PRINT '1'; END ELSE BEGIN "
                + "INSERT INTO tbl" + businessNumber + "Brand values(@varBrandName) PRINT '0'; END";
            object[][] parms =
            {
                            new object[] { "@varBrandName", brandName}
                        };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void SetTaxesForNewInventory(int inventoryID, bool chargeTax, DateTime taxDateTime, int businessNumber)
        {
            TaxManager TM = new TaxManager();
            List<TaxRate> lTaxRate = TM.ReturnTaxListBasedOnDate(taxDateTime, businessNumber);
            foreach(TaxRate TR in lTaxRate)
            {
                TM.SaveTaxesForNewInventory(inventoryID, TR.intTaxTypeID, chargeTax, businessNumber);
            }
        }
        public void SetUpPurchasesForProcessing(InvoiceItem item, CurrentUser cu)
        {
            SavingPurchasesForProcessing(item, cu);
        }
        public void FinalTradeInManagementProcess(int purchasedInventoryID, int tradeInAction, CurrentUser cu)
        {
            if (UpdatePurchasedInventory(purchasedInventoryID, tradeInAction, cu))
            {
                MakeTradeInAvailableForSale(purchasedInventoryID, cu);
            }
        }
    }
}