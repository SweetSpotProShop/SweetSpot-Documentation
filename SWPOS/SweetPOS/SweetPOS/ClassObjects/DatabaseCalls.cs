using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;

namespace SweetPOS.ClassObjects
{
    public class DatabaseCalls
    {
        public DatabaseCalls() { }
        public DataTable MakeDatabaseCallToReturnDataTable(string sqlCmd, object[][] parms)
        {
            return callDatabaseReturnDataTable(sqlCmd, parms);
        }
        public string MakeDatabaseCallToReturnString(string sqlCmd, object[][] parms)
        {
            try
            {
                return callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0].ToString();
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
                return Convert.ToBoolean(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0]);
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public int MakeDatabaseCallToReturnInt(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToInt32(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0].ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDatabaseCallToReturnDouble(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToDouble(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][0].ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public void ExecuteNonReturnQuery(string sqlCmd, object[][] parms)
        {
            callDatabaseExecuteNonReturnQuery(sqlCmd, parms);
        }

        private DataTable callDatabaseReturnDataTable(string sqlCmd, object[][] parms)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
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
            string connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
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
            try
            {
                return callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][2].ToString();
            }
            catch (Exception e)
            {
                return "";
            }
        }
        public int MakeDatabaseCallToReturnSecondColumnAsInt(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToInt32(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][1].ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }
        public double MakeDatabaseCallToReturnSecondColumnAsDouble(string sqlCmd, object[][] parms)
        {
            try
            {
                return Convert.ToDouble(callDatabaseReturnDataTable(sqlCmd, parms).Rows[0][1].ToString());
            }
            catch (Exception e)
            {
                return -10;
            }
        }

        public DataTable MakeDatabaseCallToReturnDataTableFromArrayListFour(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            return callDatabaseReturnDataTableFromArrayListFour(sqlCmd, parms, searchText);
        }
        private DataTable callDatabaseReturnDataTableFromArrayListFour(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
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

        public DataTable MakeDatabaseCallToReturnDataTableFromArrayListThree(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            return callDatabaseReturnDataTableFromArrayListThree(sqlCmd, parms, searchText);
        }
        private DataTable callDatabaseReturnDataTableFromArrayListThree(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
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

        public DataTable MakeDatabaseCallToReturnDataTableFromArrayListTwo(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            return callDatabaseReturnDataTableFromArrayListTwo(sqlCmd, parms, searchText);
        }
        private DataTable callDatabaseReturnDataTableFromArrayListTwo(string sqlCmd, ArrayList parms, ArrayList searchText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["SweetPeaConnectionString"].ConnectionString;
            DataTable dt = new DataTable();
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