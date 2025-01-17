using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class CustomerManager
    {
        //All procedures in regards to a Customer are stored here
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Customer> ConvertFromDataTableToCustomer(DataTable dt)
        {
            //This converts from a Datatable into a Customer object
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                intCustomerID = row.Field<int>("intCustomerID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                varAddress = row.Field<string>("varAddress"),
                varHomePhone = row.Field<string>("varHomePhone"),
                varMobilePhone = row.Field<string>("varMobilePhone"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                bitAllowMarketing = row.Field<bool>("bitAllowMarketing")
            }).ToList();
            return customer;
        }
        private List<Customer> ConvertFromDataTableToCustomerWithReceipts(DataTable dt, int businessNumber)
        {
            //This converts from a Datatable into a Customer object that also has that customer previous purchases
            ReceiptManager RM = new ReceiptManager();
            List<Customer> customer = dt.AsEnumerable().Select(row =>
            new Customer
            {
                intCustomerID = row.Field<int>("intCustomerID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                varAddress = row.Field<string>("varAddress"),
                varHomePhone = row.Field<string>("varHomePhone"),
                varMobilePhone = row.Field<string>("varMobilePhone"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                varPostalCode = row.Field<string>("varPostalCode"),
                lstReceipt = RM.ReturnReceiptByCustomer(row.Field<int>("intCustomerID"), businessNumber),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                bitAllowMarketing = row.Field<bool>("bitAllowMarketing")
            }).ToList();
            return customer;
        }

        public List<Customer> ReturnCustomer(int customerID, int businessNumber)
        {
            //This converts from a Datatable into a Customer object
            string sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, "
                + "varHomePhone, varMobilePhone, varEmailAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode, dtmCreationDate, bitAllowMarketing FROM "
                + "tbl" + businessNumber + "Customer WHERE intCustomerID = @intCustomerID";
            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID },
            };
            return ConvertFromDataTableToCustomer(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        public List<Customer> ReturnCustomerWithReceiptList(int customerID, int businessNumber)
        {
            //This converts from a Datatable into a Customer object that also has that customer previous purchases
            string sqlCmd = "SELECT intCustomerID, varFirstName, varLastName, varAddress, "
                + "varHomePhone, varMobilePhone, varEmailAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode, dtmCreationDate, bitAllowMarketing FROM "
                + "tbl" + businessNumber + "Customer WHERE intCustomerID = @intCustomerID";
            object[][] parms =
            {
                 new object[] { "@intCustomerID", customerID },
            };
            return ConvertFromDataTableToCustomerWithReceipts(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Customer> ReturnCustomerBasedOnText(string searchText, int businessNumber)
        {
            //This returns all customers that matches a search string
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    //need to include all 12 in select
                    sqlCmd = "SELECT intCustomerID, varLastName, varFirstName, varAddress, varHomePhone, "
                        + "varMobilePhone, varEmailAddress, varCityName, intProvinceID, intCountryID, "
                        + "varPostalCode, dtmCreationDate, bitAllowMarketing FROM tbl" + businessNumber
                        + "Customer WHERE (CAST(intCustomerID AS VARCHAR) LIKE @parm1" + i + " OR "
                        + "CONCAT(varFirstName, varLastName) LIKE @parm2" + i + " OR CONCAT(varHomePhone, "
                        + "varMobilePhone) LIKE @parm3" + i + " OR varEmailAddress LIKE @parm4" + i + ") "
                        + "AND intCustomerID NOT IN (SELECT intCustomerID FROM tbl" + businessNumber
                        + "GuestCustomerPerLocation)";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intCustomerID, varLastName, varFirstName, varAddress, varHomePhone, "
                        + "varMobilePhone, varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, "
                        + "dtmCreationDate, bitAllowMarketing FROM tbl" + businessNumber + "Customer WHERE "
                        + "(CAST(intCustomerID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName) "
                        + "LIKE @parm2" + i + " OR CONCAT(varHomePhone, varMobilePhone) LIKE @parm3" + i + " OR "
                        + "varEmailAddress LIKE @parm4" + i + ") AND intCustomerID NOT IN (SELECT intCustomerID FROM "
                        + "tbl" + businessNumber + "GuestCustomerPerLocation))";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY varLastName ASC";
            return ConvertFromDataTableToCustomer(DBC.MakeDatabaseCallToReturnDataTableFromArrayListFour(sqlCmd, parms, strText));
        }
       
        public int AddNewCustomer(Customer customer, DateTime createDateTime, int businessNumber)
        {
            //Creates a new customer and stores it in the Customer Table
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Customer VALUES(@varFirstName, @varLastName, "
                + "@varAddress, @varHomePhone, @varMobilePhone, @varEmailAddress, @varCityName, @intProvinceID, "
                + "@intCountryID, @varPostalCode, @dtmCreationDate, @bitAllowMarketing)";

            object[][] parms =
            {
                 new object[] { "@varFirstName", customer.varFirstName },
                 new object[] { "@varLastName", customer.varLastName },
                 new object[] { "@varAddress", customer.varAddress },
                 new object[] { "@varHomePhone", customer.varHomePhone },
                 new object[] { "@varMobilePhone", customer.varMobilePhone },
                 new object[] { "@varEmailAddress", customer.varEmailAddress },
                 new object[] { "@varCityName", customer.varCityName },
                 new object[] { "@intProvinceID", customer.intProvinceID },
                 new object[] { "@intCountryID", customer.intCountryID },
                 new object[] { "@varPostalCode", customer.varPostalCode },
                 new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                 new object[] { "@bitAllowMarketing", customer.bitAllowMarketing }
            };

            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnCustomerIDFromCustomerStats(parms, businessNumber);
        }
        public int ReturnCustomerIDFromCustomerStats(object[][] parms, int businessNumber)
        {
            //After creating a new customer this uses all the saved parameters to return the customer ID for ease of use.
            string sqlCmd = "SELECT intCustomerID FROM tbl" + businessNumber
                + "Customer WHERE varFirstName = @varFirstName "
                + "AND varLastName = @varLastName AND varAddress = @varAddress AND varHomePhone = "
                + "@varHomePhone AND varMobilePhone = @varMobilePhone AND varEmailAddress = "
                + "@varEmailAddress AND varCityName = @varCityName AND intProvinceID = "
                + "@intProvinceID AND intCountryID = @intCountryID AND varPostalCode = "
                + "@varPostalCode AND dtmCreationDate = @dtmCreationDate AND bitAllowMarketing "
                + "= @bitAllowMarketing";
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        public int ReturnGuestCustomerForLocation(int storeLocationID, int businessNumber)
        {
            //Returns the Guest Customer for a store location
            string sqlCmd = "SELECT intCustomerID FROM tbl" + businessNumber + "GuestCustomerPerLocation "
                + "WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public void UpdateCurrentCustomer(Customer customer, int businessNumber)
        {
            //Updates Customer information
            string sqlCmd = "UPDATE tbl" + businessNumber + "Customer SET varFirstName = @varFirstName, varLastName = "
                + "@varLastName, varAddress = @varAddress, varHomePhone = @varHomePhone, varMobilePhone = "
                + "@varMobilePhone, varEmailAddress = @varEmailAddress, varCityName = @varCityName, "
                + "intProvinceID = @intProvinceID, intCountryID = @intCountryID, varPostalCOde = "
                + "@varPostalCode, bitAllowMarketing = @bitAllowMarketing WHERE intCustomerID = "
                + "@intCustomerID";

            object[][] parms =
            {
                new object[] { "@intCustomerID", customer.intCustomerID },
                new object[] { "@varFirstName", customer.varFirstName },
                new object[] { "@varLastName", customer.varLastName },
                new object[] { "@varAddress", customer.varAddress },
                new object[] { "@varHomePhone", customer.varHomePhone },
                new object[] { "@varMobilePhone", customer.varMobilePhone },
                new object[] { "@varEmailAddress", customer.varEmailAddress },
                new object[] { "@varCityName", customer.varCityName },
                new object[] { "@intProvinceID", customer.intProvinceID },
                new object[] { "@intCountryID", customer.intCountryID },
                new object[] { "@varPostalCode", customer.varPostalCode },
                new object[] { "@bitAllowMarketing", customer.bitAllowMarketing }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}