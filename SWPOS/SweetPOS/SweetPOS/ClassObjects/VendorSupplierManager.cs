using System;
using System.Collections;
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
                varVendorSupplierCode = row.Field<string>("varVendorSupplierCode"),
                bitIsActive = row.Field<bool>("bitIsActive")
            }).ToList();
            return vendorSupplier;
        }
        private List<VendorSupplier> ConvertFromDataTableToVendorSupplierWithInventoryAndPO(DataTable dt, int businessNumber)
        {
            PurchaseOrderManager POM = new PurchaseOrderManager();
            List<VendorSupplier> vendorSupplier = dt.AsEnumerable().Select(row =>
            new VendorSupplier
            {
                intVendorSupplierID = row.Field<int>("intVendorSupplierID"),
                varVendorSupplierName = row.Field<string>("varVendorSupplierName"),
                varAddress = row.Field<string>("varAddress"),
                varMainPhoneNumber = row.Field<string>("varMainPhoneNumber"),
                varFaxNumber = row.Field<string>("varFaxNumber"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                varPostalCode = row.Field<string>("varPostalCode"),
                varVendorSupplierCode = row.Field<string>("varVendorSupplierCode"),
                bitIsActive = row.Field<bool>("bitIsActive"),
                lstVendorSupplierItems = ReturnVendorSupplierProduct(row.Field<int>("intVendorSupplierID"), businessNumber),
                lstPurchaseOrders = POM.ReturnVendorSupplierPurchaseOrders(row.Field<int>("intVendorSupplierID"), businessNumber)
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
                + "AND varPostalCode = @varPostalCode AND varVendorSupplierCode = "
                + "@varVendorSupplierCode AND bitIsActive = @bitIsActive";

            return ConvertFromDataTableToVendorSupplier(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<VendorSupplier> ReturnVendorSupplierFromVendorSupplierID(int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName, varAddress, varMainPhoneNumber, varFaxNumber, "
                + "varEmailAddress, varCityName, intProvinceID, intCountryID, dtmCreationDate, varPostalCode, varVendorSupplierCode, "
                + "bitIsActive FROM tbl" + businessNumber + "VendorSupplier WHERE intVendorSupplierID = @intVendorSupplierID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID }
            };
            return ConvertFromDataTableToVendorSupplier(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<VendorSupplier> ReturnVendorSupplierFromVendorSupplierSearchText(string searchText, int businessNumber)
        {
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    //need to include all 12 in select
                    sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName, varAddress, varMainPhoneNumber, varFaxNumber, "
                        + "varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, dtmCreationDate, "
                        + "varVendorSupplierCode, bitIsActive FROM tbl" + businessNumber + "VendorSupplier WHERE (CAST(intVendorSupplierID "
                        + "AS VARCHAR) LIKE @parm1" + i + " OR varVendorSupplierName LIKE @parm2" + i + " OR "
                        + "CONCAT(varMainPhoneNumber, varFaxNumber) LIKE @parm3" + i + " OR CONCAT(varEmailAddress, "
                        + "varVendorSupplierCode) LIKE @parm4" + i + ") ";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
                else
                {
                    sqlCmd += "INTERSECT (SELECT intVendorSupplierID, varVendorSupplierName, varAddress, varMainPhoneNumber, "
                        + "varFaxNumber, varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, dtmCreationDate, "
                        + "varVendorSupplierCode, bitIsActive FROM tbl" + businessNumber + "VendorSupplier WHERE (CAST(intVendorSupplierID "
                        + "AS VARCHAR) LIKE @parm1" + i + " OR varVendorSupplierName LIKE @parm2" + i + " OR "
                        + "CONCAT(varMainPhoneNumber, varFaxNumber) LIKE @parm3" + i + " OR CONCAT(varEmailAddress, "
                        + "varVendorSupplierCode) LIKE @parm4" + i + "))";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                    parms.Add("@parm3" + i);
                    parms.Add("@parm4" + i);
                }
            }
            sqlCmd += " ORDER BY varVendorSupplierName ASC";
            return ConvertFromDataTableToVendorSupplier(DBC.MakeDatabaseCallToReturnDataTableFromArrayListFour(sqlCmd, parms, strText));
        }
        private List<VendorSupplier> ReturnVendorSupplierFromVendorSupplierIDWithInventoryAndPO(int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "SELECT intVendorSupplierID, varVendorSupplierName, varAddress, varMainPhoneNumber, varFaxNumber, "
                + "varEmailAddress, varCityName, intProvinceID, intCountryID, dtmCreationDate, varPostalCode, varVendorSupplierCode, "
                + "bitIsActive FROM tbl" + businessNumber + "VendorSupplier WHERE intVendorSupplierID = @intVendorSupplierID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID }
            };
            return ConvertFromDataTableToVendorSupplierWithInventoryAndPO(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }

        public List<VendorSupplier> ReturnVendorSupplier(int vendorSupplierID, int businessNumber)
        {
            return ReturnVendorSupplierFromVendorSupplierID(vendorSupplierID, businessNumber);
        }
        public List<VendorSupplier> ReturnVendorSupplierBasedOnText(string searchText, int businessNumber)
        {
            return ReturnVendorSupplierFromVendorSupplierSearchText(searchText, businessNumber);
        }
        public List<VendorSupplier> AddNewVendorSupplier(VendorSupplier vendorSupplier, DateTime currentDate, int businessNumber)
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
                new object[] { "@varVendorSupplierCode", vendorSupplier.varVendorSupplierCode },
                new object[] { "@bitIsActive", vendorSupplier.bitIsActive }
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
                    + "@varPostalCode, varVendorSupplierCode = @varVendorSupplierCode, bitIsActive = @bitIsActive WHERE "
                    + "intVendorSupplierID = @intVendorSupplierID";
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
                new object[] { "@intVendorSupplierID", vendorSupplier.intVendorSupplierID },
                new object[] { "@bitIsActive", vendorSupplier.bitIsActive }
            };

            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnVendorSupplier(vendorSupplier.intVendorSupplierID, businessNumber);
        }
        public List<VendorSupplier> ReturnVendorSupplierWithInventoryAndPO(int vendorSupplierID, int businessNumber)
        {
            return ReturnVendorSupplierFromVendorSupplierIDWithInventoryAndPO(vendorSupplierID, businessNumber);
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
            string sqlCmd = "SELECT MIN(intVendorSupplierID)FROM tbl" + businessNumber + "VendorSupplier "
                + "WHERE bitIsActive = 1";
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
        private List<VendorSupplierProduct> ConvertFromDataTableToVendorSupplierProduct(DataTable dt)
        {
            List<VendorSupplierProduct> vendorSupplierProduct = dt.AsEnumerable().Select(row =>
            new VendorSupplierProduct
            {
                intVendorSupplierID = row.Field<int>("intVendorSupplierID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varSku = row.Field<string>("varSku"),
                varDescription = row.Field<string>("varDescription"),
                varVendorSupplierProductCode = row.Field<string>("varVendorSupplierProductCode")                
            }).ToList();
            return vendorSupplierProduct;
        }
        public List<VendorSupplierProduct> ReturnVendorSupplierProduct(int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "SELECT VSP.intVendorSupplierID, VSP.intInventoryID, I.varSku, I.varDescription, VSP.varVendorSupplierProductCode "
                + "FROM tbl" + businessNumber + "VendorSupplierProduct VSP JOIN tbl" + businessNumber + "Inventory I ON I.intInventoryID = "
                + "VSP.intInventoryID WHERE VSP.intVendorSupplierID = @intVendorSupplierID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID }
            };
            return ConvertFromDataTableToVendorSupplierProduct(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        public DataTable ReturnInventoryNotSuppliedByVendor(int vendorSupplierID, int businessNumber)
        {
            return GatherInventoryNotSuppliedByVendor(vendorSupplierID, businessNumber);
        }
        private DataTable GatherInventoryNotSuppliedByVendor(int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "SELECT I.intInventoryID, I.varDescription FROM tbl" + businessNumber + "Inventory I WHERE "
                + "I.intInventoryID NOT IN(SELECT intInventoryID FROM tbl" + businessNumber + "VendorSupplierProduct "
                + "WHERE intVendorSupplierID = @intVendorSupplierID) AND I.intInventoryID NOT IN(SELECT intInventoryID "
                + "FROM tbl" + businessNumber + "TradeInSkuPerLocation) AND bitIsRegularProduct = 1 AND I.intInventoryID "
                + "NOT IN(SELECT intInventoryID FROM tbl" + businessNumber + "PurchasedInventory WHERE "
                + "bitIsProcessedIntoInventory = 0)";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public void AddVendorSupplierForInventoryItem(VendorSupplierProduct vendorSupplierProduct, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "VendorSupplierProduct VALUES(@intVendorSupplierID, @intInventoryID, "
                + "@varVendorSupplierProductCode)";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierProduct.intVendorSupplierID },
                new object[] { "@intInventoryID", vendorSupplierProduct.intInventoryID },
                new object[] { "@varVendorSupplierProductCode", vendorSupplierProduct.varVendorSupplierProductCode }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void RemoveVendorSupplierForInventoryItem(int inventoryID, int vendorSupplierID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "VendorSupplierProduct WHERE intVendorSupplierID = "
                + "@intVendorSupplierID AND intInventoryID = @intInventoryID";
            object[][] parms =
            {
                new object[] { "@intVendorSupplierID", vendorSupplierID },
                new object[] { "@intInventoryID", inventoryID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}