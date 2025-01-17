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
        //All procedures in regards to an Inventory Item are stored here
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Inventory> ConvertFromDataTableToInventory(DataTable dt, int businessNumber)
        {
            //Converts from datatable into Inventory object
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
        private List<Inventory> ReturnInventorySearchQueryString(string searchText, int quantity, int businessNumber)
        {
            //This returns all inventory objects that matches a search string
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, intStoreLocationID, "
                        + "varUPCcode, intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, bitIsRegularProduct, "
                        + "bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, varAdditionalInformation FROM tbl" + businessNumber + "Inventory "
                        + "WHERE ((intBrandID IN(SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName LIKE "
                        + "@parm1" + i + ") OR CONCAT(varSku, varModelName, varDescription, varUPCcode) LIKE @parm2" + i + ") "
                        + "AND intInventoryID NOT IN(SELECT intInventoryID FROM tbl" + businessNumber + "TradeInSkuPerLocation)) "
                        + "AND (bitIsNonStockedProduct = 1 OR intQuantity > " + quantity + ") AND intInventoryID NOT IN (SELECT "
                        + "intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0)";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, "
                        + "intStoreLocationID, varUPCcode, intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, "
                        + "bitIsRegularProduct, bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, varAdditionalInformation FROM tbl"
                        + businessNumber + "Inventory WHERE ((intBrandID IN(SELECT intBrandID FROM tbl" + businessNumber
                        + "Brand WHERE varBrandName LIKE @parm1" + i + ") OR CONCAT(varSku, varModelName, varDescription, "
                        + "varUPCcode) LIKE @parm2" + i + ") AND intInventoryID NOT IN(SELECT intInventoryID FROM tbl"
                        + businessNumber + "TradeInSkuPerLocation)) AND (bitIsNonStockedProduct = 1 OR intQuantity > "
                        + quantity + ") AND intInventoryID NOT IN (SELECT intInventoryID FROM tbl" + businessNumber
                        + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0))";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
            }
            sqlCmd += " ORDER BY intInventoryID DESC";
            return ConvertFromDataTableToInventory(DBC.MakeDatabaseCallToReturnDataTableFromArrayListTwo(sqlCmd, parms, strText), businessNumber);
        }

        public List<Inventory> ReturnInventoryItem(int inventoryID, int businessNumber)
        {
            //Returns an Inventory object based it's ID
            string sqlCmd = "SELECT intInventoryID, varSku, intBrandID, varModelName, varDescription, intStoreLocationID, varUPCcode, "
                + "intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, bitIsRegularProduct, bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, "
                + "varAdditionalInformation FROM tbl" + businessNumber + "Inventory WHERE intInventoryID = @intInventoryID AND intInventoryID "
                + "NOT IN (SELECT intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0)";

            object[][] parms =
            {
                 new object[] { "@intInventoryID", inventoryID }
            };
            return ConvertFromDataTableToInventory(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Inventory> ReturnInventoryFromSearchString(string searchText, bool zeroQuantity, int businessNumber)
        {
            //This checks if the user requested to return all inventory including those with 0 quantity
            //Or just the items whose quantity are greater than 0.
            int quantity = 0;
            if (zeroQuantity)
            {
                quantity = -1;
            }
            return ReturnInventorySearchQueryString(searchText, quantity, businessNumber);
        }

        private DataTable CallForTradeInsToProcess(CurrentUser cu)
        {
            //Returns all Trade Ins that been "purchased" but not yet processed.
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

        public DataTable ReturnTradeInsForProcessing(CurrentUser cu)
        {
            //Public call for above process
            return CallForTradeInsToProcess(cu);
        }
        public DataTable ReturnDropDownForBrand(int businessNumber)
        {
            //Returns all Brands in the brand table to be used in a dropdown 
            string sqlCmd = "SELECT intBrandID, varBrandName FROM tbl" + businessNumber + "Brand ORDER BY varBrandName";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        private int GetBrandID(string brandName, int businessNumber)
        {
            //Returns Brand ID based on the string  
            string sqlCmd = "SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName = @varBrandName";
            object[][] parms =
            {
                new object[] { "@varBrandName", brandName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int ReturnTradeInBrand(int businessNumber)
        {
            //Returns the Brand ID where name is "Trade In"
            string sqlCmd = "SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName = 'Trade In'";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int InsertTradeInToInventoryTable(ReceiptItem receiptItem, DateTime createDateTime, CurrentUser cu)
        {
            //Inserts a Trade In into the Inventory table so that it can then be sold
            //(Difference from below: TradeIn is when the store trades customers items in exchange items the customer is purchasing from store)
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
            //Inserts a Purchased Trade In into the Inventory table so that it can then be sold
            //(Difference from above: PurchaseTradeIn is when the store only purchases customers items in exchange for cash or gift card)
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "Inventory VALUES(@varSku, @intBrandID, "
                + "@varModelName, @varDescription, @intStoreLocationID, @varUPCcode, @intQuantity, @fltPrice, @fltAverageCost, "
                + "@bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsUsedProduct, @bitIsActiveProduct, @dtmCreationDate, @varAdditionalInformation)";

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

        public int ReturnBrandIDFromBrandName(string brandName, int businessNumber)
        {
            //Returns Brand ID fromm the Brand Name
            return GetBrandID(brandName, businessNumber);
        }
        public int AddNewInventoryItem(Inventory inventory, DateTime createDateTime, CurrentUser cu)
        {
            //Process for adding a new item into the stores Inventory
            int newItem = AddNewInventory(inventory, createDateTime, cu.terminal.intBusinessNumber);
            SetTaxesForNewInventory(newItem, cu.currentStoreLocation.intProvinceID, true, createDateTime, cu.terminal.intBusinessNumber);
            return newItem;
        }
        public int AddNewInventory(Inventory inventory, DateTime createDateTime, int businessNumber)
        {
            //Adds Inventory Item into database
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
            //After creating a new inventory this uses all the saved parameters to return the inventory ID for ease of use.
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
            //Process for new Trade Ins inserting into Inventory table
            int newTrade = InsertTradeInToInventoryTable(receiptItem, createDateTime, cu);
            SetTaxesForNewInventory(newTrade, cu.currentStoreLocation.intProvinceID, false, createDateTime, cu.terminal.intBusinessNumber);
            return newTrade;
        }
        public int NewPurchaseTradeIn(InvoiceItem invoiceItem, DateTime createDateTime, CurrentUser cu)
        {
            //Process for new Purchased Trade Ins inserting into Inventory table
            int newTrade = InsertPurchaseTradeInToInventoryTable(invoiceItem, createDateTime, cu);
            SetTaxesForNewInventory(newTrade, cu.currentStoreLocation.intProvinceID, false, createDateTime, cu.terminal.intBusinessNumber);
            return newTrade;
        }
        public int ReturnDefaultBrand(int businessNumber)
        {
            //Returns the MIN ID in the Brand table
            string sqlCmd = "SELECT MIN(intBrandID) FROM tbl" + businessNumber + "Brand";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        private bool UpdatePurchasedInventory(int purchasedInventoryID, int tradeInAction, CurrentUser cu)
        {
            //Codes what is happening to a Purchased/TradeIn item
            bool added = false;
            bool noShow = false;
            bool parts = false;
            if (tradeInAction == 1)
            {
                //Added into the Inventory to be sold
                added = true;
            }
            else if (tradeInAction == 2)
            {
                //Item cannot be sold and is a loss
                noShow = true;
            }
            else if (tradeInAction == 3)
            {
                //Item is broken down and used for parts
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
            //Returns the next number to create a unique Inventory Sku
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
            //Returns the next number to create a unique TradeIn Sku
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
            //Updates the Inventory table for the Trade In item to qualify it as available for sale
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
            //Saves a TradeIn into the PurchasedInventory table so that it can then be marked as Inventory, Loss, or Parts.
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
            //Saves a PurchasedInventory into the PurchasedInventory table so that it can then be marked as Inventory, Loss, or Parts.
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
            //This updates an Inventory item with a new Average cost and new Quantity as the item would have received from a Purchase Order
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
            //Updates the numbering system for the next Inventory Sku to be created
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
            //Updates the numbering system for the next trade in sku to be created
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
            //Updates any information in regards to an Inventory item
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
            //Adds new Brand Name to the Brand table provided it doesn't already exist
            string sqlCmd = "IF EXISTS(SELECT TOP 1 intBrandID FROM tbl" + businessNumber + "Brand WHERE "
                + "varBrandName = @varBrandName) BEGIN PRINT '1'; END ELSE BEGIN "
                + "INSERT INTO tbl" + businessNumber + "Brand values(@varBrandName) PRINT '0'; END";
            object[][] parms =
            {
                            new object[] { "@varBrandName", brandName}
                        };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void SetTaxesForNewInventory(int inventoryID, int provinceID, bool chargeTax, DateTime taxDateTime, int businessNumber)
        {
            //Creates entries into the TaxTypePerInventoryItem table for any newly created Item.
            TaxManager TM = new TaxManager();
            List<ProvinceTax> lProcTax = TM.ReturnTaxListBasedOnDate(provinceID, taxDateTime, businessNumber);
            foreach(ProvinceTax PT in lProcTax)
            {
                TM.SaveTaxesForNewInventory(inventoryID, PT.intTaxTypeID, chargeTax, businessNumber);
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