using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SweetPOS.ClassObjects
{
    public class ReceiptManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Receipt> ConvertFromDataTableToReceipt(DataTable dt, int businessNumber)
        {
            EmployeeManager EM = new EmployeeManager();
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            TaxManager TM = new TaxManager();
            List<Receipt> receipt = dt.AsEnumerable().Select(row =>
            new Receipt
            {                
                intReceiptID = row.Field<int>("intReceiptID"),
                intReceiptGroupID = row.Field<int>("intReceiptGroupID"),
                varReceiptNumber = row.Field<string>("varReceiptNumber"),
                intReceiptSubNumber = row.Field<int>("intReceiptSubNumber"),
                dtmReceiptCompletionDate = row.Field<DateTime>("dtmReceiptCompletionDate"),
                dtmReceiptCompletionTime = row.Field<DateTime>("dtmReceiptCompletionTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostTotal = row.Field<double>("fltCostTotal"),
                fltCartTotal = row.Field<double>("fltCartTotal"),
                fltDiscountTotal = row.Field<double>("fltDiscountTotal"),
                fltTradeInTotal = row.Field<double>("fltTradeInTotal"),
                fltShippingTotal = row.Field<double>("fltShippingTotal"),
                lstReceiptItem = ReturnInventoryFromReceiptItem(row.Field<int>("intReceiptID"), businessNumber),
                fltBalanceDueTotal = (row.Field<double>("fltCartTotal") - row.Field<double>("fltDiscountTotal") + row.Field<double>("fltTradeInTotal")) + row.Field<double>("fltShippingTotal"),
                lstReceiptItemTax = TM.ReturnReceiptItemTaxes(row.Field<int>("intReceiptID"), businessNumber), 
                //bitIsGSTCharged
                //bitIsPSTCharged
                lstReceiptPayment = ReturnReceiptPayment(row.Field<int>("intReceiptID"), businessNumber),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varReceiptComments = row.Field<string>("varReceiptComments"),
                fltTenderedAmount = row.Field<double>("fltTenderedAmount"),
                fltChangeAmount = row.Field<double>("fltChangeAmount")
            }).ToList();
            foreach(Receipt r in receipt)
            {
                r.fltGovernmentTaxTotal = TM.ReturnGovernmentTaxTotal(r.lstReceiptItemTax);
                r.fltProvincialTaxTotal = TM.ReturnProvincialTaxTotal(r.lstReceiptItemTax);
                r.fltBalanceDueTotal += r.fltGovernmentTaxTotal + r.fltProvincialTaxTotal;
            }
            return receipt;
        }
        private List<Receipt> ConvertFromDataTableToReceiptCurrent(DataTable dt, int businessNumber)
        {
            EmployeeManager EM = new EmployeeManager();
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            TaxManager TM = new TaxManager();
            List<Receipt> receipt = dt.AsEnumerable().Select(row =>
            new Receipt
            {
                intReceiptID = row.Field<int>("intReceiptID"),
                intReceiptGroupID = row.Field<int>("intReceiptGroupID"),
                varReceiptNumber = row.Field<string>("varReceiptNumber"),
                intReceiptSubNumber = row.Field<int>("intReceiptSubNumber"),
                dtmReceiptCreationDate = row.Field<DateTime>("dtmReceiptCreationDate"),
                dtmReceiptCreationTime = row.Field<DateTime>("dtmReceiptCreationTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostTotal = row.Field<double>("fltCostTotal"),
                fltCartTotal = row.Field<double>("fltCartTotal"),
                fltDiscountTotal = row.Field<double>("fltDiscountTotal"),
                fltTradeInTotal = row.Field<double>("fltTradeInTotal"),
                fltShippingTotal = row.Field<double>("fltShippingTotal"),
                lstReceiptItem = ReturnInventoryInSaleCart(row.Field<int>("intReceiptID"), businessNumber),
                lstReceiptPayment = ReturnPaymentInSaleCart(row.Field<int>("intReceiptID"), businessNumber),
                lstReceiptItemTax = TM.ReturnReceiptItemTaxesCurrentToProcessSale(row.Field<int>("intReceiptID"), businessNumber),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varReceiptComments = row.Field<string>("varReceiptComments"),
                
                fltTenderedAmount = row.Field<double>("fltTenderedAmount"),
                fltChangeAmount = row.Field<double>("fltChangeAmount")
            }).ToList();
            foreach (Receipt r in receipt)
            {
                r.fltGovernmentTaxTotal = TM.ReturnGovernmentTaxTotal(r.lstReceiptItemTax);
                r.fltProvincialTaxTotal = TM.ReturnProvincialTaxTotal(r.lstReceiptItemTax);
                r.fltBalanceDueTotal += r.fltGovernmentTaxTotal + r.fltProvincialTaxTotal;
            }
            return receipt;
        }
        private List<Receipt> ConvertFromDataTableReceiptListByCustomer(DataTable dt, int businessNumber)
        {
            EmployeeManager EM = new EmployeeManager();
            LocationManager LM = new LocationManager();
            TaxManager TM = new TaxManager();
            List<Receipt> receipt = dt.AsEnumerable().Select(row =>
            new Receipt
            {
                intReceiptID = row.Field<int>("intReceiptID"),
                intReceiptGroupID = row.Field<int>("intReceiptGroupID"),
                varReceiptNumber = row.Field<string>("varReceiptNumber"),
                intReceiptSubNumber = row.Field<int>("intReceiptSubNumber"),
                dtmReceiptCompletionDate = row.Field<DateTime>("dtmReceiptCompletionDate"),
                dtmReceiptCompletionTime = row.Field<DateTime>("dtmReceiptCompletionTime"),
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostTotal = row.Field<double>("fltCostTotal"),
                fltCartTotal = row.Field<double>("fltCartTotal"),
                fltDiscountTotal = row.Field<double>("fltDiscountTotal"),
                fltTradeInTotal = row.Field<double>("fltTradeInTotal"),
                fltShippingTotal = row.Field<double>("fltShippingTotal"),
                fltBalanceDueTotal= row.Field<double>("fltCartTotal") - (row.Field<double>("fltDiscountTotal") + row.Field<double>("fltTradeInTotal")) + row.Field<double>("fltShippingTotal"),
                lstReceiptItemTax = TM.ReturnReceiptItemTaxes(row.Field<int>("intReceiptID"), businessNumber),

                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varReceiptComments = row.Field<string>("varReceiptComments"),
                fltTenderedAmount = row.Field<double>("fltTenderedAmount"),
                fltChangeAmount = row.Field<double>("fltChangeAmount")
            }).ToList();
            foreach (Receipt r in receipt)
            {
                r.fltGovernmentTaxTotal = TM.ReturnGovernmentTaxTotal(r.lstReceiptItemTax);
                r.fltProvincialTaxTotal = TM.ReturnProvincialTaxTotal(r.lstReceiptItemTax);
            }
            return receipt;
        }
        private List<Receipt> InsertIntoReceiptCurrent(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptCurrent VALUES(@intReceiptGroupID, @varReceiptNumber, @intReceiptSubNumber, "
                + "@dtmReceiptCreationDate, @dtmReceiptCreationTime, @intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, "
                + "@fltCostTotal, @fltCartTotal, @fltDiscountTotal, @fltTradeInTotal, @fltShippingTotal, @intTransactionTypeID, @varReceiptComments, "
                + "@fltTenderedAmount, @fltChangeAmount)";

            object[][] parms =
            {
                new object[] { "@intReceiptGroupID", receipt.intReceiptGroupID },
                new object[] { "@varReceiptNumber", receipt.varReceiptNumber },
                new object[] { "@intReceiptSubNumber", receipt.intReceiptSubNumber },
                new object[] { "@dtmReceiptCreationDate", receipt.dtmReceiptCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReceiptCreationTime", receipt.dtmReceiptCreationTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intEmployeeID", receipt.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", receipt.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", receipt.intTerminalID },
                new object[] { "@fltCostTotal", receipt.fltCostTotal },
                new object[] { "@fltCartTotal", receipt.fltCartTotal },
                new object[] { "@fltDiscountTotal", receipt.fltDiscountTotal },
                new object[] { "@fltTradeInTotal", receipt.fltTradeInTotal },
                new object[] { "@fltShippingTotal", receipt.fltShippingTotal },
                new object[] { "@intTransactionTypeID", receipt.intTransactionTypeID },
                new object[] { "@varReceiptComments", receipt.varReceiptComments },
                new object[] { "@fltTenderedAmount", receipt.fltTenderedAmount },
                new object[] { "@fltChangeAmount", receipt.fltChangeAmount }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnNewReceiptFromReceiptNumber(parms, businessNumber);
        }
        private List<Receipt> ReturnNewReceiptFromReceiptNumber(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID FROM tbl" + businessNumber + "ReceiptCurrent WHERE intReceiptGroupID = @intReceiptGroupID AND "
                + "varReceiptNumber = @varReceiptNumber AND intReceiptSubNumber = @intReceiptSubNumber AND dtmReceiptCreationDate = "
                + "@dtmReceiptCreationDate AND dtmReceiptCreationTime = @dtmReceiptCreationTime AND intCustomerID = @intCustomerID  AND "
                + "intEmployeeID = @intEmployeeID AND intStoreLocationID = @intStoreLocationID AND intTerminalID = @intTerminalID AND "
                + "fltCostTotal = @fltCostTotal AND fltCartTotal = @fltCartTotal AND fltDiscountTotal = @fltDiscountTotal AND fltTradeInTotal "
                + "= @fltTradeInTotal AND fltShippingTotal = @fltShippingTotal AND intTransactionTypeID = @intTransactionTypeID AND "
                + "varReceiptComments = @varReceiptComments AND fltTenderedAmount = @fltTenderedAmount AND fltChangeAmount = @fltChangeAmount";
            return ReturnReceiptCurrent(DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms), businessNumber);
        }

        public List<Receipt> ReturnReceipt(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCompletionDate, "
                + "CAST(dtmReceiptCompletionTime AS DATETIME) AS dtmReceiptCompletionTime, intCustomerID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostTotal, fltCartTotal, fltShippingTotal, fltDiscountTotal, fltTradeInTotal, "
                + "intTransactionTypeID, varReceiptComments, fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber + "Receipt "
                + "WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceipt(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Receipt> ReturnReceiptCurrent(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCreationDate, "
                + "CAST(dtmReceiptCreationTime AS DATETIME) AS dtmReceiptCreationTime, intCustomerID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostTotal, fltCartTotal, fltShippingTotal, fltDiscountTotal, fltTradeInTotal, "
                + "intTransactionTypeID, varReceiptComments, fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber 
                + "ReceiptCurrent WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Receipt> CreateNewSaleReceipt(int customerID, DateTime createDateTime, CurrentUser cu)
        {
            Receipt receipt = new Receipt();
            CustomerManager CM = new CustomerManager();
            object[] receiptNumberAndGroup = ReturnNextReceiptNumberForNewReceipt(cu);
            receipt.intReceiptGroupID = Convert.ToInt32(receiptNumberAndGroup[1]);
            receipt.varReceiptNumber = receiptNumberAndGroup[0].ToString();
            receipt.intReceiptSubNumber = 1;
            receipt.dtmReceiptCreationDate = createDateTime;
            receipt.dtmReceiptCreationTime = createDateTime;
            receipt.customer = CM.ReturnCustomer(customerID, cu.terminal.intBusinessNumber)[0];
            receipt.employee = cu.employee;
            receipt.storeLocation = cu.currentStoreLocation;
            receipt.intTerminalID = cu.terminal.intTerminalID;
            receipt.fltCostTotal = 0;
            receipt.fltCartTotal = 0;
            receipt.fltDiscountTotal = 0;
            receipt.fltTradeInTotal = 0;
            receipt.fltShippingTotal = 0;
            receipt.intTransactionTypeID = 1;
            receipt.varReceiptComments = "";
            receipt.fltTenderedAmount = 0;
            receipt.fltChangeAmount = 0;
            return InsertIntoReceiptCurrent(receipt, cu.terminal.intBusinessNumber);
        }
        public List<Receipt> ReturnReceiptByCustomer(int customerID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCompletionDate, "
                + "CAST(dtmReceiptCompletionTime AS DATETIME) AS dtmReceiptCompletionTime, intCustomerID, intEmployeeID, "
                + "intStoreLocationID, intTerminalID, fltCostTotal, fltCartTotal, fltDiscountTotal, fltTradeInTotal, fltShippingTotal, "
                + "intTransactionTypeID, varReceiptComments, fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber + "Receipt "
                + "WHERE intCustomerID = @intCustomerID";
            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID }
            };
            return ConvertFromDataTableReceiptListByCustomer(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Receipt> ReturnReceiptBasedOnSearchCriteria(DateTime startDate, DateTime endDate, string searchText, int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCompletionDate, "
                + "CAST(dtmReceiptCompletionTime AS DATETIME) AS dtmReceiptCompletionTime, intCustomerID, intEmployeeID, intStoreLocationID, "
                + "intTerminalID, fltCostTotal, fltCartTotal, fltDiscountTotal, fltTradeInTotal, fltShippingTotal, intTransactionTypeID, varReceiptComments, "
                + "fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber + "Receipt WHERE(varReceiptNumber LIKE '%" + searchText + "%' OR ";
            if (searchText != "")
            {
                ArrayList strText = new ArrayList();
                for (int i = 0; i < searchText.Split(' ').Length; i++)
                {
                    strText.Add(searchText.Split(' ')[i]);
                }
                sqlCmd += "intReceiptID IN (SELECT DISTINCT intReceiptID FROM tbl" + businessNumber + "ReceiptItem WHERE intInventoryID IN(";
                sqlCmd += ReturnInventorySearchString(strText, businessNumber) + "))) AND intStoreLocationID = @storeLocationID";
            }
            else
            {
                sqlCmd += " dtmReceiptCompletionDate BETWEEN '" + startDate.ToShortDateString() + "' AND '" 
                    + endDate.ToShortDateString() + "') AND intStoreLocationID = @storeLocationID";
            }
            object[][] parms =
            {
                new object[] { "@storeLocationID", storeLocationID }
            };
            return ConvertFromDataTableToReceipt(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Receipt> CalculateNewReceiptTotalsToUpdate(int receiptID, int businessNumber)
        {
            ReceiptCalculationManager RCM = new ReceiptCalculationManager();
            //calculate subTotal, discountAmount, tradeinAmount, governmentTax, provincialTax, balanceDue
            UpdateReceiptCurrent(RCM.CalculateNewReceiptTotals(ReturnReceiptCurrent(receiptID, businessNumber)[0]), businessNumber);
            return ReturnReceiptCurrent(receiptID, businessNumber);
        }
        public List<Receipt> CreateNewReturnReceipt(Receipt receipt, DateTime createDateTime, CurrentUser CU)
        {
            //Create new sub nubnumber and save totals
            receipt.intReceiptSubNumber = CalculateNextReceiptSubNum(receipt.varReceiptNumber, CU.terminal.intBusinessNumber);
            receipt.dtmReceiptCreationDate = createDateTime;
            receipt.dtmReceiptCreationTime = createDateTime;
            receipt.fltShippingTotal = 0;
            receipt.fltCartTotal = 0;
            receipt.fltDiscountTotal = 0;
            receipt.fltTradeInTotal = 0;
            receipt.fltCostTotal = 0;
            receipt.storeLocation = CU.currentStoreLocation;
            receipt.employee = CU.employee;
            receipt.intTerminalID = CU.terminal.intTerminalID;
            receipt.intTransactionTypeID = 2;
            receipt.varReceiptComments = "";
            receipt.fltTenderedAmount = 0;
            receipt.fltChangeAmount = 0;
            return InsertIntoReceiptCurrent(receipt, CU.terminal.intBusinessNumber);
        }
        public List<Receipt> ReturnReceiptCurrentByReceiptNumber(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCreationDate, "
                + "CAST(dtmReceiptCreationTime AS DATETIME) AS dtmReceiptCreationTime, intCustomerID, intEmployeeID, intStoreLocationID, "
                + "intTerminalID, fltCostTotal, fltCartTotal, fltDiscountTotal, fltTradeInTotal, fltShippingTotal, intTransactionTypeID, "
                + "varReceiptComments, fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber + "ReceiptCurrent WHERE "
                + "intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            return ConvertFromDataTableToReceiptCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Receipt> ReturnReceiptForRemovingUnProcessedReturns(int storeLocationID, DateTime selectedDate, int businessNumber)
        {
            string sqlCmd = "SELECT intReceiptID, intReceiptGroupID, varReceiptNumber, intReceiptSubNumber, dtmReceiptCreationDate, "
                + "CAST(dtmReceiptCreationTime AS DATETIME) AS dtmReceiptCreationTime, intCustomerID, intEmployeeID, intStoreLocationID, "
                + "intTerminalID, fltCostTotal, fltCartTotal, fltDiscountTotal, fltTradeInTotal, fltShippingTotal, intTransactionTypeID, "
                + "varReceiptComments, fltTenderedAmount, fltChangeAmount FROM tbl" + businessNumber + "ReceiptCurrent WHERE "
                + "R.dtmReceiptCreationDate = @selectedDate AND R.intStoreLocationID = @intStoreLocationID AND R.intTransactionTypeID > 1";
            object[][] parms =
            {
                new object[] { "@selectedDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            return ConvertFromDataTableToReceiptCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }

        private List<ReceiptItem> ConvertFromDataTableToReceiptItem(DataTable dt)
        {
            List<ReceiptItem> receiptItem = dt.AsEnumerable().Select(row =>
            new ReceiptItem
            {
                //intReceiptItemID = row.Field<int>("intReceiptItemID"),
                //intReceiptID = row.Field<int>("intReceiptID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemAverageCostAtSale = row.Field<double>("fltItemAverageCostAtSale"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                fltItemRefund = row.Field<double>("fltItemRefund"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsTradeIn = row.Field<bool>("bitIsTradeIn"),
                bitIsPercentageDiscount = row.Field<bool>("bitIsPercentageDiscount"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return receiptItem;
        }
        private List<ReceiptItem> ConvertFromDataTableToAvaiableToReturnItems(DataTable dt, int receiptGroupID, int businessNumber)
        {
            List<ReceiptItem> receiptItem = dt.AsEnumerable().Select(row =>
            new ReceiptItem
            {
                intInventoryID = row.Field<int>("intInventoryID"),
                intItemQuantity = row.Field<int>("intItemQuantity")
            }).ToList();
            foreach(ReceiptItem ri in receiptItem)
            {
                ReceiptItem temp = GatherRemainingReturnItemInformation(ri, receiptGroupID, businessNumber);
                ri.varSku = temp.varSku;
                ri.fltItemPrice = temp.fltItemPrice;
                ri.fltItemDiscount = temp.fltItemDiscount;
                ri.bitIsNonStockedProduct = temp.bitIsNonStockedProduct;
                ri.bitIsRegularProduct = temp.bitIsRegularProduct;
                ri.bitIsTradeIn = temp.bitIsTradeIn;
                ri.bitIsPercentageDiscount = temp.bitIsPercentageDiscount;
                ri.varItemDescription = temp.varItemDescription;
            }

            return receiptItem;
        }
        private List<ReceiptItem> ConvertFromDataTableToReceiptItemPostSale(DataTable dt)
        {
            List<ReceiptItem> receiptItem = dt.AsEnumerable().Select(row =>
            new ReceiptItem
            {
                intReceiptItemID = row.Field<int>("intReceiptItemID"),
                intReceiptID = row.Field<int>("intReceiptID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemAverageCostAtSale = row.Field<double>("fltItemAverageCostAtSale"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                fltItemRefund = row.Field<double>("fltItemRefund"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsTradeIn = row.Field<bool>("bitIsTradeIn"),
                bitIsPercentageDiscount = row.Field<bool>("bitIsPercentageDiscount"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return receiptItem;
        }
        private List<ReceiptItem> ConvertFromDataTableToReceiptItemCurrent(DataTable dt)
        {
            List<ReceiptItem> receiptItem = dt.AsEnumerable().Select(row =>
            new ReceiptItem
            {
                intReceiptItemID = row.Field<int>("intReceiptItemID"),
                intReceiptID = row.Field<int>("intReceiptID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemAverageCostAtSale = row.Field<double>("fltItemAverageCostAtSale"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                fltItemRefund = row.Field<double>("fltItemRefund"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsTradeIn = row.Field<bool>("bitIsTradeIn"),
                bitIsPercentageDiscount = row.Field<bool>("bitIsPercentageDiscount"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return receiptItem;
        }
        private List<ReceiptItem> ReturnInventoryInSaleCart(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT RIC.intReceiptItemID, RIC.intReceiptID, RIC.intInventoryID, I.varSku, RIC.intItemQuantity, RIC.fltItemAverageCostAtSale, "
                + "RIC.fltItemPrice, RIC.fltItemDiscount, RIC.fltItemRefund, RIC.bitIsPercentageDiscount, RIC.bitIsNonStockedProduct, RIC.bitIsRegularProduct, "
                + "RIC.bitIsTradeIn, RIC.varItemDescription FROM tbl" + businessNumber + "ReceiptItemCurrent RIC JOIN tbl" + businessNumber + "Inventory I ON "
                + "I.intInventoryID = RIC.intInventoryID WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptItemCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptItem> ReturnInventoryFromReceiptItem(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT RIC.intReceiptItemID, RIC.intReceiptID, RIC.intInventoryID, I.varSku, RIC.intItemQuantity, RIC.fltItemAverageCostAtSale, "
                + "RIC.fltItemPrice, RIC.fltItemDiscount, RIC.fltItemRefund, RIC.bitIsPercentageDiscount, RIC.bitIsNonStockedProduct, RIC.bitIsRegularProduct, "
                + "RIC.bitIsTradeIn, RIC.varItemDescription FROM tbl" + businessNumber + "ReceiptItem RIC JOIN tbl" + businessNumber + "Inventory I ON "
                + "I.intInventoryID = RIC.intInventoryID WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptItemPostSale(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptItem> ReturnReceiptItemSearchQueryString(string searchText, int businessNumber)
        {
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT I.intInventoryID, I.varSku, CONCAT(B.varBrandName, ' ', I.varModelName, ' ', I.varDescription, "
                        + "' ', I.varAdditionalInformation) AS varItemDescription, I.intQuantity AS intItemQuantity, I.fltPrice AS "
                        + "fltItemPrice, I.fltAverageCost AS fltItemAverageCostAtSale, CAST(0 AS FLOAT) AS fltItemDiscount, CAST(0 "
                        + "AS FLOAT) AS fltItemRefund, CAST(0 AS BIT) AS bitIsPercentageDiscount, CAST(0 AS BIT) AS bitIsTradeIn, "
                        + "I.bitIsNonStockedProduct, I.bitIsRegularProduct FROM tbl" + businessNumber + "Inventory I JOIN tbl" 
                        + businessNumber + "Brand B ON B.intBrandID = I.intBrandID WHERE(I.intBrandID IN(SELECT intBrandID FROM tbl" 
                        + businessNumber + "Brand WHERE varBrandName LIKE @parm1" + i + ") OR CONCAT(varSku, varModelName, "
                        + "varDescription, varUPCcode) LIKE @parm1" + i + ") AND((intQuantity > 0 AND bitIsRegularProduct = 1) OR "
                        + "(intQuantity = 0 AND bitIsNonStockedProduct = 1)) AND intInventoryID NOT IN (SELECT intInventoryID FROM "
                        + "tbl" + businessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0) ";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
                else
                {
                    sqlCmd += "INTERSECT (SELECT I.intInventoryID, I.varSku, CONCAT(B.varBrandName, ' ', I.varModelName, ' ', I.varDescription, "
                        + "' ', I.varAdditionalInformation) AS varItemDescription, I.intQuantity AS intItemQuantity, I.fltPrice AS fltItemPrice, "
                        + "I.fltAverageCost AS fltItemAverageCostAtSale, CAST(0 AS FLOAT) AS fltItemDiscount, CAST(0 AS FLOAT) AS fltItemRefund, "
                        + "CAST(0 AS BIT) AS bitIsPercentageDiscount, CAST(0 AS BIT) AS bitIsTradeIn, I.bitIsNonStockedProduct, "
                        + "I.bitIsRegularProduct FROM tbl" + businessNumber + "Inventory I JOIN tbl" + businessNumber + "Brand B ON B.intBrandID "
                        + "= I.intBrandID WHERE(I.intBrandID IN(SELECT intBrandID FROM tbl" + businessNumber + "Brand WHERE varBrandName LIKE "
                        + "@parm1" + i + ") OR CONCAT(varSku, varModelName, varDescription, varUPCcode) LIKE @parm1" + i + ") AND((intQuantity > "
                        + "0 AND bitIsRegularProduct = 1) OR (intQuantity = 0 AND bitIsNonStockedProduct = 1)) AND intInventoryID NOT IN (SELECT "
                        + "intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE bitIsProcessedIntoInventory = 0)) ";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
            }
            sqlCmd += "ORDER BY intInventoryID DESC";
            return ConvertFromDataTableToReceiptItem(DBC.MakeDatabaseCallToReturnDataTableFromArrayListTwo(sqlCmd, parms, strText));
        }
        private List<ReceiptItem> ReturnItemFromReceiptCurrentTable(int inventoryID, int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT RIC.intReceiptItemID, RIC.intReceiptID, RIC.intInventoryID, I.varSku, RIC.intItemQuantity, "
                + "RIC.fltItemAverageCostAtSale, RIC.fltItemPrice, RIC.fltItemDiscount, RIC.fltItemRefund, RIC.bitIsPercentageDiscount, "
                + "RIC.bitIsNonStockedProduct, RIC.bitIsRegularProduct, RIC.bitIsTradeIn, RIC.varItemDescription FROM tbl" 
                + businessNumber + "ReceiptItemCurrent RIC JOIN tbl" + businessNumber + "Inventory I ON I.intInventoryID = "
                + "RIC.intInventoryID WHERE intReceiptID = @intReceiptID AND RIC.intInventoryID = @intInventoryID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID },
                new object[] { "@intInventoryID", inventoryID }
            };
            return ConvertFromDataTableToReceiptItemCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<ReceiptItem> ReturnTradeInSkuForLocation(CurrentUser cu)
        {
            string sqlCmd = "SELECT I.intInventoryID, I.varSku, CONCAT(B.varBrandName, ' ', I.varModelName, ' ', I.varDescription, ' ', "
                + "I.varAdditionalInformation) AS varItemDescription, I.intQuantity AS intItemQuantity, I.fltPrice AS fltItemPrice, "
                + "I.fltAverageCost AS fltItemAverageCostAtSale, CAST(0 AS FLOAT) AS fltItemDiscount, CAST(0 AS FLOAT) AS fltItemRefund, "
                + "CAST(0 AS BIT) AS bitIsPercentageDiscount, CAST(1 AS BIT) AS bitIsTradeIn, I.bitIsNonStockedProduct, I.bitIsRegularProduct "
                + "FROM tbl" + cu.terminal.intBusinessNumber + "Inventory I JOIN tbl" + cu.terminal.intBusinessNumber + "Brand B ON "
                + "B.intBrandID = I.intBrandID JOIN tbl" + cu.terminal.intBusinessNumber + "TradeInSkuPerLocation TIS ON TIS.intInventoryID = "
                + "I.intInventoryID WHERE TIS.intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return ConvertFromDataTableToReceiptItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<ReceiptItem> ReturnReceiptItemFromSearchString(string searchText, int businessNumber)
        {
            return ReturnReceiptItemSearchQueryString(searchText, businessNumber);
        }
        public List<ReceiptItem> ReturnReceiptItemFromReceiptCurrentTable(int inventoryID, int receiptID, int businessNumber)
        {
            return ReturnItemFromReceiptCurrentTable(inventoryID, receiptID, businessNumber);
        }
        public List<ReceiptItem> ReturnReceiptItemsFromProcessedSalesForReturn(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "SELECT RI2.intInventoryID, (SUM(DISTINCT RI2.intFirstSaleQuantity) - ((CASE WHEN SUM(RIR.intReturnQuantity) IS NULL "
                + "OR SUM(RIR.intReturnQuantity) = '' THEN 0 ELSE SUM(RIR.intReturnQuantity) END) +(CASE WHEN SUM(RIC.intCurrentReturnQuantity) "
                + "IS NULL OR SUM(RIC.intCurrentReturnQuantity) = '' THEN 0 ELSE SUM(RIC.intCurrentReturnQuantity) END))) AS intItemQuantity "
                + "FROM (SELECT RIQ.intInventoryID, SUM(RIQ.intItemQuantity) AS intFirstSaleQuantity FROM tbl" + businessNumber + "ReceiptItem "
                + "RIQ JOIN tbl" + businessNumber + "Receipt RIQ2 ON RIQ.intReceiptID = RIQ2.intReceiptID WHERE RIQ2.intReceiptGroupID = "
                + "@intReceiptGroupID AND RIQ2.intReceiptSubNumber = 1 GROUP BY RIQ.intInventoryID) RI2 LEFT JOIN (SELECT RIQ3.intInventoryID, "
                + "SUM(RIQ3.intItemQuantity) AS intReturnQuantity FROM tbl" + businessNumber + "ReceiptItem RIQ3 JOIN tbl" + businessNumber
                + "Receipt RIQ4 ON RIQ3.intReceiptID = RIQ4.intReceiptID WHERE RIQ4.intReceiptGroupID = @intReceiptGroupID AND "
                + "RIQ4.intReceiptSubNumber > 1 GROUP BY RIQ3.intInventoryID) RIR ON RI2.intInventoryID = RIR.intInventoryID LEFT JOIN (SELECT "
                + "RIQ5.intInventoryID, SUM(RIQ5.intItemQuantity) AS intCurrentReturnQuantity FROM tbl" + businessNumber + "ReceiptItemCurrent "
                + "RIQ5 JOIN tbl" + businessNumber + "ReceiptCurrent RIQ6 ON RIQ5.intReceiptID = RIQ6.intReceiptID WHERE RIQ6.intReceiptGroupID "
                + "= @intReceiptGroupID AND RIQ6.intReceiptSubNumber > 1 GROUP BY RIQ5.intInventoryID) RIC ON RI2.intInventoryID = "
                + "RIC.intInventoryID GROUP BY RI2.intInventoryID";

            object[][] parms =
            {
                new object[] { "@intReceiptGroupID", receipt.intReceiptGroupID }
            };

            return ConvertFromDataTableToAvaiableToReturnItems(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), receipt.intReceiptGroupID, businessNumber);
        }

        private ReceiptItem GatherRemainingReturnItemInformation(ReceiptItem receiptItem, int receiptGroupID, int businessNumber)
        {
            string sqlCmd = "SELECT I.varSku, RI.fltItemPrice, RI.fltItemDiscount, RI.bitIsNonStockedProduct, RI.bitIsRegularProduct, "
                + "RI.bitIsTradeIn, RI.bitIsPercentageDiscount, RI.varItemDescription FROM tbl" + businessNumber + "ReceiptItem RI "
                + "JOIN tbl" + businessNumber + "Inventory I ON I.intInventoryID = RI.intInventoryID JOIN tbl" + businessNumber
                + "Receipt R ON R.intReceiptID = RI.intReceiptID WHERE R.intReceiptGroupID = @intReceiptGroupID AND "
                + "R.intReceiptSubNumber = 1 AND RI.intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intReceiptGroupID", receiptGroupID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID }
            };
            return TurnToTemReceiptItemForReturn(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private ReceiptItem TurnToTemReceiptItemForReturn(DataTable dt)
        {
            List<ReceiptItem> receiptItem = dt.AsEnumerable().Select(row =>
            new ReceiptItem
            {
                varSku = row.Field<string>("varSku"),
                fltItemPrice = row.Field<double>("fltItemPrice"),
                fltItemDiscount = row.Field<double>("fltItemDiscount"),
                bitIsNonStockedProduct = row.Field<bool>("bitIsNonStockedProduct"),
                bitIsRegularProduct = row.Field<bool>("bitIsRegularProduct"),
                bitIsTradeIn = row.Field<bool>("bitIsTradeIn"),
                bitIsPercentageDiscount = row.Field<bool>("bitIsPercentageDiscount"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return receiptItem[0];
        }

        public ReceiptItem ReturnReceiptItemForReturnProcess(int inventoryID, int receiptGroupID, int businessNumber)
        {
            string sqlCmd = "SELECT RI.intReceiptItemID, RI.intReceiptID, RI.intInventoryID, I.varSku, RI.intItemQuantity, "
                + "RI.fltItemAverageCostAtSale, RI.fltItemPrice, RI.fltItemDiscount, RI.fltItemRefund, RI.bitIsPercentageDiscount, "
                + "RI.bitIsNonStockedProduct, RI.bitIsRegularProduct, RI.bitIsTradeIn, RI.varItemDescription FROM tbl" 
                + businessNumber + "ReceiptItem RI JOIN tbl" + businessNumber + "Inventory I ON I.intInventoryID = RI.intInventoryID "
                + "JOIN tbl" + businessNumber + "Receipt R ON R.intReceiptID = RI.intReceiptID WHERE RI.intReceiptID IN(SELECT "
                + "intReceiptID FROM tbl" + businessNumber + "Receipt WHERE intReceiptGroupID = @intReceiptGroupID AND "
                + "intReceiptSubNumber = 1) AND RI.intInventoryID = @intInventoryID";
            
            object[][] parms =
            {
                new object[] { "@intReceiptGroupID", receiptGroupID },
                new object[] { "@intInventoryID", inventoryID }
            };

            ReceiptItem RI = ConvertFromDataTableToReceiptItem(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms))[0];
            RI.intItemQuantity = ReturnQuantityOfItem(RI.intInventoryID, businessNumber);
            return RI;
        }

        private List<ReceiptPayment> ReturnReceiptPaymentFromReceiptGroupID(int receiptGroupID, int businessNumber)
        {
            string sqlCmd = "SELECT RPC.intPaymentID, RPC.intReceiptID, RPC.intMethodOfPaymentID, MOP.varMethodOfPaymentName, RPC.fltAmountPaid "
                + "FROM tbl" + businessNumber + "ReceiptPayment RPC JOIN tblMethodOfPayment MOP ON MOP.intMethodOfPaymentID = "
                + "RPC.intMethodOfPaymentID JOIN tbl" + businessNumber + "Receipt R ON R.intReceiptID = RPC.intReceiptID WHERE R.intReceiptID "
                + "IN(SELECT intReceiptID FROM tbl" + businessNumber + "Receipt WHERE intReceiptGroupID = @intReceiptGroupID) AND "
                + "R.intReceiptSubNumber = 1";
            object[][] parms =
            {
                new object[] { "@intReceiptGroupID", receiptGroupID }
            };
            return ConvertFromDataTableToReceiptPaymentCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptPayment> ConvertFromDataTableToReceiptPaymentCurrent(DataTable dt)
        {
            List<ReceiptPayment> receiptPayment = dt.AsEnumerable().Select(row =>
            new ReceiptPayment
            {
                intPaymentID = row.Field<int>("intPaymentID"),
                intReceiptID = row.Field<int>("intReceiptID"),
                intMethodOfPaymentID = row.Field<int>("intMethodOfPaymentID"),
                varMethodOfPaymentName = row.Field<string>("varMethodOfPaymentName"),
                fltAmountPaid = row.Field<double>("fltAmountPaid")
            }).ToList();
            return receiptPayment;
        }
        private List<ReceiptPayment> ReturnPaymentInSaleCart(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT RPC.intPaymentID, RPC.intReceiptID, RPC.intMethodOfPaymentID, MOP.varMethodOfPaymentName, "
                + "RPC.fltAmountPaid FROM tbl" + businessNumber + "ReceiptPaymentCurrent RPC JOIN tblMethodOfPayment MOP ON "
                + "MOP.intMethodOfPaymentID = RPC.intMethodOfPaymentID WHERE RPC.intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };

            return ConvertFromDataTableToReceiptPaymentCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<ReceiptPayment> ReturnReceiptPayment(int receiptID, int businessNumber)
        {
            //TODO **Change from RPC.intPaymentIdentifierID to RPC.intPaymentID
            string sqlCmd = "SELECT RPC.intPaymentID, RPC.intReceiptID, RPC.intMethodOfPaymentID, MOP.varMethodOfPaymentName, "
                + "RPC.fltAmountPaid FROM tbl" + businessNumber + "ReceiptPayment RPC JOIN tblMethodOfPayment MOP ON "
                + "MOP.intMethodOfPaymentID = RPC.intMethodOfPaymentID WHERE RPC.intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            return ConvertFromDataTableToReceiptPaymentCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<ReceiptPayment> ReturnReceiptPaymentsFromOriginalReceipt(int receiptGroupID, int businessNumber)
        {
            return ReturnReceiptPaymentFromReceiptGroupID(receiptGroupID, businessNumber);
        }

        private DataTable InventorySearchQueryForTradeIn(string searchText, CurrentUser cu)
        {
            string sqlCmd = "SELECT I.intInventoryID FROM tbl" + cu.terminal.intBusinessNumber + "Inventory I JOIN (SELECT T.intInventoryID "
                + "FROM tbl" + cu.terminal.intBusinessNumber + "TradeInSkuPerLocation T WHERE T.intStoreLocationID = @intStoreLocationID) "
                + "TIS ON TIS.intInventoryID = I.intInventoryID WHERE (I.intBrandID IN (SELECT intBrandID FROM tbl" + cu.terminal.intBusinessNumber 
                + "Brand WHERE varBrandName LIKE @searchText) OR CONCAT(varSku, varModelName, varDescription, varUPCcode) LIKE @searchText)";
            object[][] parms =
            {
                new object[] { "@searchText", "%" + searchText + "%" },
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnReceiptBasedOnSearchCriteriaForReturns(DateTime startDate, string searchText, int businessNumber)
        {
            string sqlCmd = "SELECT R.intReceiptID, R.varReceiptNumber, R.dtmReceiptCompletionDate, CONCAT(C.varLastName, ', ', C.varFirstName) "
                + "AS varCustomerFullName, RP.fltAmountPaid, S.varStoreName FROM tbl" + businessNumber + "Receipt R INNER JOIN(SELECT intReceiptID, "
                + "SUM(fltAmountPaid) AS fltAmountPaid FROM tbl" + businessNumber + "ReceiptPayment GROUP BY intReceiptID) RP ON RP.intReceiptID = "
                + "R.intReceiptID JOIN tbl" + businessNumber + "Customer C ON C.intCustomerID = R.intCustomerID JOIN tbl" + businessNumber 
                + "StoreLocation S ON S.intStoreLocationID = R.intStoreLocationID WHERE R.intReceiptSubNumber = 1 AND (R.dtmReceiptCompletionDate = "
                + "@dtmReceiptCompletionDate ";

            if (searchText != "")
            {
                sqlCmd += "OR CONCAT(R.varReceiptNumber, C.varFirstName, C.varLastName, C.varHomePhone, "
                    + "C.varmobilePhone) LIKE '%" + searchText + "%'";
            }
            sqlCmd += ") ORDER BY R.varReceiptNumber DESC";
            object[][] parms =
            {
                 new object[] { "@dtmReceiptCompletionDate", startDate }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        private object[] ReturnNextReceiptNumberForNewReceipt(CurrentUser cu)
        {
            string sqlCmd = "SELECT intReceiptStoreLocationID, intReceiptNumberSystem, CONCAT(varStoreCodeReceipt, CASE WHEN "
                + "LEN(CAST(intReceiptNumberSystem AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intReceiptNumberSystem AS "
                + "VARCHAR(6))),6) ELSE CAST(intReceiptNumberSystem AS VARCHAR(MAX)) END) AS varReceiptNumber FROM tbl" 
                + cu.terminal.intBusinessNumber + "StoredReceiptNumber WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            int receiptGroupNumber = DBC.MakeDatabaseCallToReturnSecondColumnAsInt(sqlCmd, parms) + 1;
            //Creates the new receipt number
            CreateReceiptNumberForNextReceipt(receiptGroupNumber, cu.currentStoreLocation.intStoreLocationID, cu.terminal.intBusinessNumber);
            //Returns the receipt number for use on new sale
            object[] receiptNumberAndGroup = { DBC.MakeDatabaseCallToReturnThirdColumnAsString(sqlCmd, parms), receiptGroupNumber };
            return receiptNumberAndGroup;
        }

        private string ReturnInventorySearchString(ArrayList array, int businessNumber)
        {
            string sqlCmd = "";
            for (int i = 0; i < array.Count; i++)
            {
                if (i == 0)
                {
                    sqlCmd = "SELECT intInventoryID FROM tbl" + businessNumber + "Inventory WHERE(intBrandID IN(SELECT intBrandID FROM tbl" 
                        + businessNumber + "Brand WHERE varBrandName LIKE '%" + array[i] + "%') OR CONCAT(varSku, varModelName, varDescription, "
                        + "varUPCcode) LIKE '%" + array[i] + "%') ";
                }
                else
                {
                    sqlCmd += "INTERSECT (SELECT intInventoryID FROM tbl" + businessNumber + "Inventory WHERE(intBrandID IN(SELECT intBrandID "
                        + "FROM tbl" + businessNumber + "Brand WHERE varBrandName LIKE '%" + array[i] + "%') OR CONCAT(varSku, varModelName, "
                        + "varDescription, varUPCcode) LIKE '%" + array[i] + "%')) ";
                }
            }
            //sqlCmd += " ORDER BY intInventoryID DESC";
            return sqlCmd;
        }

        private int ReturnQuantityOfItem(int inventoryID, int businessNumber)
        {
            string sqlCmd = "SELECT intQuantity FROM tbl" + businessNumber + "Inventory WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID }
            };
            //Returns the quantity of the searched item
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int ReturnQuantityOfItem(int inventoryID, int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intItemQuantity FROM tbl" + businessNumber + "ReceiptItemCurrent WHERE intInventoryID = @intInventoryID "
                + "AND intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID },
                new object[] { "@intInventoryID", inventoryID }
            };
            //Returns the quantity of the searched item
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int GetMOPTypeIDFromMOPName(string methodOfPaymentName)
        {
            string sqlCmd = "SELECT intMethodOfPaymentID FROM tblMethodOfPayment WHERE varMethodOfPaymentName = @varMethodOfPaymentName";
            object[][] parms =
            {
                new object[] { "@varMethodOfPaymentName", methodOfPaymentName }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int TransactionTypeFromReceiptNumber(int receiptID, int businessNumber)
        {
            string sqlCmd = "SELECT intTransactionTypeID FROM tbl" + businessNumber + "Receipt WHERE intReceiptID = @intReceiptID";
            object[][] parms =
            {
                 new object[] { "@intReceiptID", receiptID },
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int ReturnMOPTypeID(string methodOfPaymentName)
        {
            return GetMOPTypeIDFromMOPName(methodOfPaymentName);
        }
        public int AddTradeInToInventoryTable(ReceiptItem receiptItem, DateTime createDateTime, CurrentUser cu)
        {
            InventoryManager IM = new InventoryManager();
            return IM.NewTradeIn(receiptItem, createDateTime, cu);
        }
        public int ReturnTransactionTypeFromReceiptNumber(int receiptID, int businessNumber)
        {
            return TransactionTypeFromReceiptNumber(receiptID, businessNumber);
        }
        public int ReturnQuantityFromCurrentSaleCart(int inventoryID, int receiptID, int businessNumber)
        {
            return ReturnQuantityOfItem(inventoryID, receiptID, businessNumber);
        }
        public int CalculateNextReceiptSubNum(string receiptNumber, int businessNumber)
        {
            string sqlCmd = "SELECT MAX(intReceiptSubNumber) AS intReceiptSubNumber FROM tbl" + businessNumber 
                + "Receipt WHERE varReceiptNumber = @varReceiptNumber";

            object[][] parms =
            {
                new object[] { "@varReceiptNumber", receiptNumber }
            };
            //Return the invoice sub num
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) + 1;
        }

        private double ReturnNewBalanceDueOnCashRounding(double balanceDueTotal)
        {
            int lastCent = Convert.ToInt32(Convert.ToString(balanceDueTotal).Substring(Convert.ToString(balanceDueTotal).Length - 1));
            switch (lastCent)
            {
                case 1:
                    balanceDueTotal -= 1;
                    break;
                case 2:
                    balanceDueTotal -= 2;
                    break;
                case 3:
                    balanceDueTotal += 2;
                    break;
                case 4:
                    balanceDueTotal += 1;
                    break;
                case 6:
                    balanceDueTotal -= 1;
                    break;
                case 7:
                    balanceDueTotal -= 2;
                    break;
                case 8:
                    balanceDueTotal += 2;
                    break;
                case 9:
                    balanceDueTotal += 1;
                    break;
            }
            return balanceDueTotal;
        }

        public double CalculateRoundingForCash(Receipt receipt, int businessNumber)
        {
            receipt.fltBalanceDueTotal = ReturnNewBalanceDueOnCashRounding(receipt.fltBalanceDueTotal);
            UpdateReceiptCurrent(receipt, businessNumber);
            return receipt.fltBalanceDueTotal;
        }

        public bool ValidInventoryQuantity(ReceiptItem receiptItem, int businessNumber)
        {
            bool hasValidQuantity = true;
            int quantityInCurrentStock = ReturnQuantityOfItem(receiptItem.intInventoryID, businessNumber);
            int quantityOnCurrentSale = ReturnQuantityOfItem(receiptItem.intInventoryID, receiptItem.intReceiptID, businessNumber);

            int remaingQTYAvailForSale = quantityInCurrentStock - (receiptItem.intItemQuantity - quantityOnCurrentSale);

            if (remaingQTYAvailForSale < 0)
            {
                hasValidQuantity = false;
            }
            return hasValidQuantity;
        }
        public bool ItemAlreadyInCart(ReceiptItem receiptItem, int businessNumber)
        {
            bool itemInCart = false;

            string sqlCmd = "SELECT intInventoryID FROM tbl" + businessNumber + "ReceiptItemCurrent WHERE "
                + "intReceiptID = @intReceiptID AND intInventoryID = @intInventoryID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID }
            };
            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            if (dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }
        public bool ItemAlreadyInReturnCartRefundAmountCheck(ReceiptItem receiptItem, int businessNumber)
        {
            bool itemInCart = false;
            string sqlCmd = "SELECT intInventoryID FROM tbl" + businessNumber + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID "
                + "AND intInventoryID = @intInventoryID AND fltItemRefund = @fltItemRefund";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID },
                new object[] { "@fltItemRefund", receiptItem.fltItemRefund }
            };

            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            if (dt.Rows.Count > 0)
            {
                itemInCart = true;
            }
            return itemInCart;
        }
        public bool VerifyAMethodOfPaymentHasBeenAdded(int receiptID, int businessNumber)
        {
            bool paymentOnReceipt = false;
            string sqlCmd = "SELECT COUNT(intReceiptID) AS intReceiptID FROM tbl" + businessNumber + "ReceiptPaymentCurrent WHERE "
                + "intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };

            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                paymentOnReceipt = true;
            }
            return paymentOnReceipt;
        }
        public bool InventorySearchReturnsTradeIn(string searchText, CurrentUser cu)
        {
            bool tradeInFound = false;
            DataTable dt = InventorySearchQueryForTradeIn(searchText, cu);
            if (dt.Rows.Count > 0)
            {
                tradeInFound = true;
            }
            return tradeInFound;
        }
        
        private void CreateReceiptNumberForNextReceipt(int receiptNumber, int storeLocationID, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "StoredReceiptNumber SET intReceiptNumberSystem "
                + "= @receiptNumber WHERE intStoreLocationID = @storeLocationID";
            object[][] parms =
            {
                new object[] { "@receiptNumber", receiptNumber },
                new object[] { "@storeLocationID", storeLocationID }

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void AddInventoryBackIntoStock(ReceiptItem ri, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "Inventory SET intQuantity += @intQuantity WHERE intInventoryID = @intInventoryID";
            object[][] parms =
            {
                new object[] { "@intQuantity", ri.intItemQuantity },
                new object[] { "@intInventoryID", ri.intInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemFromReceiptItemCurrentTable(ReceiptItem ri, int receiptID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID AND intInventoryID = @intInventoryID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID },
                new object[] { "@intInventoryID", ri.intInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertItemIntoReceiptItemCurrent(ReceiptItem receiptItem, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemCurrent VALUES(@intReceiptID, @intInventoryID, @intItemQuantity, "
                + "@fltItemAverageCostAtSale, @fltItemPrice, @fltItemDiscount, @fltItemRefund, @bitIsPercenageDiscount, @bitIsNonStockedProduct, "
                + "@bitIsRegularProduct, @bitIsTradeIn, @varItemDescription)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID },
                new object[] { "@intItemQuantity", receiptItem.intItemQuantity },
                new object[] { "@fltItemAverageCostAtSale", receiptItem.fltItemAverageCostAtSale },
                new object[] { "@fltItemPrice", receiptItem.fltItemPrice },
                new object[] { "@fltItemDiscount", receiptItem.fltItemDiscount },
                new object[] { "@fltItemRefund", receiptItem.fltItemRefund },
                new object[] { "@bitIsPercenageDiscount", receiptItem.bitIsPercentageDiscount },
                new object[] { "@bitIsNonStockedProduct", receiptItem.bitIsNonStockedProduct },
                new object[] { "@bitIsRegularProduct", receiptItem.bitIsRegularProduct },
                new object[] { "@bitIstradeIn", receiptItem.bitIsTradeIn },
                new object[] { "@varItemDescription", receiptItem.varItemDescription }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReceiptPaymentFromReceiptCurrent(int paymentID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptPaymentCurrent WHERE intPaymentID = @intPaymentID";
            object[][] parms =
            {
                new object[] { "@intPaymentID", paymentID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemFromCurrentSalesTable(ReceiptItem receiptItem, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID AND intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveItemTaxesFromCurrentSalesTable(ReceiptItem receiptItem, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemTaxesCurrent WHERE intReceiptItemID = @intReceiptItemID";

            object[][] parms =
            {
                new object[] { "@intReceiptItemID", receiptItem.intReceiptItemID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptItemTaxesCurrentIntoReceiptItemTaxes(Receipt receipt, int businessNumber)
        {
            TaxManager TM = new TaxManager();
            List<ReceiptItemTax> itemTax = TM.ReturnReceiptItemTaxesCurrentToProcessSale(receipt.intReceiptID, businessNumber);
            foreach (ReceiptItemTax tax in itemTax)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemTaxes VALUES(@intReceiptItemID, @intTaxTypeID, @fltTaxAmount, @bitIsTaxCharged)";
                object[][] parms =
                {
                    new object[] { "@intReceiptItemID", tax.intReceiptItemID },
                    new object[] { "@intTaxTypeID", tax.intTaxTypeID },
                    new object[] { "@fltTaxAmount", tax.fltTaxAmount },
                    new object[] { "@bitIsTaxCharged", tax.bitIsTaxCharged }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            }
        }
        private void RemoveReceiptItemTaxesCurrent(Receipt receipt, int businessNumber)
        {
            foreach (ReceiptItemTax tax in receipt.lstReceiptItemTax)
            {
                RemoveSingleReceiptItemTaxesCurrent(tax.intReceiptItemID, businessNumber);
            }
        }
        private void RemoveSingleReceiptItemTaxesCurrent(int receiptItemID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemTaxesCurrent WHERE intReceiptItemID = @intReceiptItemID";
            object[][] parms =
            {
                new object[] { "@intReceiptItemID", receiptItemID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptItemCurrentIntoReceiptItem(Receipt receipt, CurrentUser cu)
        {
            InventoryManager IM = new InventoryManager();
            foreach (ReceiptItem item in receipt.lstReceiptItem)
            {
                string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "ReceiptItem VALUES(@intReceiptItemID, @intReceiptID, "
                    + "@intInventoryID, @intItemQuantity, @fltItemAverageCostAtSale, @fltItemPrice, @fltItemDiscount, @fltItemRefund, "
                    + "@bitIsPercentageDiscount, @bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsTradeIn, @varItemDescription)";

                object[][] parms =
                {
                    new object[] { "@intReceiptItemID", item.intReceiptItemID },
                    new object[] { "@intReceiptID", item.intReceiptID },
                    new object[] { "@intInventoryID", item.intInventoryID },
                    new object[] { "@intItemQuantity", item.intItemQuantity },
                    new object[] { "@fltItemAverageCostAtSale", item.fltItemAverageCostAtSale },
                    new object[] { "@fltItemPrice", item.fltItemPrice },
                    new object[] { "@fltItemDiscount", item.fltItemDiscount },
                    new object[] { "@fltItemRefund", item.fltItemRefund },
                    new object[] { "@bitIsPercentageDiscount", item.bitIsPercentageDiscount },
                    new object[] { "@bitIsNonStockedProduct", item.bitIsNonStockedProduct },
                    new object[] { "@bitIsRegularProduct", item.bitIsRegularProduct },
                    new object[] { "@bitIsTradeIn", item.bitIsTradeIn },
                    new object[] { "@varItemDescription", item.varItemDescription }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
                if (item.bitIsTradeIn)
                {
                    IM.SetUpTradeInForProcessing(item, cu);
                }
            }
        }
        private void RemoveReceiptItemCurrent(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptPaymentCurrentIntoReceiptPayment(Receipt receipt, int businessNumber)
        {
            foreach (ReceiptPayment payment in receipt.lstReceiptPayment)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptPayment VALUES(@intPaymentID, @intReceiptID, "
                    + "@intMethodOfPaymentID, @fltAmountPaid)";

                object[][] parms =
                {
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@intReceiptID", payment.intReceiptID },
                    new object[] { "@intMethodOfPaymentID", payment.intMethodOfPaymentID },
                    new object[] { "@fltAmountPaid", payment.fltAmountPaid }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);

                if(payment.varMethodOfPaymentName == "Charge To Account")
                {
                    CustomerPaidOnAccount(receipt, payment, businessNumber);
                }
            }
        }
        private void CustomerPaidOnAccount(Receipt receipt, ReceiptPayment payment, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ChargedToAccount VALUES(@intCustomerID, @intReceiptID, "
                + "@fltChargedToAccountAmount)";
            object[][] parms =
            {
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intReceiptID", receipt.intReceiptID },
                new object[] { "@fltChargedToAccountAmount", payment.fltAmountPaid }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReceiptPaymentCurrent(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptPaymentCurrent WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptCurrentIntoReceipt(Receipt receipt, DateTime completeDateTime, CurrentUser cu)
        {
            string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "Receipt VALUES(@intReceiptID, @intReceiptGroupID, "
                + "@varReceiptNumber, @intReceiptSubNumber, @dtmReceiptCreationDate, @dtmReceiptCreationTime, @dtmReceiptCompletionDate, "
                + "@dtmReceiptCompletionTime, @intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, @fltCostTotal, "
                + "@fltCartTotal, @fltDiscountTotal, @fltTradeInTotal, @fltShippingTotal, @intTransactionTypeID, @varReceiptComments, "
                + "@fltTenderedAmount, @fltChangeAmount)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID },
                new object[] { "@intReceiptGroupID", receipt.intReceiptGroupID },
                new object[] { "@varReceiptNumber", receipt.varReceiptNumber },
                new object[] { "@intReceiptSubNumber", receipt.intReceiptSubNumber },
                new object[] { "@dtmReceiptCreationDate", receipt.dtmReceiptCreationDate },
                new object[] { "@dtmReceiptCreationTime", receipt.dtmReceiptCreationTime },
                new object[] { "@dtmReceiptCompletionDate", completeDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReceiptCompletionTime", completeDateTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intEmployeeID", receipt.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", receipt.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@fltCostTotal", receipt.fltCostTotal },
                new object[] { "@fltCartTotal", receipt.fltCartTotal },
                new object[] { "@fltDiscountTotal", receipt.fltDiscountTotal },
                new object[] { "@fltTradeInTotal", receipt.fltTradeInTotal },
                new object[] { "@fltShippingTotal", receipt.fltShippingTotal },
                new object[] { "@intTransactionTypeID", receipt.intTransactionTypeID },
                new object[] { "@varReceiptComments", receipt.varReceiptComments},
                new object[] { "@fltTenderedAmount", receipt.fltTenderedAmount},
                new object[] { "@fltChangeAmount", receipt.fltChangeAmount}
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReceiptCurrent(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptCurrent WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveUnfinishedTradeInFromInventoryTradeIn(Receipt receipt, int businessNumber)
        {
            foreach(ReceiptItem receiptItem in receipt.lstReceiptItem)
            {
                if (receiptItem.bitIsTradeIn)
                {
                    RemoveTaxTypePerInventory(receiptItem.intInventoryID, businessNumber);
                    RemoveInventory(receiptItem.intInventoryID, businessNumber);
                }
            }
        }
        private void RemoveTaxTypePerInventory(int inventoryID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "TaxTypePerInventoryItem WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveInventory(int inventoryID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "Inventory WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptCurrentIntoReceiptVoidCancel(Receipt receipt, DateTime cancelDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptVoidCancel VALUES(@intReceiptID, @intReceiptGroupID, "
                + "@varReceiptNumber, @intReceiptSubNumber, @dtmReceiptCreationDate, @dtmReceiptCreationTime, @dtmReceiptVoidCancelDate, "
                + "@dtmReceiptVoidCancelTime, @intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, @fltCostTotal, "
                + "@fltCartTotal, @fltDiscountTotal, @fltTradeInTotal, @fltShippingTotal, @intTransactionTypeID, @varReceiptComments, "
                + "@fltTenderedAmount, @fltChangeAmount, @bitIsReceiptVoided, @bitIsReceiptCancelled)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID },
                new object[] { "@intReceiptGroupID", receipt.intReceiptGroupID },
                new object[] { "@varReceiptNumber", receipt.varReceiptNumber },
                new object[] { "@intReceiptSubNumber", receipt.intReceiptSubNumber },
                new object[] { "@dtmReceiptCreationDate", receipt.dtmReceiptCreationDate },
                new object[] { "@dtmReceiptCreationTime", receipt.dtmReceiptCreationTime },
                new object[] { "@dtmReceiptVoidCancelDate", cancelDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReceiptVoidCancelTime", cancelDateTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intEmployeeID", receipt.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", receipt.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", receipt.intTerminalID },
                new object[] { "@fltCostTotal", receipt.fltCostTotal },
                new object[] { "@fltCartTotal", receipt.fltCartTotal },
                new object[] { "@fltDiscountTotal", receipt.fltDiscountTotal },
                new object[] { "@fltTradeInTotal", receipt.fltTradeInTotal },
                new object[] { "@fltShippingTotal", receipt.fltShippingTotal },
                new object[] { "@intTransactionTypeID", receipt.intTransactionTypeID },
                new object[] { "@varReceiptComments", receipt.varReceiptComments },
                new object[] { "@fltTenderedAmount", receipt.fltTenderedAmount },
                new object[] { "@fltChangeAmount", receipt.fltChangeAmount },
                new object[] { "@bitIsReceiptVoided", receipt.bitIsReceiptVoided },
                new object[] { "@bitIsReceiptCancelled", receipt.bitIsReceiptCancelled }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReceiptItemCurrentIntoReceiptItemVoidCancel(Receipt receipt, int businessNumber)
        {
            foreach (ReceiptItem item in receipt.lstReceiptItem)
            {
                if (!item.bitIsTradeIn)
                {
                    string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemVoidCancel VALUES(@intReceiptItemID, @intReceiptID, "
                        + "@intInventoryID, @intItemQuantity, @fltItemAverageCostAtSale, @fltItemPrice, @fltItemDiscount, @fltItemRefund, "
                        + "@bitIsPercentageDiscount, @bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsTradeIn, @varItemDescription)";

                    object[][] parms =
                    {
                        new object[] { "@intReceiptItemID", item.intReceiptItemID },
                        new object[] { "@intReceiptID", item.intReceiptID },
                        new object[] { "@intInventoryID", item.intInventoryID },
                        new object[] { "@intItemQuantity", item.intItemQuantity },
                        new object[] { "@fltItemAverageCostAtSale", item.fltItemAverageCostAtSale },
                        new object[] { "@fltItemPrice", item.fltItemPrice },
                        new object[] { "@fltItemDiscount", item.fltItemDiscount },
                        new object[] { "@fltItemRefund", item.fltItemRefund },
                        new object[] { "@bitIsPercentageDiscount", item.bitIsPercentageDiscount },
                        new object[] { "@bitIsNonStockedProduct", item.bitIsNonStockedProduct },
                        new object[] { "@bitIsRegularProduct", item.bitIsRegularProduct },
                        new object[] { "@bitIsTradeIn", item.bitIsTradeIn },
                        new object[] { "@varItemDescription", item.varItemDescription }
                    };
                    DBC.ExecuteNonReturnQuery(sqlCmd, parms);
                }
            }
        }
        private void InsertReceiptItemTaxesCurrentIntoReceiptItemTaxesVoidCancel(Receipt receipt, int businessNumber)
        {
            TaxManager TM = new TaxManager();
            List<ReceiptItemTax> itemTax = TM.ReturnReceiptItemTaxesCurrentToProcessSale(receipt.intReceiptID, businessNumber);
            foreach (ReceiptItemTax tax in itemTax)
            {
                if (!TM.ItemTaxIsFromTradeIn(businessNumber)) {
                    string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemTaxesVoidCancel VALUES("
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
            }
        }
        private void InsertReceiptPaymentCurrentIntoReceiptPaymentVoidCancel(Receipt receipt, int businessNumber)
        {
            foreach (ReceiptPayment payment in receipt.lstReceiptPayment)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptPaymentVoidCancel VALUES("
                    + "@intPaymentID, @intReceiptID, @intMethodOfPaymentID, @fltAmountPaid)";

                object[][] parms =
                {
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@intReceiptID", payment.intReceiptID },
                    new object[] { "@intMethodOfPaymentID", payment.intMethodOfPaymentID },
                    new object[] { "@fltAmountPaid", payment.fltAmountPaid }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            }
        }

        public void UpdateReceiptCurrent(Receipt receipt, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "ReceiptCurrent SET intCustomerID = @intCustomerID, intEmployeeID = @intEmployeeID, "
                + "intTerminalID = @intTerminalID, fltCostTotal = @fltCostTotal, fltCartTotal = @fltCartTotal, fltDiscountTotal = @fltDiscountTotal, "
                + "fltTradeInTotal = @fltTradeInTotal, fltShippingTotal = @fltShippingTotal, varReceiptComments = @varReceiptComments, "
                + "fltTenderedAmount = @fltTenderedAmount, fltChangeAmount = @fltChangeAmount WHERE intReceiptID = @intReceiptID";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID },
                new object[] { "@intCustomerID", receipt.customer.intCustomerID },
                new object[] { "@intEmployeeID", receipt.employee.intEmployeeID },
                new object[] { "@intTerminalID", receipt.intTerminalID },
                new object[] { "@fltCostTotal", receipt.fltCostTotal },
                new object[] { "@fltCartTotal", receipt.fltCartTotal },
                new object[] { "@fltDiscountTotal", receipt.fltDiscountTotal },
                new object[] { "@fltTradeInTotal", receipt.fltTradeInTotal },
                new object[] { "@fltShippingTotal", receipt.fltShippingTotal },
                new object[] { "@varReceiptComments", receipt.varReceiptComments },
                new object[] { "@fltTenderedAmount", receipt.fltTenderedAmount },
                new object[] { "@fltChangeAmount", receipt.fltChangeAmount }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void ReturnReceiptItemQuantityToInventory(int inventoryID, int receiptID, int businessNumber)
        {
            //gather sku info
            ReceiptItem itemToReturn = ReturnItemFromReceiptCurrentTable(inventoryID, receiptID, businessNumber)[0];
            //use info to add quantity back
            if (!itemToReturn.bitIsNonStockedProduct)
            {
                AddInventoryBackIntoStock(itemToReturn, businessNumber);
            }
            //remove the sku from currentSales table
            RemoveSingleReceiptItemTaxesCurrent(itemToReturn.intReceiptItemID, businessNumber);
            RemoveItemFromReceiptItemCurrentTable(itemToReturn, receiptID, businessNumber);
        }
        public void AddNewPaymentIntoReceiptPaymentCurrent(int receiptID, double amountPaid, int methodOfPaymentID, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptPaymentCurrent VALUES(@intReceiptID, "
                + "@intMethodOfPaymentID, @fltAmountPaid)";

            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID },
                new object[] { "@intMethodOfPaymentID", methodOfPaymentID },
                new object[] { "@fltAmountPaid", amountPaid }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void UpdateItemFromCurrentSalesTable(ReceiptItem receiptItem, int businessNumber)
        {
            int quantityInCurrentStock = ReturnQuantityOfItem(receiptItem.intInventoryID, businessNumber);
            int quantityOnCurrentSale = ReturnQuantityOfItem(receiptItem.intInventoryID, receiptItem.intReceiptID, businessNumber);

            RemoveQuantityFromInventory(receiptItem.intInventoryID, quantityInCurrentStock - (receiptItem.intItemQuantity - quantityOnCurrentSale), businessNumber);
            UpdateItemFromCurrentSalesTableActualQuery(receiptItem, businessNumber);
        }
        public void RemoveQuantityFromInventory(int inventoryID, int remainingQuantity, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "Inventory SET intQuantity = @intQuantity WHERE intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intInventoryID", inventoryID },
                new object[] { "@intQuantity", remainingQuantity }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void UpdateItemFromCurrentSalesTableActualQuery(ReceiptItem receiptItem, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "ReceiptItemCurrent SET intItemQuantity = @intItemQuantity, fltItemDiscount = "
                + "@fltItemDiscount, bitIsPercentageDiscount = @bitIsPercentageDiscount WHERE intReceiptID = @intReceiptIdentifdierID AND "
                + "intInventoryID = @intInventoryID";

            object[][] parms =
            {
                new object[] { "@intItemQuantity", receiptItem.intItemQuantity },
                new object[] { "@fltItemDiscount", receiptItem.fltItemDiscount },
                new object[] { "@bitIsPercentageDiscount", receiptItem.bitIsPercentageDiscount },
                new object[] { "@intReceiptIdentifdierID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void LoopThroughTheItemsToReturnToInventory(List<ReceiptItem> receiptItems, int businessNumber)
        {
            //Loop through DataTable
            foreach (ReceiptItem receiptItem in receiptItems)
            {
                if (!receiptItem.bitIsNonStockedProduct)
                {
                    AddInventoryBackIntoStock(receiptItem, businessNumber);
                }
            }
        }
        public void CancelReceiptCurrent(Receipt receipt, DateTime cancelDateTime, int businessNumber)
        {
            //NEW Step 1: Save ReceieptCurrent into Receipt
            InsertReceiptCurrentIntoReceiptVoidCancel(receipt, cancelDateTime, businessNumber);

            //NEW Step 2: Save ReceiptItemCurrent into ReceiptItem
            InsertReceiptItemCurrentIntoReceiptItemVoidCancel(receipt, businessNumber);

            //NEW Step 3: Save ReceiptItemTaxesCurrent into ReceiptItemTaxes
            InsertReceiptItemTaxesCurrentIntoReceiptItemTaxesVoidCancel(receipt, businessNumber);

            //NEW Step 4: Save ReceiptPaymentCurrent into ReceiptPayment
            InsertReceiptPaymentCurrentIntoReceiptPaymentVoidCancel(receipt, businessNumber);

            //NEW Step 5: Remove ReceiptPaymentCurrent
            RemoveReceiptPaymentCurrent(receipt, businessNumber);

            //NEW Step 6: Remove ReceiptItemTaxesCurrent
            RemoveReceiptItemTaxesCurrent(receipt, businessNumber);

            //NEW Step 7: Remove ReceiptItemCurrent
            RemoveReceiptItemCurrent(receipt, businessNumber);

            //NEW Step 8: Remove ReceiptCurrent
            RemoveReceiptCurrent(receipt, businessNumber);

            //NEW Step 9: Remove Any Trade Ins from the Inventory Table
            RemoveUnfinishedTradeInFromInventoryTradeIn(receipt, businessNumber);

        }
        public void AddingItemToTheSale(ReceiptItem receiptItem, int transactionTypeID, DateTime currentDateTime, CurrentUser cu)
        {
            InsertItemIntoReceiptItemCurrent(receiptItem, cu.terminal.intBusinessNumber);
            TaxManager TM = new TaxManager();
            TM.LoopThroughTaxesForEachItemAddingToReceiptItemTaxesCurrent(receiptItem, transactionTypeID, currentDateTime, cu);
        }
        public void RemoveReceiptPaymentCurrent(int paymentID, int businessNumber)
        {
            RemoveReceiptPaymentFromReceiptCurrent(paymentID, businessNumber);
        }
        public void DoNotReturnTheItemOnReturn(ReceiptItem receiptItem, int businessNumber)
        {
            RemoveItemTaxesFromCurrentSalesTable(receiptItem, businessNumber);
            RemoveItemFromCurrentSalesTable(receiptItem, businessNumber);
        }
        public void FinalizeReceiptCurrent(Receipt receipt, DateTime completeDateTime, CurrentUser cu)
        {
            //NEW Step 1: Save ReceieptCurrent into Receipt
            InsertReceiptCurrentIntoReceipt(receipt, completeDateTime, cu);

            //NEW Step 2: Save ReceiptItemCurrent into ReceiptItem
            InsertReceiptItemCurrentIntoReceiptItem(receipt, cu);

            //NEW Step 3: Save ReceiptItemTaxesCurrent into ReceiptItemTaxes
            InsertReceiptItemTaxesCurrentIntoReceiptItemTaxes(receipt, cu.terminal.intBusinessNumber);

            //NEW Step 4: Save ReceiptPaymentCurrent into ReceiptPayment
            InsertReceiptPaymentCurrentIntoReceiptPayment(receipt, cu.terminal.intBusinessNumber);

            //NEW Step 5: Remove ReceiptPaymentCurrent
            RemoveReceiptPaymentCurrent(receipt, cu.terminal.intBusinessNumber);

            //NEW Step 6: Remove ReceiptItemTaxesCurrent
            RemoveReceiptItemTaxesCurrent(receipt, cu.terminal.intBusinessNumber);

            //NEW Step 7: Remove ReceiptItemCurrent
            RemoveReceiptItemCurrent(receipt, cu.terminal.intBusinessNumber);

            //NEW Step 8: Remove ReceiptCurrent
            RemoveReceiptCurrent(receipt, cu.terminal.intBusinessNumber);
        }
    }
}