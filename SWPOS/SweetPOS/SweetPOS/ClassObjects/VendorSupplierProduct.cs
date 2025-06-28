using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class VendorSupplierProduct
    {
        public int intVendorSupplierID { get; set; }
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public string varDescription { get; set; }
        public string varVendorSupplierProductCode { get; set; }
    }
}