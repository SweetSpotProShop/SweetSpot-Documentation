using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    [Serializable]
    public class Inventory
    {
        public int intInventoryID { get; set; }
        public string varSku { get; set; }
        public int intBrandID { get; set; }
        public string varModelName { get; set; }
        public string varDescription { get; set; }
        public int intStoreLocationID { get; set; }
        public StoreLocation storeLocation { get; set; }
        public string varUPCcode { get; set; }
        public int intQuantity { get; set; }
        public double fltPrice { get; set; }
        public double fltAverageCost { get; set; }
        public bool bitIsNonStockedProduct { get; set; }
        public bool bitIsRegularProduct { get; set; }
        public bool bitIsUsedProduct { get; set; }
        public bool bitIsActiveProduct { get; set; }
        public DateTime dtmCreationDate { get; set; }
        public string varAdditionalInformation { get; set; }
        public List<TaxTypePerInventoryItem> lstTaxTypePerInventoryItem { get; set; }

        public Inventory() { }
    }
}