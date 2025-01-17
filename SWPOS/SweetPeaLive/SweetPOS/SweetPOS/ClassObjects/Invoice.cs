using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class Invoice
    {
        public int intInvoiceID { get; set; }
        public string varInvoiceNumber { get; set; }
        public DateTime dtmInvoiceCreationDate { get; set; }
        public DateTime dtmInvoiceCreationTime { get; set; }
        public DateTime dtmInvoiceCompletionDate { get; set; }
        public DateTime dtmInvoiceCompletionTime { get; set; }
        public Customer customer { get; set; }
        public Employee employee { get; set; }
        public StoreLocation storeLocation { get; set; }
        public int intTerminalID { get; set; }
        public List<InvoiceItem> lstInvoiceItem { get; set; }
        public List<InvoicePayment> lstInvoicePayment { get; set; }
        public double fltCostTotal { get; set; }
        public int intTransactionTypeID { get; set; }
        public string varInvoiceComments { get; set; }
        public bool bitIsInvoiceVoided { get; set; }
        public bool bitIsInvoiceCancelled { get; set; }

        public Invoice() { }
    }
}