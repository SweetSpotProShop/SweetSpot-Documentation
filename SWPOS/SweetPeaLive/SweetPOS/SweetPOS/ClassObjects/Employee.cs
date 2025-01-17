using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class Employee
    {
        //This object stores an Employee Information
        public int intEmployeeID { get; set; }
        public string varFirstName { get; set; }
        public string varLastName { get; set; }
        public int intJobCodeID { get; set; }
        //public StoreLocation storeLocation { get; set; }
        public string varAddress { get; set; }
        public string varHomePhone { get; set; }
        public string varMobilePhone { get; set; }
        public string varEmailAddress { get; set; }
        public string varCityName { get; set; }
        public int intProvinceID { get; set; }
        public int intCountryID { get; set; }
        public DateTime dtmCreationDate { get; set; }
        public string varPostalCode { get; set; }
        public bool bitIsEmployeeActive { get; set; }

        public Employee() { }
    }
}