using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.UI.WebControls;

namespace SweetPOS.ClassObjects
{
    public class ImportFile
    {
        DatabaseCalls DBC = new DatabaseCalls();
        //Inventory Upload Process **Start**
        //Used - Called from SettingsHomePage 1
        public DataTable ErrorCheckInventoryList(FileUpload fupInventorySheet, DateTime createDateTime, int businessNumber)
        {
            //***************************************************************************************************
            //Step 1: Clear temp tables
            //***************************************************************************************************
            ClearTempInventoryTables(businessNumber);

            //***************************************************************************************************
            //Step 2: Create an excel sheet and set its content to the uploaded file
            //***************************************************************************************************
            //Load the uploaded file into the memorystream
            using (MemoryStream stream = new MemoryStream(fupInventorySheet.FileBytes))
            //Lets the server know to use the excel package
            using (ExcelPackage xlPackage = new ExcelPackage(stream))
            {
                //***************************************************************************************************
                //Step 3: CopyData into DataTable for easier handling
                //***************************************************************************************************
                DataTable listItems = CopyExcelInventoryDataIntoDataTable(xlPackage);

                //***************************************************************************************************
                //Step 4: Create the temp tables for storing the items and skus that cause an error
                //***************************************************************************************************
                CreateTempInventoryTables(businessNumber);

                //***************************************************************************************************
                //Step 5: Check each item in the datatable to see if it will cause an error. If not, insert into the temp item table
                //***************************************************************************************************
                foreach (DataRow row in listItems.Rows)
                {
                    EnterInventoryToTempTable(row, createDateTime, businessNumber);
                }

                //***************************************************************************************************
                //Step 6: If no data is found in the error table
                //***************************************************************************************************
                //Start inserting into actual tables
                EnterInventoryToFinalTable(businessNumber);
                //TODO: Need to add tax types for all new inventory
                LoopThroughInventoryToAddTaxes(businessNumber, createDateTime);
                UpdateExistingInventoryToFinalTable(businessNumber);
            }

            DataTable tempDT = ReturnDataTableOfInventoryError(businessNumber);

            //***************************************************************************************************
            //Step 7: Delete the temp tables that were used for storage
            //***************************************************************************************************
            ClearTempInventoryTables(businessNumber);

            //***************************************************************************************************
            //Step 8: Return all errors
            //***************************************************************************************************
            return tempDT;
        }
        //Used - Called from Interal Call 2
        private void ClearTempInventoryTables(int businessNumber)
        {
            //Deleting temp tables
            string sqlCmd = "IF OBJECT_ID('temp" + businessNumber + "InventoryUpdate', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "InventoryUpdate; "
                + "IF OBJECT_ID('temp" + businessNumber + "InventoryNew', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "InventoryNew; "
                + "IF OBJECT_ID('temp" + businessNumber + "ErrorItems', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "ErrorItems;";

            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Used - Called from Interal Call 1
        private void CreateTempInventoryTables(int businessNumber)
        {
            //Creating the temp tables  
            string sqlCmd = "CREATE TABLE temp" + businessNumber + "InventoryUpdate(intInventoryID INT NOT NULL PRIMARY KEY, intBrandID INT NOT NULL, "
                + "varModelName VARCHAR(50) NOT NULL, varDescription VARCHAR(MAX) NOT NULL, intStoreLocationID INT NOT NULL, varUPCcode VARCHAR(25) NOT "
                + "NULL, fltPrice FLOAT NOT NULL, bitIsActiveProduct BIT NOT NULL, varAdditionalInformation VARCHAR(MAX) NOT NULL); CREATE TABLE temp" 
                + businessNumber + "InventoryNew(varSku VARCHAR(25) NOT NULL, intBrandID INT NOT NULL, varModelName VARCHAR(50) NOT NULL, varDescription "
                + "VARCHAR(MAX) NOT NULL, intStoreLocationID INT NOT NULL, varUPCcode VARCHAR(25) NOT NULL, intQuantity INT NOT NULL, fltPrice FLOAT NOT "
                + "NULL, fltAverageCost FLOAT NOT NULL, bitIsNonStockedProduct BIT NOT NULL, bitIsRegularProduct BIT NOT NULL, bitIsUsedProduct BIT NOT "
                + "NULL, bitIsActiveProduct BIT NOT NULL, dtmCreationDate DATE NOT NULL, varAdditionalInformation VARCHAR(MAX) NOT NULL); CREATE TABLE "
                + "temp" + businessNumber + "ErrorItems(varDescription VARCHAR(MAX) NOT NULL, intBrandID INT NOT NULL, intStoreLocationID INT NOT NULL);";

            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Used - Called from Interal Call 1
        private void EnterInventoryToTempTable(DataRow dataRow, DateTime createDateTime, int businessNumber)
        {
            InventoryManager IM = new InventoryManager();
            LocationManager LM = new LocationManager();

            //This query will look up the brand, model, and locationID of the item being passed in.
            //If all three are found, it will insert the item into the tempItemStorage table.
            //If not, it is added to the tempErrorSkus table
            int storeLocationID = LM.ReturnStoreLocationIDFromStoreName(dataRow[5].ToString(), businessNumber);
            // Add to this a third table for skus that are already in the system and need updating
            //the Three table system will be: One for errors, One for skus to update, and One for brand new skus.
            string sqlCmd = "IF(@intBrandID >= 0 AND @intStoreLocationID >= 0) BEGIN "
                    //Create another if statement to see if the item already exisits
                    //Update Enter item into an UpdateInventoryStorage
                    + "IF((SELECT TOP 1 intInventoryID FROM tbl" + businessNumber + "Inventory WHERE intInventoryID = @intInventoryID) >= 0) BEGIN "
                    + "INSERT INTO temp" + businessNumber + "InventoryUpdate VALUES(@intInventoryID, @intBrandID, @varModelName, @varDescription, "
                    + "@intStoreLocationID, @varUPCcode, @fltPrice, @bitIsActiveProduct, @varAdditionalInformation) END "
                    //Else insert into InventoryStorage
                    + "ELSE BEGIN INSERT INTO temp" + businessNumber + "InventoryNew VALUES(@varSku, @intBrandID, @varModelName, @varDescription, "
                    + "@intStoreLocationID, @varUPCcode, @intQuantity, @fltPrice, @fltAverageCost, @bitIsNonStockedProduct, @bitIsRegularProduct, "
                    + "@bitIsUsedProduct, @bitIsActiveProduct, @dtmCreationDate, @varAdditionalInformation) END "

                //End of second if statement
                + "END ELSE BEGIN INSERT INTO temp" + businessNumber + "ErrorItems VALUES(@varDescription, CASE WHEN @intBrandID >= 0 THEN 0 ELSE 1 END, "
                + "CASE WHEN @intStoreLocationID >= 0 THEN 0 ELSE 1 END) END";

            object[][] parms =
            {
                new object[] { "@intInventoryID", dataRow[0] },
                new object[] { "@varSku", IM.ReturnNextSKUNumberforLocation(storeLocationID, businessNumber) },
                new object[] { "@intBrandID", IM.ReturnBrandIDFromBrandName(dataRow[2].ToString(), businessNumber) },
                new object[] { "@varModelName", dataRow[3] },
                new object[] { "@varDescription", dataRow[4] },
                new object[] { "@intStoreLocationID", storeLocationID },
                new object[] { "@varUPCcode", dataRow[6] },
                new object[] { "@intQuantity", dataRow[7] },
                new object[] { "@fltPrice", dataRow[8] },
                new object[] { "@fltAverageCost", dataRow[9] },
                new object[] { "@bitIsNonStockedProduct", dataRow[10] },
                new object[] { "@bitIsRegularProduct", dataRow[11] },
                new object[] { "@bitIsUsedProduct", dataRow[12] },
                new object[] { "@bitIsActiveProduct", dataRow[13] },
                new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@varAdditionalInformation", dataRow[14] }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Used - Called from Interal Call 1
        private void EnterInventoryToFinalTable(int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Inventory (varSku, intBrandID, varModelName, varDescription, "
                + "intStoreLocationID, varUPCcode, intQuantity, fltPrice, fltAverageCost, bitIsNonStockedProduct, bitIsRegularProduct, "
                + "bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, varAdditionalInformation) SELECT varSku, intBrandID, "
                + "varModelName, varDescription, intStoreLocationID, varUPCcode, intQuantity, fltPrice, fltAverageCost, "
                + "bitIsNonStockedProduct, bitIsRegularProduct, bitIsUsedProduct, bitIsActiveProduct, dtmCreationDate, "
                + "varAdditionalInformation FROM temp" + businessNumber + "InventoryNew";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void LoopThroughInventoryToAddTaxes(int businessNumber, DateTime createDateTime)
        {
            InventoryManager IM = new InventoryManager();
            string sqlCmd = "SELECT intInventoryID FROM tbl" + businessNumber + "Inventory I JOIN temp" + businessNumber
                + "InventoryNew IN ON I.varSku = IN.varSku AND I.intBrandID = IN.intBrandID AND I.varModelName = IN.varModelName AND "
                + "I.varDescription = IN.varDescription AND I.intStoreLocationID = IN.intStoreLocationID AND I.varUPCcode = IN.varUPCcode "
                + "AND I.intQuantity = IN.intQuantity AND I.fltPrice = IN.fltPrice AND I.fltAverageCost = IN.fltAverageCost AND "
                + "I.bitIsNonStockedProduct = IN.bitIsNonStockedProduct AND I.bitIsRegularProduct = IN.bitIsRegularProduct AND "
                + "I.bitIsUsedProduct = IN.bitIsUsedProduct AND I.bitIsActiveProduct = IN.bitIsActiveProduct AND I.dtmCreationDate = "
                + "IN.dtmCreationDate AND I.varAdditionalInformation = IN.varAdditionalInformation";
            object[][] parms = { };
            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            foreach(DataRow dr in dt.Rows)
            {
                IM.SetTaxesForNewInventory(Convert.ToInt32(dr[0]), true, createDateTime, businessNumber);
            }
        }
        //Used - Called from Interal Call 1
        private void UpdateExistingInventoryToFinalTable(int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "Inventory SET intBrandID = IU.intBrandID, varModelName = IU.varModelName, "
                + "varDescription = IU.varDescription, intStoreLocationID = IU.intStoreLocationID, varUPCcode = IU.varUPCcode, fltPrice "
                + "= IU.fltPrice, bitIsActiveProduct = IU.bitIsActiveProduct, varAdditionalInformation = IU.varAdditionalInformation FROM "
                + "tbl" + businessNumber + "Inventory IV INNER JOIN temp" + businessNumber + "InventoryUpdate IU ON IV.intInventoryID = "
                + "IU.intInventoryID";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Used - Called from Interal Call 1
        private DataTable CopyExcelInventoryDataIntoDataTable(ExcelPackage xlPackage)
        {
            // get the first worksheet in the workbook
            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
            var worksheetRowCount = worksheet.Dimension.End.Row; //Gets the row count                   

            //***************************************************************************************************
            //Step 4: Looping through the data found in the excel sheet and storing it in the datatable
            //***************************************************************************************************
            DataTable listItems = new DataTable();
            listItems.Columns.Add("intInventoryID", typeof(string));
            listItems.Columns.Add("varSku", typeof(string));
            listItems.Columns.Add("varBrandName", typeof(string));
            listItems.Columns.Add("varModelName", typeof(string));
            listItems.Columns.Add("varDescription", typeof(string));
            listItems.Columns.Add("varStoreName", typeof(string));
            listItems.Columns.Add("varUPCCode", typeof(string));
            listItems.Columns.Add("intQuantity", typeof(int));
            listItems.Columns.Add("fltPrice", typeof(double));
            listItems.Columns.Add("fltAverageCost", typeof(double));
            listItems.Columns.Add("bitIsNonStockedProduct", typeof(bool));
            listItems.Columns.Add("bitIsRegularProduct", typeof(bool));
            listItems.Columns.Add("bitIsUsedProduct", typeof(bool));
            listItems.Columns.Add("bitIsActiveProduct", typeof(bool));
            listItems.Columns.Add("varAdditionalInformation", typeof(string));

            //Beginning the loop for data gathering
            for (int i = 2; i <= worksheetRowCount; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
            {
                listItems.Rows.Add(
                    //***************intInventoryID***************
                    worksheet.Cells[i, 1].Value.ToNullSafeString(),
                    //***************varSku***************************
                    worksheet.Cells[i, 2].Value.ToNullSafeString(),
                    //***************varBrandName*********************
                    worksheet.Cells[i, 3].Value.ToNullSafeString(),
                    //***************varModelName*********************
                    worksheet.Cells[i, 4].Value.ToNullSafeString(),
                    //***************varDescription*******************
                    worksheet.Cells[i, 5].Value.ToNullSafeString(),
                    //***************varStoreLocationName***************
                    worksheet.Cells[i, 6].Value.ToNullSafeString(),
                    //***************varUPCcode***********************
                    worksheet.Cells[i, 7].Value.ToNullSafeString(),
                    //***************intQuantity**********************
                    Convert.ToInt32(worksheet.Cells[i, 8].Value.ToNullSafeString()),
                    //***************fltPrice*************************
                    Convert.ToDouble(worksheet.Cells[i, 9].Value),
                    //***************fltAverageCost*******************
                    Convert.ToDouble(worksheet.Cells[i, 10].Value),
                    //***************bitIsNonStockedProduct***********
                    Convert.ToBoolean(worksheet.Cells[i, 11].Value),
                    //***************bitIsRegularProduct**************
                    Convert.ToBoolean(worksheet.Cells[i, 12].Value),
                    //***************bitIsUsedProduct*****************
                    Convert.ToBoolean(worksheet.Cells[i, 13].Value),
                    //***************bitIsActiveProduct******************
                    Convert.ToBoolean(worksheet.Cells[i, 14].Value),
                    //***************varAdditionalInformation*********
                    worksheet.Cells[i, 15].Value.ToNullSafeString());
            }
            return listItems;
        }
        //Used - Called from Interal Call 1
        private DataTable ReturnDataTableOfInventoryError(int businessNumber)
        {
            string sqlCmd = "SELECT * FROM temp" + businessNumber + "ErrorItems";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        //Inventory Upload Process **End**


        //***This Process Still Needs a Small adjustment
        //***Add in a Customer Update as well as having Customer New
        //***Need to distinguish between the 2
        //***DB Calls also need to be corrected (sqlCmd & parms)
        //Customer Upload Process **Start**
        public DataTable ErrorCheckCustomerList(FileUpload fupCustomerSheet, DateTime createDateTime, int businessNumber)
        {
            ClearTempCustomerTables(businessNumber);
            using (MemoryStream stream = new MemoryStream(fupCustomerSheet.FileBytes))
            //Lets the server know to use the excel package
            using (ExcelPackage xlPackage = new ExcelPackage(stream))
            {
                DataTable listItems = CopyExcelCustomerDataIntoDataTable(xlPackage);
                CreateTempCustomerTables(businessNumber);
                foreach (DataRow row in listItems.Rows)
                {
                    EnterCustomerToTempTable(row, createDateTime, businessNumber);
                }
                EnterCustomerToFinalTable(businessNumber);
                UpdateExistingCustomerToFinalTable(businessNumber);
            }
            DataTable tempDT = ReturnDataTableOfCustomerError(businessNumber);
            ClearTempCustomerTables(businessNumber);
            return tempDT;
        }
        private void ClearTempCustomerTables(int businessNumber)
        {
            DatabaseCalls DBC = new DatabaseCalls();
            //Deleting temp tables
            string sqlCmd = "IF OBJECT_ID('temp" + businessNumber + "CustomerUpdate', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "CustomerUpdate; "
                + "IF OBJECT_ID('temp" + businessNumber + "CustomerNew', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "CustomerNew; "
                + "IF OBJECT_ID('temp" + businessNumber + "ErrorCustomers', 'U') IS NOT NULL DROP TABLE temp" + businessNumber + "ErrorCustomers;";
            object[][] parms =
            {

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void CreateTempCustomerTables(int businessNumber)
        {
            DatabaseCalls DBC = new DatabaseCalls();
            //Creating the temp tables  
            string sqlCmd = "CREATE TABLE temp" + businessNumber + "CustomerUpdate(intCustomerID INT NOT NULL PRIMARY KEY, varFirstName "
                + "VARCHAR(50) NOT NULL, varLastName VARCHAR(50) NOT NULL, varAddress VARCHAR(100) NOT NULL, varHomePhone VARCHAR(20) NOT "
                + "NULL, varMobilePhone VARCHAR(20) NOT NULL, varEmailAddress VARCHAR(100) NOT NULL, varCityName VARCHAR(100) NOT NULL, "
                + "intProvinceID INT NOT NULL, intCountryID INT NOT NULL, varPostalCode VARCHAR(10) NOT NULL, bitAllowMarketing BIT NOT "
                + "NULL); CREATE TABLE temp" + businessNumber + "CustomerNew(varFirstName VARCHAR(50) NOT NULL, varLastName VARCHAR(50) "
                + "NOT NULL, varAddress VARCHAR(100) NOT NULL, varHomePhone VARCHAR(20) NOT NULL, varMobilePhone VARCHAR(20) NOT NULL, "
                + "varEmailAddress VARCHAR(100) NOT NULL, varCityName VARCHAR(100) NOT NULL, intProvinceID INT NOT NULL, intCountryID INT "
                + "NOT NULL, varPostalCode VARCHAR(10) NOT NULL, dtmCreationDate DATE NOT NULL, bitAllowMarketing BIT NOT NULL); CREATE "
                + "TABLE temp" + businessNumber + "ErrorCustomers(varFirstName VARCHAR(50) NOT NULL, varLastName VARCHAR(50) NOT NULL, "
                + "intProvinceID INT NOT NULL, intCountryID INT NOT NULL);";

            object[][] parms =
            {

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private DataTable CopyExcelCustomerDataIntoDataTable(ExcelPackage xlPackage)
        {
            // get the first worksheet in the workbook
            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
            var worksheetRowCount = worksheet.Dimension.End.Row; //Gets the row count
            DataTable listItems = new DataTable();
            listItems.Columns.Add("intCustomerID", typeof(string));
            listItems.Columns.Add("varFirstName", typeof(string));
            listItems.Columns.Add("varLastName", typeof(string));
            listItems.Columns.Add("varAddress", typeof(string));
            listItems.Columns.Add("varHomePhone", typeof(string));
            listItems.Columns.Add("varMobilePhone", typeof(string));
            listItems.Columns.Add("varEmailAddress", typeof(string));
            listItems.Columns.Add("varCityName", typeof(string));
            listItems.Columns.Add("varProvinceName", typeof(string));
            listItems.Columns.Add("varCountryName", typeof(string));
            listItems.Columns.Add("varPostalCode", typeof(string));
            listItems.Columns.Add("bitAllowMarketing", typeof(bool));

            //Beginning the loop for data gathering
            for (int i = 2; i <= worksheetRowCount; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
            {
                listItems.Rows.Add(
                    //***************intCustomerID***************
                    worksheet.Cells[i, 1].Value.ToNullSafeString(),
                    //***************varFirstName****************
                    worksheet.Cells[i, 2].Value.ToNullSafeString(),
                    //***************varLastName*****************
                    worksheet.Cells[i, 3].Value.ToNullSafeString(),
                    //***************varAddress******************
                    worksheet.Cells[i, 4].Value.ToNullSafeString(),
                    //***************varHomePhone****************
                    worksheet.Cells[i, 5].Value.ToNullSafeString(),
                    //***************varMobilePhone**************
                    worksheet.Cells[i, 6].Value.ToNullSafeString(),
                    //***************varEmailAddress*************
                    worksheet.Cells[i, 7].Value.ToNullSafeString(),
                    //***************varCityName*****************
                    worksheet.Cells[i, 8].Value.ToNullSafeString(),
                    //***************varProvinceName*************
                    worksheet.Cells[i, 9].Value.ToNullSafeString(),
                    //***************varCountryName**************
                    worksheet.Cells[i, 10].Value.ToNullSafeString(),
                    //***************varPostalCode***************
                    worksheet.Cells[i, 11].Value.ToNullSafeString(),
                    //***************bitAllowMarketing***********
                    Convert.ToBoolean(worksheet.Cells[i, 12].Value));
            }
            return listItems;
        }
        private void EnterCustomerToTempTable(DataRow dataRow, DateTime createDateTime, int businessNumber)
        {
            LocationManager LM = new LocationManager();
            DatabaseCalls DBC = new DatabaseCalls();
            //the Three table system will be: One for errors, One for skus to update, and One for brand new skus.
            string sqlCmd = "IF(@intProvinceID >= 0 AND @intCountryID >= 0) BEGIN "
                    //Create another if statement to see if the item already exisits
                    //Update Enter customer into an UpdateCustomerStorage
                    + "IF((SELECT TOP 1 intCustomerID FROM tbl" + businessNumber + "Customer WHERE intCustomerID = @intCustomerID) >= 0) BEGIN "
                    + "INSERT INTO temp" + businessNumber + "CustomerUpdate VALUES(@intCustomerID, @varFirstName, @varLastName, @varAddress, "
                    + "@varHomePhone, @varMobilePhone, @varEmailAddress, @varCityName, @intProvinceID, @intCountryID, @varPostalCode, "
                    + "@bitAllowMarketing) END "
                    //Else insert into InventoryStorage
                    + "ELSE BEGIN INSERT INTO temp" + businessNumber + "CustomerNew VALUES(@varFirstName, @varLastName, @varAddress, "
                    + "@varHomePhone, @varMobilePhone, @varEmailAddress, @varCityName, @intProvinceID, @intCountryID, @varPostalCode, "
                    + "@dtmCreationDate, @bitAllowMarketing) END "

                //End of second if statement
                + "END ELSE BEGIN INSERT INTO temp" + businessNumber + "ErrorCustomers VALUES(@varFirstName, @varLastName, CASE WHEN "
                + "@intProvinceID >= 0 THEN 0 ELSE 1 END, CASE WHEN @intCountryID >= 0 THEN 0 ELSE 1 END) END";

            object[][] parms =
            {
                new object[] { "@intCustomerID", dataRow[0] },
                new object[] { "@varFirstName", dataRow[1] },
                new object[] { "@varLastName", dataRow[2] },
                new object[] { "@varAddress", dataRow[3] },
                new object[] { "@varHomePhone", dataRow[4] },
                new object[] { "@varMobilePhone", dataRow[5] },
                new object[] { "@varEmailAddress", dataRow[6] },
                new object[] { "@varCityName", dataRow[7] },
                new object[] { "@intProvinceID", LM.ReturnProvinceIDFromProvinceName(dataRow[8].ToString()) },
                new object[] { "@intCountryID", LM.ReturnCountryIDFromCountryName(dataRow[9].ToString()) },
                new object[] { "@varPostalCode", dataRow[10] },
                new object[] { "@dtmCreationDate", createDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@bitAllowMarketing", dataRow[11] }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void EnterCustomerToFinalTable(int businessNumber)
        {
            DatabaseCalls DBC = new DatabaseCalls();
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Customer (varFirstName, varLastName, varAddress, varHomePhone, "
                + "varMobilePhone, varEmailAddress, varCityName, intProvinceID, intCountryID, varPostalCode, dtmCreationDate, "
                + "bitAllowMarketing) SELECT varFirstName, varLastName, varAddress, varHomePhone, varMobilePhone, varEmailAddress, "
                + "varCityName, intProvinceID, intCountryID, varPostalCode, dtmCreationDate, bitAllowMarketing FROM temp" 
                + businessNumber + "CustomerNew";
            object[][] parms =
            {

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        //Used - Called from Interal Call 1
        private void UpdateExistingCustomerToFinalTable(int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "Customer SET varFirstName = CU.varFirstName, varLastName = CU.varLastName, "
                + "varAddress = CU.varAddress, varHomePhone = CU.varHomePhone, varMobilePhone = CU.varMobilePhone, varEmailAddress = "
                + "CU.varEmailAddress, varCityName = CU.varCityName, intProvinceID = CU.intProvinceID, intCountryID = CU.intCountryID, "
                + "varPostalCode = CU.varPostalCode, bitAllowMarketing = CU.bitAllowMarketing FROM tbl" + businessNumber + "Customer CV "
                + "INNER JOIN temp" + businessNumber + "CustomerUpdate CU ON CV.intCustomerID = CU.intCustomerID";
            object[][] parms = { };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private DataTable ReturnDataTableOfCustomerError(int businessNumber)
        {
            DatabaseCalls DBC = new DatabaseCalls();
            string sqlCmd = "SELECT * FROM temp" + businessNumber + "ErrorCustomers";
            object[][] parms =
            {

            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        //Customer Upload Process **End**
    }
}