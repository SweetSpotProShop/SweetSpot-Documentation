using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class SalesHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        ReportTools RT = new ReportTools();
        CurrentUserManager CUM = new CurrentUserManager();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "SalesHomePage.aspx";
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
                    btnQuickSale.Focus();
                    if (!IsPostBack)
                    {
                        //Binds invoice list to the grid view
                        grdCurrentOpenSales.DataSource = RT.ReturnCurrentOpenReceipt(CU);
                        grdCurrentOpenSales.DataBind();
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
        protected void btnQuickSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnQuickSale_Click";
            try
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
                    nameValues.Set("customer", CM.ReturnGuestCustomerForLocation(CU.currentStoreLocation.intStoreLocationID, CU.terminal.intBusinessNumber).ToString());
                    nameValues.Set("receipt", "-10");
                    //Changes page to Sales Cart
                    Response.Redirect("SalesCart.aspx?" + nameValues, true);
                }
                else
                {
                    MessageBox.ShowMessage("This till has already been cashed out. Unprocess to continue.", this);
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
        protected void btnProcessReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessReturn_Click";
            try
            {
                //Changes page to Returns Home Page
                Response.Redirect("ReturnsHomePage.aspx", true);
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
        protected void btnReceiptSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReceiptSearch_Click";
            try
            {
                Response.Redirect("ReceiptSearch.aspx", true);
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
        protected void btnCashManagement_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCashManagement_Click";
            try
            {
                //Changes to the Reports Cash Out page
                Response.Redirect("CashManagementHomePage.aspx", true);
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

        protected void grdCurrentOpenSales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdCurrentOpenSales_RowCommand";
            try
            {
                if (!SRM.TillAlreadyCashedOut(Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU))
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    //Still need to get the cust on the Invoice
                    int index = ((GridViewRow)(((LinkButton)e.CommandSource).NamingContainer)).RowIndex;
                    nameValues.Set("customer", ((Label)grdCurrentOpenSales.Rows[index].Cells[11].FindControl("lblCustomerID")).Text);
                    int receiptIdentifier = Convert.ToInt32(e.CommandArgument);
                    nameValues.Set("receipt", receiptIdentifier.ToString());

                    int tranType = Convert.ToInt32(((Label)grdCurrentOpenSales.Rows[index].Cells[12].FindControl("lblTransactionTypeID")).Text);
                    if (tranType == 1)
                    {
                        //Changes page to Sales Cart
                        Response.Redirect("SalesCart.aspx?" + nameValues, true);
                    }
                    else if (tranType == 2)
                    {
                        //Changes page to Returns Cart
                        Response.Redirect("ReturnsCart.aspx?" + nameValues, true);
                    }
                }
                else
                {
                    MessageBox.ShowMessage("This till has already been cashed out. Unprocess to continue.", this);
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
    }
}