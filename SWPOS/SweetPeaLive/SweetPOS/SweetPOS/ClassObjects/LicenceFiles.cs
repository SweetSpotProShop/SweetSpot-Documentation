using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace SweetPOS.ClassObjects
{
    public class LicenceFiles
    {
        //These procedures check to make sure that the business trying to login is active and has the right licences
        //It will also check to make sure there is a file structure stored on the terminal in order to store the PDF receipts.

        private DatabaseCalls DBC = new DatabaseCalls();
        private string strLicenceFilePath = "";

        private bool BusinessIsActive(int businessNumber)
        {
            return ActiveBusinessCheck(businessNumber);
        }
        private bool ActiveBusinessCheck(int businessNumber)
        {
            string sqlCmd = "SELECT bitIsBusinessActive FROM tblBusinessIdentification WHERE "
                + "intBusinessNumber = @intBusinessNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", businessNumber }
            };
            return DBC.MakeDatabaseCallToReturnBool(sqlCmd, parms);
        }
        private bool BusinessTablesAreSetup(int businessNumber)
        {
            return TableCheck(businessNumber);
        }
        private bool TableCheck(int businessNumber)
        {
            string sqlCmd = "SELECT bitIsBusinessInitialized FROM tblBusinessIdentification WHERE "
                + "intBusinessNumber = @intBusinessNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", businessNumber }
            };
            return DBC.MakeDatabaseCallToReturnBool(sqlCmd, parms);
        }
        private bool IsFileStructureInPlace(int businessNumber, int terminalID)
        {
            bool bolFileStructure = false;
            string path = collectingFolderPath(businessNumber, terminalID) + "\\SweetPea\\";
            //Check to see if folder path and/or file exists
            if (Directory.Exists(path))
            {
                path += "suf\\";
                if (Directory.Exists(path))
                {
                    path += "licence.xml";
                    if (File.Exists(path))
                    {
                        bolFileStructure = true;
                    }
                }
            }
            return bolFileStructure;
        }
        private bool TableCheck(object businessNumber)
        {
            bool bolTablesExists = false;
            string sqlCmd = "SELECT COUNT(intBusinessNumber) AS countBN FROM tblBusinessIdentification WHERE intBusinessNumber = "
                + "@intBusinessNumber AND bitIsBusinessInitialized = 1";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", Convert.ToInt32(businessNumber) }
            };
            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                bolTablesExists = true;
            }
            return bolTablesExists;
        }

        public bool CheckBusinessPortfolioLogin(object[] login)
        {
            bool isValidPortfolio = false;
            if (PortfolioCheck(login) > 0)
            {
                isValidPortfolio = true;
            }
            return isValidPortfolio;
        }

        private int LicenceVerification(string licenceNumber)
        {
            string sqlCmd = "SELECT CASE WHEN (varLicenceNumber = @varLicenceNumber AND bitLicenceInUse = 1) THEN 2 WHEN "
                + "varLicenceNumber = @varLicenceNumber THEN 0 ELSE 1 END AS licenceCase FROM tblIssuingLicences WHERE "
                + "varLicenceNumber = @varLicenceNumber";
            object[][] parms =
            {
                new object[] { "@varLicenceNumber", licenceNumber }
            };

            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int MACAndLicenceCheck(object[] MLSandT)
        {
            string sqlCmd = "SELECT CASE WHEN(intStoreLocationID = @intStoreLocationID AND intTillNumber = @intTillNumber) "
                + "THEN 0 ELSE 1 END AS licenceCheck FROM tbl" + Convert.ToInt32(MLSandT[4]) + "Licence L JOIN "
                + "tblIssuingLicences IL ON IL.intLicenceID = L.intLicenceID WHERE varLicenceNumber = @varLicenceNumber";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", Convert.ToInt32(MLSandT[2]) },
                new object[] { "@intTillNumber", Convert.ToInt32(MLSandT[3]) },
                new object[] { "@varLicenceNumber", Convert.ToString(MLSandT[1]) }
            };

            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int PortfolioCheck(object[] login)
        {
            string sqlCmd = "SELECT COUNT(intBusinessNumber) AS BusinessExists FROM tblBusinessIdentification WHERE intBusinessNumber = "
                + "@userName AND varBusinessCodex = @password AND bitIsBusinessActive = 1";
            object[][] parms =
            {
                new object[] { "@userName", Convert.ToInt32(login[0]) },
                new object[] { "@password", Convert.ToString(login[1]) }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int CreateGuestCustomer(StoreLocation sLocation, DateTime createDateTime, int businessNumber)
        {
            //this will create the Guest customer for this location.
            CustomerManager CM = new CustomerManager();
            Customer customer = new Customer
            {
                varFirstName = "Guest",
                varLastName = "Customer",
                varAddress = sLocation.varAddress,
                varHomePhone = sLocation.varPhoneNumber,
                varMobilePhone = "",
                varEmailAddress = sLocation.varEmailAddress,
                varCityName = sLocation.varCityName,
                intProvinceID = sLocation.intProvinceID,
                intCountryID = sLocation.intCountryID,
                varPostalCode = sLocation.varPostalCode,
                bitAllowMarketing = false
            };
            return CM.AddNewCustomer(customer, createDateTime, businessNumber);
        }
        private int CreateAdminEmployee(StoreLocation sLocation, DateTime createDateTime, int businessNumber)
        {
            //This will create the first employee to manage system
            EmployeeManager EM = new EmployeeManager();
            Employee employee = new Employee
            {
                varFirstName = "SweetPeaSuDo",
                varLastName = sLocation.varStoreName,
                intJobCodeID = EM.ReturnDefaultJobCode(businessNumber),
                //storeLocation = sLocation,
                varAddress = sLocation.varAddress,
                varHomePhone = sLocation.varPhoneNumber,
                varMobilePhone = "",
                varEmailAddress = sLocation.varEmailAddress,
                varCityName = sLocation.varCityName,
                intProvinceID = sLocation.intProvinceID,
                intCountryID = sLocation.intCountryID,
                dtmCreationDate = createDateTime,
                varPostalCode = sLocation.varPostalCode,
                bitIsEmployeeActive = true
            };
            return EM.AddEmployee(employee, businessNumber);
        }
        private int CreateFirstTradeInInventory(StoreLocation sLocation, DateTime createDateTime, int businessNumber)
        {
            //This will create the Trade In for this location
            InventoryManager IM = new InventoryManager();
            Inventory inventory = new Inventory
            {
                intBrandID = IM.ReturnDefaultBrand(businessNumber),
                varModelName = "",
                varDescription = "",
                intStoreLocationID = sLocation.intStoreLocationID,
                varUPCcode = "",
                intQuantity = 0,
                fltPrice = 0,
                fltAverageCost = 0,
                bitIsNonStockedProduct = true,
                bitIsRegularProduct = false,
                bitIsUsedProduct = false,
                bitIsActiveProduct = true,
                dtmCreationDate = createDateTime,
                varAdditionalInformation = ""
            };
            return IM.AddNewInventory(inventory, createDateTime, businessNumber);
        }
        private int RetrieveLicenseID(Terminal terminal)
        {
            string sqlCmd = "SELECT intLicenceID FROM tblIssuingLicences "
                + "WHERE varLicenceNumber = @varLicenceNumber";
            object[][] parms =
            {
                new object[] { "@varLicenceNumber", terminal.varLicenceNumber }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        

        public int SystemCheck(int businessNumber, int terminalID)
        {
            int systemCheck = 0;
            if (IsFileStructureInPlace(businessNumber, terminalID))
            {
                GetFilePathForXMLFile(businessNumber, terminalID);
                string Litem = RetrieveLicence();
                int licenceCheck = LicenceVerification(Litem);
                if (licenceCheck == 0)
                {
                    //0 = License is valid and available ***re-establishing this terminal
                    systemCheck = 1;
                }
                else if (licenceCheck == 1)
                {
                    //1 = Licence is not a valid ID in the system ***Incorrect Licence
                    systemCheck = 3;
                }
                else if (licenceCheck == 2)
                {
                    //2 = Licence is in use
                    //when this happens then we need to see if it is registered to this machine
                    //object[] MLSandT = RetrieveMLSandT();
                    //int intMacCheck = MACAndLicenceCheck(MLSandT);
                    //if (intMacCheck == 0)
                    //{
                    //0 = this checks out goto login page
                    systemCheck = 2;
                    //}
                    //else
                    //{
                    //error
                    //systemCheck = 3;
                    //}
                }
            }
            return systemCheck;
        }
        public int SystemCheck2(int businessNumber)
        {
            int systemCheck = 0;
            if (TableCheck(businessNumber))
            {
                //0 = This result is if business tables already set up
                //just need to select terminal
                systemCheck = 1;
            }
            else
            {
                //1 = New business need to create tables
                systemCheck = 2;
            }
            return systemCheck;
        }
        public int ReturnLicenceIDFromLicenceNumber(string licenceNumber)
        {
            string sqlCmd = "SELECT intLicenceID FROM tblIssuingLicences WHERE varLicenceNumber = @varLicenceNumber";

            object[][] parms =
            {
                new object[] { "@varLicenceNumber", licenceNumber }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        private string collectingFolderPath(int businessNumber, int terminalID)
        {
            string sqlCmd = "SELECT IL.varMACAddress FROM tblIssuingLicences IL JOIN tbl" + businessNumber + "Licence L ON L.intLicenceID "
                + "= IL.intLicenceID WHERE intTerminalID = @intTerminalID";
            object[][] parms =
            {
                new object[] { "@intTerminalID", terminalID }
            };
            return DBC.MakeDatabaseCallToReturnString(sqlCmd, parms);
        }
        private string RetrieveLicence()
        {
            //Licence file exists
            string licence = "";
            //Open file and retrieve Licence
            var xml = XDocument.Load(@strLicenceFilePath);
            // Query the data and write out a subset of contacts
            var query = from c in xml.Root.Descendants("TillInformation")
                        select c.Element("Licence").Value;
            foreach (string L in query)
            {
                licence = L;
            }
            return licence;
        }

        public string collectFolderPath(int businessNumber, int terminalID)
        {
            return collectingFolderPath(businessNumber, terminalID);
        }
        public string GatherLicenceFromFile(int businessNumber, int terminalID)
        {
            GetFilePathForXMLFile(businessNumber, terminalID);
            return RetrieveLicence();
        }
        
        private object[] RetrieveMLSandT()
        {
            //Licence file exists
            string mac = "";
            string licence = "";
            string store = "";
            string till = "";
            string busN = "";
            //Open file and retrieve Licence
            var xml = XDocument.Load(@strLicenceFilePath);
            // Query the data and write out a subset
            //var queryM = from c in xml.Root.Descendants("TillInformation")
            //             select c.Element("MACAddress").Value;
            //foreach (string M in queryM)
            //{
            //    mac = M;
            //}
            var queryL = from c in xml.Root.Descendants("TillInformation")
                         select c.Element("Licence").Value;
            foreach (string L in queryL)
            {
                licence = L;
            }
            var queryS = from c in xml.Root.Descendants("StoreLocation")
                         select c.Element("LocationID").Value;
            foreach (string S in queryS)
            {
                store = S;
            }
            var queryT = from c in xml.Root.Descendants("TillInformation")
                         select c.Element("TillNumber").Value;
            foreach (string T in queryT)
            {
                till = T;
            }
            var queryB = from c in xml.Root.Descendants("BusinessNumber")
                         select c.Element("BNumber").Value;
            foreach (string B in queryB)
            {
                busN = B;
            }
            object[] MLSandT = { mac, licence, store, till, busN };
            return MLSandT;
        }

        private DataTable ReturnLocationAndMakeDatabaseCalls(int businessNumber)
        {
            string sqlCmd = "SELECT intStoreLocationID, varStoreName, varPhoneNumber, varAddress, varEmailAddress, varCityName, intProvinceID, "
                + "intCountryID, varPostalCode, varTaxNumber, varStoreCode, bitIsRetailLocation FROM tbl" + businessNumber + "StoreLocation";
            object[][] parms = { };

            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            dt.Rows.Add(new object[] { 0, "New Location", "", "", "", "", 1, 1, "", "", "", false });

            DataView dv = dt.DefaultView;
            dv.Sort = "intStoreLocationID ASC";
            return dv.ToTable();
        }
        private DataTable CreateLocationDropDownDataTable(DataTable dt)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("intStoreLocationID", typeof(Int32));
            dt2.Columns.Add("varStoreName", typeof(String));
            foreach (DataRow dr in dt.Rows)
            {
                dt2.Rows.Add(new object[] { dr[0], dr[1] });
            }
            return dt2;
        }
        private DataTable ReturnTerminalAndMakeDatabaseCalls(int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT intTerminalID, intTillNumber FROM tbl" + businessNumber
                + "Licence WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID }
            };

            DataTable dt = DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            dt.Rows.Add(new object[] { 0, 0 });

            DataView dv = dt.DefaultView;
            dv.Sort = "intTerminalID ASC";
            return dv.ToTable();
        }
        private DataTable CreateTerminalDropDownDataTable(DataTable dt)
        {
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("intTerminalID", typeof(Int32));
            dt2.Columns.Add("intTillNumber", typeof(Int32));
            foreach (DataRow dr in dt.Rows)
            {
                dt2.Rows.Add(new object[] { dr[0], dr[1] });
            }
            return dt2;
        }

        public DataTable ChooseLocationForTerminal(int businessNumber)
        {
            return ReturnLocationAndMakeDatabaseCalls(businessNumber);
        }
        public DataTable CreateLocationForTerminal()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("intStoreLocationID", typeof(Int32));
            dt.Columns.Add("varStoreName", typeof(String));
            dt.Rows.Add(new object[] { 0, "New Location" });

            DataView dv = dt.DefaultView;
            dv.Sort = "intStoreLocationID ASC";
            return dv.ToTable();
        }
        public DataTable ReturnDropDownForLocations(DataTable dt)
        {
            return CreateLocationDropDownDataTable(dt);
        }        
        public DataTable ChooseTerminalFromLocation(int storeLocationID, int businessNumber)
        {
            return ReturnTerminalAndMakeDatabaseCalls(storeLocationID, businessNumber);
        }        
        public DataTable CreateTerminalFromLocation()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("intTerminalID", typeof(Int32));
            dt.Columns.Add("intTillNumber", typeof(Int32));
            dt.Rows.Add(new object[] { 0, 0 });

            DataView dv = dt.DefaultView;
            dv.Sort = "intTerminalID ASC";
            return dv.ToTable();

        }
        public DataTable ReturnDropDownForTerminals(DataTable dt)
        {
            return CreateTerminalDropDownDataTable(dt);
        }

        private void GetFilePathForXMLFile(int businessNumber, int terminalID)
        {
            strLicenceFilePath = collectingFolderPath(businessNumber, terminalID) + "\\SweetPea\\suf\\licence.xml";
        }
        private void CreateEFSHJEMVIFForAdmin(int employeeID, int businessNumber)
        {
            //This will create a default EFSHJEMVIF for admin employee
            EmployeeManager EM = new EmployeeManager();
            EM.SaveNewPassword(employeeID, 753159, businessNumber);
        }
        private void SetTaxTypesForTradeInInventory(int inventoryID, int provinceID, DateTime taxDateTime, int businessNumber)
        {
            //This will tie all taxes to the item as all items require taxtypes
            InventoryManager IM = new InventoryManager();
            IM.SetTaxesForNewInventory(inventoryID, provinceID, false, taxDateTime, businessNumber);
        }
        private void ConnectFirstGuestToFirstStoreLocation(StoreLocation sLocation, int customerID, int businessNumber)
        {
            LocationManager LM = new LocationManager();
            LM.ConnectGuestCustomerToStoreLocation(businessNumber, sLocation.intStoreLocationID, customerID);
        }
        private void ConnectTradeInToFirstLocation(StoreLocation sLocation, int inventoryID, int businessNumber)
        {
            LocationManager LM = new LocationManager();
            LM.ConnectTradeInInventoryToStoreLocation(businessNumber, sLocation.intStoreLocationID, inventoryID);
        }
        private void SetupXMLFile(object[] setupInfo)
        {
            Terminal TS = (Terminal)setupInfo[2];
            XDocument doc = new XDocument(
                new XElement("BusinessAccount",
                    new XElement("BusinessNumber",
                        new XElement("BNumber", Convert.ToInt32(setupInfo[0]).ToString())),
                    new XElement("StoreLocation",
                        new XElement("LocationID", ((StoreLocation)setupInfo[1]).intStoreLocationID.ToString()),
                        new XElement("TillInformation",
                            //new XElement("MACAddress", TS.varMACAddress),
                            new XElement("Licence", TS.varLicenceNumber.ToString()),
                            new XElement("TillNumber", TS.intTillNumber.ToString()),
                            new XElement("TillFloat", TS.fltDrawerFloatAmount.ToString())
                        )
                    )
                )
            );

            string path = strLicenceFilePath + "\\SweetPea\\suf\\licence.xml";
            doc.Save(path);
        }
        private void SetupFileStructure(int businessNumber, int terminalID)
        {
            string path = collectingFolderPath(businessNumber, terminalID) + "\\SweetPea\\";
            Directory.CreateDirectory(path);
            string pathR = path + "receipts\\";
            Directory.CreateDirectory(pathR);
            string pathP = path + "purchase_orders\\";
            Directory.CreateDirectory(pathP);
            string pathI = path + "invoices\\";
            Directory.CreateDirectory(pathI);
            path += "suf\\";
            Directory.CreateDirectory(path);
        }

        public void CreateFirstObjectData(StoreLocation storeLocation, DateTime currentDate, int businessNumber)
        {
            int customerID = CreateGuestCustomer(storeLocation, currentDate, businessNumber);
            int employeeID = CreateAdminEmployee(storeLocation, currentDate, businessNumber);
            CreateEFSHJEMVIFForAdmin(employeeID, businessNumber);
            int inventoryID = CreateFirstTradeInInventory(storeLocation, currentDate, businessNumber);
            SetTaxTypesForTradeInInventory(inventoryID, storeLocation.intProvinceID, currentDate, businessNumber);
            ConnectFirstGuestToFirstStoreLocation(storeLocation, customerID, businessNumber);
            ConnectTradeInToFirstLocation(storeLocation, inventoryID, businessNumber);
        }
    }
}