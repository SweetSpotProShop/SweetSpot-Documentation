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
    public partial class ReturnsCheckout : System.Web.UI.Page
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
            Session["currentPage"] = "ReturnsCheckout.aspx";
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
                        UpdatePageTotals();
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

        //American Express
        protected void mopAmericanExpress_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "mopAmericanExpress_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
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

        protected void gvCurrentMOPs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "gvCurrentMOPs_RowCommand";
            try
            {
                RM.RemoveReceiptPaymentCurrent(Convert.ToInt32(e.CommandArgument), CU.terminal.intBusinessNumber);
                //Clear the row index
                gvCurrentMOPs.EditIndex = -1;
                UpdatePageTotals();
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

        //Other functionality
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelReturn_Click";
            try
            {
                RM.LoopThroughTheItemsToReturnToInventory(receipt.lstReceiptItem, CU.terminal.intBusinessNumber);
                RM.CancelReceiptCurrent(receipt, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                Response.Redirect("HomePage.aspx", true);
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
        protected void btnReturnToCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReturnToCart_Click";
            try
            {
                //Changes page to Returns Cart page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", receipt.intReceiptID.ToString());
                Response.Redirect("ReturnsCart.aspx?" + nameValues, true);
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
        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnFinalize_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Checks to make sure total is 0
                if (!txtAmountRefunding.Text.Equals("$0.00"))
                {
                    //Displays message box that refund will need to = 0
                    MessageBox.ShowMessage("Remaining Refund Does NOT Equal $0.00.", this);
                }
                else
                {
                    if (RM.VerifyAMethodOfPaymentHasBeenAdded(receipt.intReceiptID, CU.terminal.intBusinessNumber))
                    {
                        receipt.varReceiptComments = txtComments.Text;
                        RM.FinalizeReceiptCurrent(receipt, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("receipt", receipt.intReceiptID.ToString());
                        Response.Redirect("PrintableReceiptReturn.aspx?" + nameValues, true);
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 return.", this);
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

        //Populating gridview with MOPs
        protected void populateGridviewMOP(int methodOfPaymentID)
        {
            //Collects current method for error tracking
            string strMethod = "populateGridviewMOP";
            try
            {
                double amountToReturn = 0;
                if (double.TryParse(txtAmountRefunding.Text.Replace("$", ""), out amountToReturn))
                {
                    RM.AddNewPaymentIntoReceiptPaymentCurrent(receipt.intReceiptID, amountToReturn, methodOfPaymentID, CU.terminal.intBusinessNumber);
                    UpdatePageTotals();
                }
                else
                {
                    MessageBox.ShowMessage("The amount entered for payment is not valid. "
                    + "Please try again.", this);
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
        public void buttonDisable(double rb)
        {
            string strMethod = "buttonDisable";
            try
            {
                if (rb >= -.001 && rb <= 0.001)
                {
                    mopAmericanExpress.Enabled = false;
                    mopCash.Enabled = false;
                    mopCheque.Enabled = false;
                    mopDebit.Enabled = false;
                    mopDiscover.Enabled = false;
                    mopGiftCard.Enabled = false;
                    mopMastercard.Enabled = false;
                    mopVisa.Enabled = false;
                }
                else
                {
                    List<ReceiptPayment> lPayment = RM.ReturnReceiptPaymentsFromOriginalReceipt(receipt.intReceiptGroupID, CU.terminal.intBusinessNumber);
                    foreach(ReceiptPayment rp in lPayment)
                    {
                        switch (rp.varMethodOfPaymentName)
                        {
                            case "American Express":
                                mopAmericanExpress.Enabled = true;
                                break;
                            case "Cash":
                                mopCash.Enabled = true;
                                break;
                            case "Cheque":
                                mopCheque.Enabled = true;
                                break;
                            case "Debit":
                                mopDebit.Enabled = true;
                                break;
                            case "Discover":
                                mopDiscover.Enabled = true;
                                break;
                            case "Gift Card":
                                mopGiftCard.Enabled = true;
                                break;
                            case "MasterCard":
                                mopMastercard.Enabled = true;
                                break;
                            case "Visa":
                                mopVisa.Enabled = true;
                                break;
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
        private void UpdatePageTotals()
        {
            string strMethod = "UpdatePageTotals";
            try
            {
                receipt = RM.ReturnReceiptCurrent(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];
                lblGovernment.Visible = true;
                lblGovernmentAmount.Text = receipt.fltGovernmentTaxTotal.ToString("$#.##;-$#.##;$0.00");
                lblGovernmentAmount.Visible = true;
                lblProvincial.Visible = true;
                lblProvincialAmount.Text = receipt.fltProvincialTaxTotal.ToString("$#.##;-$#.##;$0.00");
                lblProvincialAmount.Visible = true;

                //***Assign each item to its Label.
                double subAmount = receipt.fltCartTotal;
                lblRefundSubTotalAmount.Text = subAmount.ToString("$#.##;-$#.##;$0.00");
                subAmount += receipt.fltGovernmentTaxTotal + receipt.fltProvincialTaxTotal;
                lblRefundBalanceAmount.Text = subAmount.ToString("$#.##;-$#.##;$0.00");

                foreach (var methodOfPayment in receipt.lstReceiptPayment)
                {
                    //Adds the total amount paid fropm each mop type
                    receipt.fltTenderedAmount += methodOfPayment.fltAmountPaid;
                }
                gvCurrentMOPs.DataSource = receipt.lstReceiptPayment;
                gvCurrentMOPs.DataBind();

                subAmount -= receipt.fltTenderedAmount;

                lblRemainingRefundDisplay.Text = subAmount.ToString("$#.##;-$#.##;$0.00");
                txtAmountRefunding.Text = subAmount.ToString("$#.##;-$#.##;$0.00");
                buttonDisable(subAmount);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Log all info into error table
                ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
                //Display message box
                MessageBox.ShowMessage("An error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }        
    }
}