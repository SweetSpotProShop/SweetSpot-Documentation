using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class PurchaseOrderItem
    {
        public int intPurchaseOrderItemID { get; set; }
        public int intPurchaseOrderID { get; set; }
        public int intVendorSupplierProductID { get; set; }
        public string varSku { get; set; }
        public string varVendorSku { get; set; }
        public int intPurchaseOrderQuantity { get; set; }
        public int intReceivedQuantity { get; set; }
        public double fltPurchaseOrderCost { get; set; }
        public double fltReceivedCost { get; set; }
        public string varItemDescription { get; set; }

        public PurchaseOrderItem() { }
    }
}