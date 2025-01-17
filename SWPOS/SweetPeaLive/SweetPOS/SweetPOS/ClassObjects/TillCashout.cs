using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class TillCashout
    {
        public int intTerminalID { get; set; }
        public DateTime dtmTillCashoutDate { get; set; }
        public DateTime dtmTillCashoutProcessedDate { get; set; }
        public DateTime dtmTillCashoutProcessedTime { get; set; }
        public int intHundredDollarBillCount{ get; set; }
        public int intFiftyDollarBillCount { get; set; }
        public int intTwentyDollarBillCount{ get; set; }
        public int intTenDollarBillCount { get; set; }
        public int intFiveDollarBillCount { get; set; }
        public int intToonieCoinCount { get; set; }
        public int intLoonieCoinCount { get; set; }
        public int intQuarterCoinCount { get; set; }
        public int intDimeCoinCount { get; set; }
        public int intNickelCoinCount { get; set; }
        public double fltCashDrawerTotal { get; set; }
        public double fltCountedTotal { get; set; }
        public double fltCashDrawerFloat { get; set; }
        public double fltCashDrawerCashDrop { get; set; }
        public bool bitIsProcessed { get; set; }
        public bool bitIsFinalized { get; set; }

        public TillCashout() { }
    }
}