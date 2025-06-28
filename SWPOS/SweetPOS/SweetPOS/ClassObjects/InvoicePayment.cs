using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class InvoicePayment
    {
        public int intPaymentID { get; set; }
        public int intInvoiceID { get; set; }
        public int intMethodOfPaymentID { get; set; }
        public string varMethodOfPaymentName { get; set; }
        public double fltAmountReceived { get; set; }
        public int intChequeNumber { get; set; }

        public InvoicePayment() { }
    }
}