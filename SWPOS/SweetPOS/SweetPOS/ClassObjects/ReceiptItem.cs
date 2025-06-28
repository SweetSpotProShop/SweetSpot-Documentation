using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class ReceiptItem
    {
        public int intReceiptItemID { get; set; }
        public int intReceiptID { get; set; }
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public int intItemQuantity { get; set; }
        public double fltItemAverageCostAtSale { get; set; }
        public double fltItemPrice { get; set; }
        public double fltItemDiscount { get; set; }
        public double fltItemRefund { get; set; }
        public bool bitIsNonStockedProduct { get; set; }
        public bool bitIsRegularProduct { get; set; }
        public bool bitIsPercentageDiscount { get; set; }
        public bool bitIsTradeIn { get; set; }
        public string varItemDescription { get; set; }

        public ReceiptItem() { }
    }
}