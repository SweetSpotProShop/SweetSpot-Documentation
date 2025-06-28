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
    public partial class ReceiptSearch : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        ReceiptManager RM = new ReceiptManager();
        CurrentUserManager CUM = new CurrentUserManager();
        LocationManager LM = new LocationManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "ReceiptSearch.aspx";
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
                    txtReceiptNumber.Focus();
                    if (!IsPostBack)
                    {
                        //Sets the calendar and text boxes start and end dates
                        calStartDate.SelectedDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        calEndDate.SelectedDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                        ddlStoreLocation.DataBind();
                        ddlStoreLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
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
        protected void btnReceiptSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReceiptSearch_Click";
            try
            {
                //Binds invoice list to the grid view
                grdReceiptSelection.DataSource = RM.ReturnReceiptBasedOnSearchCriteria(calStartDate.SelectedDate, calEndDate.SelectedDate, 
                    txtReceiptNumber.Text, Convert.ToInt32(ddlStoreLocation.SelectedValue), CU.terminal.intBusinessNumber);
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
                if (e.CommandName == "returnReceipt")
                {
                    //Changes to printable invoice page
                    Response.Redirect("PrintableReceipt.aspx?receipt=" + e.CommandArgument.ToString(), true);
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