using SweetPOS.ClassObjects;
using System;
using System.Threading;

namespace SweetPOS
{
    public partial class TillReconciliation : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;
        private static TillCashout tillCashout;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "TillReconciliation.aspx";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("SweetPea.aspx", true);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    txtHundredDollarBills.Focus();
                    if (!IsPostBack)
                    {
                        //Gathering the start and end dates
                        DateTime selectedDate = DateTime.Parse(Request.QueryString["selectedDate"].ToString());
                        lblTillCashoutDate.Text = "Till Cashout on: " + selectedDate.ToString("d") + " for "
                            + CU.currentStoreLocation.varStoreName.ToString() + ", Till #" + CU.terminal.intTillNumber;

                        if (SRM.IsCashoutAlreadyInPlace(selectedDate, CU))
                        {
                            tillCashout = SRM.ReturnTillCashout(selectedDate, CU)[0];
                            txtHundredDollarBills.Text = tillCashout.intHundredDollarBillCount.ToString();
                            txtFiftyDollarBills.Text = tillCashout.intFiftyDollarBillCount.ToString();
                            txtTwentyDollarBills.Text = tillCashout.intTwentyDollarBillCount.ToString();
                            txtTenDollarBills.Text = tillCashout.intTenDollarBillCount.ToString();
                            txtFiveDollarBills.Text = tillCashout.intFiveDollarBillCount.ToString();
                            txtToonieCoins.Text = tillCashout.intToonieCoinCount.ToString();
                            txtLoonieCoins.Text = tillCashout.intLoonieCoinCount.ToString();
                            txtQuarterCoins.Text = tillCashout.intQuarterCoinCount.ToString();
                            txtDimeCoins.Text = tillCashout.intDimeCoinCount.ToString();
                            txtNickelCoins.Text = tillCashout.intNickelCoinCount.ToString();

                            lblCountedCashTotal.Text = tillCashout.fltCountedTotal.ToString("C");
                            tillCashout.fltCashDrawerTotal = (SRM.ReturnExpectedCashDrawerTotal(selectedDate, CU) + CU.terminal.fltDrawerFloatAmount);
                            lblOverShortDisplay.Text = (tillCashout.fltCountedTotal - tillCashout.fltCashDrawerTotal).ToString("C");
                            lblDrawerFloat.Text = tillCashout.fltCashDrawerFloat.ToString("C");
                            lblTillCashDrop.Text = tillCashout.fltCashDrawerCashDrop.ToString("C");

                        }
                        else
                        {
                            tillCashout = new TillCashout
                            {
                                fltCashDrawerTotal = (SRM.ReturnExpectedCashDrawerTotal(selectedDate, CU) + CU.terminal.fltDrawerFloatAmount),
                                bitIsProcessed = false
                            };
                        }
                        lblExpectedCashTotal.Text = tillCashout.fltCashDrawerTotal.ToString("C");
                        tillCashout.intTerminalID = CU.terminal.intTerminalID;
                        tillCashout.fltCashDrawerFloat = CU.terminal.fltDrawerFloatAmount;
                        tillCashout.dtmTillCashoutDate = selectedDate;
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
        protected void btnProcessCashCount_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessCashCount_Click";
            try
            {
                tillCashout.intHundredDollarBillCount = Convert.ToInt32(txtHundredDollarBills.Text);
                tillCashout.intFiftyDollarBillCount = Convert.ToInt32(txtFiftyDollarBills.Text);
                tillCashout.intTwentyDollarBillCount = Convert.ToInt32(txtTwentyDollarBills.Text);
                tillCashout.intTenDollarBillCount = Convert.ToInt32(txtTenDollarBills.Text);
                tillCashout.intFiveDollarBillCount = Convert.ToInt32(txtFiveDollarBills.Text);
                tillCashout.intToonieCoinCount = Convert.ToInt32(txtToonieCoins.Text);
                tillCashout.intLoonieCoinCount = Convert.ToInt32(txtLoonieCoins.Text);
                tillCashout.intQuarterCoinCount = Convert.ToInt32(txtQuarterCoins.Text);
                tillCashout.intDimeCoinCount = Convert.ToInt32(txtDimeCoins.Text);
                tillCashout.intNickelCoinCount = Convert.ToInt32(txtNickelCoins.Text);

                tillCashout.fltCountedTotal = (tillCashout.intHundredDollarBillCount * 100) + (tillCashout.intFiftyDollarBillCount * 50)
                    + (tillCashout.intTwentyDollarBillCount * 20) + (tillCashout.intTenDollarBillCount * 10)
                    + (tillCashout.intFiveDollarBillCount * 5) + (tillCashout.intToonieCoinCount * 2) + (tillCashout.intLoonieCoinCount * 1)
                    + (tillCashout.intQuarterCoinCount * 0.25) + (tillCashout.intDimeCoinCount * 0.10) + (tillCashout.intNickelCoinCount * 0.05);
                lblCountedCashTotal.Text = tillCashout.fltCountedTotal.ToString("C");

                double expectedCash = 0;
                if (double.TryParse(lblExpectedCashTotal.Text.Replace("$", ""), out expectedCash))
                {
                    lblOverShortDisplay.Text = (tillCashout.fltCountedTotal - expectedCash).ToString("C");
                }
                lblDrawerFloat.Text = tillCashout.fltCashDrawerFloat.ToString("C");
                tillCashout.fltCashDrawerCashDrop = (tillCashout.fltCountedTotal - tillCashout.fltCashDrawerFloat);
                lblTillCashDrop.Text = tillCashout.fltCashDrawerCashDrop.ToString("C");
                btnProcessTillCashout.Enabled = true;
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
        protected void btnProcessTillCashout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessTillCashout_Click";
            try
            {
                if (SRM.IsCashoutAlreadyInPlace(DateTime.Parse(Request.QueryString["selectedDate"].ToString()), CU))
                {
                    tillCashout.bitIsProcessed = true;
                    SRM.UpdateTerminalReconciliation(tillCashout, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                }
                else
                {
                    
                }
                
                txtHundredDollarBills.Enabled = false;
                txtToonieCoins.Enabled = false;
                txtFiftyDollarBills.Enabled = false;
                txtLoonieCoins.Enabled = false;
                txtTwentyDollarBills.Enabled = false;
                txtQuarterCoins.Enabled = false;
                txtTenDollarBills.Enabled = false;
                txtDimeCoins.Enabled = false;
                txtFiveDollarBills.Enabled = false;
                txtNickelCoins.Enabled = false;
                
                btnProcessCashCount.Enabled = false;
                btnProcessTillCashout.Enabled = false;
                btnPrint.Enabled = true;
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