using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class VendorSupplier
    {
        public int intVendorSupplierID { get; set; }
        public string varVendorSupplierName { get; set; }
        public string varAddress { get; set; }
        public string varMainPhoneNumber { get; set; }
        public string varFaxNumber { get; set; }
        public string varEmailAddress { get; set; }
        public string varCityName { get; set; }
        public int intProvinceID { get; set; }
        public int intCountryID { get; set; }
        public DateTime dtmCreationDate { get; set; }
        public string varPostalCode { get; set; }
        public string varVendorSupplierCode { get; set; }

        public VendorSupplier() { }
    }
}