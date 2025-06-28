using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class ProvinceTax
    {
        public int intProvinceID { get; set; }
        public int intTaxTypeID { get; set; }
        public string varTaxName { get; set; }
        public DateTime dtmTaxEffectiveDate { get; set; }
        public double fltTaxRate { get; set; }

        public ProvinceTax() { }
    }
    public class ReceiptItemTax
    {
        public int intReceiptItemID { get; set; }
        public int intTaxTypeID { get; set; }
        public double fltTaxAmount { get; set; }
        public bool bitIsTaxCharged { get; set; }

        public ReceiptItemTax() { }
    }
    public class TaxTypePerInventoryItem
    {
        public int intInventoryID { get; set; }
        public int intTaxTypeID { get; set; }
        public string varTaxName { get; set; }
        public double fltTaxRate { get; set; }
        public bool bitChargeTax { get; set; }

        public TaxTypePerInventoryItem() { }
    }
    public class TaxRate
    {
        public int intTaxTypeID { get; set; }
        public string varTaxName { get; set; }
        public DateTime dtmTaxEffectiveDate { get; set; }
        public double fltTaxRate { get; set; }

        public TaxRate() { }
    }
}