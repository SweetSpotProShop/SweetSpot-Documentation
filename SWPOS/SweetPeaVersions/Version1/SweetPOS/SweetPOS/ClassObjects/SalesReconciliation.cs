using System;

namespace SweetPOS.ClassObjects
{
    public class SalesReconciliation
    {
        public int intSalesReconciliationID { get; set; }
        public DateTime dtmSalesReconciliationDate { get; set; }
        public DateTime dtmSalesReconciliationProcessedDate { get; set; }
        public DateTime dtmSalesReconciliationProcessedTime { get; set; }
        public double fltAmericanExpressSales { get; set; }
        public double fltCashSales { get; set; }
        public double fltChequeSales { get; set; }
        public double fltDebitSales { get; set; }
        public double fltDiscoverSales { get; set; }
        public double fltGiftCardSales { get; set; }
        public double fltMastercardSales { get; set; }
        public double fltTradeInSales { get; set; }
        public double fltVisaSales { get; set; }
        public double fltAmericanExpressCounted { get; set; }
        public double fltCashCounted { get; set; }
        public double fltChequeCounted { get; set; }
        public double fltDebitCounted { get; set; }
        public double fltDiscoverCounted { get; set; }
        public double fltGiftCardCounted { get; set; }
        public double fltMastercardCounted { get; set; }
        public double fltTradeInCounted { get; set; }
        public double fltVisaCounted { get; set; }
        public double fltPreTaxSalesTotal { get; set; }
        public double fltGovernmentTaxTotal { get; set; }
        public double fltProvincialTaxTotal { get; set; }
        public double fltOverShort { get; set; }
        public double fltCashPurchases { get; set; }
        public bool bitIsFinalized { get; set; }
        public bool bitIsProcessed { get; set; }
        public int intStoreLocationID { get; set; }
        public int intEmployeeID { get; set; }

        public SalesReconciliation() { }
    }
}