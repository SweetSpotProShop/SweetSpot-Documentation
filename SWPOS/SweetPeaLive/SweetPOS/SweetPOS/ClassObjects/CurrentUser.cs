using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class CurrentUser
    {
        //This object stores the current user using the application
        public Terminal terminal { get; set; }
        public Employee employee { get; set; }
        public int intEFSHJEMVIF { get; set; }
        public StoreLocation currentStoreLocation { get; set; }

        public CurrentUser() { }        
    }
}