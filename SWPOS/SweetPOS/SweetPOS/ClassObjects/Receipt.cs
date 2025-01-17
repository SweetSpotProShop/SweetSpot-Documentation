using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class Receipt
    {
        public int intReceiptID { get; set; }
        public int intReceiptGroupID { get; set; }
        public string varReceiptNumber { get; set; }
        public int intReceiptSubNumber { get; set; }
        public DateTime dtmReceiptCreationDate { get; set; }
        public DateTime dtmReceiptCreationTime { get; set; }
        public DateTime dtmReceiptCompletionDate { get; set; }
        public DateTime dtmReceiptCompletionTime { get; set; }
        public DateTime dtmReceiptVoidCancelDate { get; set; }
        public DateTime dtmReceiptVoidCancelTime { get; set; }
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public StoreLocation storeLocation { get; set; }
        public int intTerminalID { get; set; }
        public double fltCostTotal { get; set; }
        public double fltCartTotal { get; set; }
        public double fltDiscountTotal { get; set; }
        public double fltTradeInTotal { get; set; }
        public double fltShippingTotal { get; set; }
        public double fltBalanceDueTotal { get; set; }
        public double fltGovernmentTaxTotal { get; set; }
        public double fltProvincialTaxTotal { get; set; }
        public bool bitIsGSTCharged { get; set; }
        public bool bitIsPSTCharged { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varReceiptComments { get; set; }
        public double fltTenderedAmount { get; set; }
        public double fltChangeAmount { get; set; }
        public bool bitIsReceiptVoided { get; set; }
        public bool bitIsReceiptCancelled { get; set; }
        public List<ReceiptItem> lstReceiptItem { get; set; }
        public List<ReceiptPayment> lstReceiptPayment { get; set; }
        public List<ReceiptItemTax> lstReceiptItemTax { get; set; }

        public Receipt() { }
    }
}