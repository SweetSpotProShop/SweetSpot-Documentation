using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    [Serializable]
    public class StoreLocation
    {
        public int intStoreLocationID { get; set; }
        public string varStoreName { get; set; }
        public string varPhoneNumber { get; set; }
        public string varAddress { get; set; }
        public string varEmailAddress { get; set; }
        public string varCityName { get; set; }
        public int intProvinceID { get; set; }
        public int intCountryID { get; set; }
        public string varPostalCode { get; set; }
        public string varTaxNumber { get; set; }
        public string varStoreCode { get; set; }
        public bool bitIsRetailLocation { get; set; }

        public StoreLocation() { }
    }
}