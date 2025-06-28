using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SweetPOS.ClassObjects
{
    public class PurchaseOrderManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<PurchaseOrder> ConvertFromDataTableToPurchaseOrder(DataTable dt, int businessNumber)
        {
            VendorSupplierManager VSM = new VendorSupplierManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            List<PurchaseOrder> purchaseOrder = dt.AsEnumerable().Select(row =>
            new PurchaseOrder
            {
                intPurchaseOrderID = row.Field<int>("intPurchaseOrderID"),
                intPurchaseOrderGroupID = row.Field<int>("intPurchaseOrderGroupID"),
                varPurchaseOrderNumber = row.Field<string>("varPurchaseOrderNumber"),
                dtmPurchaseOrderCreationDate = row.Field<DateTime>("dtmPurchaseOrderCreationDate"),
                dtmPurchaseOrderCreationTime = row.Field<DateTime>("dtmPurchaseOrderCreationTime"),
                dtmPurchaseOrderCompletionDate = row.Field<DateTime>("dtmPurchaseOrderCompletionDate"),
                dtmPurchaseOrderCompletionTime = row.Field<DateTime>("dtmPurchaseOrderCompletionTime"),
                vendorSupplier = VSM.ReturnVendorSupplier(row.Field<int>("intVendorSupplierID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostSubTotal = row.Field<double>("fltCostSubTotal"),
                fltGSTTotal = row.Field<double>("fltGSTTotal"),
                fltPSTTotal = row.Field<double>("fltPSTTotal"),
                bitGSTCharged = row.Field<bool>("bitGSTCharged"),
                bitPSTCharged = row.Field<bool>("bitPSTCharged"),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                lstPurchaseOrderItem = ReturnPurchaseOrderItem(row.Field<int>("intPurchaseOrderID"), businessNumber),
                lstPurchaseOrderItemTax = ReturnPurchaseOrderItemTax(row.Field<int>("intPurchaseOrderID"), businessNumber),
                varPurchaseOrderComments = row.Field<string>("varPurchaseOrderComments")
            }).ToList();
            return purchaseOrder;
        }
        private List<PurchaseOrder> ConvertFromDataTableToPurchaseOrderCurrent(DataTable dt, int businessNumber)
        {
            VendorSupplierManager VSM = new VendorSupplierManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            TaxManager TM = new TaxManager();
            List<PurchaseOrder> purchaseOrder = dt.AsEnumerable().Select(row =>
            new PurchaseOrder
            {
                intPurchaseOrderID = row.Field<int>("intPurchaseOrderID"),
                intPurchaseOrderGroupID = row.Field<int>("intPurchaseOrderGroupID"),
                varPurchaseOrderNumber = row.Field<string>("varPurchaseOrderNumber"),
                dtmPurchaseOrderCreationDate = row.Field<DateTime>("dtmPurchaseOrderCreationDate"),
                dtmPurchaseOrderCreationTime = row.Field<DateTime>("dtmPurchaseOrderCreationTime"),
                vendorSupplier = VSM.ReturnVendorSupplier(row.Field<int>("intVendorSupplierID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostSubTotal = row.Field<double>("fltCostSubTotal"),
                fltGSTTotal = row.Field<double>("fltGSTTotal"),
                fltPSTTotal = row.Field<double>("fltPSTTotal"),
                bitGSTCharged = row.Field<bool>("bitGSTCharged"),
                bitPSTCharged = row.Field<bool>("bitPSTCharged"),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                lstPurchaseOrderItem = ReturnPurchaseOrderItemCurrent(row.Field<int>("intPurchaseOrderID"), businessNumber),
                lstPurchaseOrderItemTax = TM.ReturnPurchaseOrderItemTaxesCurrentToProcessPO(row.Field<int>("intPurchaseOrderID"), businessNumber),
                lstPurchaseOrderReceivedItem = new List<PurchaseOrderItem>(),
                lstPurchaseOrderReceivedItemTax = new List<PurchaseOrderItemTax>(),
                varPurchaseOrderComments = row.Field<string>("varPurchaseOrderComments")
            }).ToList();
            foreach (PurchaseOrder po in purchaseOrder)
            {
                po.fltGSTTotal = TM.ReturnGovernmentTaxTotalPO(po.lstPurchaseOrderItemTax);
                po.fltPSTTotal = TM.ReturnProvincialTaxTotalPO(po.lstPurchaseOrderItemTax);
                po.fltCostSubTotal += po.fltGSTTotal + po.fltPSTTotal;
            }
            return purchaseOrder;
        }
        private List<PurchaseOrder> ConvertFromDataTableToPurchaseOrderReceivingCurrent(DataTable dt, DateTime currentDateTime, CurrentUser cu)
        {
            VendorSupplierManager VSM = new VendorSupplierManager();
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            TaxManager TM = new TaxManager();
            List<PurchaseOrder> purchaseOrder = dt.AsEnumerable().Select(row =>
            new PurchaseOrder
            {
                intPurchaseOrderID = row.Field<int>("intPurchaseOrderID"),
                intPurchaseOrderGroupID = row.Field<int>("intPurchaseOrderGroupID"),
                varPurchaseOrderNumber = row.Field<string>("varPurchaseOrderNumber"),
                dtmPurchaseOrderCreationDate = row.Field<DateTime>("dtmPurchaseOrderCreationDate"),
                dtmPurchaseOrderCreationTime = row.Field<DateTime>("dtmPurchaseOrderCreationTime"),
                vendorSupplier = VSM.ReturnVendorSupplier(row.Field<int>("intVendorSupplierID"), cu.terminal.intBusinessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), cu.terminal.intBusinessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), cu.terminal.intBusinessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostSubTotal = row.Field<double>("fltCostSubTotal"),
                fltGSTTotal = row.Field<double>("fltGSTTotal"),
                fltPSTTotal = row.Field<double>("fltPSTTotal"),
                bitGSTCharged = row.Field<bool>("bitGSTCharged"),
                bitPSTCharged = row.Field<bool>("bitPSTCharged"),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                lstPurchaseOrderItem = ReturnPurchaseOrderItemCurrent(row.Field<int>("intPurchaseOrderID"), cu.terminal.intBusinessNumber),
                lstPurchaseOrderItemTax = TM.ReturnPurchaseOrderItemTaxesCurrentToProcessPO(row.Field<int>("intPurchaseOrderID"), cu.terminal.intBusinessNumber),
                varPurchaseOrderComments = row.Field<string>("varPurchaseOrderComments")
            }).ToList();
            foreach (PurchaseOrder po in purchaseOrder)
            {
                foreach (PurchaseOrderItemTax poit in po.lstPurchaseOrderItemTax)
                {
                    poit.fltTaxAmount = TM.CalculateReceivingTaxAmount(poit, currentDateTime, cu);
                }
                po.fltCostSubTotal = CalculateReceivingCostSubTotal(po.lstPurchaseOrderItem);

                po.fltGSTTotal = TM.ReturnGovernmentTaxTotalPO(po.lstPurchaseOrderItemTax);
                po.fltPSTTotal = TM.ReturnProvincialTaxTotalPO(po.lstPurchaseOrderItemTax);
                po.fltCostSubTotal += po.fltGSTTotal + po.fltPSTTotal;
            }
            return purchaseOrder;
        }
        private List<PurchaseOrder> GatherReceivingPurchaseOrder(int purchaseOrderID, DateTime currentDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchaseOrderID, intPurchaseOrderGroupID, varPurchaseOrderNumber, dtmPurchaseOrderCreationDate, "
                + "CAST(dtmPurchaseOrderCreationTime AS DATETIME) AS dtmPurchaseOrderCreationTime, intVendorSupplierID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostSubTotal, fltGSTTotal, fltPSTTotal, bitGSTCharged, bitPSTCharged, intTransactionTypeID, "
                + "varPurchaseOrderComments FROM tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent WHERE intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderReceivingCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), currentDateTime, cu);
        }
        private List<PurchaseOrder> ReturnPurchaseOrderCurrentFromPurchaseOrderDescription(object[][] parms, CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchaseOrderID FROM tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent WHERE "
                + "intPurchaseOrderGroupID = @intPurchaseOrderGroupID AND varPurchaseOrderNumber = @varPurchaseOrderNumber AND "
                + "dtmPurchaseOrderCreationDate = @dtmPurchaseOrderCreationDate AND dtmPurchaseOrderCreationTime = "
                + "@dtmPurchaseOrderCreationTime AND intVendorSupplierID = @intVendorSupplierID AND intEmployeeID = @intEmployeeID "
                + "AND intStoreLocationID = @intStoreLocationID AND intTerminalID = @intTerminalID AND fltCostSubTotal = @fltCostSubTotal "
                + "AND fltGSTTotal = @fltGSTTotal AND fltPSTTotal = @fltPSTTotal AND bitGSTCharged = @bitGSTCharged AND bitPSTCharged = "
                + "@bitPSTCharged AND intTransactionTypeID = @intTransactionTypeID AND varPurchaseOrderNumber = @varPurchaseOrderNumber";

            return ReturnPurchaseOrderCurrentFromID(DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms), cu);
        }
        private List<PurchaseOrder> InsertIntoPurchaseOrderCurrent(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent VALUES(@intPurchaseOrderGroupID, "
                + "@varPurchaseOrderNumber, @dtmPurchaseOrderCreationDate, @dtmPurchaseOrderCreationTime, @intVendorSupplierID, @intEmployeeID, "
                + "@intStoreLocationID, @intTerminalID, @fltCostSubTotal, @fltGSTTotal, @fltPSTTotal, @bitGSTCharged, @bitPSTCharged, "
                + "@intTransactionTypeID, @varPurchaseOrderComments)";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderGroupID", purchaseOrder.intPurchaseOrderGroupID },
                new object[] { "@varPurchaseOrderNumber", purchaseOrder.varPurchaseOrderNumber },
                new object[] { "@dtmPurchaseOrderCreationDate", purchaseOrder.dtmPurchaseOrderCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmPurchaseOrderCreationTime", purchaseOrder.dtmPurchaseOrderCreationTime.ToString("HH:mm:ss") },
                new object[] { "@intVendorSupplierID", purchaseOrder.vendorSupplier.intVendorSupplierID },
                new object[] { "@intEmployeeID", purchaseOrder.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", purchaseOrder.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", purchaseOrder.intTerminalID },
                new object[] { "@fltCostSubTotal", purchaseOrder.fltCostSubTotal },
                new object[] { "@fltGSTTotal", purchaseOrder.fltGSTTotal },
                new object[] { "@fltPSTTotal", purchaseOrder.fltPSTTotal },
                new object[] { "@bitGSTCharged", purchaseOrder.bitGSTCharged },
                new object[] { "@bitPSTCharged", purchaseOrder.bitPSTCharged },
                new object[] { "@intTransactionTypeID", purchaseOrder.intTransactionTypeID },
                new object[] { "@varPurchaseOrderComments", purchaseOrder.varPurchaseOrderComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnPurchaseOrderCurrentFromPurchaseOrderDescription(parms, cu);
        }
        private List<PurchaseOrder> ReturnPurchaseOrderCurrentFromID(int purchaseOrderID, CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchaseOrderID, intPurchaseOrderGroupID, varPurchaseOrderNumber, dtmPurchaseOrderCreationDate, "
                + "CAST(dtmPurchaseOrderCreationTime AS DATETIME) AS dtmPurchaseOrderCreationTime, intVendorSupplierID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostSubTotal, fltGSTTotal, fltPSTTotal, bitGSTCharged, bitPSTCharged, "
                + "intTransactionTypeID, varPurchaseOrderComments FROM tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent "
                + "WHERE intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), cu.terminal.intBusinessNumber);
        }
        private List<PurchaseOrder> GatherPurchaseOrder(int purchaseOrderID, CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchaseOrderID, intPurchaseOrderGroupID, varPurchaseOrderNumber, dtmPurchaseOrderCreationDate, "
                + "CAST(dtmPurchaseOrderCreationTime AS DATETIME) AS dtmPurchaseOrderCreationTime, dtmPurchaseOrderCompletionDate, "
                + "CAST(dtmPurchaseOrderCompletionTime AS DATETIME) AS dtmPurchaseOrderCompletionTime, intVendorSupplierID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostSubTotal, fltGSTTotal, fltPSTTotal, bitGSTCharged, bitPSTCharged, "
                + "intTransactionTypeID, varPurchaseOrderComments FROM tbl" + cu.terminal.intBusinessNumber + "PurchaseOrder WHERE "
                + "intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrder(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), cu.terminal.intBusinessNumber);
        }
        private List<PurchaseOrder> UpdatePurchaseOrderInformationCurrent(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent SET intEmployeeID = @intEmployeeID, intTerminalID = "
                + "@intTerminalID, fltCostSubTotal = @fltCostSubTotal, fltGSTTotal = @fltGSTTotal, fltPSTTotal = @fltPSTTotal, bitGSTCharged = "
                + "@bitGSTCharged, bitPSTCharged = @bitPSTCharged, intTransactionTypeID = @intTransactionTypeID, varPurchaseOrderComments = "
                + "@varPurchaseOrderComments WHERE intPurchaseOrderID = @intPurchaseOrderID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID },
                new object[] { "@intEmployeeID", purchaseOrder.employee.intEmployeeID },
                new object[] { "@intTerminalID", purchaseOrder.intTerminalID },
                new object[] { "@fltCostSubTotal", purchaseOrder.fltCostSubTotal },
                new object[] { "@fltGSTTotal", purchaseOrder.fltGSTTotal },
                new object[] { "@fltPSTTotal", purchaseOrder.fltPSTTotal },
                new object[] { "@bitGSTCharged", purchaseOrder.bitGSTCharged },
                new object[] { "@bitPSTCharged", purchaseOrder.bitPSTCharged },
                new object[] { "@intTransactionTypeID", purchaseOrder.intTransactionTypeID },
                new object[] { "@varPurchaseOrderComments", purchaseOrder.varPurchaseOrderComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnPurchaseOrderCurrentFromID(purchaseOrder.intPurchaseOrderID, cu);
        }

        public List<PurchaseOrder> ReturnReceivingPurchaseOrder(int purchaseOrderID, DateTime currentDateTime, CurrentUser cu)
        {
            return GatherReceivingPurchaseOrder(purchaseOrderID, currentDateTime, cu);
        }        
        public List<PurchaseOrder> CreateNewPurchaseOrder(int vendorID, DateTime createDateTime, CurrentUser cu)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            VendorSupplierManager VSM = new VendorSupplierManager();
            object[] purchaseOrderNumberAndGroup = ReturnNextPurchaseOrderNumberForNewPurchaseOrder(cu);
            purchaseOrder.intPurchaseOrderGroupID = Convert.ToInt32(purchaseOrderNumberAndGroup[1]);
            purchaseOrder.varPurchaseOrderNumber = purchaseOrderNumberAndGroup[0].ToString();
            purchaseOrder.dtmPurchaseOrderCreationDate = createDateTime;
            purchaseOrder.dtmPurchaseOrderCreationTime = createDateTime;
            purchaseOrder.vendorSupplier = VSM.ReturnVendorSupplier(vendorID, cu.terminal.intBusinessNumber)[0];
            purchaseOrder.employee = cu.employee;
            purchaseOrder.storeLocation = cu.currentStoreLocation;
            purchaseOrder.intTerminalID = cu.terminal.intTerminalID;
            purchaseOrder.fltCostSubTotal = 0;
            purchaseOrder.fltGSTTotal = 0;
            purchaseOrder.fltPSTTotal = 0;
            purchaseOrder.bitGSTCharged = true;
            purchaseOrder.bitPSTCharged = true;
            purchaseOrder.intTransactionTypeID = 5;
            purchaseOrder.varPurchaseOrderComments = "";

            return InsertIntoPurchaseOrderCurrent(purchaseOrder, cu);
        }
        public List<PurchaseOrder> CreateNewPurchaseOrderPOSupplied(int vendorID, string suppliedPO, DateTime createDateTime, CurrentUser cu)
        {
            PurchaseOrder purchaseOrder = new PurchaseOrder();
            VendorSupplierManager VSM = new VendorSupplierManager();
            object[] purchaseOrderNumberAndGroup = ReturnNextPurchaseOrderNumberForNewPurchaseOrder(cu);
            purchaseOrder.intPurchaseOrderGroupID = Convert.ToInt32(purchaseOrderNumberAndGroup[1]);
            purchaseOrder.varPurchaseOrderNumber = suppliedPO;
            purchaseOrder.dtmPurchaseOrderCreationDate = createDateTime;
            purchaseOrder.dtmPurchaseOrderCreationTime = createDateTime;
            purchaseOrder.vendorSupplier = VSM.ReturnVendorSupplier(vendorID, cu.terminal.intBusinessNumber)[0];
            purchaseOrder.employee = cu.employee;
            purchaseOrder.storeLocation = cu.currentStoreLocation;
            purchaseOrder.intTerminalID = cu.terminal.intTerminalID;
            purchaseOrder.fltCostSubTotal = 0;
            purchaseOrder.fltGSTTotal = 0;
            purchaseOrder.fltPSTTotal = 0;
            purchaseOrder.bitGSTCharged = true;
            purchaseOrder.bitPSTCharged = true;
            purchaseOrder.intTransactionTypeID = 5;
            purchaseOrder.varPurchaseOrderComments = "";

            return InsertIntoPurchaseOrderCurrent(purchaseOrder, cu);
        }        
        public List<PurchaseOrder> ReturnPurchaseOrder(int purchaseOrderID, CurrentUser cu)
        {
            return GatherPurchaseOrder(purchaseOrderID, cu);
        }
        public List<PurchaseOrder> ReturnPurchaseOrderCurrent(int purchaseOrderID, CurrentUser cu)
        {
            return ReturnPurchaseOrderCurrentFromID(purchaseOrderID, cu);
        }
        public List<PurchaseOrder> ReturnPurchaseOrderTotals(int purchaseOrderID, CurrentUser cu)
        {
            return UpdatePurchaseOrderInformationCurrent(CalculatePurchaseOrderTotal(ReturnPurchaseOrderCurrentFromID(purchaseOrderID, cu)[0]), cu);
        }

        private PurchaseOrder CalculatePurchaseOrderTotal(PurchaseOrder purchaseOrder)
        {
            double costTotal = 0;
            foreach (PurchaseOrderItem poi in purchaseOrder.lstPurchaseOrderItem)
            {
                costTotal += poi.fltPurchaseOrderCost * poi.intPurchaseOrderQuantity;
            }

            purchaseOrder.fltCostSubTotal = costTotal;
            return purchaseOrder;
        }

        private List<PurchaseOrderItem> ReturnSelectedItemForPurchaseOrder(int vendorSupplierProductID, int businessNumber)
        {
            string sqlCmd = "SELECT VSP.intVendorSupplierProductID, I.varSku, VSP.varVendorSupplierProductCode, I.intQuantity, "
                + "I.fltAverageCost, I.varDescription FROM tbl" + businessNumber + "Inventory I JOIN tbl" + businessNumber
                + "VendorSupplierProduct VSP ON VSP.intInventoryID = I.intInventoryID WHERE VSP.intVendorSupplierProductID = "
                + "@intVendorSupplierProductID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierProductID", vendorSupplierProductID }
            };
            return ConvertFromInventoryDataTableToPurchaseOrderItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<PurchaseOrderItem> ConvertFromInventoryDataTableToPurchaseOrderItem(DataTable dt)
        {
            List<PurchaseOrderItem> purchaseOrderItem = dt.AsEnumerable().Select(row =>
            new PurchaseOrderItem
            {
                intVendorSupplierProductID = row.Field<int>("intVendorSupplierProductID"),
                varSku = row.Field<string>("varSku"),
                varVendorSku = row.Field<string>("varVendorSupplierProductCode"),
                intPurchaseOrderQuantity = row.Field<int>("intQuantity"),
                fltPurchaseOrderCost = row.Field<double>("fltAverageCost"),
                varItemDescription = row.Field<string>("varDescription")
            }).ToList();
            return purchaseOrderItem;
        }
        private List<PurchaseOrderItem> ConvertFromDataTableToPurchaseOrderItem(DataTable dt)
        {
            List<PurchaseOrderItem> purchaseOrderItem = dt.AsEnumerable().Select(row =>
            new PurchaseOrderItem
            {
                intPurchaseOrderItemID = row.Field<int>("intPurchaseOrderItemID"),
                intPurchaseOrderID = row.Field<int>("intPurchaseOrderID"),
                intVendorSupplierProductID = row.Field<int>("intVendorSupplierProductID"),
                varSku = row.Field<string>("varSku"),
                varVendorSku = row.Field<string>("varVendorSupplierProductCode"),
                intPurchaseOrderQuantity = row.Field<int>("intPurchaseOrderQuantity"),
                fltPurchaseOrderCost = row.Field<double>("fltPurchaseOrderCost"),
                intReceivedQuantity = row.Field<int>("intReceivedQuantity"),
                fltReceivedCost = row.Field<double>("fltReceivedCost"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return purchaseOrderItem;
        }
        private List<PurchaseOrderItem> ReturnPurchaseOrderItem(int purchaseOrderID, int businessNumber)
        {
            string sqlCmd = "SELECT POI.intPurchaseOrderItemID, POI.intPurchaseOrderID, POI.intVendorSupplierProductID, I.varSku, "
                + "VSP.varVendorSupplierProductCode, POI.intPurchaseOrderQuantity, POI.intReceivedQuantity, POI.fltPurchaseOrderCost, "
                + "POI.fltReceivedCost, POI.varItemDescription FROM tbl" + businessNumber + "PurchaseOrderItem POI JOIN tbl"
                + businessNumber + "VendorSupplierProduct VSP ON VSP.intVendorSupplierProductID = POI.intVendorSupplierProductID "
                + "JOIN tbl" + businessNumber + "Inventory I ON I.intInventoryID = VSP.intInventoryID WHERE "
                + "POI.intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<PurchaseOrderItem> ReturnAvailableVendorSupplierItems(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            string sqlCmd = "SELECT VSP.intVendorSupplierProductID, I.varSku, VSP.varVendorSupplierProductCode, I.intQuantity, I.fltAverageCost, "
                + "I.varDescription FROM tbl" + cu.terminal.intBusinessNumber + "Inventory I JOIN tbl" + cu.terminal.intBusinessNumber
                + "VendorSupplierProduct VSP ON VSP.intInventoryID = I.intInventoryID WHERE VSP.intVendorSupplierProductID NOT IN(SELECT "
                + "POIC.intVendorSupplierProductID FROM tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderItemCurrent POIC WHERE "
                + "intPurchaseOrderID = @intPurchaseOrderID) AND VSP.intVendorSupplierID = @intVendorSupplierID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID },
                new object[] { "@intVendorSupplierID", purchaseOrder.vendorSupplier.intVendorSupplierID }
            };
            return ConvertFromInventoryDataTableToPurchaseOrderItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<PurchaseOrderItem> ReturnPurchaseOrderItemCurrent(int purchaseOrderID, int businessNumber)
        {
            string sqlCmd = "SELECT POIC.intPurchaseOrderItemID, POIC.intPurchaseOrderID, POIC.intVendorSupplierProductID, I.varSku, "
                + "VSP.varVendorSupplierProductCode, POIC.intPurchaseOrderQuantity, POIC.intReceivedQuantity, POIC.fltPurchaseOrderCost, "
                + "POIC.fltReceivedCost, POIC.varItemDescription FROM tbl" + businessNumber + "PurchaseOrderItemCurrent POIC JOIN tbl"
                + businessNumber + "VendorSupplierProduct VSP ON VSP.intVendorSupplierProductID = POIC.intVendorSupplierProductID JOIN "
                + "tbl" + businessNumber + "Inventory I ON I.intInventoryID = VSP.intInventoryID WHERE POIC.intPurchaseOrderID = "
                + "@intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<PurchaseOrderItem> ReturnPurchaseOrderItemForPurchaseOrder(int inventoryID, int businessNumber)
        {
            return ReturnSelectedItemForPurchaseOrder(inventoryID, businessNumber);
        }
        public List<PurchaseOrderItem> ListOfAvailableVendorSupplierItems(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            return ReturnAvailableVendorSupplierItems(purchaseOrder, cu);
        }

        private List<PurchaseOrderItemTax> ReturnPurchaseOrderItemTax(int purchaseOrderID, int businessNumber)
        {
            string sqlCmd = "SELECT POIT.intPurchaseOrderItemID, POIT.intTaxTypeID, POIT.fltTaxAmount, POIT.bitIsTaxCharged FROM tbl"
                + businessNumber + "PurchaseOrderItemTaxes POIT JOIN tbl" + businessNumber + "PurchaseOrderItem POI ON "
                + "POI.intPurchaseOrderItemID = POIT.intPurchaseOrderItemID WHERE POI.intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderID }
            };
            return ConvertFromDataTableToPurchaseOrderItemTax(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<PurchaseOrderItemTax> ConvertFromDataTableToPurchaseOrderItemTax(DataTable dt)
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

        private double CalculateReceivingCostSubTotal(List<PurchaseOrderItem> purchaseOrderItems)
        {
            double costTotal = 0;
            foreach (PurchaseOrderItem poi in purchaseOrderItems)
            {
                costTotal += poi.fltReceivedCost * poi.intReceivedQuantity;
            }
            return costTotal;
        }

        private object[] ReturnNextPurchaseOrderNumberForNewPurchaseOrder(CurrentUser cu)
        {
            string sqlCmd = "SELECT intPurchaseOrderStoreLocationID, intPurchaseOrderNumberSystem, CONCAT(varStoreCodePurchaseOrder, CASE "
                + "WHEN LEN(CAST(intPurchaseOrderNumberSystem AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intPurchaseOrderNumberSystem "
                + "AS VARCHAR(6))),6) ELSE CAST(intPurchaseOrderNumberSystem AS VARCHAR(MAX)) END) AS varPurchaseOrderNumber FROM tbl" 
                + cu.terminal.intBusinessNumber + "StoredPurchaseOrderNumber WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            int purchaseOrderGroupNumber = DBC.MakeDatabaseCallToReturnSecondColumnAsInt(sqlCmd, parms) + 1;
            //Creates the new receipt number
            CreatePurchaseOrderNumberForNextPurchaseOrder(purchaseOrderGroupNumber, cu);
            //Returns the receipt number for use on new sale
            object[] purchaseOrderNumberAndGroup = { DBC.MakeDatabaseCallToReturnThirdColumnAsString(sqlCmd, parms), purchaseOrderGroupNumber };
            return purchaseOrderNumberAndGroup;
        }

        private DataTable GatherOpenPurchaseOrders(CurrentUser cu)
        {
            string sqlCmd = "SELECT POC.intPurchaseOrderID, POC.intPurchaseOrderGroupID, POC.dtmPurchaseOrderCreationDate, POC.varPurchaseOrderNumber, "
                + "VS.varVendorSupplierName, CONCAT(E.varLastName, ', ', E.varFirstName) AS varEmployeeName, POC.fltCostSubTotal FROM tbl"
                + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent POC JOIN tbl" + cu.terminal.intBusinessNumber + "VendorSupplier VS "
                + "ON VS.intVendorSupplierID = POC.intVendorSupplierID JOIN tbl" + cu.terminal.intBusinessNumber + "Employee E ON E.intEmployeeID "
                + "= POC.intEmployeeID WHERE POC.intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] {"intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnOpenPurchaseOrders(CurrentUser cu)
        {
            return GatherOpenPurchaseOrders(cu);
        }

        private void CreatePurchaseOrderNumberForNextPurchaseOrder(int purchaseOrderNumber, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "StoredPurchaseOrderNumber SET intPurchaseOrderNumberSystem "
                + "= @purchaseOrderNumber WHERE intStoreLocationID = @storeLocationID";
            object[][] parms =
            {
                new object[] { "@purchaseOrderNumber", purchaseOrderNumber },
                new object[] { "@storeLocationID", cu.currentStoreLocation.intStoreLocationID }

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void ResetPurchaseOrderCost(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            double dblCost = 0;
            foreach (PurchaseOrderItem poi in purchaseOrder.lstPurchaseOrderItem)
            {
                dblCost = poi.intPurchaseOrderQuantity * poi.fltPurchaseOrderCost;
            }
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent SET fltCostSubTotal "
                + "= @fltCostSubTotal, intTransactionTypeID = 5 WHERE intPurchaseOrderID = @intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@fltCostSubTotal", dblCost },
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void setReceivingPurchaseOrder(PurchaseOrderItem purchaseOrderItem, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "PurchaseOrderItemCurrent SET intReceivedQuantity = @intReceivedQuantity, "
                + "fltReceivedCost = @fltReceivedCost WHERE intPurchaseOrderItemID = @intPurchaseOrderItemID";
            object[][] parms =
            {
                new object[] { "@intReceivedQuantity", 0 },
                new object[] { "@fltReceivedCost", 0 },
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItem.intPurchaseOrderItemID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemovePurchaseOrderItemInformationCurrent(int purchaseOrderItemID, CurrentUser cu)
        {
            string sqlCmd = "DELETE tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderItemCurrent WHERE intPurchaseOrderItemID = "
                + "@intPurchaseOrderItemID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItemID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemovePurchaseOrderCurrent(PurchaseOrder purchaseOrder, int businessNumber)
        {
            RemovePurchaseOrderInformationCurrent(purchaseOrder, businessNumber);
        }
        private void RemovePurchaseOrderInformationCurrent(PurchaseOrder purchaseOrder, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "PurchaseOrderCurrent WHERE intPurchaseOrderID = "
                + "@intPurchaseOrderID";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void StorePurchaseOrderItemInformationCurrent(PurchaseOrderItem purchaseOrderItem, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderItemCurrent VALUES(@intPurchaseOrderID, "
                + "@intVendorSupplierProductID, @intPurchaseOrderQuantity, @intReceivedQuantity, @fltPurchaseOrderCost, @fltReceivedCost, "
                + "@varItemDescription)";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrderItem.intPurchaseOrderID },
                new object[] { "@intVendorSupplierProductID", purchaseOrderItem.intVendorSupplierProductID },
                new object[] { "@intPurchaseOrderQuantity", purchaseOrderItem.intPurchaseOrderQuantity },
                new object[] { "@intReceivedQuantity", purchaseOrderItem.intReceivedQuantity },
                new object[] { "@fltPurchaseOrderCost", purchaseOrderItem.fltPurchaseOrderCost },
                new object[] { "@fltReceivedCost", purchaseOrderItem.fltReceivedCost },
                new object[] { "@varItemDescription", purchaseOrderItem.varItemDescription }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void UpdatePurchaseOrderItemInformationCurrent(PurchaseOrderItem purchaseOrderItem, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "PurchaseOrderItemCurrent SET intReceivedQuantity = @intReceivedQuantity, "
                + "fltReceivedCost = @fltReceivedCost WHERE intPurchaseOrderItemID = @intPurchaseOrderItemID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItem.intPurchaseOrderItemID },
                new object[] { "@intReceivedQuantity", purchaseOrderItem.intReceivedQuantity },
                new object[] { "@fltReceivedCost", purchaseOrderItem.fltReceivedCost }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void StorePurchaseOrderInformation(PurchaseOrder purchaseOrder, DateTime completeDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "PurchaseOrder VALUES(@intPurchaseOrderID, @intPurchaseOrderGroupID, "
                + "@varPurchaseOrderNumber, @dtmPurchaseOrderCreationDate, @dtmPurchaseOrderCreationTime, @dtmPurchaseOrderCompletionDate, "
                + "@dtmPurchaseOrderCompletionTime, @intVendorSupplierID, @intEmployeeID, @intStoreLocationID, @intTerminalID, @fltCostSubTotal, @fltGSTTotal, "
                + "@fltPSTTotal, @bitGSTCharged, @bitPSTCharged, @intTransactionTypeID, @varPurchaseOrderComments)";
            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID },
                new object[] { "@intPurchaseOrderGroupID", purchaseOrder.intPurchaseOrderGroupID },
                new object[] { "@varPurchaseOrderNumber", purchaseOrder.varPurchaseOrderNumber },
                new object[] { "@dtmPurchaseOrderCreationDate", purchaseOrder.dtmPurchaseOrderCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmPurchaseOrderCreationTime", purchaseOrder.dtmPurchaseOrderCreationTime.ToString("HH:mm:ss") },
                new object[] { "@dtmPurchaseOrderCompletionDate", completeDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmPurchaseOrderCompletionTime", completeDateTime.ToString("HH:mm:ss") },
                new object[] { "@intVendorSupplierID", purchaseOrder.vendorSupplier.intVendorSupplierID },
                new object[] { "@intEmployeeID", purchaseOrder.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", purchaseOrder.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", purchaseOrder.intTerminalID },
                new object[] { "@fltCostSubTotal", purchaseOrder.fltCostSubTotal },
                new object[] { "@fltGSTTotal", purchaseOrder.fltGSTTotal },
                new object[] { "@fltPSTTotal", purchaseOrder.fltPSTTotal },
                new object[] { "@bitGSTCharged", purchaseOrder.bitGSTCharged },
                new object[] { "@bitPSTCharged", purchaseOrder.bitPSTCharged },
                new object[] { "@intTransactionTypeID", purchaseOrder.intTransactionTypeID },
                new object[] { "@varPurchaseOrderComments", purchaseOrder.varPurchaseOrderComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void StorePurchaseOrderItemInformation(List<PurchaseOrderItem> purchaseOrderItem, int businessNumber)
        {
            foreach (PurchaseOrderItem poi in purchaseOrderItem)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "PurchaseOrderItem VALUES(@intPurchaseOrderItemID, @intPurchaseOrderID, "
                    + "@intVendorSupplierProductID, @intPurchaseOrderQuantity, @intReceivedQuantity, @fltPurchaseOrderCost, @fltReceivedCost, @varItemDescription)";
                object[][] parms =
                {
                    new object[] { "@intPurchaseOrderItemID", poi.intPurchaseOrderItemID },
                    new object[] { "@intPurchaseOrderID", poi.intPurchaseOrderID },
                    new object[] { "@intVendorSupplierProductID", poi.intVendorSupplierProductID },
                    new object[] { "@intPurchaseOrderQuantity", poi.intPurchaseOrderQuantity },
                    new object[] { "@intReceivedQuantity", poi.intReceivedQuantity },
                    new object[] { "@fltPurchaseOrderCost", poi.fltPurchaseOrderCost },
                    new object[] { "@fltReceivedCost", poi.fltReceivedCost },
                    new object[] { "@varItemDescription", poi.varItemDescription }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            }
        }
        private void StorePurchaseOrderItemTaxesInformation(List<PurchaseOrderItemTax> purchaseOrderItemTax, int businessNumber)
        {
            foreach (PurchaseOrderItemTax poit in purchaseOrderItemTax)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "PurchaseOrderItemTaxes "
                    + "VALUES(@intPurchaseOrderItemID, @intTaxTypeID, @fltTaxAmount, @bitIsTaxCharged)";
                object[][] parms =
                {
                    new object[] { "@intPurchaseOrderItemID", poit.intPurchaseOrderItemID },
                    new object[] { "@intTaxTypeID", poit.intTaxTypeID },
                    new object[] { "@fltTaxAmount", poit.fltTaxAmount },
                    new object[] { "@bitIsTaxCharged", poit.bitIsTaxCharged }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            }
        }
        private void AddItemIntoStock(PurchaseOrderItem purchaseOrderItem, int businessNumber)
        {
            string sqlCmd = "SELECT NIC.intInventoryID, ROUND(SUM(NIC.fltTotalCost) / SUM(NIC.intReceivedQuantity), 2) AS "
                + "fltNewAverageCost FROM (SELECT intInventoryID, intReceivedQuantity, (intReceivedQuantity * "
                + "fltReceivedCost) AS fltTotalCost FROM tbl" + businessNumber + "PurchaseOrderItem POI JOIN tbl"
                + businessNumber + "VendorSupplierProduct VSP ON VSP.intVendorSupplierProductID = "
                + "POI.intVendorSupplierProductID WHERE intInventoryID = (SELECT intInventoryID FROM tbl" + businessNumber
                + "VendorSupplierProduct WHERE intVendorSupplierProductID = @intVendorSupplierProductID)) NIC GROUP BY "
                + "intInventoryID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierProductID", purchaseOrderItem.intVendorSupplierProductID }
            };
            double newCost = DBC.MakeDatabaseCallToReturnSecondColumnAsDouble(sqlCmd, parms);

            //Change cost in inventory table to new cost
            //Add additional quantity into invnetory table, can these be acomplished in a single query?

            string sqlCmd2 = "UPDATE I SET intQuantity += @intQuantity, fltAverageCost = @fltAverageCost FROM tbl"
                + businessNumber + "Inventory I INNER JOIN tbl" + businessNumber + "VendorSupplierProduct VSP ON "
                + "VSP.intInventoryID = I.intInventoryID WHERE VSP.intVendorSupplierProductID = @intVendorSupplierProductID";

            object[][] parms2 =
            {
                new object[] { "@intQuantity", purchaseOrderItem.intReceivedQuantity },
                new object[] { "@fltAverageCost", newCost },
                new object[] { "@intVendorSupplierProductID", purchaseOrderItem.intVendorSupplierProductID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd2, parms2);
        }
        private void UpdatePurchaseOrderInformationCurrentNonReturn(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "PurchaseOrderCurrent SET intEmployeeID = @intEmployeeID, intTerminalID "
                + "= @intTerminalID, fltCostSubTotal = @fltCostSubTotal, fltGSTTotal = @fltGSTTotal, fltPSTTotal = @fltPSTTotal, bitGSTCharged = "
                + "@bitGSTCharged, bitPSTCharged = @bitPSTCharged, intTransactionTypeID = @intTransactionTypeID, varPurchaseOrderComments = "
                + "@varPurchaseOrderComments WHERE intPurchaseOrderID = @intPurchaseOrderID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID },
                new object[] { "@intEmployeeID", purchaseOrder.employee.intEmployeeID },
                new object[] { "@fltCostSubTotal", purchaseOrder.fltCostSubTotal },
                new object[] { "@intTerminalID", purchaseOrder.intTerminalID },
                new object[] { "@fltGSTTotal", purchaseOrder.fltGSTTotal },
                new object[] { "@fltPSTTotal", purchaseOrder.fltPSTTotal },
                new object[] { "@bitGSTCharged", purchaseOrder.bitGSTCharged },
                new object[] { "@bitPSTCharged", purchaseOrder.bitPSTCharged },
                new object[] { "@intTransactionTypeID", purchaseOrder.intTransactionTypeID },
                new object[] { "@varPurchaseOrderComments", purchaseOrder.varPurchaseOrderComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemFromPurchaseOrderItemCurrent(PurchaseOrder purchaseOrder, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "PurchaseOrderItemCurrent WHERE intPurchaseOrderID = "
                + "@intPurchaseOrderID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderID", purchaseOrder.intPurchaseOrderID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemFromPurchaseOrderItemTaxesCurrent(int purchaseOrderItemID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "PurchaseOrderItemTaxesCurrent WHERE intPurchaseOrderItemID = "
                + "@intPurchaseOrderItemID";

            object[][] parms =
            {
                new object[] { "@intPurchaseOrderItemID", purchaseOrderItemID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void ResetReceivingPurchaseOrderItem(PurchaseOrderItem purchaseOrderItem, CurrentUser cu)
        {
            setReceivingPurchaseOrder(purchaseOrderItem, cu.terminal.intBusinessNumber);
        }
        public void ResetReceivingPurchaseOrder(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            foreach (PurchaseOrderItem poi in purchaseOrder.lstPurchaseOrderItem)
            {
                ResetReceivingPurchaseOrderItem(poi, cu);
            }
            ResetPurchaseOrderCost(purchaseOrder, cu);
        }
        public void RemoveSelectedItemFromPurchaseOrderCart(int purchaseOrderItemID, CurrentUser cu)
        {
            RemoveItemFromPurchaseOrderItemTaxesCurrent(purchaseOrderItemID, cu.terminal.intBusinessNumber);
            RemovePurchaseOrderItemInformationCurrent(purchaseOrderItemID, cu);
        }
        public void AddItemIntoPurchaseOrderCart(PurchaseOrderItem purchaseOrderItem, DateTime currentDateTime, CurrentUser cu)
        {
            StorePurchaseOrderItemInformationCurrent(purchaseOrderItem, cu);
            TaxManager TM = new TaxManager();
            TM.LoopThroughTaxesForEachItemAddingToPurchaseOrderItemTaxesCurrent(purchaseOrderItem, currentDateTime, cu);
        }
        public void UpdateItemIntoPurchaseOrderCartReceiving(PurchaseOrderItem purchaseOrderItem, int businessNumber)
        {
            UpdatePurchaseOrderItemInformationCurrent(purchaseOrderItem, businessNumber);
        }
        public void SavePurchaseOrderInformationCurrentNonReturn(PurchaseOrder purchaseOrder, CurrentUser cu)
        {
            UpdatePurchaseOrderInformationCurrentNonReturn(purchaseOrder, cu);
        }
        public void RemovePurchaseOrderFromCurrentTable(PurchaseOrder purchaseOrder, int businessNumber)
        {
            foreach (PurchaseOrderItem poi in purchaseOrder.lstPurchaseOrderItem)
            {
                RemoveItemFromPurchaseOrderItemTaxesCurrent(poi.intPurchaseOrderItemID, businessNumber);
            }
            RemoveItemFromPurchaseOrderItemCurrent(purchaseOrder, businessNumber);
            RemovePurchaseOrderCurrent(purchaseOrder, businessNumber);
        }
        public void FinalizePurchaseOrder(PurchaseOrder po, DateTime completeDateTime, int businessNumber)
        {
            //Step 1: Save New Purchase Order to the Final Purchase Order Table
            StorePurchaseOrderInformation(po, completeDateTime, businessNumber);

            //Step 2: Save New Purchase Order Items to the Final Purchase Order Items Table
            StorePurchaseOrderItemInformation(po.lstPurchaseOrderItem, businessNumber);

            //Step 3: Save New Purchase Order Items Tax to the Final Purchase Order Items Tax Table
            StorePurchaseOrderItemTaxesInformation(po.lstPurchaseOrderItemTax, businessNumber);

            //Step 4
            foreach (PurchaseOrderItem poi in po.lstPurchaseOrderItem)
            {
                //Step 4.1: Add ech item into stock and calculate new average cost for each
                AddItemIntoStock(poi, businessNumber);
            }

            //Step 5: Remove Purchase Order Items Tax from the Current Purchase Order Items Tax Table
            foreach (PurchaseOrderItemTax poit in po.lstPurchaseOrderItemTax)
            {
                RemoveItemFromPurchaseOrderItemTaxesCurrent(poit.intPurchaseOrderItemID, businessNumber);
            }

            //Step 6: Remove Purchase Order Items from the Current Purchase Order Items Table
            RemoveItemFromPurchaseOrderItemCurrent(po, businessNumber);

            //Step 7: Remove Purchase Order from the Current Purchase Order Table
            RemovePurchaseOrderInformationCurrent(po, businessNumber);
        }
    }
}