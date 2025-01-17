using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class ReportTools
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private void logReportCall(object[] reportLog, DateTime clickDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "ReportLogs VALUES(@reportID, "
                + "@dateClicked, @timeClicked, @employeeID, @locationID)";
            object[][] parms =
            {
                new object[] { "@reportID", Convert.ToInt32(reportLog[0]) },
                new object[] { "@dateClicked", clickDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@timeClicked", clickDateTime.ToString("HH:mm:ss") },
                new object[] { "@employeeID", Convert.ToInt32(reportLog[1]) },
                new object[] { "@locationID", Convert.ToInt32(reportLog[2]) }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void CallReportLogger(object[] reportLog, DateTime clickDateTime, int businessNumber)
        {
            logReportCall(reportLog, clickDateTime, businessNumber);
        }

        private bool CashoutInDateRange(object[] reportCriteria)
        {
            bool bolCIDR = false;
            DateTime[] dtm = (DateTime[])reportCriteria[0];
            CurrentUser cu = (CurrentUser)reportCriteria[2];
            string sqlCmd = "SELECT COUNT(dtmSalesReconciliationDate) FROM tbl" + cu.terminal.intBusinessNumber + "SalesReconciliation "
                        + "WHERE dtmSalesReconciliationDate BETWEEN @dtmStartDate AND @dtmEndDate AND intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@dtmStartDate", dtm[0].ToShortDateString() },
                new object[] { "@dtmEndDate", dtm[1].ToShortDateString() },
                new object[] { "@intStoreLocationID", Convert.ToInt32(reportCriteria[1]) }
            };
            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolCIDR = true;
            }
            return bolCIDR;
        }

        public int CashoutsProcessed(object[] reportCriteria)
        {
            int indicator = 0;
            if (!CashoutInDateRange(reportCriteria))
            {
                indicator = 1;
            }
            return indicator;
        }

        public DataTable ReturnSameDaySales(DateTime selectedDate, CurrentUser cu)
        {            
            //Gets a list of all receipts based on date and location
            string sqlCmd = "SELECT R.intReceiptID, R.varReceiptNumber, R.intReceiptSubNumber, CONCAT(C.varLastName, ', ', C.varFirstName) AS "
                + "varCustomerFullName, CONCAT(E.varLastName, ', ', E.varFirstName) AS varEmployeeFullName, R.fltDiscountTotal, R.fltTradeInTotal, "
                + "(TS.fltSubTotal + R.fltShippingTotal - R.fltDiscountTotal) AS fltSubTotal, ISNULL(T.fltGovernmentTax, 0) AS fltGovernmentTax, "
                + "ISNULL(T.fltProvincialTax, 0) AS fltProvincialTax, (TS.fltSubTotal + R.fltShippingTotal - R.fltDiscountTotal + "
                + "ISNULL(T.fltGovernmentTax, 0) + ISNULL(T.fltProvincialTax, 0)) AS fltReceiptTotal, M.varMethodOfPaymentName, M.fltAmountPaid, "
                + "R.intTransactionTypeID FROM tbl" + cu.terminal.intBusinessNumber + "Receipt R INNER JOIN (SELECT RP.intReceiptID, "
                + "MP.varMethodOfPaymentName, RP.fltAmountPaid FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptPayment RP JOIN "
                + "tblMethodOfPayment MP ON MP.intMethodOfPaymentID = RP.intMethodOfPaymentID) M ON M.intReceiptID = R.intReceiptID JOIN (SELECT "
                + "RI5.intReceiptID, SUM((CASE WHEN R5.intTransactionTypeID = 1 THEN RI5.fltItemPrice * RI5.intItemQuantity ELSE RI5.fltItemRefund "
                + "* RI5.intItemQuantity END)) AS fltSubTotal FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptItem RI5 JOIN tbl" 
                + cu.terminal.intBusinessNumber + "Receipt R5 ON R5.intReceiptID = RI5.intReceiptID GROUP BY RI5.intReceiptID) TS ON "
                + "TS.intReceiptID = R.intReceiptID JOIN tbl" + cu.terminal.intBusinessNumber + "Customer C ON C.intCustomerID = "
                + "R.intCustomerID JOIN tbl" + cu.terminal.intBusinessNumber + "Employee E ON E.intEmployeeID = R.intEmployeeID LEFT JOIN "
                + "(SELECT RI.intReceiptID, (SUM(fltGST) +SUM(fltHST) + SUM(fltQST)) AS fltGovernmentTax, (SUM(fltPST) + SUM(fltRST)) AS "
                + "fltProvincialTax FROM (SELECT intReceiptItemID, ISNULL([1], 0) AS fltGST, ISNULL([2], 0) AS fltHST, ISNULL([3], 0) AS fltPST, "
                + "ISNULL([4], 0) AS fltRST, ISNULL([5], 0) AS fltQST FROM (SELECT intReceiptItemID, intTaxTypeID, SUM(fltTaxAmount) AS "
                + "fltTaxTotal FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptItemTaxes GROUP BY intReceiptItemID, intTaxTypeID) PS "
                + "PIVOT(SUM(fltTaxTotal) FOR intTaxTypeID IN([1], [2], [3], [4], [5])) AS PVT) TN JOIN tbl" + cu.terminal.intBusinessNumber 
                + "ReceiptItem RI ON RI.intReceiptItemID = TN.intReceiptItemID GROUP BY RI.intReceiptID) T ON T.intReceiptID = R.intReceiptID "
                + "WHERE R.dtmReceiptCompletionDate BETWEEN @dtmStartDate AND @dtmEndDate AND R.intStoreLocationID = @intStoreLocationID";

            object[][] parms =
            {
                 new object[] { "@dtmStartDate", selectedDate.ToString("yyyy-MM-dd") },
                 new object[] { "@dtmEndDate", selectedDate.ToString("yyyy-MM-dd") },
                 new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnCurrentOpenReceipt(CurrentUser cu)
        {
            string sqlCmd = "SELECT R.intReceiptID, R.varReceiptNumber, R.intReceiptSubNumber, dtmReceiptCreationDate, CONCAT(C.varLastName, "
                + "', ', C.varFirstName) AS varCustomerFullName, R.intCustomerID, CONCAT(E.varLastName, ', ', E.varFirstName) AS "
                + "varEmployeeFullName, R.fltDiscountTotal, R.fltTradeInTotal, (R.fltCartTotal + R.fltTradeInTotal + fltShippingTotal - "
                + "R.fltDiscountTotal) AS fltSubTotal, ISNULL(T.fltGovernmentTax, 0) AS fltGovernmentTax, ISNULL(T.fltProvincialTax, 0) AS "
                + "fltProvincialTax, (R.fltCartTotal + R.fltTradeInTotal + R.fltShippingTotal - R.fltDiscountTotal + ISNULL(T.fltGovernmentTax, "
                + "0) + ISNULL(T.fltProvincialTax, 0)) AS fltReceiptTotal, TN.varTransactionName, TN.intTransactionTypeID FROM tbl" 
                + cu.terminal.intBusinessNumber + "ReceiptCurrent R JOIN tblTransactionType TN ON TN.intTransactionTypeID = "
                + "R.intTransactionTypeID JOIN tbl" + cu.terminal.intBusinessNumber + "Customer C ON C.intCustomerID = R.intCustomerID "
                + "JOIN tbl" + cu.terminal.intBusinessNumber + "Employee E ON E.intEmployeeID = R.intEmployeeID LEFT JOIN (SELECT "
                + "RIC.intReceiptID, (SUM(fltGST) +SUM(fltHST) + SUM(fltQST)) AS fltGovernmentTax, (SUM(fltPST) + SUM(fltRST)) AS "
                + "fltProvincialTax FROM (SELECT intReceiptItemID, ISNULL([1], 0) AS fltGST, ISNULL([2], 0) AS fltHST, ISNULL([3], 0) AS "
                + "fltPST, ISNULL([4], 0) AS fltRST, ISNULL([5], 0) AS fltQST FROM (SELECT intReceiptItemID, intTaxTypeID, SUM(fltTaxAmount) AS "
                + "fltTaxTotal FROM tbl" + cu.terminal.intBusinessNumber + "ReceiptItemTaxesCurrent GROUP BY intReceiptItemID, "
                + "intTaxTypeID) PS PIVOT (SUM(fltTaxTotal) FOR intTaxTypeID IN([1], [2], [3], [4], [5])) AS PVT) TN JOIN tbl" 
                + cu.terminal.intBusinessNumber + "ReceiptItemCurrent RIC ON RIC.intReceiptItemID = TN.intReceiptItemID GROUP BY "
                + "RIC.intReceiptID) T ON T.intReceiptID = R.intReceiptID WHERE R.intStoreLocationID = @intStoreLocationID";

            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
    }
}