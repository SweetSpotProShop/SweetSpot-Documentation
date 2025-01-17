using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class PurchasesCheckout : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InvoiceManager IM = new InvoiceManager();
        ReceiptManager RM = new ReceiptManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;
        private static Invoice invoice;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PurchasesCheckout.aspx";
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
                    btnCash.Focus();
                    if (!Page.IsPostBack)
                    {
                        UpdatePageTotals();
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
        //Cash
        protected void btnCash_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCash_Click";
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
        protected void btnCheque_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCheque_Click";
            try
            {
                string mopName = ((Button)sender).Text;
                populateGridviewMOP(RM.ReturnMOPTypeID(mopName));
                txtChequeNumber.Text = "0000";                
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
        protected void btnDebit_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnDebit_Click";
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
        protected void btnGiftCard_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnGiftCard_Click";
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
        //Populating gridview with MOPs
        protected void populateGridviewMOP(int methodOfPaymentID)
        {
            //Collects current method for error tracking
            string strMethod = "populateGridviewMOP";
            try
            {
                double amountPaid = 0;
                if(double.TryParse(txtPurchaseAmount.Text.Replace("$",""), out amountPaid))
                {
                    object[] paymentCriteria = { invoice.intInvoiceID, methodOfPaymentID, amountPaid, txtChequeNumber.Text };
                    IM.AddPaymentToInvoice(paymentCriteria, CU.terminal.intBusinessNumber);
                    //Center the mop grid view
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
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "OnRowDeleting";
            try
            {
                //Retrieves index of selected row
                IM.RemovePaymentFromInvoice(Convert.ToInt32(((Label)gvCurrentPayment.Rows[e.RowIndex].Cells[3].FindControl("lblPaymnetID")).Text), CU.terminal.intBusinessNumber);
                //Clear the selected index
                gvCurrentPayment.EditIndex = -1;
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
        protected void UpdatePageTotals()
        {
            string strMethod = "UpdatePageTotals";
            try
            {
                invoice = IM.ReturnInvoiceCurrent(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.terminal.intBusinessNumber)[0];
                lblTotalPurchaseAmount.Text = invoice.fltCostTotal.ToString("C");
                double dblAmountPaid = 0;
                foreach (var payment in invoice.lstInvoicePayment)
                {
                    //Adds the total amount paid fropm each mop type
                    dblAmountPaid += payment.fltAmountReceived;
                }
                gvCurrentPayment.DataSource = invoice.lstInvoicePayment;
                gvCurrentPayment.DataBind();

                lblRemainingPurchaseDueDisplay.Text = (invoice.fltCostTotal - dblAmountPaid).ToString("C");
                //Updates the amount paying with the remaining balance
                txtPurchaseAmount.Text = (invoice.fltCostTotal - dblAmountPaid).ToString("C");
                buttonDisable(invoice.fltCostTotal - dblAmountPaid);
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
                if (rb >= -.001 && rb <= 0.001)
                {
                    if (IM.VerifyPaymentOnInvoice(invoice.intInvoiceID, CU.terminal.intBusinessNumber))
                    {
                        btnCash.Enabled = false;
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
                    }
                    btnDebit.Enabled = false;
                    btnGiftCard.Enabled = false;
                    btnCheque.Enabled = false;
                }
                else
                {
                    btnCash.Enabled = true;
                    btnDebit.Enabled = true;
                    btnGiftCard.Enabled = true;
                    btnCheque.Enabled = true;
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
        //Other functionality
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelPurchase_Click";
            try
            {
                invoice.bitIsInvoiceCancelled = true;
                IM.CancelPurchaseInvoice(invoice, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                //Change to Home Page
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
        protected void btnSavePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnSavePurchase_Click";
            try
            {
                //Change to Home Page
                Response.Redirect("PurchaseOrderHomePage.aspx", true);
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
        protected void btnReturnToPurchaseCart_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnReturnToPurchaseCart_Click";
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                Response.Redirect("PurchasesCart.aspx?" + nameValues, true);
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
        protected void btnFinalizePurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnFinalizePurchase_Click";
            try
            {
                CU = (CurrentUser)Session["currentUser"];
                //Checks the amount paid and the bypass check box
                if (!txtPurchaseAmount.Text.Equals("$0.00"))
                {
                    //Displays message
                    MessageBox.ShowMessage("Remaining Amount Does NOT Equal $0.00.", this);
                }
                else
                {
                    if (IM.VerifyPaymentOnInvoice(invoice.intInvoiceID, CU.terminal.intBusinessNumber))
                    {
                        invoice.varInvoiceComments = txtComments.Text;
                        //Stores all the Sales data to the database
                        IM.FinalizeInvoice(invoice, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                        var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                        //Changes page to printable invoice
                        Response.Redirect("PrintablePurchase.aspx?" + nameValues, true);
                    }
                    else
                    {
                        MessageBox.ShowMessage("At least one method of payment "
                            + "is required even for a $0.00 sale.", this);
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
    }
}