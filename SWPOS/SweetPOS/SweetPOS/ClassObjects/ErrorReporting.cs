using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class ErrorReporting
    {
        CurrentUserManager CUM = new CurrentUserManager();
        //This method is used to log errors in the database
        //Used - Called from All Pages by All Methods 1
        public void logError(Exception er, CurrentUser CU, string page, string strMethod, System.Web.UI.Page webPage)
        {
            DatabaseCalls DBC = new DatabaseCalls();
            string sqlCmd = "INSERT INTO tbl" + CU.terminal.intBusinessNumber + "Error VALUES(@intEmployeeID, "
                + "@dtmErrorDate, @dtmErrorTime, @varErrorPageOccurrence, "
                + "@varErrorMethodOccurrence, @intErrorCode, @varErrorMessage)";
            object[][] parms =
            {
                new object[] { "intEmployeeID", CU.employee.intEmployeeID },
                new object[] { "dtmErrorDate", Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())).ToString("yyyy-MM-dd") },
                new object[] { "dtmErrorTime", Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())).ToString("HH:mm:ss") },
                new object[] { "varErrorPageOccurrence", er.Source + " - " + page },
                new object[] { "varErrorMethodOccurrence", strMethod },
                new object[] { "intErrorCode", er.HResult },
                new object[] { "varErrorMessage", er.Message }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
    }
}