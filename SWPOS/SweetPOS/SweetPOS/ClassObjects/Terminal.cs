namespace SweetPOS.ClassObjects
{
    public class Terminal
    {
        public int intTerminalID { get; set; }
        public int intStoreLocationID { get; set; }
        public int intLicenceID { get; set; }
        public int intBusinessNumber { get; set; }
        public int intTillNumber { get; set; }
        public string varLicenceNumber { get; set; }
        public double fltDrawerFloatAmount { get; set; }

        public Terminal() { }
    }
}