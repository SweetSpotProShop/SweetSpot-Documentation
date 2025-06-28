using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class PurchaseOrderItemTax
    {
        public int intPurchaseOrderItemID { get; set; }
        public int intTaxTypeID { get; set; }
        public double fltTaxAmount { get; set; }
        public bool bitIsTaxCharged { get; set; }

        public PurchaseOrderItemTax() { }
    }
}