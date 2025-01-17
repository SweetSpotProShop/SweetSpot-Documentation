using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class CurrentUserManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        public object ConvertDate(DateTime inputTime)
        {
            // Ensure that the given date and time is not a specific kind.
            inputTime = DateTime.SpecifyKind(inputTime, DateTimeKind.Unspecified);
            var fromTimeOffset = new TimeSpan(0, 0, 0);
            var to = TimeZoneInfo.FindSystemTimeZoneById("Canada Central Standard Time");
            var offset = new DateTimeOffset(inputTime, fromTimeOffset);
            var destination = TimeZoneInfo.ConvertTime(offset, to);
            return destination.DateTime;
        }

        private List<CurrentUser> ConvertFromDataTableToCurrentUser(DataTable dt, int businessNumber, int terminalID)
        {
            LocationManager LM = new LocationManager();
            EmployeeManager EM = new EmployeeManager();
            TerminalManager TM = new TerminalManager();
            List<CurrentUser> currentUser = dt.AsEnumerable().Select(row =>
            new CurrentUser
            {
                terminal = TM.CallTerminalForCurrentUser(businessNumber, terminalID)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                currentStoreLocation = LM.ReturnLocationFromTerminalID(terminalID, businessNumber)[0],
                intEFSHJEMVIF = row.Field<int>("intEmployeeEFSHJEMVIF")
            }).ToList();
            return currentUser;
        }

        public List<CurrentUser> ReturnCurrentUserFromPassword(int EmployeeEFSHJEMVIF, int businessNumber, int terminalID)
        {
            string sqlCmd = "SELECT E.intEmployeeID, U.intEmployeeEFSHJEMVIF FROM tbl" + businessNumber + "Employee E JOIN tbl"
                + businessNumber + "EHSJYNLKHFNKLUCLFJ U ON E.intEmployeeID = U.intEmployeeID WHERE U.intEmployeeEFSHJEMVIF = "
                + "@intEmployeeEFSHJEMVIF";
            object[][] parms =
            {
                new object[] { "@intEmployeeEFSHJEMVIF", EmployeeEFSHJEMVIF }
            };
            return ConvertFromDataTableToCurrentUser(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber, terminalID);
        }

    }
}