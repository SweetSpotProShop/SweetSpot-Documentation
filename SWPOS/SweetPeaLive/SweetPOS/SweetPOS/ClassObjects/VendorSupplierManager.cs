using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class VendorSupplierManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<VendorSupplier> ConvertFromDataTableToVendorSupplier(DataTable dt)
        {
            List<VendorSupplier> vendorSupplier = dt.AsEnumerable().Select(row =>
            new VendorSupplier
            {
                intVendorSupplierID = row.Field<int>("intVendorSupplierID"),
                varVendorSupplierName = row.Field<string>("varVendorSupplierName"),
                varAddress = row.Field<string>("varAddress"),
                varMainPhoneNumber = row.Field<string>("varMainPhoneNumber"),
                varFaxNumber = row.Field<string>("varFaxNumber"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                //lstVendorSupplierContact = ReturnVendorSupplierContact(row.Field<int>("intVendorSupplierID")),
                //lstVendorSupplierProduct = ReturnVendorSupplierProduct(row.Field<int>("intVendorSupplierID")),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                varPostalCode = row.Field<string>("varPostalCode"),
                varVendorSupplierCode = row.Field<string>("varVendorSupplierCode")
            }).ToList();
            return vendorSupplier;
        }
        private List<VendorSupplier> ReturnVendorSupplierFromFullVendorSupplierDescription(object[][] parms)
        {
            string sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName, varAddress, "
                + "varMainPhoneNumber, varFaxNumber, varEmailAddress, intCityID, intProvinceID, "
                + "intCountryID, varPostalCode, varVendorSupplierCode FROM tblVendorSupplier "
                + "WHERE varVendorSupplierName = @varVendorSupplierName AND varAddress = "
                + "@varAddress AND varMainPhoneNumber = @varMainPhoneNumber AND varFaxNumber "
                + "= @varFaxNumber AND varEmailAddress = @varEmailAddress AND intCityID = "
                + "@intCityID AND intProvinceID = @intProvinceID AND intCountryID = @intCountryID "
                + "AND varPostalCode = @varPostalCode AND varVendorSupplierCode = @varVendorSupplierCode";

            return ConvertFromDataTableToVendorSupplier(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<VendorSupplier> ReturnVendorSupplierFromVendorSupplierID(int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName, varAddress, varMainPhoneNumber, varFaxNumber, "
                + "varEmailAddress, varCityName, intProvinceID, intCountryID, dtmCreationDate, varPostalCode, varVendorSupplierCode "
                + "FROM tbl" + businessNumber + "VendorSupplier WHERE intVendorSupplierID = @intVendorSupplierID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID }
            };
            return ConvertFromDataTableToVendorSupplier(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public List<VendorSupplier> ReturnVendorSupplier(int vendorSupplierID, int businessNumber)
        {
            return ReturnVendorSupplierFromVendorSupplierID(vendorSupplierID, businessNumber);
        }
        public List<VendorSupplier> StoreVendorInformation(VendorSupplier vendorSupplier)
        {
            string sqlCmd = "INSERT INTO tblVendorSupplier VALUES(@varVendorSupplierName, @varAddress, @varMainPhoneNumber, "
                + "@varFaxNumber, @varEmailAddress, @varCityName, @intProvinceID, @intCountryID, @varPostalCode, @varVendorSupplierCode)";

            object[][] parms =
            {
                new object[] { "@varVendorSupplierName", vendorSupplier.varVendorSupplierName },
                new object[] { "@varAddress", vendorSupplier.varAddress },
                new object[] { "@varMainPhoneNumber", vendorSupplier.varMainPhoneNumber },
                new object[] { "@varFaxNumber", vendorSupplier.varFaxNumber },
                new object[] { "@varEmailAddress", vendorSupplier.varEmailAddress },
                new object[] { "@varCityName", vendorSupplier.varCityName },
                new object[] { "@intProvinceID", vendorSupplier.intProvinceID },
                new object[] { "@intCountryID", vendorSupplier.intCountryID },
                new object[] { "@varPostalCode", vendorSupplier.varPostalCode },
                new object[] { "@varVendorSupplierCode", vendorSupplier.varVendorSupplierCode }
            };

            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnVendorSupplierFromFullVendorSupplierDescription(parms);
        }
        public List<VendorSupplier> UpdateVendorInformation(VendorSupplier vendorSupplier, int businessNumber)
        {
            string sqlCmd = "";
            if (vendorSupplier.intVendorSupplierID > 0)
            {
                sqlCmd += "UPDATE tblVendorSupplier SET varVendorSupplierName = @varVendorSupplierName, varAddress = @varAddress, "
                    + "varMainPhoneNumber = @varMainPhoneNumber, varFaxNumber = @varFaxNumber, varEmailAddress = @varEmailAddress, "
                    + "varCityName = @varCityName, intProvinceID = @intProvinceID, intCountryID = @intCountryID, varPostalCode = "
                    + "@varPostalCode, varVendorSupplierCode = @varVendorSupplierCode WHERE intVendorSupplierID = @intVendorSupplierID";
            }
            object[][] parms =
            {
                new object[] { "@varVendorSupplierName", vendorSupplier.varVendorSupplierName },
                new object[] { "@varAddress", vendorSupplier.varAddress },
                new object[] { "@varMainPhoneNumber", vendorSupplier.varMainPhoneNumber },
                new object[] { "@varFaxNumber", vendorSupplier.varFaxNumber },
                new object[] { "@varEmailAddress", vendorSupplier.varEmailAddress },
                new object[] { "@varCityName", vendorSupplier.varCityName },
                new object[] { "@intProvinceID", vendorSupplier.intProvinceID },
                new object[] { "@intCountryID", vendorSupplier.intCountryID },
                new object[] { "@varPostalCode", vendorSupplier.varPostalCode },
                new object[] { "@varVendorSupplierCode", vendorSupplier.varVendorSupplierCode },
                new object[] { "@intVendorSupplierID", vendorSupplier.intVendorSupplierID }
            };

            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnVendorSupplier(vendorSupplier.intVendorSupplierID, businessNumber);
        }

        private DataTable GatherVendorSupplierList(int businessNumber)
        {
            string sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName FROM tbl" + businessNumber + "VendorSupplier "
                + "ORDER BY varVendorSupplierName ASC";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnVendorSupplierList(int businessNumber)
        {
            return GatherVendorSupplierList(businessNumber);
        }


        //Not sure if this is needed or not, will keep available 
        public int ReturnVendorSupplierListDefaultSelected(int businessNumber)
        {
            return GatherVendorSupplierListDefaultSelected(businessNumber);
        }
        //Not sure if this is needed or not, will keep available
        private int GatherVendorSupplierListDefaultSelected(int businessNumber)
        {
            string sqlCmd = "SELECT MIN(intVendorSupplierID)FROM tbl" + businessNumber + "VendorSupplier";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        //private List<VendorSupplierContact> ConvertFromDataTableToVendorSupplierContact(DataTable dt)
        //{
        //    List<VendorSupplierContact> vendorSupplierContact = dt.AsEnumerable().Select(row =>
        //    new VendorSupplierContact
        //    {
        //        intVendorSupplierID = row.Field<int>("intVendorSupplierID"),
        //        varContactFirstName = row.Field<string>("varVendorSupplierName"),
        //        varContactLastName = row.Field<string>("varAddress"),
        //        varContactPhoneNumber = row.Field<string>("varMainPhoneNumber")
        //    }).ToList();
        //    return vendorSupplierContact;
        //}
        //public List<VendorSupplierContact> ReturnVendorSupplierContact(int vendorID)
        //{
        //    string sqlCmd = "SELECT intVendorSupplierID, varContactFirstName, varContactLastName, "
        //        + "varContactPhoneNumber FROM tblVendorSupplierContact WHERE intVendorSupplierID = "
        //        + "@intVendorSupplierID";
        //    object[][] parms =
        //    {
        //        new object[] { "@intVendorSupplierID", vendorID }
        //    };
        //    return ConvertFromDataTableToVendorSupplierContact(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        //}
        //private List<VendorSupplierProduct> ConvertFromDataTableToVendorSupplierProduct(DataTable dt)
        //{
        //    List<VendorSupplierProduct> vendorSupplierProduct = dt.AsEnumerable().Select(row =>
        //    new VendorSupplierProduct
        //    {
        //        intVendorSupplierID = row.Field<int>("intVendorSupplierID"),
        //        intItemIndicatorID = row.Field<int>("intItemIndicatorID"),
        //        varSku = row.Field<string>("varSku"),
        //        varDescription = row.Field<string>("varDescription"),
        //        varVendorSupplierProductCode = row.Field<string>("varVendorSupplierProductCode"),
        //        intAverageCost = row.Field<int>("intAverageCost")
        //    }).ToList();
        //    return vendorSupplierProduct;
        //}
        //public List<VendorSupplierProduct> ReturnVendorSupplierProduct(int vendorID)
        //{
        //    string sqlCmd = "SELECT VSP.intVendorSupplierID, VSP.intItemIndicatorID, I.varSku, "
        //        + "I.varDescription, VSP.varVendorSupplierProductCode, I.intAverageCost "
        //        + "FROM tblVendorSupplierProduct VSP JOIN tblInventory I ON I.intItemIndicatorID "
        //        + "= VSP.intItemIndicatorID WHERE VSP.intVendorSupplierID = @intVendorSupplierID";
        //    object[][] parms =
        //    {
        //        new object[] { "@intVendorSupplierID", vendorID }
        //    };
        //    return ConvertFromDataTableToVendorSupplierProduct(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        //}
        //public void AddVendorSupplierForInventoryItem(VendorInventory vendorInventory)
        //{
        //    string sqlCmd = "INSERT INTO tblVendorSupplierProduct "
        //        + "VALUES(@intVendorSupplierID, @intItemIndicatorID, "
        //        + "@varVendorSupplierProductCode)";
        //    object[][] parms =
        //    {
        //        new object[] { "@intVendorSupplierID", vendorInventory.intVendorSupplierID },
        //        new object[] { "@intItemIndicatorID", vendorInventory.intItemIndicatorIDID },
        //        new object[] { "@varVendorSupplierProductCode", vendorInventory.varVendorSupplierProductCode }
        //    };
        //    DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        //}
        //public void RemoveVendorSupplierForInventoryItem(VendorInventory vendorInventory)
        //{
        //    string sqlCmd = "DELETE tblVendorSupplierProduct WHERE intVendorSupplierID = "
        //        + "@intVendorSupplierID AND intItemIndicatorID = @intItemIndicatorIDID";
        //    object[][] parms =
        //    {
        //        new object[] { "@intVendorSupplierID", vendorInventory.intVendorSupplierID },
        //        new object[] { "@intItemIndicatorIDID", vendorInventory.intItemIndicatorIDID }
        //    };
        //    DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        //}
    }
}