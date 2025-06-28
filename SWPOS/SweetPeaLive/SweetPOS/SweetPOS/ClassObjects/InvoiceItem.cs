using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class InvoiceItem
    {
        public int intInvoiceItemID { get; set; }
        public int intInventoryID { get; set; }
        public int intInvoiceID { get; set; }
        public string varItemSku { get; set; }
        public int intItemQuantity { get; set; }
        public double fltItemCost { get; set; }
        public string varItemDescription { get; set; }

        public InvoiceItem() { }
    }
}