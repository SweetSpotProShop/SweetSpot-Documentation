using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SweetPOS.ClassObjects
{
    public class SalesReconciliationManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<SalesReconciliation> ConvertFromDataTableToSalesReconciliation(DataTable dt)
        {
            List<SalesReconciliation> salesReconciliation = dt.AsEnumerable().Select(row =>
            new SalesReconciliation
            {
                intSalesReconciliationID = row.Field<int>("intSalesReconciliationID"),
                dtmSalesReconciliationDate = row.Field<DateTime>("dtmSalesReconciliationDate"),
                dtmSalesReconciliationProcessedDate = row.Field<DateTime>("dtmSalesReconciliationProcessedDate"),
                dtmSalesReconciliationProcessedTime = row.Field<DateTime>("dtmSalesReconciliationProcessedTime"),
                fltAmericanExpressSales = row.Field<double>("fltAmericanExpressSales"),
                fltCashSales = row.Field<double>("fltCashSales"),
                fltChequeSales = row.Field<double>("fltChequeSales"),
                fltDebitSales = row.Field<double>("fltDebitSales"),
                fltDiscoverSales = row.Field<double>("fltDiscoverSales"),
                fltGiftCardSales = row.Field<double>("fltGiftCardSales"),
                fltMastercardSales = row.Field<double>("fltMastercardSales"),
                fltTradeInSales = row.Field<double>("fltTradeInSales"),
                fltVisaSales = row.Field<double>("fltVisaSales"),
                fltAmericanExpressCounted = row.Field<double>("fltAmericanExpressCounted"),
                fltCashCounted = row.Field<double>("fltCashCounted"),
                fltChequeCounted = row.Field<double>("fltChequeCounted"),
                fltDebitCounted = row.Field<double>("fltDebitCounted"),
                fltDiscoverCounted = row.Field<double>("fltDiscoverCounted"),
                fltGiftCardCounted = row.Field<double>("fltGiftCardCounted"),
                fltMastercardCounted = row.Field<double>("fltMastercardCounted"),
                fltTradeInCounted = row.Field<double>("fltTradeInCounted"),
                fltVisaCounted = row.Field<double>("fltVisaCounted"),
                fltPreTaxSalesTotal = row.Field<double>("fltPreTaxSalesTotal"),
                fltGovernmentTaxTotal = row.Field<double>("fltGovernmentTaxTotal"),
                fltProvincialTaxTotal = row.Field<double>("fltProvincialTaxTotal"),
                fltOverShort = row.Field<double>("fltOverShort"),
                fltCashPurchases = row.Field<double>("fltCashPurchases"),
                bitIsFinalized = row.Field<bool>("bitIsFinalized"),
                bitIsProcessed = row.Field<bool>("bitIsProcessed"),
                intStoreLocationID = row.Field<int>("intStoreLocationID"),
                intEmployeeID = row.Field<int>("intEmployeeID")
            }).ToList();
            return salesReconciliation;
        }
        private List<SalesReconciliation> InsertIntoSalesReconciliation(SalesReconciliation salesReconciliation, DateTime processDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "SalesReconciliation VALUES(@dtmSalesReconciliationDate, @dtmSalesReconciliationProcessedDate, "
                + "@dtmSalesReconciliationProcessedTime, @fltAmericanExpressSales, @fltCashSales, @fltChequeSales, @fltDebitSales, @fltDiscoverSales, "
                + "@fltGiftCardSales, @fltMastercardSales, @fltTradeInSales, @fltVisaSales, @fltAmericanExpressCounted, @fltCashCounted, @fltChequeCounted, "
                + "@fltDebitCounted, @fltDiscoverCounted, @fltGiftCardCounted, @fltMastercardCounted, @fltTradeInCounted, @fltVisaCounted, @fltPreTaxSalesTotal, "
                + "@fltGovernmentTaxTotal, @fltProvincialTaxTotal, @fltOverShort, @fltCashPurchases, @bitIsFinalized, @bitIsProcessed, @intStoreLocationID, "
                + "@intEmployeeID)";

            object[][] parms =
            {
                new object[] { "@dtmSalesReconciliationDate", salesReconciliation.dtmSalesReconciliationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmSalesReconciliationProcessedDate", processDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmSalesReconciliationProcessedTime", processDateTime.ToString("HH:mm:ss") },
                new object[] { "@fltAmericanExpressSales", salesReconciliation.fltAmericanExpressSales },
                new object[] { "@fltCashSales", salesReconciliation.fltCashSales },
                new object[] { "@fltChequeSales", salesReconciliation.fltChequeSales },
                new object[] { "@fltDebitSales", salesReconciliation.fltDebitSales },
                new object[] { "@fltDiscoverSales", salesReconciliation.fltDiscoverSales },
                new object[] { "@fltGiftCardSales", salesReconciliation.fltGiftCardSales },
                new object[] { "@fltMastercardSales", salesReconciliation.fltMastercardSales },
                new object[] { "@fltTradeInSales", salesReconciliation.fltTradeInSales },
                new object[] { "@fltVisaSales", salesReconciliation.fltVisaSales },
                new object[] { "@fltAmericanExpressCounted", salesReconciliation.fltAmericanExpressCounted },
                new object[] { "@fltCashCounted", salesReconciliation.fltCashCounted },
                new object[] { "@fltChequeCounted", salesReconciliation.fltChequeCounted },
                new object[] { "@fltDebitCounted", salesReconciliation.fltDebitCounted },
                new object[] { "@fltDiscoverCounted", salesReconciliation.fltDiscoverCounted },
                new object[] { "@fltGiftCardCounted", salesReconciliation.fltGiftCardCounted },
                new object[] { "@fltMastercardCounted", salesReconciliation.fltMastercardCounted },
                new object[] { "@fltTradeInCounted", salesReconciliation.fltTradeInCounted },
                new object[] { "@fltVisaCounted", salesReconciliation.fltVisaCounted },
                new object[] { "@fltPreTaxSalesTotal", salesReconciliation.fltPreTaxSalesTotal },
                new object[] { "@fltGovernmentTaxTotal", salesReconciliation.fltGovernmentTaxTotal },
                new object[] { "@fltProvincialTaxTotal", salesReconciliation.fltProvincialTaxTotal },
                new object[] { "@fltOverShort", salesReconciliation.fltOverShort },
                new object[] { "@fltCashPurchases", salesReconciliation.fltCashPurchases },
                new object[] { "@bitIsFinalized", salesReconciliation.bitIsFinalized },
                new object[] { "@bitIsProcessed", salesReconciliation.bitIsProcessed },
                new object[] { "@intStoreLocationID", salesReconciliation.intStoreLocationID },
                new object[] { "@intEmployeeID", salesReconciliation.intEmployeeID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnNewSalesReconciliationFromReconciliationData(parms, businessNumber);
        }
        private List<SalesReconciliation> ReturnNewSalesReconciliationFromReconciliationData(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT intSalesReconciliationID FROM tbl" + businessNumber + "SalesReconciliation WHERE dtmSalesReconciliationDate "
                + "= @dtmSalesReconciliationDate AND dtmSalesReconciliationProcessedDate = @dtmSalesReconciliationProcessedDate AND "
                + "dtmSalesReconciliationProcessedTime = @dtmSalesReconciliationProcessedTime AND fltAmericanExpressSales = "
                + "@fltAmericanExpressSales AND fltCashSales = @fltCashSales AND fltChequeSales = @fltChequeSales AND fltDebitSales = "
                + "@fltDebitSales AND fltDiscoverSales = @fltDiscoverSales AND fltGiftCardSales = @fltGiftCardSales AND fltMastercardSales = "
                + "@fltMastercardSales AND fltTradeInSales = @fltTradeInSales AND fltVisaSales = @fltVisaSales AND fltAmericanExpressCounted = "
                + "@fltAmericanExpressCounted AND fltCashCounted = @fltCashCounted AND fltChequeCounted = @fltChequeCounted AND fltDebitCounted "
                + "= @fltDebitCounted AND fltDiscoverCounted = @fltDiscoverCounted AND fltGiftCardCounted = @fltGiftCardCounted AND "
                + "fltMastercardCounted = @fltMastercardCounted AND fltTradeInCounted = @fltTradeInCounted AND fltVisaCounted = @fltVisaCounted "
                + "AND fltPreTaxSalesTotal = @fltPreTaxSalesTotal AND fltGovernmentTaxTotal = @fltGovernmentTaxTotal AND fltProvincialTaxTotal "
                + "= @fltProvincialTaxTotal AND fltOverShort = @fltOverShort AND fltCashPurchases = @fltCashPurchases AND bitIsFinalized = "
                + "@bitIsFinalized AND bitIsProcessed = @bitIsProcessed AND intStoreLocationID = @intStoreLocationID AND intEmployeeID = "
                + "@intEmployeeID";
            return ReturnSalesReconciliation(DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms), businessNumber);
        }
        private List<SalesReconciliation> CallSelectedSalesReconciliation(int storeLocationID, DateTime dtmSelectedDate, int businessNumber)
        {
            string sqlCmd = "SELECT intSalesReconciliationID, dtmSalesReconciliationDate, dtmSalesReconciliationProcessedDate, "
                + "CAST(dtmSalesReconciliationProcessedTime AS DATETIME) AS dtmSalesReconciliationProcessedTime, fltAmericanExpressSales, "
                + "fltCashSales, fltChequeSales, fltDebitSales, fltDiscoverSales, fltGiftCardSales, fltMastercardSales, fltTradeInSales, "
                + "fltVisaSales, fltAmericanExpressCounted, fltCashCounted, fltChequeCounted, fltDebitCounted, fltDiscoverCounted, "
                + "fltGiftCardCounted, fltMastercardCounted, fltTradeInCounted, fltVisaCounted, fltPreTaxSalesTotal, "
                + "fltGovernmentTaxTotal, fltProvincialTaxTotal, fltOverShort, fltCashPurchases, bitIsFinalized, bitIsProcessed, "
                + "intStoreLocationID, intEmployeeID FROM tbl" + businessNumber + "SalesReconciliation WHERE dtmSalesReconciliationDate = "
                + "@dtmSalesReconciliationDate AND intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@dtmSalesReconciliationDate", dtmSelectedDate },
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            return ConvertFromDataTableToSalesReconciliation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<SalesReconciliation> CreateNewSalesReconciliation(DateTime selectedDate, int storeLocationID, DateTime processDateTime, CurrentUser cu)
        {
            DataTable dt = ReturnTotalsForNewSalesReconciliation(storeLocationID, selectedDate, cu.terminal.intBusinessNumber);

            //Save all into a cashout and return
            SalesReconciliation salesReconciliation = new SalesReconciliation
            {
                dtmSalesReconciliationDate = DateTime.Parse(dt.Rows[0][0].ToString()),
                dtmSalesReconciliationProcessedDate = processDateTime,
                dtmSalesReconciliationProcessedTime = processDateTime,
                fltAmericanExpressSales = Convert.ToDouble(dt.Rows[0][2].ToString()),
                fltCashSales = Convert.ToDouble(dt.Rows[0][3].ToString()),
                fltChequeSales = Convert.ToDouble(dt.Rows[0][4].ToString()),
                fltDebitSales = Convert.ToDouble(dt.Rows[0][5].ToString()),
                fltDiscoverSales = Convert.ToDouble(dt.Rows[0][6].ToString()),
                fltGiftCardSales = Convert.ToDouble(dt.Rows[0][7].ToString()),
                fltMastercardSales = Convert.ToDouble(dt.Rows[0][8].ToString()),
                fltTradeInSales = Convert.ToDouble(dt.Rows[0][10].ToString()),
                fltVisaSales = Convert.ToDouble(dt.Rows[0][9].ToString()),

                fltAmericanExpressCounted = 0,
                fltCashCounted = ReturnAllTillCashoutTotals(selectedDate, cu),
                fltChequeCounted = 0,
                fltDebitCounted = 0,
                fltDiscoverCounted = 0,
                fltGiftCardCounted = 0,
                fltMastercardCounted = 0,
                fltTradeInCounted = 0,
                fltVisaCounted = 0,

                fltPreTaxSalesTotal = Convert.ToDouble(dt.Rows[0][11].ToString()),
                fltGovernmentTaxTotal = Convert.ToDouble(dt.Rows[0][12].ToString()),
                fltProvincialTaxTotal = Convert.ToDouble(dt.Rows[0][13].ToString()),
                fltOverShort = 0,
                fltCashPurchases = ReturnCashPurchasesForDate(selectedDate, cu),
                bitIsFinalized = false,
                bitIsProcessed = false,
                intStoreLocationID = Convert.ToInt32(dt.Rows[0][1].ToString()),
                intEmployeeID = cu.employee.intEmployeeID
            };

            return InsertIntoSalesReconciliation(salesReconciliation, processDateTime, cu.terminal.intBusinessNumber);
        }
        public List<SalesReconciliation> ReturnSalesReconciliation(int salesReconciliationID, int businessNumber)
        {
            string sqlCmd = "SELECT intSalesReconciliationID, dtmSalesReconciliationDate, dtmSalesReconciliationProcessedDate, "
                + "CAST(dtmSalesReconciliationProcessedTime AS DATETIME) AS dtmSalesReconciliationProcessedTime, fltAmericanExpressSales, "
                + "fltCashSales, fltChequeSales, fltDebitSales, fltDiscoverSales, fltGiftCardSales, fltMastercardSales, fltTradeInSales, "
                + "fltVisaSales, fltAmericanExpressCounted, fltCashCounted, fltChequeCounted, fltDebitCounted, fltDiscoverCounted, "
                + "fltGiftCardCounted, fltMastercardCounted, fltTradeInCounted, fltVisaCounted, fltPreTaxSalesTotal, "
                + "fltGovernmentTaxTotal, fltProvincialTaxTotal, fltOverShort, fltCashPurchases, bitIsFinalized, bitIsProcessed, "
                + "intStoreLocationID, intEmployeeID FROM tbl" + businessNumber + "SalesReconciliation WHERE intSalesReconciliationID = "
                + "@intSalesReconciliationID";
            object[][] parms =
            {
                new object[] { "@intSalesReconciliationID", salesReconciliationID }
            };
            return ConvertFromDataTableToSalesReconciliation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<SalesReconciliation> ReturnSelectedRangeSalesReconciliation(object[] reportCriteria)
        {
            DateTime[] reportDates = (DateTime[])reportCriteria[0];
            CurrentUser cu = (CurrentUser)reportCriteria[2];
            string sqlCmd = "SELECT intSalesReconciliationID, dtmSalesReconciliationDate, dtmSalesReconciliationProcessedDate, "
                + "CAST(dtmSalesReconciliationProcessedTime AS DATETIME) AS dtmSalesReconciliationProcessedTime, fltAmericanExpressSales, "
                + "fltCashSales, fltChequeSales, fltDebitSales, fltDiscoverSales, fltGiftCardSales, fltMastercardSales, fltTradeInSales, "
                + "fltVisaSales, fltAmericanExpressCounted, fltCashCounted, fltChequeCounted, fltDebitCounted, fltDiscoverCounted, "
                + "fltGiftCardCounted, fltMastercardCounted, fltTradeInCounted, fltVisaCounted, fltPreTaxSalesTotal, "
                + "fltGovernmentTaxTotal, fltProvincialTaxTotal, fltOverShort, fltCashPurchases, bitIsFinalized, bitIsProcessed, "
                + "intStoreLocationID, intEmployeeID FROM tbl" + cu.terminal.intBusinessNumber + "SalesReconciliation WHERE "
                + "dtmSalesReconciliationDate BETWEEN @dtmStartDate AND @dtmEndDate AND intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", reportDates[0].ToShortDateString() },
                new object[] { "@dtmEndDate", reportDates[1].ToShortDateString() },
                new object[] { "@intStoreLocationID", Convert.ToInt32(reportCriteria[1]) }
            };
            return ConvertFromDataTableToSalesReconciliation(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public SalesReconciliation ReturnSelectedSalesReconciliation(int storeLocationID, DateTime dtmSelectedDate, CurrentUser cu)
        {
            SalesReconciliation salesReconciliation = CallSelectedSalesReconciliation(storeLocationID, dtmSelectedDate, cu.terminal.intBusinessNumber)[0];

            DataTable dt = ReturnTotalsForNewSalesReconciliation(storeLocationID, dtmSelectedDate, cu.terminal.intBusinessNumber);

            //now go through and update the items needed
            salesReconciliation.fltAmericanExpressSales = Convert.ToDouble(dt.Rows[0][2].ToString());
            salesReconciliation.fltCashSales = Convert.ToDouble(dt.Rows[0][3].ToString());
            salesReconciliation.fltChequeSales = Convert.ToDouble(dt.Rows[0][4].ToString());
            salesReconciliation.fltDebitSales = Convert.ToDouble(dt.Rows[0][5].ToString());
            salesReconciliation.fltDiscoverSales = Convert.ToDouble(dt.Rows[0][6].ToString());
            salesReconciliation.fltGiftCardSales = Convert.ToDouble(dt.Rows[0][7].ToString());
            salesReconciliation.fltMastercardSales = Convert.ToDouble(dt.Rows[0][8].ToString());
            salesReconciliation.fltTradeInSales = Convert.ToDouble(dt.Rows[0][10].ToString());
            salesReconciliation.fltVisaSales = Convert.ToDouble(dt.Rows[0][9].ToString());

            salesReconciliation.fltCashCounted = ReturnAllTillCashoutTotals(dtmSelectedDate, cu);
            salesReconciliation.fltCashPurchases = ReturnCashPurchasesForDate(dtmSelectedDate, cu);
            salesReconciliation.fltPreTaxSalesTotal = Convert.ToDouble(dt.Rows[0][11].ToString());
            salesReconciliation.fltGovernmentTaxTotal = Convert.ToDouble(dt.Rows[0][12].ToString());
            salesReconciliation.fltProvincialTaxTotal = Convert.ToDouble(dt.Rows[0][13].ToString());
            salesReconciliation.intEmployeeID = cu.employee.intEmployeeID;

            //update the sales rec table
            UpdateSalesReconciliation(salesReconciliation, cu);
            return salesReconciliation;
        }

        private List<TillCashout> ConvertFromDataTableToTillCashout(DataTable dt)
        {
            List<TillCashout> tillCashout = dt.AsEnumerable().Select(row =>
            new TillCashout
            {
                intTerminalID = row.Field<int>("intTerminalID"),
                dtmTillCashoutDate = row.Field<DateTime>("dtmTillCashoutDate"),
                dtmTillCashoutProcessedDate = row.Field<DateTime>("dtmTillCashoutProcessedDate"),
                dtmTillCashoutProcessedTime = row.Field<DateTime>("dtmTillCashoutProcessedTime"),
                intHundredDollarBillCount = row.Field<int>("intHundredDollarBillCount"),
                intFiftyDollarBillCount = row.Field<int>("intFiftyDollarBillCount"),
                intTwentyDollarBillCount = row.Field<int>("intTwentyDollarBillCount"),
                intTenDollarBillCount = row.Field<int>("intTenDollarBillCount"),
                intFiveDollarBillCount = row.Field<int>("intFiveDollarBillCount"),
                intToonieCoinCount = row.Field<int>("intToonieCoinCount"),
                intLoonieCoinCount = row.Field<int>("intLoonieCoinCount"),
                intQuarterCoinCount = row.Field<int>("intQuarterCoinCount"),
                intDimeCoinCount = row.Field<int>("intDimeCoinCount"),
                intNickelCoinCount = row.Field<int>("intNickelCoinCount"),
                fltCashDrawerTotal = row.Field<double>("fltCashDrawerTotal"),
                fltCountedTotal = row.Field<double>("fltCountedTotal"),
                fltCashDrawerFloat = row.Field<double>("fltCashDrawerFloat"),
                fltCashDrawerCashDrop = row.Field<double>("fltCashDrawerCashDrop"),
                bitIsProcessed = row.Field<bool>("bitIsProcessed"),
                bitIsFinalized = row.Field<bool>("bitIsFinalized")
            }).ToList();
            return tillCashout;
        }
        private List<TillCashout> ReturnSelectedTillCashout(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT intTerminalID, dtmTillCashoutDate, dtmTillCashoutProcessedDate, CAST(dtmTillCashoutProcessedTime "
                + "AS DATETIME) AS dtmTillCashoutProcessedTime, intHundredDollarBillCount, intFiftyDollarBillCount, "
                + "intTwentyDollarBillCount, intTenDollarBillCount, intFiveDollarBillCount, intToonieCoinCount, intLoonieCoinCount, "
                + "intQuarterCoinCount, intDimeCoinCount, intNickelCoinCount, fltCashDrawerTotal, fltCountedTotal, fltCashDrawerFloat, "
                + "fltCashDrawerCashDrop, bitIsProcessed, bitIsFinalized FROM tbl" + cu.terminal.intBusinessNumber + "TillCashout "
                + "WHERE intTerminalID = @intTerminalID AND dtmTillCashoutDate = @dtmTillCashoutDate";
            object[][] parms =
            {
                new object[] { "@dtmTillCashoutDate", selectedDate },
                new object[] { "@intTerminalID", cu.terminal.intTerminalID }
            };
            return ConvertFromDataTableToTillCashout(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<TillCashout> ReturnTillCashout(DateTime selectedDate, CurrentUser cu)
        {
            return ReturnSelectedTillCashout(selectedDate, cu);
        }

        private DataTable ReturnTotalsForNewSalesReconciliation(int storeLocationID, DateTime selectedDate, int businessNumber)
        {
            string sqlCmd = "SELECT RPT.dtmReceiptCompletionDate, RPT.intStoreLocationID, fltAmericanExpressSales, fltCashSales, fltChequeSales, "
                + "fltDebitSales, fltDiscoverSales, fltGiftCardSales, fltMastercardSales, fltVisaSales, fltTradeInTotal, fltPreTaxSalesTotal, "
                + "fltGovernmentTaxTotal, fltProvincialTaxTotal FROM (SELECT dtmReceiptCompletionDate, intStoreLocationID, ISNULL([1], 0) AS "
                + "fltAmericanExpressSales, ISNULL([2], 0) AS fltCashSales, ISNULL([3], 0) AS fltChequeSales, ISNULL([4],0) AS fltDebitSales, "
                + "ISNULL([5], 0) AS fltDiscoverSales, ISNULL([6],0) AS fltGiftCardSales, ISNULL([7],0) AS fltMastercardSales, ISNULL([8],0) AS "
                + "fltVisaSales FROM (SELECT R.dtmReceiptCompletionDate, R.intStoreLocationID, RP.intMethodOfPaymentID, SUM(fltAmountPaid) AS "
                + "fltAmountPaid FROM tbl" + businessNumber + "ReceiptPayment RP JOIN tbl" + businessNumber + "Receipt R ON RP.intReceiptID = "
                + "R.intReceiptID WHERE R.dtmReceiptCompletionDate = @dtmSelectedDate AND R.intStoreLocationID = @intStoreLocationID GROUP BY "
                + "R.dtmReceiptCompletionDate, R.intStoreLocationID, RP.intMethodOfPaymentID) AS PS PIVOT (SUM(fltAmountPaid) FOR "
                + "intMethodOfPaymentID IN([1], [2], [3], [4], [5], [6], [7], [8])) AS PVT) AS RPT JOIN (SELECT dtmReceiptCompletionDate, "
                + "intStoreLocationID, SUM(fltCartTotal) + SUM(fltShippingTotal) - SUM(fltDiscountTotal) + SUM(fltTradeInTotal) AS "
                + "fltPreTaxSalesTotal, SUM(fltTradeInTotal) AS fltTradeInTotal, SUM(fltGST) + SUM(fltHST) + SUM(fltQST) AS fltGovernmentTaxTotal"
                + ", SUM(fltPST) + SUM(fltRST) AS fltProvincialTaxTotal FROM tbl" + businessNumber + "Receipt R LEFT JOIN (SELECT intReceiptID, "
                + "ISNULL([1], 0) AS fltGST, ISNULL([3], 0) AS fltPST, ISNULL([2], 0) AS fltHST, ISNULL([4],0) AS fltRST, ISNULL([5], 0) AS "
                + "fltQST FROM (SELECT R.intReceiptID, RIT.intTaxTypeID, SUM(CASE WHEN RIT.bitIsTaxCharged = 1 THEN fltTaxAmount ELSE 0 END) AS "
                + "fltTaxAmount FROM tbl" + businessNumber + "ReceiptItemTaxes RIT JOIN tbl" + businessNumber + "ReceiptItem RI ON "
                + "RI.intReceiptItemID = RIT.intReceiptItemID JOIN tbl" + businessNumber + "Receipt R ON R.intReceiptID = RI.intReceiptID WHERE "
                + "R.dtmReceiptCompletionDate = @dtmSelectedDate AND R.intStoreLocationID = @intStoreLocationID GROUP BY R.intReceiptID, "
                + "R.intStoreLocationID, RIT.intTaxTypeID) AS PS PIVOT (SUM(fltTaxAmount) FOR intTaxTypeID IN([1], [3], [2], [4], [5])) AS PVT) "
                + "AS RITP ON RITP.intReceiptID = R.intReceiptID WHERE dtmReceiptCompletionDate = @dtmSelectedDate AND intStoreLocationID = "
                + "@intStoreLocationID GROUP BY dtmReceiptCompletionDate, intStoreLocationID) AS RT ON RT.dtmReceiptCompletionDate = "
                + "RPT.dtmReceiptCompletionDate AND RT.intStoreLocationID = RPT.intStoreLocationID";

            object[][] parms =
            {
                new object[] { "@dtmSelectedDate", selectedDate },
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        private DataTable ProcessedTillClosed(DateTime tillDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT intTillCashoutID, L.intTillNumber, fltCashDrawerTotal, fltCountedTotal, fltCashDrawerFloat, "
                + "fltCashDrawerCashDrop, bitIsProcessed, bitIsFinalized FROM tbl" + cu.terminal.intBusinessNumber + "TillCashout "
                + "TC JOIN tbl" + cu.terminal.intBusinessNumber + "Licence L ON L.intTerminalID = TC.intTerminalID WHERE "
                + "L.intStoreLocationID = @intStoreLocationID AND dtmTillCashoutDate = @dtmTillCashoutDate AND bitIsProcessed = 1";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@dtmTillCashoutDate", tillDateTime.ToString("yyyy-MM-dd") }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnProcessedTillClosed(DateTime tillDateTime, CurrentUser cu)
        {
            return ProcessedTillClosed(tillDateTime, cu);
        }

        public DataTable ReturnOpenTills(DateTime tillDateTime, CurrentUser cu)
        {
            return ProcessedOpenTills(tillDateTime, cu);
        }
        private DataTable ProcessedOpenTills(DateTime tillDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT intTillCashoutID, L.intTillNumber, fltCashDrawerTotal, fltCountedTotal, fltCashDrawerFloat, "
                + "fltCashDrawerCashDrop, bitIsProcessed, bitIsFinalized FROM tbl" + cu.terminal.intBusinessNumber + "TillCashout "
                + "TC JOIN tbl" + cu.terminal.intBusinessNumber + "Licence L ON L.intTerminalID = TC.intTerminalID WHERE "
                + "L.intStoreLocationID = @intStoreLocationID AND dtmTillCashoutDate = @dtmTillCashoutDate AND bitIsProcessed = 0";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@dtmTillCashoutDate", tillDateTime.ToString("yyyy-MM-dd") }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        private double ReturnAllTillCashoutTotals(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT ROUND(SUM(fltCashDrawerCashDrop), 2) AS fltCashSales FROM tbl" + cu.terminal.intBusinessNumber
                + "TillCashout TC JOIN tbl" + cu.terminal.intBusinessNumber + "Licence L ON L.intTerminalID = TC.intTerminalID WHERE "
                + "L.intStoreLocationID = @intStoreLocationID AND dtmTillCashoutDate = @dtmSelectedDate";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID },
                new object[] { "@dtmSelectedDate", selectedDate }
            };
            return DBC.MakeDatabaseCallToReturnDouble(sqlCmd, parms);
        }
        private double ReturnCashPurchasesForDate(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT ISNULL(SUM(fltAmountReceived), 0) AS fltICashAmount FROM tbl" + cu.terminal.intBusinessNumber 
                + "InvoicePayment IP JOIN tbl" + cu.terminal.intBusinessNumber + "Invoice I ON I.intInvoiceID = IP.intInvoiceID "
                + "JOIN tblMethodOfPayment MP ON MP.intMethodOfPaymentID = IP.intMethodOfPaymentID WHERE I.dtmInvoiceCompletionDate "
                + "= @dtmInvoiceCompletionDate AND MP.varMethodOfPaymentName = 'Cash'";
            object[][] parms =
            {
                new object[] { "@dtmInvoiceCompletionDate", selectedDate.ToString("yyyy-MM-dd") }
            };
            return DBC.MakeDatabaseCallToReturnDouble(sqlCmd, parms);
        }        
        private double ReturnCashDrawerTotal(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT ISNULL(RC.fltRCashAmount,0) - ISNULL(IC.fltICashAmount,0) FROM(SELECT R.intTerminalID, SUM(fltAmountPaid) "
                + "AS fltRCashAmount FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptPayment RP JOIN tbl" + cu.terminal.intBusinessNumber 
                + "Receipt R ON R.intReceiptID = RP.intReceiptID JOIN tblMethodOfPayment MP ON MP.intMethodOfPaymentID = RP.intMethodOfPaymentID "
                + "WHERE R.dtmReceiptCompletionDate = @dtmReceiptCompletionDate AND R.intTerminalID = @intTerminalID AND MP.varMethodOfPaymentName "
                + "= 'Cash' GROUP BY R.intTerminalID) AS RC LEFT JOIN(SELECT I.intTerminalID, SUM(fltAmountReceived) AS fltICashAmount FROM tbl" 
                + cu.terminal.intBusinessNumber + "InvoicePayment IP JOIN tbl" + cu.terminal.intBusinessNumber + "Invoice I ON I.intInvoiceID = "
                + "IP.intInvoiceID JOIN tblMethodOfPayment MP ON MP.intMethodOfPaymentID = IP.intMethodOfPaymentID WHERE I.dtmInvoiceCompletionDate "
                + "= @dtmReceiptCompletionDate AND I.intTerminalID = @intTerminalID AND MP.varMethodOfPaymentName = 'Cash' GROUP BY I.intTerminalID) "
                + "AS IC ON IC.intTerminalID = RC.intTerminalID";
            object[][] parms =
            {
                new object[] { "@dtmReceiptCompletionDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intTerminalID", cu.terminal.intTerminalID.ToString() }
            };
            return DBC.MakeDatabaseCallToReturnDouble(sqlCmd, parms);
        }

        public double ReturnExpectedCashDrawerTotal(DateTime selectedDate, CurrentUser cu)
        {
            return ReturnCashDrawerTotal(selectedDate, cu);
        }

        private int TillReconciliationHasBeenCompletedCheck(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT COUNT(intTerminalID) FROM tbl" + cu.terminal.intBusinessNumber + "TillCashout WHERE intTerminalID = "
                + "@intTerminalID AND dtmTillCashoutDate = @dtmTillCashoutDate AND bitIsFinalized = 1";

            object[][] parms =
            {
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@dtmTillCashoutDate", selectedDate.ToString("yyyy-MM-dd")}
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int TransactionsCheck(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT COUNT(intReceiptID) FROM tbl" + cu.terminal.intBusinessNumber + "Receipt WHERE "
                + "intTerminalID = @intTerminalID AND dtmReceiptCompletionDate = @dtmReceiptCompletionDate";
            object[][] parms =
            {
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@dtmReceiptCompletionDate", selectedDate.ToString("yyyy-MM-dd")}
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int OpenTransactions(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT COUNT(intReceiptID) FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptCurrent "
                        + "WHERE intTerminalID = @intTerminalID AND dtmReceiptCreationDate = @dtmReceiptCreationDate";
            object[][] parms =
            {
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@dtmReceiptCreationDate", selectedDate.ToString("yyyy-MM-dd")}
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int CashoutInTable(DateTime selectedDate, CurrentUser cu)
        {
            string sqlCmd = "SELECT COUNT(intTerminalID) FROM tbl" + cu.terminal.intBusinessNumber
                + "TillCashout WHERE intTerminalID = @intTerminalID AND dtmTillCashoutDate = @dtmTillCashoutDate";
            object[][] parms =
            {
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@dtmTillCashoutDate", selectedDate.ToString("yyyy-MM-dd") }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int TodaysCashoutInTable(DateTime tillDateTime, CurrentUser cu)
        {
            string sqlCmd = "SELECT COUNT(intTerminalID) FROM tbl" + cu.terminal.intBusinessNumber + "TillCashout WHERE "
                + "intTerminalID = @intTerminalID AND dtmTillCashoutDate = @dtmTillCashoutDate AND bitIsProcessed = 1";
            object[][] parms =
            {
                new object[] { "@intTerminalID", cu.terminal.intTerminalID },
                new object[] { "@dtmTillCashoutDate", tillDateTime.ToString("yyyy-MM-dd") }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int VerifyReconciliationCanBeProcessed(DateTime selectedDate, CurrentUser cu)
        {
            int indicator = 0;
            if (AllOpenTillsHaveBeenClosed(selectedDate, cu))
            {
                if (DailyReconciliationExistsAndProcessed(selectedDate, cu))
                {
                    indicator = 2;
                }
                else
                {
                    indicator = 1;
                }
            }
            return indicator;
        }
        public int TillReconciliationProcessCheck(DateTime selectedDate, CurrentUser cu)
        {
            int indicator = 0;
            if (TransactionsProcessedForTill(selectedDate, cu))
            {
                if (TillReconciliationHasBeenCompleted(selectedDate, cu))
                {
                    indicator = 2;
                }
                else if (TransactionsStillOpenForTill(selectedDate, cu))
                {
                    indicator = 3;
                }
                else
                {
                    indicator = 1;
                }
            }
            return indicator;
        }

        private bool TransactionsProcessedForTill(DateTime selectedDate, CurrentUser cu)
        {
            bool transactionAvail = false;
            if (TransactionsCheck(selectedDate, cu) > 0)
            {
                transactionAvail = true;
            }
            return transactionAvail;
        }
        private bool TillReconciliationHasBeenCompleted(DateTime selectedDate, CurrentUser cu)
        {
            bool tillInTable = false;
            if (TillReconciliationHasBeenCompletedCheck(selectedDate, cu) > 0)
            {
                tillInTable = true;
            }
            return tillInTable;
        }
        private bool TransactionsStillOpenForTill(DateTime selectedDate, CurrentUser cu)
        {
            bool openTran = false;
            if (OpenTransactions(selectedDate, cu) > 0)
            {
                openTran = true;
            }
            return openTran;
        }

        public bool IsCashoutAlreadyInPlace(DateTime selectedDate, CurrentUser cu)
        {
            bool cashoutInTable = false;
            if (CashoutInTable(selectedDate, cu) > 0)
            {
                cashoutInTable = true;
            }
            return cashoutInTable;
        }
        public bool TillAlreadyCashedOut(DateTime tillDateTime, CurrentUser cu)
        {
            bool cashoutInTable = false;
            if (TodaysCashoutInTable(tillDateTime, cu) > 0)
            {
                cashoutInTable = true;
            }
            return cashoutInTable;
        }
        public bool AllOpenTillsHaveBeenClosed(DateTime selectedDate, CurrentUser cu)
        {
            bool bolTA = true;
            string sqlCmd = "SELECT L.intTerminalID, ISNULL(RC.intReceiptCount, 0) AS intReceiptCount, ISNULL(TC.intCompletedCashout, "
                + "0) AS intCompletedCashout FROM tbl" + cu.terminal.intBusinessNumber + "Licence L LEFT JOIN (SELECT intTerminalID, "
                + "COUNT(intReceiptID) AS intReceiptCount FROM tbl" + cu.terminal.intBusinessNumber + "Receipt WHERE "
                + "dtmReceiptCompletionDate = @dtmSelectedDate GROUP BY intTerminalID) RC ON RC.intTerminalID = L.intTerminalID LEFT "
                + "JOIN (SELECT intTerminalID, COUNT(bitIsProcessed) AS intCompletedCashout FROM tbl" + cu.terminal.intBusinessNumber
                + "TillCashout WHERE dtmTillCashoutDate = @dtmSelectedDate AND bitIsProcessed = 1 GROUP BY intTerminalID) TC ON "
                + "TC.intTerminalID = L.intTerminalID WHERE L.intStoreLocationID = @intStoreLocationID";

            object[][] parms =
            {
                new object[] { "@dtmSelectedDate", selectedDate.ToString("yyyy-MM-dd") },
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };

            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            List<SalesRecCheck> salesRecCheck = dt.AsEnumerable().Select(row =>
            new SalesRecCheck
            {
                intTerminalID = row.Field<int>("intTerminalID"),
                intReceiptCount = row.Field<int>("intReceiptCount"),
                intCompletedCashout = row.Field<int>("intCompletedCashout")
            }).ToList();

            foreach (SalesRecCheck src in salesRecCheck)
            {
                if (src.intReceiptCount > 0 && src.intCompletedCashout == 0)
                {
                    bolTA = false;
                }
            }
            return bolTA;
        }
        public bool DailyReconciliationExistsAndProcessed(DateTime selectedDate, CurrentUser cu)
        {
            bool cashoutExists = false;
            string sqlCmd = "SELECT COUNT(dtmSalesReconciliationDate) FROM tbl" + cu.terminal.intBusinessNumber + "SalesReconciliation "
                + "WHERE dtmSalesReconciliationDate = @dtmSalesReconciliationDate AND intStoreLocationID = @intStoreLocationID AND "
                + "bitIsProcessed = 1";
            object[][] parms =
            {
                new object[] { "@dtmSalesReconciliationDate", selectedDate },
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                cashoutExists = true;
            }
            return cashoutExists;
        }
        public bool DailyReconciliationExists(DateTime selectedDate, int storeLocationID, CurrentUser cu)
        {
            bool cashoutExists = false;
            string sqlCmd = "SELECT COUNT(dtmSalesReconciliationDate) FROM tbl" + cu.terminal.intBusinessNumber + "SalesReconciliation "
                + "WHERE dtmSalesReconciliationDate = @dtmSalesReconciliationDate AND intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@dtmSalesReconciliationDate", selectedDate },
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                cashoutExists = true;
            }
            return cashoutExists;
        }

        private void RemoveReturnsFromReceiptCurrentTable(int receiptID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptCurrent WHERE intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReturnsFromReceiptItemCurrentTable(int receiptID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemCurrent WHERE intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReturnsFromReceiptPaymentCurrentTable(int receiptID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptPaymentCurrent WHERE intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveReturnsFromReceiptItemTaxesCurrentTable(int receiptID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "ReceiptItemTaxesCurrent WHERE intReceiptID = @intReceiptID";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReturnsFromReceiptCurrentToVoidCancel(Receipt receipt, DateTime cancelDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptVoidCancel VALUES(@intReceiptID, @intReceiptGroupID, "
                + "@varReceiptNumber, @dtmReceiptCreationDate, @dtmReceiptCreationTime, @dtmReceiptVoidCancelDate, @dtmReceiptVoidCancelTime, "
                + "@intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, @fltCostTotal, @fltCartTotal, @fltDiscountTotal, "
                + "@fltTradeInTotal, @fltShippingTotal, @intTransactionTypeID, @varReceiptComments, @fltTenderedAmount, @fltChangeAmount, "
                + "@bitReceiptVoided, @bitReceiptCancelled)";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receipt.intReceiptID },
                new object[] { "@intReceiptGroupID", receipt.intReceiptGroupID },
                new object[] { "@varReceiptNumber", receipt.varReceiptNumber },
                new object[] { "@dtmReceiptCreationDate", receipt.dtmReceiptCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmReceiptCreationTime", receipt.dtmReceiptCreationTime.ToString("HH:mm:ss") },
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
                new object[] { "@bitReceiptVoided", 1 },
                new object[] { "@bitReceiptCancelled", 0 }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReturnsFromReceiptItemCurrentToVoidCanecl(ReceiptItem receiptItem, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemVoidCancel VALUES(@intReceiptID, @intInventoryID, "
                + "@intItemQuantity, @fltItemAverageCostAtSale, @fltItemPrice, @fltItemDiscount, @fltItemRefund, @bitIsPercentageDiscount, "
                + "@bitIsNonStockedProduct, @bitIsRegularProduct, @bitIsTradeIn, @varItemDescription)";
            object[][] parms =
            {
                new object[] { "@intReceiptID", receiptItem.intReceiptID },
                new object[] { "@intInventoryID", receiptItem.intInventoryID },
                new object[] { "@intItemQuantity", receiptItem.intItemQuantity },
                new object[] { "@fltItemAverageCostAtSale", receiptItem.fltItemAverageCostAtSale },
                new object[] { "@fltItemPrice", receiptItem.fltItemPrice },
                new object[] { "@fltItemDiscount", receiptItem.fltItemDiscount },
                new object[] { "@fltItemRefund", receiptItem.fltItemRefund },
                new object[] { "@bitIsPercentageDiscount", receiptItem.bitIsPercentageDiscount },
                new object[] { "@bitIsNonStockedProduct", receiptItem.bitIsNonStockedProduct },
                new object[] { "@bitIsRegularProduct", receiptItem.bitIsRegularProduct },
                new object[] { "@bitIsTradeIn", receiptItem.bitIsTradeIn },
                new object[] { "@varItemDescription", receiptItem.varItemDescription }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReturnsFromReceiptPaymentCurrentToVoidCancel(ReceiptPayment receiptPayment, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptPaymentVoidCancel VALUES(@intPaymentID, "
                + "@intReceiptID, @intMethodOfPaymentID, @fltAmountPaid)";
            object[][] parms =
            {
                new object[] { "@intPaymentID", receiptPayment.intPaymentID },
                new object[] { "@intReceiptID", receiptPayment.intReceiptID },
                new object[] { "@intMethodOfPaymentID", receiptPayment.intMethodOfPaymentID },
                new object[] { "@fltAmountPaid", receiptPayment.fltAmountPaid }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertReturnsFromReceiptItemTaxesCurrentToVoidCancel(ReceiptItemTax receiptItemTax, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReceiptItemTaxesVoidCancel VALUES("
                + "@intReceiptItemID, @intTaxTypeID, @fltTaxAmount, @bitIsTaxCharged)";
            object[][] parms =
            {
                new object[] { "@intReceiptItemID", receiptItemTax.intReceiptItemID },
                new object[] { "@intTaxTypeID", receiptItemTax.intTaxTypeID },
                new object[] { "@fltTaxAmount", receiptItemTax.fltTaxAmount },
                new object[] { "@bitIsTaxCharged", receiptItemTax.bitIsTaxCharged }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void ProcessTerminalReconciliationToTillCashoutTable(TillCashout tillCashout, DateTime processDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "TillCashout VALUES(@intTerminalID, @dtmTillCashoutDate, "
                + "@dtmTillCashoutProcessedDate, @dtmTillCashoutProcessedTime, @intHundredDollarBillCount, "
                + "@intFiftyDollarBillCount, @intTwentyDollarBillCount, @intTenDollarBillCount, @intFiveDollarBillCount, "
                + "@intToonieCoinCount, @intLoonieCoinCount, @intQuarterCoinCount, @intDimeCoinCount, @intNickelCoinCount, "
                + "@fltCashDrawerTotal, @fltCountedTotal, @fltCashDrawerFloat, @fltCashDrawerCashDrop, @bitIsProcessed, "
                + "@bitIsFinalized)";

            object[][] parms =
            {
                new object[] { "@intTerminalID", tillCashout.intTerminalID },
                new object[] { "@dtmTillCashoutDate", tillCashout.dtmTillCashoutDate },
                new object[] { "@dtmTillCashoutProcessedDate", processDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmTillCashoutProcessedTime", processDateTime.ToString("HH:mm:ss") },
                new object[] { "@intHundredDollarBillCount", tillCashout.intHundredDollarBillCount },
                new object[] { "@intFiftyDollarBillCount", tillCashout.intFiftyDollarBillCount },
                new object[] { "@intTwentyDollarBillCount", tillCashout.intTwentyDollarBillCount },
                new object[] { "@intTenDollarBillCount", tillCashout.intTenDollarBillCount },
                new object[] { "@intFiveDollarBillCount", tillCashout.intFiveDollarBillCount },
                new object[] { "@intToonieCoinCount", tillCashout.intToonieCoinCount },
                new object[] { "@intLoonieCoinCount", tillCashout.intLoonieCoinCount },
                new object[] { "@intQuarterCoinCount", tillCashout.intQuarterCoinCount },
                new object[] { "@intDimeCoinCount", tillCashout.intDimeCoinCount },
                new object[] { "@intNickelCoinCount", tillCashout.intNickelCoinCount },
                new object[] { "@fltCashDrawerTotal", tillCashout.fltCashDrawerTotal },
                new object[] { "@fltCountedTotal", tillCashout.fltCountedTotal },
                new object[] { "@fltCashDrawerFloat", tillCashout.fltCashDrawerFloat },
                new object[] { "@fltCashDrawerCashDrop", tillCashout.fltCashDrawerCashDrop },
                new object[] { "@bitIsProcessed", false },
                new object[] { "@bitIsFinalized", false }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void UpdateTerminalReconciliationToTillCashoutTable(TillCashout tillCashout, DateTime processDateTime, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "TillCashout SET dtmTillCashoutProcessedDate = @dtmTillCashoutProcessedDate, "
                + "dtmTillCashoutProcessedTime = @dtmTillCashoutProcessedTime, intHundredDollarBillCount = @intHundredDollarBillCount, "
                + "intFiftyDollarBillCount = @intFiftyDollarBillCount, intTwentyDollarBillCount = @intTwentyDollarBillCount, "
                + "intTenDollarBillCount = @intTenDollarBillCount, intFiveDollarBillCount = @intFiveDollarBillCount, intToonieCoinCount "
                + "= @intToonieCoinCount, intLoonieCoinCount = @intLoonieCoinCount, intQuarterCoinCount = @intQuarterCoinCount, "
                + "intDimeCoinCount = @intDimeCoinCount, intNickelCoinCount = @intNickelCoinCount, fltCashDrawerTotal = "
                + "@fltCashDrawerTotal, fltCountedTotal = @fltCountedTotal, fltCashDrawerFloat = @fltCashDrawerFloat, "
                + "fltCashDrawerCashDrop = @fltCashDrawerCashDrop, bitIsProcessed = @bitIsProcessed WHERE intTerminalID = @intTerminalID "
                + "AND dtmTillCashoutDate = @dtmTillCashoutDate";

            object[][] parms =
            {
                new object[] { "@intTerminalID", tillCashout.intTerminalID },
                new object[] { "@dtmTillCashoutDate", tillCashout.dtmTillCashoutDate },
                new object[] { "@dtmTillCashoutProcessedDate", processDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmTillCashoutProcessedTime", processDateTime.ToString("HH:mm:ss") },
                new object[] { "@intHundredDollarBillCount", tillCashout.intHundredDollarBillCount },
                new object[] { "@intFiftyDollarBillCount", tillCashout.intFiftyDollarBillCount },
                new object[] { "@intTwentyDollarBillCount", tillCashout.intTwentyDollarBillCount },
                new object[] { "@intTenDollarBillCount", tillCashout.intTenDollarBillCount },
                new object[] { "@intFiveDollarBillCount", tillCashout.intFiveDollarBillCount },
                new object[] { "@intToonieCoinCount", tillCashout.intToonieCoinCount },
                new object[] { "@intLoonieCoinCount", tillCashout.intLoonieCoinCount },
                new object[] { "@intQuarterCoinCount", tillCashout.intQuarterCoinCount },
                new object[] { "@intDimeCoinCount", tillCashout.intDimeCoinCount },
                new object[] { "@intNickelCoinCount", tillCashout.intNickelCoinCount },
                new object[] { "@fltCashDrawerTotal", tillCashout.fltCashDrawerTotal },
                new object[] { "@fltCountedTotal", tillCashout.fltCountedTotal },
                new object[] { "@fltCashDrawerFloat", tillCashout.fltCashDrawerFloat },
                new object[] { "@fltCashDrawerCashDrop", tillCashout.fltCashDrawerCashDrop },
                new object[] { "@bitIsProcessed", tillCashout.bitIsProcessed }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void SetTillCashoutToUnprocess(int cashoutID, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "TillCashout SET bitIsProcessed = 0 "
                + "WHERE intTillCashoutID = @intTillCashoutID";
            object[][] parms =
            {
                new object[] { "@intTillCashoutID", cashoutID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void RemoveUnprocessedReturns(int storeLocationID, DateTime selectedDate, DateTime cancelDateTime, int businessNumber)
        {
            ReceiptManager RM = new ReceiptManager();
            List<Receipt> lstReceipt = RM.ReturnReceiptForRemovingUnProcessedReturns(storeLocationID, selectedDate, businessNumber);
            foreach (Receipt receipt in lstReceipt)
            {
                InsertReturnsFromReceiptCurrentToVoidCancel(receipt, cancelDateTime, businessNumber);
                foreach (ReceiptItem receiptItem in receipt.lstReceiptItem)
                {
                    InsertReturnsFromReceiptItemCurrentToVoidCanecl(receiptItem, businessNumber);
                }
                foreach (ReceiptItemTax receiptItemTax in receipt.lstReceiptItemTax)
                {
                    InsertReturnsFromReceiptItemTaxesCurrentToVoidCancel(receiptItemTax, businessNumber);
                }
                foreach (ReceiptPayment receiptPayment in receipt.lstReceiptPayment)
                {
                    InsertReturnsFromReceiptPaymentCurrentToVoidCancel(receiptPayment, businessNumber);
                }

                RemoveReturnsFromReceiptPaymentCurrentTable(receipt.intReceiptID, businessNumber);
                RemoveReturnsFromReceiptItemTaxesCurrentTable(receipt.intReceiptID, businessNumber);
                RemoveReturnsFromReceiptItemCurrentTable(receipt.intReceiptID, businessNumber);
                RemoveReturnsFromReceiptCurrentTable(receipt.intReceiptID, businessNumber);
            }
        }
        public void UpdateSalesReconciliation(SalesReconciliation salesReconciliation, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "SalesReconciliation SET fltAmericanExpressCounted = "
                + "@fltAmericanExpressCounted, fltChequeCounted = @fltChequeCounted, fltDebitCounted = @fltDebitCounted, "
                + "fltDiscoverCounted = @fltDiscoverCounted, fltGiftCardCounted = @fltGiftCardCounted, fltMastercardCounted = "
                + "@fltMastercardCounted, fltTradeInCounted = @fltTradeInCounted, fltVisaCounted = @fltVisaCounted, fltOverShort = "
                + "@fltOverShort, fltCashPurchases = @fltCashPurchases, bitIsProcessed = @bitIsProcessed, bitIsFinalized = "
                + "@bitIsFinalized WHERE intSalesReconciliationID = @intSalesReconciliationID";
            object[][] parms =
            {
                new object[] { "@fltAmericanExpressCounted", salesReconciliation.fltAmericanExpressCounted },
                new object[] { "@fltChequeCounted", salesReconciliation.fltChequeCounted },
                new object[] { "@fltDebitCounted", salesReconciliation.fltDebitCounted },
                new object[] { "@fltDiscoverCounted", salesReconciliation.fltDiscoverCounted },
                new object[] { "@fltGiftCardCounted", salesReconciliation.fltGiftCardCounted },
                new object[] { "@fltMastercardCounted", salesReconciliation.fltMastercardCounted },
                new object[] { "@fltTradeInCounted", salesReconciliation.fltTradeInCounted },
                new object[] { "@fltVisaCounted", salesReconciliation.fltVisaCounted },
                new object[] { "@fltOverShort", salesReconciliation.fltOverShort },
                new object[] { "@fltCashPurchases", salesReconciliation.fltCashPurchases },
                new object[] { "@bitIsProcessed", salesReconciliation.bitIsProcessed },
                new object[] { "@bitIsFinalized", salesReconciliation.bitIsFinalized },
                new object[] { "@intSalesReconciliationID", salesReconciliation.intSalesReconciliationID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void FinalizeDailyReconciliation(int salesReconciliationID, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "SalesReconciliation SET bitIsFinalized = 1 "
                + "WHERE intSalesReconciliationID = @intSalesReconciliationID";
            object[][] parms =
            {
                new object[] { "@intSalesReconciliationID", salesReconciliationID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void ProcessTerminalReconciliation(TillCashout tillCashout, DateTime processDateTime, int businessNumber)
        {
            ProcessTerminalReconciliationToTillCashoutTable(tillCashout, processDateTime, businessNumber);
        }
        public void UpdateTerminalReconciliation(TillCashout tillCashout, DateTime processDateTime, int businessNumber)
        {
            UpdateTerminalReconciliationToTillCashoutTable(tillCashout, processDateTime, businessNumber);
        }
        public void UnProcessTillCashout(int cashoutID, CurrentUser cu)
        {
            SetTillCashoutToUnprocess(cashoutID, cu);
        }
    }
    public class SalesRecCheck
    {
        public int intTerminalID { get; set; }
        public int intReceiptCount { get; set; }
        public int intCompletedCashout { get; set; }

        public SalesRecCheck() { }
    }
}