using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class DatabaseCalls
    {
        //This class is used for all calls to the database
        private string connectionString;
        public DatabaseCalls()
        {
            connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
        }

        public System.Data.DataTable MakeDatabaseCallToReturnDataTable(string sqlCmd, object[][] parms)
        {
            //This returns a datatable
            return callDatabaseReturnDataTable(sqlCmd, parms);
        }
        public string MakeDatabaseCallToReturnString(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a string (only the string in first column and first row.
            //99% of the time when this is called there is only one row and column
            try
            {
                return (callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0]).ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public bool MakeDatabaseCallToReturnBool(string sqlCmd, object[][] parms)
        {
            try
            {
                //This turns the returned datatable into a boolean (only the string in first column and first row.
                //99% of the time when this is called there is only one row and column
                return Convert.ToBoolean(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0]);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public int MakeDatabaseCallToReturnInt(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a int (only the string in first column and first row.
            //99% of the time when this is called there is only one row and column
            try
            {
                return Convert.ToInt32((callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDatabaseCallToReturnDouble(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a double (only the string in first column and first row.
            //99% of the time when this is called there is only one row and column
            try
            {
                return Convert.ToDouble((callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public void ExecuteNonReturnQuery(string sqlCmd, object[][] parms)
        {
            //This runs against the database when there is no need to return any values
            callDatabaseExecuteNonReturnQuery(sqlCmd, parms);
        }

        private System.Data.DataTable callDatabaseReturnDataTable(string sqlCmd, object[][] parms)
        {
            //Private access that returns all datatables
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                times++;
            }
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
        private void callDatabaseExecuteNonReturnQuery(string sqlCmd, object[][] parms)
        {
            //Private access against database when there is no need to return any values 
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int times = 0;
            while (parms.Count() > times)
            {
                cmd.Parameters.AddWithValue(parms[times][0].ToString(), parms[times][1]);
                times++;
            }
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public string MakeDatabaseCallToReturnThirdColumnAsString(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a string (only the string in first row and third column.
            try
            {
                return (callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][2]).ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public int MakeDatabaseCallToReturnSecondColumnAsInt(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a int (only the string in first row and second column.
            try
            {
                return Convert.ToInt32((callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][1]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDatabaseCallToReturnSecondColumnAsDouble(string sqlCmd, object[][] parms)
        {
            //This turns the returned datatable into a double (only the string in first row and second column.
            try
            {
                return Convert.ToDouble((callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][1]).ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }

        public System.Data.DataTable MakeDatabaseCallToReturnDataTableFromArrayListFour(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Some queries required array lists in order to work properly
            //This returns a datatable using those array lists
            return callDatabaseReturnDataTableFromArrayListFour(sqlCmd, parms, searchText);
        }
        private System.Data.DataTable callDatabaseReturnDataTableFromArrayListFour(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Private access returns datatable from parameters and array list
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int searchTimes = 0;
            int parmsTimes = 0;
            while (searchText.Count > searchTimes)
            {
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                searchTimes++;
            }
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public System.Data.DataTable MakeDatabaseCallToReturnDataTableFromArrayListThree(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Some queries required array lists in order to work properly
            //This returns a datatable using those array lists
            return callDatabaseReturnDataTableFromArrayListThree(sqlCmd, parms, searchText);
        }
        private System.Data.DataTable callDatabaseReturnDataTableFromArrayListThree(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Private access returns datatable from parameters and array list
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int searchTimes = 0;
            int parmsTimes = 0;
            while (searchText.Count > searchTimes)
            {
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                searchTimes++;
            }
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public System.Data.DataTable MakeDatabaseCallToReturnDataTableFromArrayListTwo(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Some queries required array lists in order to work properly
            //This returns a datatable using those array lists
            return callDatabaseReturnDataTableFromArrayListTwo(sqlCmd, parms, searchText);
        }
        private System.Data.DataTable callDatabaseReturnDataTableFromArrayListTwo(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            //Private access returns datatable from parameters and array list
            System.Data.DataTable dt = new System.Data.DataTable();
            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = sqlCmd;
            int searchTimes = 0;
            int parmsTimes = 0;
            while (searchText.Count > searchTimes)
            {
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                cmd.Parameters.AddWithValue(parms[parmsTimes].ToString(), searchText[searchTimes]);
                parmsTimes++;
                searchTimes++;
            }
            cmd.Connection = con;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }
    }
}