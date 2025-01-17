using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SweetPOS.ClassObjects
{
    public class EmployeeManager
    {
        //All procedures in regards to an Employee are stored here
        DatabaseCalls DBC = new DatabaseCalls();
        private List<Employee> ConvertFromDataTableToEmployee(DataTable dt, int businessNumber)
        {
            //Converts from a data table into an Employee object
            LocationManager LM = new LocationManager();
            List<Employee> employee = dt.AsEnumerable().Select(row =>
            new Employee
            {
                intEmployeeID = row.Field<int>("intEmployeeID"),
                varFirstName = row.Field<string>("varFirstName"),
                varLastName = row.Field<string>("varLastName"),
                intJobCodeID = row.Field<int>("intJobCodeID"),
                //storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                varAddress = row.Field<string>("varAddress"),
                varHomePhone = row.Field<string>("varHomePhone"),
                varMobilePhone = row.Field<string>("varMobilePhone"),
                varEmailAddress = row.Field<string>("varEmailAddress"),
                varCityName = row.Field<string>("varCityName"),
                intProvinceID = row.Field<int>("intProvinceID"),
                intCountryID = row.Field<int>("intCountryID"),
                dtmCreationDate = row.Field<DateTime>("dtmCreationDate"),
                varPostalCode = row.Field<string>("varPostalCode"),
                bitIsEmployeeActive = row.Field<bool>("bitIsEmployeeActive")
            }).ToList();
            return employee;
        }

        public List<Employee> ReturnEmployee(int employeeID, int businessNumber)
        {
            //Returns an Employee based on their ID
            string sqlCmd = "SELECT intEmployeeID, varFirstName, varLastName, intJobCodeID, "
                //+ "intStoreLocationID, "
                + "varAddress, varHomePhone, varMobilePhone, varEmailAddress, "
                + "varCityName, intProvinceID, intCountryID, dtmCreationDate, varPostalCode, bitIsEmployeeActive FROM "
                + "tbl" + businessNumber + "Employee WHERE intEmployeeID = @intEmployeeID";
            object[][] parms =
            {
                 new object[] { "@intEmployeeID", employeeID },
            };
            return ConvertFromDataTableToEmployee(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Employee> ReturnEmployeeFromEFSHJEMVIF(string employeeString, int businessNumber)
        {
            //Returns an Employee based on their entered password
            int employeeEFSHJEMVIF;
            int.TryParse(employeeString, out employeeEFSHJEMVIF);
            string sqlCmd = "SELECT E.intEmployeeID, E.varFirstName, E.varLastName, E.intJobCodeID, "
                //+ "E.intStoreLocationID, "
                + "E.varAddress, E.varHomePhone, E.varMobilePhone, E.varEmailAddress, E.varCityName, E.intProvinceID, E.intCountryID, "
                + "E.dtmCreationDate, E.varPostalCode, E.bitIsEmployeeActive FROM tbl" + businessNumber + "Employee E JOIN tbl"
                + businessNumber + "EHSJYNLKHFNKLUCLFJ U ON U.intEmployeeID = E.intEmployeeID WHERE U.intEmployeeEFSHJEMVIF = "
                + "@intEmployeeEFSHJEMVIF";
            object[][] parms =
            {
                new object[] { "@intEmployeeEFSHJEMVIF", employeeEFSHJEMVIF }
            };
            return ConvertFromDataTableToEmployee(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }

        private int ExecuteJobIDCheck(int employeeEFSHJEMVIF, int businessNumber)
        {
            //This call returns the ID of their Job Code based on the entered password
            string sqlCmd = "SELECT E.intJobCodeID FROM tbl" + businessNumber + "Employee E JOIN tbl" + businessNumber
                + "EHSJYNLKHFNKLUCLFJ U ON U.intEmployeeID = E.intEmployeeID WHERE U.intEmployeeEFSHJEMVIF = @intEmployeeEFSHJEMVIF";
            object[][] parms =
            {
                new object[] { "@intEmployeeEFSHJEMVIF", employeeEFSHJEMVIF }
            };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        private int ExecuteAdminCheckForLocation(CurrentUser cu)
        {
            //This return the Super User Job Code ID
            string sqlCmd = "SELECT intJobCodeID FROM tbl" + cu.terminal.intBusinessNumber + "JobCode WHERE varJobDescription = 'SuDo'";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public int AddEmployee(Employee employee, int businessNumber)
        {
            //This process adds a new employee into the database
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Employee VALUES(@varFirstName, @varLastName, @intJobCodeID, "
                //+ "@intStoreLocationID, "
                + "@varAddress, @varHomePhone, @varMobilePhone, @varEmailAddress, "
                + "@varCityName, @intProvinceID, @intCountryID, @dtmCreationDate, @varPostalCode, @bitIsEmployeeActive)";
            object[][] parms =
            {
                new object[] { "@varFirstName", employee.varFirstName },
                new object[] { "@varLastName", employee.varLastName },
                new object[] { "@intJobCodeID", employee.intJobCodeID },
                //new object[] { "@intStoreLocationID", employee.storeLocation.intStoreLocationID },
                new object[] { "@varAddress", employee.varAddress },
                new object[] { "@varHomePhone", employee.varHomePhone },
                new object[] { "@varMobilePhone", employee.varMobilePhone },
                new object[] { "@varEmailAddress", employee.varEmailAddress },
                new object[] { "@varCityName", employee.varCityName },
                new object[] { "@intProvinceID", employee.intProvinceID },
                new object[] { "@intCountryID", employee.intCountryID },
                new object[] { "@dtmCreationDate", employee.dtmCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@varPostalCode", employee.varPostalCode },
                new object[] { "@bitIsEmployeeActive", employee.bitIsEmployeeActive }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnEmployeeIDFromEmployeeStats(parms, businessNumber);
        }
        public int ReturnEmployeeIDFromEmployeeStats(object[][] parms, int businessNumber)
        {
            //After creating a new employee this uses all the saved parameters to return the employee ID for ease of use.
            string sqlCmd = "SELECT intEmployeeID FROM tbl" + businessNumber + "Employee WHERE varFirstName = @varFirstName AND "
                + "varLastName = @varLastName AND intJobCodeID = @intJobCodeID AND "
                //+ "intStoreLocationID = @intStoreLocationID AND "
                + "varAddress = @varAddress AND varHomePhone = @varHomePhone AND varMobilePhone = @varMobilePhone AND "
                + "varEmailAddress = @varEmailAddress AND varCityName = @varCityName AND intProvinceID = @intProvinceID AND "
                + "intCountryID = @intCountryID AND varPostalCode = @varPostalCode AND dtmCreationDate = @dtmCreationDate AND "
                + "bitIsEmployeeActive = @bitIsEmployeeActive";
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }
        public int ReturnDefaultJobCode(int businessNumber)
        {
            //Returns the lowest Job Code ID in the table
            string sqlCmd = "SELECT MIN(intJobCodeID) FROM tbl" + businessNumber + "JobCode";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms);
        }

        public bool SaveNewPassword(int employeeID, int employeeEFSHJEMVIF, int businessNumber)
        {
            //Saves a password for the employee so they can login
            bool bolAdded = false;
            //First check if the password is in use by another user.
            string sqlCmd = "SELECT intEmployeeID FROM tbl" + businessNumber + "EHSJYNLKHFNKLUCLFJ "
                + "WHERE intEmployeeEFSHJEMVIF = @intEmployeeEFSHJEMVIF";
            object[][] parms =
            {
                new object[] { "@intEmployeeEFSHJEMVIF", employeeEFSHJEMVIF }
            };

            //Checks to see if the password is already in use
            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) < 0)
            {
                //When password not in use check if the employee is already in the user info table
                sqlCmd = "SELECT intEmployeeID FROM tbl" + businessNumber + "EHSJYNLKHFNKLUCLFJ "
                    + "WHERE intEmployeeID = @intEmployeeID";
                object[][] parms1 =
                {
                    new object [] { "@intEmployeeID", employeeID }
                };

                if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms1) > -10)
                {
                    //Employee is in the userInfo table update password
                    sqlCmd = "UPDATE tbl" + businessNumber + "EHSJYNLKHFNKLUCLFJ SET intEmployeeEFSHJEMVIF"
                        + "= @intEmployeeEFSHJEMVIF WHERE intEmployeeID = @intEmployeeID";
                }
                else
                {
                    //Employee is not in the table add user and password
                    sqlCmd = "INSERT INTO tbl" + businessNumber + "EHSJYNLKHFNKLUCLFJ VALUES(@intEmployeeID, "
                        + "@intEmployeeEFSHJEMVIF)";
                }
                object[][] parms2 =
                {
                    new object[] { "@intEmployeeID", employeeID },
                    new object[] { "@intEmployeeEFSHJEMVIF", employeeEFSHJEMVIF }
                };

                DBC.ExecuteNonReturnQuery(sqlCmd, parms2);
                bolAdded = true;
            }
            return bolAdded;
        }
        public bool ReturnSuDoIDForLocation(CurrentUser cu)
        {
            //Returns true or false if the Current User has a job code of Super User
            bool bolAdmin = false;
            if(ExecuteAdminCheckForLocation(cu) == cu.employee.intJobCodeID) { bolAdmin = true; }
            return bolAdmin;
        }
        public bool ReturnCanEmployeeMakeSale(string employeeString, CurrentUser cu)
        {
            //Runs a database check to see if the employee has authorization to make sales.
            bool bolValid = false;
            int employeeEFSHJEMVIF;
            if (int.TryParse(employeeString, out employeeEFSHJEMVIF))
            {
                int jobID = ExecuteJobIDCheck(employeeEFSHJEMVIF, cu.terminal.intBusinessNumber);
                if(jobID != -10)
                {
                    if (jobID != ExecuteAdminCheckForLocation(cu)) { bolValid = true; }
                }
            }
            return bolValid;
        }        

        public void UpdateEmployee(Employee employee, int businessNumber)
        {
            //Updates employee information
            string sqlCmd = "UPDATE tbl" + businessNumber + "Employee SET varFirstName = @varFirstName, varLastName = @varLastName, "
                + "intJobCodeID = @intJobCodeID, "
                //+ "intStoreLocationID = @intStoreLocationID, "
                + "varAddress = @varAddress, varHomePhone = @varHomePhone, varMobilePhone = @varMobilePhone, varEmailAddress = "
                + "@varEmailAddress, varCityName = @varCityName, intProvinceID = @intProvinceID, intCountryID = @intCountryID, "
                + "varPostalCode = @varPostalCode, bitIsEmployeeActive = @bitIsEmployeeActive WHERE intEmployeeID = @intEmployeeID";
            object[][] parms =
            {
                new object[] { "@intEmployeeID", employee.intEmployeeID },
                new object[] { "@varFirstName", employee.varFirstName },
                new object[] { "@varLastName", employee.varLastName },
                new object[] { "@intJobCodeID", employee.intJobCodeID },
                //new object[] { "@intStoreLocationID", employee.storeLocation.intStoreLocationID },
                new object[] { "@varAddress", employee.varAddress },
                new object[] { "@varHomePhone", employee.varHomePhone },
                new object[] { "@varMobilePhone", employee.varMobilePhone },
                new object[] { "@varEmailAddress", employee.varEmailAddress },
                new object[] { "@varCityName", employee.varCityName },
                new object[] { "@intProvinceID", employee.intProvinceID },
                new object[] { "@intCountryID", employee.intCountryID },
                new object[] { "@varPostalCode", employee.varPostalCode },
                new object[] { "@bitIsEmployeeActive", employee.bitIsEmployeeActive }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public DataTable ReturnEmployeeLessThanAdminBasedOnText(string searchText, int businessNumber)
        {
            //This returns all employees that matches a search string AND are not Super Users in the system
            ArrayList strText = new ArrayList();
            ArrayList parms = new ArrayList();
            string sqlCmd = "";
            for (int i = 0; i < searchText.Split(' ').Length; i++)
            {
                strText.Add("%" + searchText.Split(' ')[i] + "%");
                if (i == 0)
                {
                    sqlCmd = "SELECT intEmployeeID, CONCAT(varLastName, ', ', varFirstName) AS varEmployeeFullName, "
                        + "CONCAT('H: ', varHomePhone, 'M: ', varMobilePhone) AS varPhoneNumber, varAddress, "
                        + "varCityName FROM tbl" + businessNumber + "Employee E JOIN tbl" + businessNumber
                        + "JobCode JC ON JC.intJobCodeID = E.intJobCodeID WHERE (CAST(intEmployeeID AS VARCHAR) "
                        + "LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName, varHomePhone, varMobilePhone, "
                        + "varEmailAddress) LIKE @parm2" + i + ") AND NOT varJobDescription = 'SuDo'";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intEmployeeID, CONCAT(varLastName, ', ', varFirstName) AS "
                        + "varEmployeeFullName, CONCAT('H: ', varHomePhone, 'M: ', varMobilePhone) AS "
                        + "varPhoneNumber, varAddress, varCityName FROM tbl" + businessNumber + "Employee E "
                        + "WHERE (CAST(intEmployeeID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, "
                        + "varLastName, varHomePhone, varMobilePhone, varEmailAddress) LIKE @parm2" + i + ") AND "
                        + "NOT varJobDescription = 'SuDo')";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
            }
            sqlCmd += " ORDER BY varLastName ASC";
            return DBC.MakeDatabaseCallToReturnDataTableFromArrayListTwo(sqlCmd, parms, strText);
        }
        public DataTable ReturnEmployeeAllBasedOnText(string searchText, int businessNumber)
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
                    sqlCmd = "SELECT intEmployeeID, CONCAT(varLastName, ', ', varFirstName) AS varEmployeeFullName, "
                        + "CONCAT('H: ', varHomePhone, 'M: ', varMobilePhone) AS varPhoneNumber, varAddress, "
                        + "varCityName FROM tbl" + businessNumber + "Employee WHERE CAST(intEmployeeID AS VARCHAR) "
                        + "LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName, varHomePhone, varMobilePhone, "
                        + "varEmailAddress) LIKE @parm2" + i + "";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
                else
                {
                    sqlCmd += " INTERSECT (SELECT intEmployeeID, CONCAT(varLastName, ', ', varFirstName) AS "
                        + "varEmployeeFullName, CONCAT('H: ', varHomePhone, 'M: ', varMobilePhone) AS "
                        + "varPhoneNumber, varAddress, varCityName FROM tbl" + businessNumber + "Employee WHERE "
                        + "CAST(intEmployeeID AS VARCHAR) LIKE @parm1" + i + " OR CONCAT(varFirstName, varLastName, "
                        + "varHomePhone, varMobilePhone, varEmailAddress) LIKE @parm2" + i + ")";
                    parms.Add("@parm1" + i);
                    parms.Add("@parm2" + i);
                }
            }
            sqlCmd += " ORDER BY varLastName ASC";
            return DBC.MakeDatabaseCallToReturnDataTableFromArrayListTwo(sqlCmd, parms, strText);
        }
        public DataTable ReturnJobListings(int businessNumber)
        {
            //Returns all job codes and IDs
            string sqlCmd = "SELECT intJobCodeID, varJobDescription FROM tbl" + businessNumber + "JobCode";
            object[][] parms = { };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }        
    }
}