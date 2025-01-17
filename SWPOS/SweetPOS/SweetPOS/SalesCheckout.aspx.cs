using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class SalesCheckout : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        ReceiptManager RM = new ReceiptManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;
        private static Receipt receipt;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "SalesCheckout.aspx";
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
                    mopAmericanExpress.Focus();
                    if (!Page.IsPostBack)
                    {
                        receipt = RM.ReturnReceiptCurrent(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.terminal.intBusinessNumber)[0];

                        lblGovernment.Visible = true;
                        lblGovernmentAmount.Text = receipt.fltGovernmentTaxTotal.ToString("C");
                        lblGovernmentAmount.Visible = true;
                        lblProvincial.Visible = true;
                        lblProvincialAmount.Text = receipt.fltProvincialTaxTotal.ToString("C");
                        lblProvincialAmount.Visible = true;
                        UpdatePageTotals();
                        //***Assign each item to its Label.
                        lblTotalInCartAmount.Text = receipt.fltCartTotal.ToString("C");
                        lblTotalInDiscountsAmount.Text = receipt.fltDiscountTotal.ToString("C");
                        lblTradeInsAmount.Text = receipt.fltTradeInTotal.ToString("C");
                        lblSubTotalAmount.Text = (receipt.fltCartTotal - receipt.fltDiscountTotal + receipt.fltTradeInTotal).ToString("C");
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

        //American Express
        protected void mopAmericanExpress_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopAmericanExress_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Cash
        protected void mopCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopCash_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Cheque
        protected void mopCheque_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopCheque_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Debit
        protected void mopDebit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopDebit_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Discover
        protected void mopDiscover_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopDiscover_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Gift Card
        protected void mopGiftCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopGiftCard_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Mastercard
        protected void mopMastercard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopMastercard_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Visa
        protected void mopVisa_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopVisa_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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
        //Charge To Account
        protected void mopChargeToAccount_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopChargeToAccount_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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

        protected void gvCurrentMOPs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "gvCurrentMOPs_RowCommand";
            try
            {
                RM.RemoveReceiptPaymentCurrent(Convert.ToInt32(e.CommandArgument), CU.terminal.intBusinessNumber);
                ////Clear the selected index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
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

        //Other functionality
        protected void btnCancelSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelSale_Click";
            try
            {
                receipt.bitIsReceiptCancelled = true;
                RM.LoopThroughTheItemsToReturnToInventory(receipt.lstReceiptItem, CU.terminal.intBusinessNumber);
                RM.CancelReceiptCurrent(receipt, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                Response.Redirect("HomePage.aspx", true);
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
        protected void btnExitSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnExitSale_Click";
            try
            {
                Response.Redirect("SalesHomePage.aspx", true);
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
        protected void btnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReturnToCart_Click";
            try
            {
                //Sets session to true
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                nameValues.Set("receipt", receipt.intReceiptID.ToString());
                //Changes to Sales Cart page
                Response.Redirect("SalesCart.aspx?" + nameValues, true);
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
        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnFinalize_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Employee
                EmployeeManager EM = new EmployeeManager();
                if (EM.ReturnCanEmployeeMakeSale(txtEmployeePasscode.Text, CU))
                {
                    //Checks the amount paid and the bypass check box
                    if (!txtAmountPaying.Text.Equals("$0.00"))
                    {
                        //Displays message
                        MessageBox.ShowMessage("Remaining Balance Does NOT Equal $0.00.", this);
                    }
                    else
                    {
                        if (RM.VerifyAMethodOfPaymentHasBeenAdded(receipt.intReceiptID, CU.terminal.intBusinessNumber))
                        {
                            //Stores all the Sales data to the database
                            receipt = RM.ReturnReceiptCurrent(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];
                            receipt.employee = EM.ReturnEmployeeFromEFSHJEMVIF(txtEmployeePasscode.Text, CU.terminal.intBusinessNumber)[0];
                            receipt.varReceiptComments = txtComments.Text;
                            RM.FinalizeReceiptCurrent(receipt, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                            nameValues.Set("receipt", receipt.intReceiptID.ToString());
                            Response.Redirect("PrintableReceipt.aspx?" + nameValues, true);
                        }
                        else
                        {
                            MessageBox.ShowMessage("At least one method of payment "
                                + "is required even for a $0.00 sale.", this);
                        }
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Invalid employee passcode entered. "
                    + "Please try again.", this);
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

        //Populating gridview with MOPs
        private void populateGridviewMOP(int methodOfPaymentID)
        {
            //Collects current method for error tracking
            string strMethod = "populateGridviewMOP";
            try
            {
                double amountPaid = 0;
                if (double.TryParse(txtAmountPaying.Text.Replace("$", ""), out amountPaid))
                {
                    if (methodOfPaymentID == RM.ReturnMOPTypeID("Cash"))
                    {
                        //receipt.fltBalanceDueTotal = RM.CalculateRoundingForCash(receipt, CU.terminal.intBusinessNumber);
                        //Cash Purchase, check to see if there is change due
                        if (amountPaid > receipt.fltBalanceDueTotal)
                        {
                            //Change is due to customer
                            receipt.fltChangeAmount = amountPaid - receipt.fltBalanceDueTotal;
                            amountPaid = receipt.fltBalanceDueTotal;
                        }
                    }
                    RM.AddNewPaymentIntoReceiptPaymentCurrent(receipt.intReceiptID, amountPaid, methodOfPaymentID, CU.terminal.intBusinessNumber);
                    UpdatePageTotals();
                }
                else
                {
                    MessageBox.ShowMessage("The amount entered for payment is not valid. "
                    + "Please try again.", this);
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
        private void buttonDisable(double rb)
        {
            string strMethod = "buttonDisable";
            try
            {
                if (rb >= -.001 && rb <= .001)
                {
                    if (RM.VerifyAMethodOfPaymentHasBeenAdded(receipt.intReceiptID, CU.terminal.intBusinessNumber))
                    {
                        mopCash.Enabled = false;
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
                    }
                    mopAmericanExpress.Enabled = false;
                    mopCheque.Enabled = false;
                    mopDebit.Enabled = false;
                    mopDiscover.Enabled = false;
                    mopGiftCard.Enabled = false;
                    mopMastercard.Enabled = false;
                    mopVisa.Enabled = false;
                    mopChargeToAccount.Enabled = false;
                    btnRemoveGov.Enabled = false;
                    btnRemoveProv.Enabled = false;
                }
                else
                {
                    mopAmericanExpress.Enabled = true;
                    mopCash.Enabled = true;
                    mopCheque.Enabled = true;
                    mopDebit.Enabled = true;
                    mopDiscover.Enabled = true;
                    mopGiftCard.Enabled = true;
                    mopMastercard.Enabled = true;
                    mopVisa.Enabled = true;
                    if(receipt.customer.bitCustomerHasAccount)
                    {
                        mopChargeToAccount.Enabled = true;
                    }
                    btnRemoveGov.Enabled = true;
                    btnRemoveProv.Enabled = true;
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
                    + "If you continue to receive the message please contact "
                    + "your system administrator.", this);
            }
        }
        private void UpdatePageTotals()
        {
            string strMethod = "UpdatePageTotals";
            try
            {
                receipt = RM.ReturnReceiptCurrent(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];
                //Loops through each mop
                foreach (var methodOfPayment in receipt.lstReceiptPayment)
                {
                    //Adds the total amount paid fropm each mop type
                    receipt.fltTenderedAmount += methodOfPayment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = receipt.lstReceiptPayment;
                gvCurrentMOPs.DataBind();
                double taxTotal = 0;
                taxTotal += receipt.fltGovernmentTaxTotal;
                taxTotal += receipt.fltProvincialTaxTotal;
                //Displays the remaining balance
                receipt.fltBalanceDueTotal = (receipt.fltCartTotal - receipt.fltDiscountTotal + receipt.fltTradeInTotal) + receipt.fltShippingTotal + taxTotal;
                double remainingBalance = receipt.fltBalanceDueTotal - receipt.fltTenderedAmount;
                lblBalanceAmount.Text = receipt.fltBalanceDueTotal.ToString("C");
                lblRemainingBalanceDueDisplay.Text = remainingBalance.ToString("C");
                txtAmountPaying.Text = remainingBalance.ToString("C");
                buttonDisable(remainingBalance);
            }
            catch (ThreadAbortException) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
                //Display message box
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive the message please contact "
                    + "your system administrator.", this);
            }
        }
    }
}