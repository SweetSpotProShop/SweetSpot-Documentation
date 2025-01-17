using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class CustomerHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        CurrentUserManager CUM = new CurrentUserManager();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "CustomerHomePage";
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
        protected void btnCustomerSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnCustomerSearch_Click";
            try
            {
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                grdCustomersSearched.DataBind();
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
        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnAddNewCustomer_Click";
            try
            {
                //Opens the page to add a new customer
                Response.Redirect("CustomerAddNew.aspx?customer=-10", true);
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
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdCustomersSearched_RowCommand";
            try
            {
                if (e.CommandName == "ViewProfile")
                {
                    //open Add New Customer page
                    Response.Redirect("CustomerAddNew.aspx?customer=" + e.CommandArgument.ToString(), true);
                }
                else if (e.CommandName == "StartSale")
                {
                    DateTime dtmToday = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                    if (!SRM.TillAlreadyCashedOut(dtmToday, CU))
                    {
                        if (SRM.TillReconciliationProcessCheck(dtmToday, CU) == 0)
                        {
                            TillCashout tillCashout = new TillCashout
                            {
                                intTerminalID = CU.terminal.intTerminalID,
                                dtmTillCashoutDate = dtmToday,
                                dtmTillCashoutProcessedDate = dtmToday,
                                dtmTillCashoutProcessedTime = dtmToday,
                                intHundredDollarBillCount = 0,
                                intFiftyDollarBillCount = 0,
                                intTwentyDollarBillCount = 0,
                                intTenDollarBillCount = 0,
                                intFiveDollarBillCount = 0,
                                intToonieCoinCount = 0,
                                intLoonieCoinCount = 0,
                                intQuarterCoinCount = 0,
                                intDimeCoinCount = 0,
                                intNickelCoinCount = 0,
                                fltCashDrawerTotal = 0,
                                fltCountedTotal = 0,
                                fltCashDrawerFloat = CU.terminal.fltDrawerFloatAmount,
                                fltCashDrawerCashDrop = 0
                            };
                            SRM.ProcessTerminalReconciliation(tillCashout, dtmToday, CU.terminal.intBusinessNumber);
                        }
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("customer", e.CommandArgument.ToString());
                        nameValues.Set("receipt", "-10");
                        Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, false);
                        //Changes page to Sales Cart
                        Response.Redirect("SalesCart.aspx?" + nameValues, true);
                    }
                    else
                    {
                        MessageBox.ShowMessage("This till has already been cashed out. Unprocess to continue.", this);
                    }
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
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdCustomersSearched_PageIndexChanging";
            try
            {
                grdCustomersSearched.PageIndex = e.NewPageIndex;
                //Looks through database and returns a list of customers
                //based on the search criteria entered
                //Binds the results to the gridview
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                grdCustomersSearched.DataBind();
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