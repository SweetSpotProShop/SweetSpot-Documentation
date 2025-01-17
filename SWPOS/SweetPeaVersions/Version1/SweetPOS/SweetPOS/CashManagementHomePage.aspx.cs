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
    public partial class CashManagementHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        ReportTools RT = new ReportTools();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "CashManagementHomePage.aspx";
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
                    btnProcessTillCashOut.Focus();
                    DateTime dtmNow = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                    calSearchDate.SelectedDate = dtmNow;
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack)
                    {
                        grdCurrentOpenTills.DataSource = SRM.ReturnOpenTills(dtmNow, CU);
                        grdCurrentOpenTills.DataBind();

                        grdCurrentClosedTills.DataSource = SRM.ReturnProcessedTillClosed(dtmNow, CU);
                        grdCurrentClosedTills.DataBind();
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
        protected void btnProcessTillCashOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessTillCashOut_Click";
            try
            {
                DateTime selectedDate = calSearchDate.SelectedDate;
                int tillReconciliation = SRM.TillReconciliationProcessCheck(selectedDate, CU);
                if(tillReconciliation == 1)
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("selectedDate", selectedDate.ToShortDateString());
                    Response.Redirect("TillReconciliation.aspx?" + nameValues, true);
                }
                else if(tillReconciliation == 0)
                {
                    MessageBox.ShowMessage("No transactions have been processed on this Till.", this);
                }
                else if(tillReconciliation == 2)
                {
                    MessageBox.ShowMessage("This till has already been cashed out for the selected date.", this);
                }
                else if(tillReconciliation == 3)
                {
                    MessageBox.ShowMessage("There are still open transactions that need to be processed or cancelled.", this);
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
        protected void btnProcessDailyReconciliation_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessDailyReconciliation_Click";
            try
            {
                int indicator = SRM.VerifyReconciliationCanBeProcessed(calSearchDate.SelectedDate, CU);
                if (indicator == 1)
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("reconcilaitionDate", calSearchDate.SelectedDate.ToShortDateString());
                    nameValues.Set("storeLocation", CU.currentStoreLocation.intStoreLocationID.ToString());
                    //Changes to the Reports Cash Out page
                    Response.Redirect("DailySalesReconciliation.aspx?" + nameValues, true);
                }
                else if (indicator == 0)
                {
                    MessageBox.ShowMessage("Either No Transactions have been processed or there are still open till(s) that will need to be closed before a Reconciliation can be completed.", this);
                }
                else if (indicator == 2)
                {
                    MessageBox.ShowMessage("A Reconciliation has already been completed for the selected date.", this);
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
        protected void btnFinalizeReconciliation_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnFinalizeReconciliation_Click";
            try
            {
                DateTime[] dtm = { calSearchDate.SelectedDate, calSearchDate.SelectedDate };
                object[] reportCriteria = new object[] { dtm, CU.currentStoreLocation.intStoreLocationID, CU };
                int indicator = RT.CashoutsProcessed(reportCriteria);
                ////Check to see if there are sales first
                if (indicator == 0)
                {
                    Session["reportCriteria"] = reportCriteria;
                    Response.Redirect("ReportsDailySalesReconciliation.aspx", true);
                }
                else if (indicator == 1)
                {
                    MessageBox.ShowMessage("No Reconciliations have been processed for selected date.", this);
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

        //protected void grdCurrentOpenTills_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    string strMethod = "grdCurrentOpenTills_RowDataBound";
        //    //Current method does nothing
        //    try
        //    {
        //        Button btn = (Button)e.Row.FindControl("btnUnprocess");
        //        if (btn != null)
        //        {
        //            if (e.Row.RowType == DataControlRowType.DataRow)
        //            {
        //                if (Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "bitIsProcessed")))
        //                {
        //                    btn.Enabled = true;
        //                }
        //            }
        //        }
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        //protected void grdCurrentOpenTills_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    //    //Collects current method for error tracking
        //    string strMethod = "grdCurrentOpenTills_RowCommand";
        //    try
        //    {
        //        SRM.UnProcessTillCashout(Convert.ToInt32(e.CommandArgument), CU);
        //        grdCurrentOpenTills.DataSource = SRM.ReturnProcessedTillClosed(Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
        //        grdCurrentOpenTills.DataBind();
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}


        protected void grdCurrentClosedTills_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            string strMethod = "grdCurrentClosedTills_RowDataBound";
            //Current method does nothing
            try
            {
                Button btn = (Button)e.Row.FindControl("btnUnprocess");
                if (btn != null)
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        if(Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "bitIsProcessed")))
                        {
                            btn.Enabled = true;
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
        protected void grdCurrentClosedTills_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        //    //Collects current method for error tracking
            string strMethod = "grdCurrentClosedTills_RowCommand";
            try
            {
                SRM.UnProcessTillCashout(Convert.ToInt32(e.CommandArgument), CU);
                DateTime dtmNow = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));

                grdCurrentOpenTills.DataSource = SRM.ReturnOpenTills(dtmNow, CU);
                grdCurrentOpenTills.DataBind();

                grdCurrentClosedTills.DataSource = SRM.ReturnProcessedTillClosed(dtmNow, CU);
                grdCurrentClosedTills.DataBind();
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