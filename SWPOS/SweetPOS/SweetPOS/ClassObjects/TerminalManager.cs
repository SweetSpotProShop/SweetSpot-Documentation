using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class TerminalManager
    {
        LicenceFiles LF = new LicenceFiles();
        DatabaseCalls DBC = new DatabaseCalls();
        public List<Terminal> CallTerminalForCurrentUser(int businessNumber, int terminalID)
        {
            return ReturnTerminalForCurrentUser(businessNumber, terminalID);
        }
        public List<Terminal> ReturnTerminal(int terminalID, int businessNumber)
        {
            string sqlCmd = "SELECT L.intTerminalID, L.intStoreLocationID, IL.intBusinessNumber, L.intTillNumber, "
                + "L.intLicenceID, IL.varLicenceNumber, L.fltDrawerFloatAmount FROM tbl" + businessNumber
                + "Licence L JOIN tblIssuingLicences IL ON IL.intLicenceID = L.intLicenceID WHERE intTerminalID = "
                + "@intTerminalID";

            object[][] parms =
            {
                 new object[] { "@intTerminalID", terminalID }
            };
            return ConvertFromDataTableToTerminalSetup(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        private List<Terminal> ConvertFromDataTableToTerminalSetup(DataTable dt, int businessNumber)
        {
            List<Terminal> terminalSetup = dt.AsEnumerable().Select(row =>
            new Terminal
            {
                intBusinessNumber = businessNumber,
                intTerminalID = row.Field<int>("intTerminalID"),
                intStoreLocationID = row.Field<int>("intStoreLocationID"),
                intTillNumber = row.Field<int>("intTillNumber"),
                intLicenceID = row.Field<int>("intLicenceID"),
                varLicenceNumber = row.Field<string>("varLicenceNumber"),
                fltDrawerFloatAmount = row.Field<double>("fltDrawerFloatAmount")
            }).ToList();
            return terminalSetup;
        }
        private List<Terminal> ReturnTerminalForCurrentUser(int businessNumber, int terminalID)
        {
            string sqlCmd = "SELECT L.intTerminalID, L.intStoreLocationID, IL.intBusinessNumber, L.intTillNumber, L.intLicenceID, "
                + "IL.varLicenceNumber, L.fltDrawerFloatAmount FROM tbl" + businessNumber + "Licence L JOIN tblIssuingLicences IL "
                + "ON IL.intLicenceID = L.intLicenceID WHERE L.intTerminalID = @intTerminalID";
            object[][] parms =
            {
                new object[] { "@intTerminalID", terminalID }
            };
            return ConvertFromDataTableToTerminalSetup(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        private List<Terminal> ConvertFromDataTableToTerminalSetup(DataTable dt)
        {
            List<Terminal> terminal = dt.AsEnumerable().Select(row =>
            new Terminal
            {
                intTerminalID = row.Field<int>("intTerminalID"),
                intStoreLocationID = row.Field<int>("intStoreLocationID"),
                intBusinessNumber = row.Field<int>("intBusinessNumber"),
                intTillNumber = row.Field<int>("intTillNumber"),
                intLicenceID = row.Field<int>("intLicenceID"),
                varLicenceNumber = row.Field<string>("varLicenceNumber"),
                fltDrawerFloatAmount = row.Field<double>("fltDrawerFloatAmount")
            }).ToList();
            return terminal;
        }
        private List<Terminal> AddNewTerminalForStoreLocation(Terminal terminal)
        {
            string sqlCmd = "INSERT INTO tbl" + terminal.intBusinessNumber + "Licence VALUES(@intStoreLocationID, @intTillNumber, "
                + "@intLicenceID, @fltDrawerFloatAmount)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", terminal.intStoreLocationID },
                new object[] { "@intTillNumber", GatherNextTillNumber(terminal.intStoreLocationID, terminal.intBusinessNumber) },
                new object[] { "@intLicenceID", LF.ReturnLicenceIDFromLicenceNumber(terminal.varLicenceNumber) },
                new object[] { "@fltDrawerFloatAmount", terminal.fltDrawerFloatAmount }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            SetInitializationOfTerminal(terminal);
            return ReturnTerminalSetupFromDetails(parms, terminal.intBusinessNumber);
        }
        private List<Terminal> ReturnTerminalSetupFromDetails(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT L.intTerminalID, L.intStoreLocationID, L.intLicenceID, IL.intBusinessNumber, L.intTillNumber, "
                + "IL.varLicenceNumber, L.fltDrawerFloatAmount FROM tbl" + businessNumber + "Licence L JOIN tblIssuingLicences IL "
                + "ON IL.intLicenceID = L.intLicenceID WHERE intStoreLocationID = @intStoreLocationID AND intTillNumber = "
                + "@intTillNumber AND L.intLicenceID = @intLicenceID AND fltDrawerFloatAmount = @fltDrawerFloatAmount";

            return ConvertFromDataTableToTerminalSetup(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }

        private Terminal InitializeSystem(object[] setupInfo)
        {
            //object[] setupInfo will contain the business number, storeLocation, terminalInformation

            int businessNumber = Convert.ToInt32(setupInfo[0]);
            int code = Convert.ToInt32(setupInfo[4]);

            //if (code == 4)
            //{
            //insert new store location if it's new
            StoreLocation storeLocation = (StoreLocation)setupInfo[1];
            Terminal terminal = (Terminal)setupInfo[2];

            if (storeLocation.intStoreLocationID == 0)
            {
                LocationManager LM = new LocationManager();
                storeLocation = LM.AddNewStoreLocationToBusiness(storeLocation, businessNumber)[0];
                LM.CallStoreLocationCreationCode(storeLocation, businessNumber);
                LF.CreateFirstObjectData(storeLocation, Convert.ToDateTime(setupInfo[3]), businessNumber);
            }
            terminal.intStoreLocationID = storeLocation.intStoreLocationID;
            //insert new terminal if it's new
            if (terminal.intTerminalID == 0)
            {
                terminal = AddNewTerminalForStoreLocation(terminal)[0];
            }

            //strLicenceFilePath = collectingFolderPath(businessNumber, terminalSetup.intTerminalID);
            //terminalSetup.intLicenceID = strLicenceFilePath;
            //UpdateTerminalSetup(terminalSetup);
            //SetupFileStructure(businessNumber, terminalSetup.intTerminalID);
            //SetupXMLFile(setupInfo);
            //}
            //else
            //{
            //setup initial tables
            //CreateAllTableData(businessNumber);

            //CreateFirstObjectData(storeLocation, Convert.ToDateTime(setupInfo[3]), businessNumber);
            //terminalID = finalTerminalSetup(setupInfo);
            //SetupFileStructure(businessNumber, terminalID);
            //SetupXMLFile(setupInfo);
            //}
            return terminal;
        }

        public Terminal SetupInitialFiles(object[] setupInfo)
        {
            return InitializeSystem(setupInfo);
        }

        private int finalTerminalSetup(object[] setupInfo)
        {
            TerminalManager TM = new TerminalManager();
            TM.SetInitializationOfTerminal((Terminal)setupInfo[2]);
            //saveLicenceInformation(setupInfo);
            return ReturnTerminalID(setupInfo);
        }
        private int ReturnTerminalID(object[] setupInfo)
        {
            Terminal terminal = (Terminal)setupInfo[2];
            string sqlCmd = "SELECT intTerminalID FROM tbl" + Convert.ToInt32(setupInfo[0]) + "Licence WHERE intStoreLocation = "
                + "@intStoreLocationID, intTillNumber = @intTillNumber, intLicenceID = @intLicenceID, fltDrawerFloatAmount = "
                + "@fltDrawerFloatAmount)";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", terminal.intStoreLocationID },
                new object[] { "@intTillNumber", terminal.intTillNumber },
                new object[] { "@intLicenceID", terminal.intLicenceID },
                new object[] { "@fltDrawerFloatAmount", terminal.fltDrawerFloatAmount }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int GatherNextTillNumber(int storeLocationID, int businessNumber)
        {
            string sqlCmd = "SELECT MAX(intTillNumber) FROM tbl" + businessNumber + "Licence WHERE intStoreLocationID "
                + "= @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", storeLocationID }
            };
            int tillNumber = DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
            if (tillNumber < 1)
            {
                tillNumber = 0;
            }
            return (tillNumber + 1);
        }

        public void SetInitializationOfTerminal(Terminal terminal)
        {
            string sqlCmd = "UPDATE tblIssuingLicences SET bitLicenceInUse = 1, intBusinessNumber = @intBusinessNumber "
                + "WHERE varLicenceNumber = @varLicenceNumber";
            object[][] parms =
            {
                new object[] { "@intBusinessNumber", terminal.intBusinessNumber },
                new object[] { "@varLicenceNumber", terminal.varLicenceNumber }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}