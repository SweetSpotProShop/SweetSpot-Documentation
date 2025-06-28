using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class TaxManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<ReceiptItemTax> ConvertFromDataTableToReceiptItemTaxes(DataTable dt)
        {
            List<ReceiptItemTax> receiptItemTax = dt.AsEnumerable().Select(row =>
            new ReceiptItemTax
            {
                intReceiptItemID = row.Field<int>("intReceiptItemID"),
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                fltTaxAmount = row.Field<double>("fltTaxAmount"),
                bitIsTaxCharged = row.Field<bool>("bitIsTaxCharged")
            }).ToList();
            return receiptItemTax;
        }
        private List<ReceiptItemTax> ReturnTaxesAvailableForItem(ReceiptItem receiptItem, int transactionTypeID, DateTime currentDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT RIC.intReceiptItemID, TTPII.intTaxTypeID, ";

            if (transactionTypeID == 1)
            {
                sqlCmd += "CASE WHEN RIC.bitIsPercentageDiscount = 1 THEN ROUND(((RIC.fltItemPrice - (RIC.fltItemPrice * (RIC.fltItemDiscount "
                    + "/ 100))) * ITR.fltTaxRate) * RIC.intItemQuantity, 2) ELSE ROUND(((RIC.fltItemPrice - RIC.fltItemDiscount) * ITR.fltTaxRate) "
                    + "* RIC.intItemQuantity, 2) END AS fltTaxAmount, ";
            }
            else if (transactionTypeID == 2)
            {
                sqlCmd += "ROUND((RIC.fltItemRefund * ITR.fltTaxRate) * RIC.intItemQuantity, 2) AS fltTaxAmount, ";
            }

            sqlCmd += "TTPII.bitChargeTax AS bitIsTaxCharged FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptItemCurrent RIC JOIN tbl"
                + cu.terminal.intBusinessNumber + "TaxTypePerInventoryItem TTPII ON TTPII.intInventoryID = RIC.intInventoryID JOIN(SELECT "
                + "TTPI.intInventoryID, TTPI.intTaxTypeID, fltTaxRate FROM tblTaxRateByProvince TR INNER JOIN(SELECT intTaxTypeID, "
                + "MAX(dtmTaxEffectiveDate) AS MTD FROM tblTaxRateByProvince WHERE dtmTaxEffectiveDate <= @dtmCurrentDate AND intProvinceID = "
                + "@intProvinceID GROUP BY intTaxTypeID) TD ON TR.intTaxTypeID = TD.intTaxTypeID AND TR.dtmTaxEffectiveDate = TD.MTD INNER "
                + "JOIN(SELECT intInventoryID, intTaxTypeID FROM tbl" + cu.terminal.intBusinessNumber + "TaxTypePerInventoryItem WHERE "
                + "bitChargeTax = 1 AND intInventoryID = @intInventoryID) TTPI ON TTPI.intTaxTypeID = TR.intTaxTypeID WHERE intProvinceID = "
                + "@intProvinceID) ITR ON ITR.intInventoryID = RIC.intInventoryID AND ITR.intTaxTypeID = TTPII.intTaxTypeID WHERE "
                + "RIC.intReceiptID = @intReceiptID AND RIC.intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@dtmCurrentDate",  currentDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@intProvinceID", cu.currentStoreLocation.intProvinceID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID }
            };
            return ConvertFromDataTableToReceiptItemTaxes(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptItemTax> ReturnReceiptItemTaxesCurrent(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptItemID, intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl" + businessNumber 
                + "ReceiptItemTaxesCurrent WHERE intReceiptItemID IN(SELECT intReceiptItemID FROM tbl" + businessNumber 
                + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID)";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            return ConvertFromDataTableToReceiptItemTaxes(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptItemTax> ReturnReceiptItemTaxesPostSale(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptItemID, intTaxTypeID, fltTaxAmount, bitIsTaxCharged FROM tbl" + businessNumber 
                + "ReceiptItemTaxes WHERE intReceiptItemID IN(SELECT intReceiptItemID FROM tbl" + businessNumber + "ReceiptItem "
                + "WHERE intReceiptID = @intReceiptID)";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            return ConvertFromDataTableToReceiptItemTaxes(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<ReceiptItemTax> ReturnReceiptItemTaxesCurrentToProcessSale(int receiptID, int businessNumber)
        {
            return ReturnReceiptItemTaxesCurrent(receiptID, businessNumber);
        }
        public List<ReceiptItemTax> ReturnReceiptItemTaxes(int receiptID, int businessNumber)
        {
            return ReturnReceiptItemTaxesPostSale(receiptID, businessNumber);
        }

        private List<PurchaseOrderItemTax> ReturnTaxesAvailableForPurchaseOrderItem(PurchaseOrderItem purchaseOrderItem, DateTime currentDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT POIC.intPurchaseOrderItemID, TTPII.intTaxTypeID, ROUND((POIC.fltPurchaseOrderCost * ITR.fltTaxRate) * "
                + "POIC.intPurchaseOrderQuantity, 2) AS fltTaxAmount, TTPII.bitChargeTax AS bitIsTaxCharged FROM tbl"
                + cu.terminal.intBusinessNumber + "PurchaseOrderItemCurrent POIC JOIN tbl" + cu.terminal.intBusinessNumber
                + "VendorSupplierProduct VSP ON VSP.intVendorSupplierProductID = POIC.intVendorSupplierProductID JOIN tbl"
                + cu.terminal.intBusinessNumber + "TaxTypePerInventoryItem TTPII ON TTPII.intInventoryID = VSP.intInventoryID JOIN("
                + "SELECT TTPI.intInventoryID, TTPI.intTaxTypeID, fltTaxRate FROM tblTaxRateByProvince TR INNER JOIN(SELECT intTaxTypeID, "
                + "MAX(dtmTaxEffectiveDate) AS MTD FROM tblTaxRateByProvince WHERE dtmTaxEffectiveDate <= @dtmCurrentDate AND "
                + "intProvinceID = @intProvinceID GROUP BY intTaxTypeID) TD ON TR.intTaxTypeID = TD.intTaxTypeID AND "
                + "TR.dtmTaxEffectiveDate = TD.MTD INNER JOIN(SELECT TTPII1.intInventoryID, intTaxTypeID FROM tbl"
                + cu.terminal.intBusinessNumber + "TaxTypePerInventoryItem TTPII1 JOIN tbl" + cu.terminal.intBusinessNumber
                + "VendorSupplierProduct VSP1 ON VSP1.intInventoryID = TTPII1.intInventoryID WHERE bitChargeTax = 1 AND "
                + "VSP1.intVendorSupplierProductID = @intVendorSupplierProductID) TTPI ON TTPI.intTaxTypeID = TR.intTaxTypeID WHERE "
                + "intProvinceID = @intProvinceID) ITR ON ITR.intInventoryID = VSP.intInventoryID AND ITR.intTaxTypeID = "
                + "TTPII.intTaxTypeID WHERE POIC.intPurchaseOrderID = @intPurchaseOrderID AND VSP.intVendorSupplierProductID = "
                + "@intVendorSupplierProductID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderItem.intPurchaseOrderID },
                new object[] { "@dtmCurrentDate",  currentDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@intProvinceID", cu.currentStoreLocation.intProvinceID },
                new object[] { "@intVendorSupplierProductID", purchaseOrderItem.intVendorSupplierProductID }
            };
            return ConvertFromDataTableToPurchaseOrderItemTaxes(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<PurchaseOrderItemTax> ReturnPurchaseOrderItemTaxesCurrent(int purchaseOrderID, int businessNumber)
        {
            string sqlCmd = "SELECT intPurchaseOrderItemID, intTaxTypeID, fltTaxAmount, bitIsTaxCharged "
                + "FROM tbl" + businessNumber + "PurchaseOrderItemTaxesCurrent WHERE intPurchaseOrderItemID "
                + "IN(SELECT intPurchaseOrderItemID FROM tbl" + businessNumber + "PurchaseOrderItemCurrent "
                + "WHERE intPurchaseOrderID = @intPurchaseOrderID)";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderItemTaxes(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<PurchaseOrderItemTax> ConvertFromDataTableToPurchaseOrderItemTaxes(DataTable dt)
        {
            List<PurchaseOrderItemTax> purchaseOrderItemTax = dt.AsEnumerable().Select(row =>
            new PurchaseOrderItemTax
            {
                intPurchaseOrderItemID = row.Field<int>("intPurchaseOrderItemID"),
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                fltTaxAmount = row.Field<double>("fltTaxAmount"),
                bitIsTaxCharged = row.Field<bool>("bitIsTaxCharged")
            }).ToList();
            return purchaseOrderItemTax;
        }

        public List<PurchaseOrderItemTax> ReturnPurchaseOrderItemTaxesCurrentToProcessPO(int purchaseOrderID, int businessNumber)
        {
            return ReturnPurchaseOrderItemTaxesCurrent(purchaseOrderID, businessNumber);
        }

        public List<ProvinceTax> ConvertFromDataTableToProvinceTax(DataTable dt)
        {
            List<ProvinceTax> tax = dt.AsEnumerable().Select(row =>
            new ProvinceTax
            {
                intProvinceID = row.Field<int>("intProvinceID"),
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                varTaxName = row.Field<string>("varTaxName"),
                dtmTaxEffectiveDate = row.Field<DateTime>("dtmTaxEffectiveDate"),
                fltTaxRate = row.Field<double>("fltTaxRate")
            }).ToList();
            return tax;
        }

        public List<TaxRate> ConvertFromDataTableToTaxRate(DataTable dt)
        {
            List<TaxRate> tax = dt.AsEnumerable().Select(row =>
            new TaxRate
            {
                intTaxTypeID = row.Field<int>("intTaxTypeID"),
                varTaxName = row.Field<string>("varTaxName"),
                dtmTaxEffectiveDate = row.Field<DateTime>("dtmTaxEffectiveDate"),
                fltTaxRate = row.Field<double>("fltTaxRate")
            }).ToList();
            return tax;
        }
        public List<TaxRate> ReturnTaxListBasedOnDate(DateTime selectedDate, int businessNumber)
        {
            return ConvertFromDataTableToTaxRate(ReturnFullTaxListBasedOnDate(businessNumber, selectedDate));
        }

        public DataTable ReturnFullTaxListBasedOnDate(int businessNumber, DateTime currentDate)
        {
            string sqlCmd = "SELECT TR.intTaxTypeID, TT.varTaxName, TR.dtmTaxEffectiveDate, TR.fltTaxRate FROM tbl" + businessNumber 
                + "TaxRateByProvince AS TR INNER JOIN tblTaxType TT ON TR.intTaxTypeID = TT.intTaxTypeID INNER JOIN(SELECT intTaxTypeID, "
                + "MAX(dtmTaxEffectiveDate) AS MTD FROM tbl" + businessNumber + "TaxRateByProvince WHERE (dtmTaxEffectiveDate <= "
                + "@dtmTaxEffectiveDate) GROUP BY intTaxTypeID) AS TD ON TR.intTaxTypeID = TD.intTaxTypeID AND TR.dtmTaxEffectiveDate = TD.MTD";
            object[][] parms =
            {
                new object[] { "@dtmTaxEffectiveDate", currentDate }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }


        public List<ProvinceTax> ReturnProvinceTaxListBasedOnDate(int provinceID, DateTime selectedDate, int businessNumber)
        {
            return ConvertFromDataTableToProvinceTax(ReturnTaxListBasedOnDateAndProvinceForUpdate(provinceID, selectedDate));
        }

        public DataTable ReturnTaxListBasedOnDateAndProvinceForUpdate(int provinceID, DateTime currentDate)
        {
            string sqlCmd = "SELECT TR.intProvinceID, TR.intTaxTypeID, TT.varTaxName, TR.dtmTaxEffectiveDate, TR.fltTaxRate FROM "
                + "tblTaxRateByProvince AS TR INNER JOIN tblTaxType TT ON TR.intTaxTypeID = TT.intTaxTypeID INNER JOIN(SELECT "
                + "intTaxTypeID, MAX(dtmTaxEffectiveDate) AS MTD FROM tblTaxRateByProvince WHERE (dtmTaxEffectiveDate <= "
                + "@dtmCurrentDate) AND (intProvinceID = @intProvinceID) GROUP BY intTaxTypeID) AS TD ON TR.intTaxTypeID = "
                + "TD.intTaxTypeID AND TR.dtmTaxEffectiveDate = TD.MTD WHERE (TR.intProvinceID = @intProvinceID)";
            object[][] parms =
            {
                new object[] { "@dtmCurrentDate", currentDate },
                new object[] { "@intProvinceID", provinceID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }



        private double LoopThroughTaxesForGSTTotal(List<ReceiptItemTax> riTax)
        {
            double gst = 0;
            foreach (ReceiptItemTax tax in riTax)
            {
                if (tax.intTaxTypeID == 1 || tax.intTaxTypeID == 2 || tax.intTaxTypeID == 4)
                {
                    if (tax.bitIsTaxCharged)
                    {
                        gst += tax.fltTaxAmount;
                    }
                }
            }
            return gst;
        }
        private double LoopThroughTaxesForPSTTotal(List<ReceiptItemTax> riTax)
        {
            double pst = 0;
            foreach (ReceiptItemTax tax in riTax)
            {
                if (tax.intTaxTypeID == 3 || tax.intTaxTypeID == 5)
                {
                    if (tax.bitIsTaxCharged)
                    {
                        pst += tax.fltTaxAmount;
                    }
                }
            }
            return pst;
        }
        private double LoopThroughTaxesForGSTTotalPO(List<PurchaseOrderItemTax> poiTax)
        {
            double gst = 0;
            foreach (PurchaseOrderItemTax tax in poiTax)
            {
                if (tax.intTaxTypeID == 1 || tax.intTaxTypeID == 2 || tax.intTaxTypeID == 4)
                {
                    if (tax.bitIsTaxCharged)
                    {
                        gst += tax.fltTaxAmount;
                    }
                }
            }
            return gst;
        }
        private double LoopThroughTaxesForPSTTotalPO(List<PurchaseOrderItemTax> poiTax)
        {
            double pst = 0;
            foreach (PurchaseOrderItemTax tax in poiTax)
            {
                if (tax.intTaxTypeID == 3 || tax.intTaxTypeID == 5)
                {
                    if (tax.bitIsTaxCharged)
                    {
                        pst += tax.fltTaxAmount;
                    }
                }
            }
            return pst;
        }
        private double GatherNewReceivingTaxAmount(PurchaseOrderItemTax purchaseOrderItemTax, DateTime currentDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT ROUND((POIC.fltReceivedCost * TR.fltTaxRate) * POIC.intReceivedQuantity, 2) AS fltTaxAmount FROM tbl"
                + cu.terminal.intBusinessNumber + "PurchaseOrderItemTaxesCurrent POITC JOIN tbl" + cu.terminal.intBusinessNumber
                + "PurchaseOrderItemCurrent POIC ON POIC.intPurchaseOrderItemID = POITC.intPurchaseOrderItemID JOIN(SELECT intTaxTypeID, "
                + "fltTaxRate, MAX(dtmTaxEffectiveDate) AS MTD FROM tblTaxRateByProvince WHERE dtmTaxEffectiveDate <= @dtmCurrentDate AND "
                + "intProvinceID = @intProvinceID GROUP BY intTaxTypeID, fltTaxRate) TR ON TR.intTaxTypeID = POITC.intTaxTypeID WHERE "
                + "POITC.intPurchaseOrderItemID = @intPurchaseOrderItemID AND POITC.intTaxTypeID = @intTaxTypeID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItemTax.intPurchaseOrderItemID },
                new object[] { "@dtmCurrentDate",  currentDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@intProvinceID", cu.currentStoreLocation.intProvinceID },
                new object[] { "@intTaxTypeID", purchaseOrderItemTax.intTaxTypeID }
            };
            return DBC.MakeDatabaseCallToReturnDouble(sqlCmd, parms);
        }

        public double ReturnProvincialTaxTotal(List<ReceiptItemTax> riTax)
        {
            return LoopThroughTaxesForPSTTotal(riTax);
        }
        public double ReturnGovernmentTaxTotal(List<ReceiptItemTax> riTax)
        {
            return LoopThroughTaxesForGSTTotal(riTax);
        }
        public double ReturnGovernmentTaxTotalPO(List<PurchaseOrderItemTax> poiTax)
        {
            return LoopThroughTaxesForGSTTotalPO(poiTax);
        }
        public double ReturnProvincialTaxTotalPO(List<PurchaseOrderItemTax> poiTax)
        {
            return LoopThroughTaxesForPSTTotalPO(poiTax);
        }
        public double CalculateReceivingTaxAmount(PurchaseOrderItemTax purchaseOrderItemTax, DateTime currentDateTime, CurrentUser cu)
        {
            return GatherNewReceivingTaxAmount(purchaseOrderItemTax, currentDateTime, cu);
        }

        private int CheckTaxIsFromTradeIN(int businessNumber)
        {
            string sqlCmd = "SELECT COUNT(RITC.intReceiptItemID) AS intReceiptItemID FROM tbl" + businessNumber
                + "ReceiptItemTaxesCurrent RITC JOIN tbl" + businessNumber + "ReceiptItemCurrent RIC ON "
                + "RIC.intReceiptItemID = RITC.intReceiptItemID WHERE RIC.bitIsTradeIn = 1";

            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public bool ItemTaxIsFromTradeIn(int businessNumber)
        {
            bool taxIsTradeIn = false;
            int check = CheckTaxIsFromTradeIN(businessNumber);
            if (check > 0)
            {
                taxIsTradeIn = true;
            }
            return taxIsTradeIn;
        }

        private void InsertTaxesIntoReceiptItemTaxesCurrent(ReceiptItemTax tax, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemTaxesCurrent VALUES("
                + "@intReceiptItemID, @intTaxTypeID, @fltTaxAmount, @bitIsTaxCharged)";

            object[][] parms =
            {
                new object[] { "@intReceiptItemID", tax.intReceiptItemID },
                new object[] { "@intTaxTypeID", tax.intTaxTypeID },
                new object[] { "@fltTaxAmount", tax.fltTaxAmount },
                new object[] { "@bitIsTaxCharged", tax.bitIsTaxCharged }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void SaveTaxIDForNewInventoryItem(int inventoryID, int taxTypeID, bool chargeTax, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "TaxTypePerInventoryItem VALUES("
                + "@intInventoryID, @intTaxTypeID, @bitChargeTax)";
            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxTypeID", taxTypeID },
                new object[] { "@bitChargeTax", chargeTax }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertTaxesIntoPurchaseOrderItemTaxesCurrent(PurchaseOrderItemTax tax, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "PurchaseOrderItemTaxesCurrent VALUES("
                + "@intPurchaseOrderItemID, @intTaxTypeID, @fltTaxAmount, @bitIsTaxCharged)";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", tax.intPurchaseOrderItemID },
                new object[] { "@intTaxTypeID", tax.intTaxTypeID },
                new object[] { "@fltTaxAmount", tax.fltTaxAmount },
                new object[] { "@bitIsTaxCharged", tax.bitIsTaxCharged }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void UpdatePOBitIsTaxCharged(PurchaseOrderItemTax purchaseOrderItemTax, bool isTaxCharged, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "PurchaseOrderItemTaxesCurrent SET bitIsTaxCharged = "
                + "@bitIsTaxCharged WHERE intPurchaseOrderItemID = @intPurchaseOrderItemID AND intTaxTypeID = @intTaxTypeID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItemTax.intPurchaseOrderItemID },
                new object[] { "@intTaxTypeID", purchaseOrderItemTax.intTaxTypeID },
                new object[] { "@bitIsTaxCharged", isTaxCharged }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void SetTaxChargedForInventory(int businessNumber, int inventoryID, int taxTypeID, bool chargeTax)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "TaxTypePerInventoryItem SET bitChargeTax = @bitChargeTax WHERE "
                + "intInventoryID = @intInventoryID AND intTaxTypeID = @intTaxTypeID";
            object[][] parms =
            {
                new object[] { "@bitChargeTax", chargeTax },
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intTaxTypeID", taxTypeID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void InsertNewTaxRate(int provinceID, int taxID, DateTime selectedDate, double taxRate, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "TaxRateByProvince VALUES(@intProvinceID, "
                + "@intTaxTypeID, @dtmTaxEffectiveDate, @fltTaxRate)";
            object[][] parms =
            {
                new object[] { "@intProvinceID", provinceID },
                new object[] { "@intTaxTypeID", taxID },
                new object[] { "@dtmTaxEffectiveDate", selectedDate },
                new object[] { "@fltTaxRate", taxRate }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void LoopThroughTaxesForEachItemAddingToReceiptItemTaxesCurrent(ReceiptItem receiptItem, int transactionTypeID, DateTime currentDateTime, CurrentUser cu)
        {
            List<ReceiptItemTax> receiptItemTaxes = ReturnTaxesAvailableForItem(receiptItem, transactionTypeID, currentDateTime, cu);
            foreach (var tax in receiptItemTaxes)
            {
                InsertTaxesIntoReceiptItemTaxesCurrent(tax, cu.terminal.intBusinessNumber);
            }
        }
        public void SaveTaxesForNewInventory(int inventoryID, int taxTypeID, bool chargeTax, int businessNumber)
        {
            SaveTaxIDForNewInventoryItem(inventoryID, taxTypeID, chargeTax, businessNumber);
        }
        public void LoopThroughTaxesForEachItemAddingToPurchaseOrderItemTaxesCurrent(PurchaseOrderItem purchaseOrderItem, DateTime currentDateTime, CurrentUser cu)
        {
            List<PurchaseOrderItemTax> purchaseOrderItemTaxes = ReturnTaxesAvailableForPurchaseOrderItem(purchaseOrderItem, currentDateTime, cu);
            foreach (var tax in purchaseOrderItemTaxes)
            {
                InsertTaxesIntoPurchaseOrderItemTaxesCurrent(tax, cu.terminal.intBusinessNumber);
            }
        }
        public void UpdateListOfPSTPurchaseOrderTaxes(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            foreach(PurchaseOrderItemTax tax in purchaseOrder.lstPurchaseOrderItemTax)
            {
                if(tax.intTaxTypeID == 3 || tax.intTaxTypeID == 5)
                {
                    UpdatePOBitIsTaxCharged(tax, purchaseOrder.bitPSTCharged, cu.terminal.intBusinessNumber);
                }
            }
        }
        public void UpdateListOfGSTPurchaseOrderTaxes(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            foreach (PurchaseOrderItemTax tax in purchaseOrder.lstPurchaseOrderItemTax)
            {
                if (tax.intTaxTypeID == 1 || tax.intTaxTypeID == 2 || tax.intTaxTypeID == 4)
                {
                    UpdatePOBitIsTaxCharged(tax, purchaseOrder.bitGSTCharged, cu.terminal.intBusinessNumber);
                }
            }
        }
        public void UpdateTaxChargedForInventory(int businessNumber, int inventoryID, int taxTypeID, bool chargeTax)
        {
            SetTaxChargedForInventory(businessNumber, inventoryID, taxTypeID, chargeTax);
        }
    }
}