using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class PurchaseOrder
    {

        public int intPurchaseOrderID { get; set; }
        public int intPurchaseOrderGroupID { get; set; }
        public string varPurchaseOrderNumber { get; set; }
        public DateTime dtmPurchaseOrderCreationDate { get; set; }
        public DateTime dtmPurchaseOrderCreationTime { get; set; }
        public DateTime dtmPurchaseOrderCompletionDate { get; set; }
        public DateTime dtmPurchaseOrderCompletionTime { get; set; }
        public VendorSupplier vendorSupplier { get; set; }
        public Employee employee { get; set; }
        public StoreLocation storeLocation { get; set; }
        public int intTerminalID { get; set; }
        public List<PurchaseOrderItem> lstPurchaseOrderItem { get; set; }
        public List<PurchaseOrderItem> lstPurchaseOrderReceivedItem { get; set; }
        public List<PurchaseOrderItemTax> lstPurchaseOrderItemTax { get; set; }
        public List<PurchaseOrderItemTax> lstPurchaseOrderReceivedItemTax { get; set; }
        public double fltCostSubTotal { get; set; }
        public double fltGSTTotal { get; set; }
        public double fltPSTTotal { get; set; }
        public bool bitGSTCharged { get; set; }
        public bool bitPSTCharged { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varPurchaseOrderComments { get; set; }

        public PurchaseOrder() { }
    }
}