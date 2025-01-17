using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class ReceiptPayment
    {
        public int intPaymentID { get; set; }
        public int intReceiptID { get; set; }
        public int intMethodOfPaymentID { get; set; }
        public string varMethodOfPaymentName { get; set; }
        public double fltAmountPaid { get; set; }

        public ReceiptPayment() { }
    }
}