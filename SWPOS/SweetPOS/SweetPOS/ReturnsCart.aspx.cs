using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class ReturnsCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        ReceiptManager RM = new ReceiptManager();
        ReceiptCalculationManager RCM = new ReceiptCalculationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;
        private static Receipt receipt;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "ReturnsCart.aspx";
            try
            {
                lblInvalidQuantity.Visible = false;
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Response.Redirect("SweetPea.aspx", true);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    btnCancelReturn.Focus();
                    if (!Page.IsPostBack)
                    {
                        List<Receipt> receiptsCalled = RM.ReturnReceipt(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.terminal.intBusinessNumber);
                        if (receiptsCalled.Count > 0)
                        {
                            receipt = receiptsCalled[0];
                            receipt = RM.CreateNewReturnReceipt(receipt, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU)[0];
                        }
                        else
                        {
                            receipt = RM.ReturnReceiptCurrentByReceiptNumber(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.terminal.intBusinessNumber)[0];
                        }
                        lblCustomerDisplay.Text = receipt.customer.varLastName.ToString() + ", " + receipt.customer.varFirstName.ToString();
                        lblReceiptNumberDisplay.Text = receipt.varReceiptNumber.ToString() + "-" + receipt.intReceiptSubNumber.ToString();
                        lblDateDisplay.Text = receipt.dtmReceiptCreationDate.ToString("yyyy-MM-dd");

                        //binds items in cart to gridview
                        grdReceiptItems.DataSource = RM.ReturnReceiptItemsFromProcessedSalesForReturn(receipt, CU.terminal.intBusinessNumber);
                        grdReceiptItems.DataBind();
                        //Wants datatable.
                        grdReturningItems.DataSource = receipt.lstReceiptItem;
                        grdReturningItems.DataBind();
                        lblReturnSubtotalDisplay.Text = receipt.fltCartTotal.ToString("C");
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
        protected void btnCancelReturn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelReturn_Click";
            try
            {
                lblInvalidQuantity.Visible = false;
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
        protected void btnProceedToReturnCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProceedToReturnCheckout_Click";
            try
            {
                if (RCM.CheckForItemsInTransaction(receipt))
                {
                    //Changes page to the returns checkout page
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("receipt", receipt.intReceiptID.ToString());
                    Response.Redirect("ReturnsCheckout.aspx?" + nameValues, true);
                }
                else
                {
                    MessageBox.ShowMessage("There are no items on this transaction.", this);
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
        protected void grdReceiptItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdReceiptItems_RowCommand";
            try
            {
                lblInvalidQuantity.Visible = false;
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                //Stores the info about the item in that index
                ReceiptItem selectedSku = RM.ReturnReceiptItemForReturnProcess(Convert.ToInt32(e.CommandArgument.ToString()), receipt.intReceiptGroupID, CU.terminal.intBusinessNumber);
                selectedSku.intReceiptID = receipt.intReceiptID;
                if (selectedSku.bitIsPercentageDiscount)
                {
                    selectedSku.fltItemPrice = selectedSku.fltItemPrice - (selectedSku.fltItemPrice * (selectedSku.fltItemDiscount / 100));
                }
                else
                {
                    selectedSku.fltItemPrice = selectedSku.fltItemPrice - selectedSku.fltItemDiscount;
                }
                double returnDollars = 0;
                string returnAmount = ((TextBox)grdReceiptItems.Rows[index].Cells[8].FindControl("txtReturnAmount")).Text.Replace("$", "");
                if (returnAmount != "")
                {
                    if (double.TryParse(returnAmount, out returnDollars))
                    {
                        returnDollars = Convert.ToDouble(returnAmount);
                    }
                }
                selectedSku.fltItemRefund = -1 * returnDollars;
                int currentQTY = selectedSku.intItemQuantity;
                string quantityForReturn = ((TextBox)grdReceiptItems.Rows[index].Cells[2].FindControl("txtQuantityToReturn")).Text;
                int quantitySold = Convert.ToInt32(((Label)grdReceiptItems.Rows[index].Cells[2].FindControl("lblQuantitySold")).Text);
                int returnQuantity = 1;
                if (quantityForReturn != "")
                {
                    if (int.TryParse(quantityForReturn, out returnQuantity))
                    {
                        returnQuantity = Convert.ToInt32(quantityForReturn);
                    }
                }
                selectedSku.intItemQuantity = returnQuantity;
                selectedSku.fltItemAverageCostAtSale = selectedSku.fltItemAverageCostAtSale * -1;
                if (returnQuantity > quantitySold || returnQuantity < 1)
                {
                    lblInvalidQuantity.Visible = true;
                }
                else
                {
                    if (!RM.ItemAlreadyInCart(selectedSku, CU.terminal.intBusinessNumber))
                    {
                        if (!selectedSku.bitIsNonStockedProduct)
                        {
                            RM.RemoveQuantityFromInventory(selectedSku.intInventoryID, (currentQTY + returnQuantity), CU.terminal.intBusinessNumber);
                        }
                        selectedSku.fltItemDiscount = 0;
                        RM.AddingItemToTheSale(selectedSku, receipt.intTransactionTypeID, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                        //deselect the indexed item
                        grdReceiptItems.EditIndex = -1;
                        //store items available for return in session
                        grdReceiptItems.DataSource = RM.ReturnReceiptItemsFromProcessedSalesForReturn(receipt, CU.terminal.intBusinessNumber);
                        grdReceiptItems.DataBind();

                        //recalculate the return total
                        receipt = RM.CalculateNewReceiptTotalsToUpdate(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];

                        grdReturningItems.DataSource = receipt.lstReceiptItem;
                        grdReturningItems.DataBind();

                        lblReturnSubtotalDisplay.Text = receipt.fltCartTotal.ToString("C");
                    }
                    else
                    {
                        if (RM.ItemAlreadyInReturnCartRefundAmountCheck(selectedSku, CU.terminal.intBusinessNumber))
                        {
                            if (!selectedSku.bitIsNonStockedProduct)
                            {
                                RM.RemoveQuantityFromInventory(selectedSku.intInventoryID, (currentQTY + returnQuantity), CU.terminal.intBusinessNumber);
                            }
                            selectedSku.intItemQuantity += RM.ReturnQuantityFromCurrentSaleCart(selectedSku.intInventoryID, selectedSku.intReceiptID, CU.terminal.intBusinessNumber);
                            RM.UpdateItemFromCurrentSalesTable(selectedSku, CU.terminal.intBusinessNumber);
                            //deselect the indexed item
                            grdReceiptItems.EditIndex = -1;
                            //store items available for return in session
                            grdReceiptItems.DataSource = RM.ReturnReceiptItemsFromProcessedSalesForReturn(receipt, CU.terminal.intBusinessNumber);
                            grdReceiptItems.DataBind();

                            //recalculate the return total
                            receipt = RM.CalculateNewReceiptTotalsToUpdate(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];

                            grdReturningItems.DataSource = receipt.lstReceiptItem;
                            grdReturningItems.DataBind();

                            lblReturnSubtotalDisplay.Text = receipt.fltCartTotal.ToString("C");
                        }
                        else
                        {
                            MessageBox.ShowMessage("Same item cannot be returned for a different amount. "
                                + "Either cancel item to set both at new return amount or process a second return.", this);
                        }
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
        protected void grdReturningItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdReturningItems_RowCommand";
            try
            {
                //lblInvalidQty.Visible = false;
                //Gathers index from selected line item
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                //Stores the info about the item in that index
                ReceiptItem selectedSku = RM.ReturnReceiptItemFromReceiptCurrentTable(Convert.ToInt32(e.CommandArgument.ToString()), receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];

                //add item to table and remove the added qty from current inventory
                RM.DoNotReturnTheItemOnReturn(selectedSku, CU.terminal.intBusinessNumber);
                RM.RemoveQuantityFromInventory(selectedSku.intInventoryID, selectedSku.intItemQuantity, CU.terminal.intBusinessNumber);

                //deselect the indexed item
                grdReceiptItems.EditIndex = -1;
                //store items available for return in session
                grdReceiptItems.DataSource = RM.ReturnReceiptItemsFromProcessedSalesForReturn(receipt, CU.terminal.intBusinessNumber);
                grdReceiptItems.DataBind();

                //recalculate the return total
                receipt = RM.CalculateNewReceiptTotalsToUpdate(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];

                grdReturningItems.DataSource = receipt.lstReceiptItem;
                grdReturningItems.DataBind();

                lblReturnSubtotalDisplay.Text = receipt.fltCartTotal.ToString("C");
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