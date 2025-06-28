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
    public partial class ReturnsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        ReceiptManager RM = new ReceiptManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "ReturnsHomePage.aspx";
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
                    txtReceiptSearch.Focus();
                    if (!IsPostBack) { calSearchDate.SelectedDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())); }
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
        protected void btnReceiptSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReceiptSearch_Click";
            try
            {
                grdReceiptSelection.DataSource = RM.ReturnReceiptBasedOnSearchCriteriaForReturns(calSearchDate.SelectedDate, txtReceiptSearch.Text, CU.terminal.intBusinessNumber);
                grdReceiptSelection.DataBind();
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
        protected void grdReceiptSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdReceiptSelection_RowCommand";
            try
            {
                //Checks that the command name is return invoice
                if (e.CommandName == "returnReceipt")
                {
                    //Retrieves all the invoices that were searched
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("receipt", Convert.ToString(e.CommandArgument));
                    //Changes to Returns cart
                    Response.Redirect("ReturnsCart.aspx?" + nameValues, true);
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