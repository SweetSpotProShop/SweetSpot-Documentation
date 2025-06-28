using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class VendorHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        VendorSupplierManager VSM = new VendorSupplierManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "VendorHomePage";
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
                    txtSearch.Focus();
                    CU = (CurrentUser)Session["currentUser"];
                }
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnVendorSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnVendorSearch_Click";
            try
            {
                //Looks through database and returns a list of vendors
                //based on the search criteria entered
                //Binds the results to the gridview
                grdVendorsSearched.Visible = true;
                grdVendorsSearched.DataSource = VSM.ReturnVendorSupplierBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                grdVendorsSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void btnAddNewVendor_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnAddNewVendor_Click";
            try
            {
                //Opens the page to add a new vendor
                Response.Redirect("VendorAddNew.aspx?vendor=-10", true);
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void grdVendorsSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdVendorsSearched_RowCommand";
            try
            {
                //open Add New Vendor page
                Response.Redirect("VendorAddNew.aspx?vendor=" + e.CommandArgument.ToString(), true);                
            }
            //Exception catch
            catch (ThreadAbortException) { }
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
        protected void grdVendorsSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdVendorsSearched_PageIndexChanging";
            try
            {
                grdVendorsSearched.PageIndex = e.NewPageIndex;
                //Looks through database and returns a list of vendors
                //based on the search criteria entered
                //Binds the results to the gridview
                grdVendorsSearched.Visible = true;
                grdVendorsSearched.DataSource = VSM.ReturnVendorSupplierBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                grdVendorsSearched.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException) { }
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