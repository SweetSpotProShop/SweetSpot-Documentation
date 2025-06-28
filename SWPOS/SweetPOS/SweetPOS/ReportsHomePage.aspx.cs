using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class ReportsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        ReportTools RT = new ReportTools();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "ReportsHomePage";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("SweetPea.aspx", true);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    ddlStoreLocation.Focus();
                    if (!IsPostBack)
                    {
                        //Sets the calendar and text boxes start and end dates
                        calStartDate.SelectedDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        calEndDate.SelectedDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                        ddlStoreLocation.DataBind();
                        ddlStoreLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();

                        EmployeeManager EM = new EmployeeManager();
                        if (EM.ReturnSuDoIDForLocation(CU))
                        {
                            //lblReportAccess.Visible = true;
                            //pnlDefaultButton.Visible = false;
                        }
                    }
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void btnCashOutReport_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCashOutReport_Click";
            try
            {
                //Stores report dates into Session
                DateTime[] dtm = { calStartDate.SelectedDate, calEndDate.SelectedDate };
                object[] reportCriteria = new object[] { dtm, Convert.ToInt32(ddlStoreLocation.SelectedValue), CU };
                int indicator = RT.CashoutsProcessed(reportCriteria);
                ////Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportCriteria"] = reportCriteria;
                    Response.Redirect("ReportsDailySalesReconciliation.aspx", true);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Reconciliations have been processed for selected date range.", this);
                }
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}