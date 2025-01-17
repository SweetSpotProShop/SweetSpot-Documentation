using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class SalesCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        ReceiptManager RM = new ReceiptManager();
        ReceiptCalculationManager RCM = new ReceiptCalculationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;
        private static Receipt receipt;

        //Still need to account for a duplicate item being added
        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "SalesCart.aspx";
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
                    txtInventorySearch.Focus();
                    if (!Page.IsPostBack)
                    {
                        DateTime createDateTime = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        lblDateDisplay.Text = createDateTime.ToString("yyyy-MM-dd");

                        if (Request.QueryString["receipt"].ToString() == "-10")
                        {
                            receipt = RM.CreateNewSaleReceipt(Convert.ToInt32(Request.QueryString["customer"].ToString()), createDateTime, CU)[0];
                        }
                        else
                        {
                            receipt = RM.ReturnReceiptCurrent(Convert.ToInt32(Request.QueryString["receipt"].ToString()), CU.terminal.intBusinessNumber)[0];
                        }
                        txtCustomer.Text = receipt.customer.varLastName + ", " + receipt.customer.varFirstName;
                        lblReceiptNumberDisplay.Text = receipt.varReceiptNumber + "-" + receipt.intReceiptSubNumber.ToString();
                        UpdateReceiptTotal();
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

        //protected void txtShipAmount_TextChanged(object sender, EventArgs e)
        //{
        //    string strMethod = "txtShipAmount_TextChanged";
        //    try
        //    {
        //        //change the needed elements
        //        double shipAmount = 0;
        //        rdbToBeShipped.Checked = false;
        //        if (txtShipAmount.Text != "")
        //        {
        //            if (Convert.ToDouble(txtShipAmount.Text) != 0)
        //            {
        //                shipAmount = Convert.ToDouble(txtShipAmount.Text);
        //                rdbToBeShipped.Checked = true;
        //            }
        //        }
        //        //send back to update
        //        IM.UpdateCurrentInvoice(i[0]);
        //    }
        //    //Exception catch
        //    catch (ThreadAbortException tae) { }
        //    catch (Exception ex)
        //    {
        //        //Log all info into error table
        //        ER.logError(ex, CU.employee.intEmployeeID, Convert.ToString(Session["currentPage"]), strMethod, this);
        //        //Display message box
        //        MessageBox.ShowMessage("An Error has occurred and been logged. "
        //            + "If you continue to receive this message please contact "
        //            + "your system administrator.", this);
        //    }
        //}
        protected void btnCustomerSelect_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCustomerSelect_Click";
            try
            {
                if (btnCustomerSelect.Text == "Cancel")
                {
                    btnCustomerSelect.Text = "Change Customer";
                    grdCustomersSearched.Visible = false;
                }
                else
                {
                    grdCustomersSearched.Visible = true;
                    grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, CU.terminal.intBusinessNumber);
                    grdCustomersSearched.DataBind();
                    if (grdCustomersSearched.Rows.Count > 0)
                    {
                        btnCustomerSelect.Text = "Cancel";
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
        protected void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            string strMethod = "btnAddNewCustomer_Click";
            try
            {
                Customer customer = new Customer();
                customer.varFirstName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtFirstName")).Text;
                customer.varLastName = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtLastName")).Text;
                customer.varAddress = "";
                customer.varHomePhone = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtHomePhone")).Text;
                customer.varMobilePhone = "";
                customer.bitAllowMarketing = ((CheckBox)grdCustomersSearched.FooterRow.FindControl("chkAllowMarketing")).Checked;
                customer.varEmailAddress = ((TextBox)grdCustomersSearched.FooterRow.FindControl("txtEmailAddress")).Text;
                customer.varCityName = "";
                customer.intProvinceID = CU.currentStoreLocation.intProvinceID;
                customer.intCountryID = CU.currentStoreLocation.intCountryID;
                customer.varPostalCode = "";
                customer.intCustomerID = CM.AddNewCustomer(customer, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                receipt.customer = customer;

                RM.UpdateReceiptCurrent(receipt, CU.terminal.intBusinessNumber);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("receipt", receipt.intReceiptID.ToString());
                nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, true);
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
        protected void grdCustomersSearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string strMethod = "grdCustomersSearched_PageIndexChanging";
            try
            {
                grdCustomersSearched.PageIndex = e.NewPageIndex;
                grdCustomersSearched.Visible = true;
                grdCustomersSearched.DataSource = CM.ReturnCustomerBasedOnText(txtCustomer.Text, CU.terminal.intBusinessNumber);
                grdCustomersSearched.DataBind();
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
        protected void grdCustomersSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdCustomersSearched_RowCommand";
            try
            {
                //grabs the command argument for the command pressed 
                if (e.CommandName == "SwitchCustomer")
                {
                    receipt.customer = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), CU.terminal.intBusinessNumber)[0];
                    RM.UpdateReceiptCurrent(receipt, CU.terminal.intBusinessNumber);
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                    nameValues.Set("receipt", receipt.intReceiptID.ToString());
                    Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, true);
                }
                btnCustomerSelect.Text = "Change Customer";
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
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnInventorySearch_Click";
            try
            {
                if (txtInventorySearch.Text != "")
                {
                    if (RM.InventorySearchReturnsTradeIn(txtInventorySearch.Text, CU))
                    {
                        grdInventorySearched.DataSource = RM.ReturnTradeInSkuForLocation(CU);
                        grdInventorySearched.Columns[5].HeaderText = "Trade In Cost";
                        //Binds list to the grid view
                        grdInventorySearched.DataBind();

                        foreach (GridViewRow item in grdInventorySearched.Rows)
                        {
                            ((TextBox)item.Cells[2].FindControl("txtSaleQuantity")).Visible = false;
                            ((TextBox)item.Cells[3].FindControl("txtTradeInDescription")).Visible = true;
                            ((CheckBox)item.Cells[5].FindControl("chkIsPercentDiscount")).Visible = false;
                        }
                    }
                    else
                    {
                        grdInventorySearched.DataSource = RM.ReturnReceiptItemFromSearchString(txtInventorySearch.Text, CU.terminal.intBusinessNumber);
                        grdInventorySearched.Columns[5].HeaderText = "Discount";
                        //Binds list to the grid view
                        grdInventorySearched.DataBind();

                        foreach (GridViewRow item in grdInventorySearched.Rows)
                        {
                            ((TextBox)item.Cells[2].FindControl("txtSaleQuantity")).Visible = true;
                            ((TextBox)item.Cells[3].FindControl("txtTradeInDescription")).Visible = false;
                            ((CheckBox)item.Cells[5].FindControl("chkIsPercentDiscount")).Visible = true;
                        }
                    }
                    lblInvalidQuantity.Visible = false;
                    //Clears search text box
                    txtInventorySearch.Text = "";
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
        //This code is still a little bulky
        //Doesn't currently add the same item together, would have seperate rows for the same sku if they were added seperatly
        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdInventorySearched_RowCommand";
            try
            {
                lblInvalidQuantity.Visible = false;
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int quantity = 1;
                string qty = ((TextBox)grdInventorySearched.Rows[index].Cells[2].FindControl("txtSaleQuantity")).Text;
                if (qty != "")
                {
                    if (int.TryParse(qty, out quantity))
                    {
                        quantity = Convert.ToInt32(qty);
                    }
                }
                int currentQty = Convert.ToInt32(((Label)grdInventorySearched.Rows[index].Cells[2].FindControl("lblAvailableQuantity")).Text);
                bool nonStocked = ((CheckBox)grdInventorySearched.Rows[index].Cells[7].FindControl("chkNonStockedProduct")).Checked;
                if ((quantity > currentQty || quantity < 1) && !nonStocked)
                {
                    lblInvalidQuantity.Visible = true;
                }
                else
                {
                    ReceiptItem selectedItem = new ReceiptItem();
                    selectedItem.intInventoryID = Convert.ToInt32(e.CommandArgument.ToString());
                    selectedItem.intReceiptID = receipt.intReceiptID;
                    selectedItem.bitIsNonStockedProduct = nonStocked;

                    if (!RM.ItemAlreadyInCart(selectedItem, CU.terminal.intBusinessNumber))
                    {
                        double discount = 0;
                        string discountAmount = ((TextBox)grdInventorySearched.Rows[index].Cells[5].FindControl("txtItemDiscount")).Text;
                        if (discountAmount != "")
                        {
                            if (double.TryParse(discountAmount, out discount))
                            {
                                discount = Convert.ToDouble(discountAmount);
                            }
                        }
                        selectedItem.fltItemDiscount = discount;
                        selectedItem.intItemQuantity = quantity;
                        if (selectedItem.intInventoryID == RM.ReturnTradeInSkuForLocation(CU)[0].intInventoryID)
                        {
                            selectedItem.fltItemAverageCostAtSale = 0;
                            selectedItem.bitIsPercentageDiscount = false;
                            selectedItem.bitIsTradeIn = true;
                            selectedItem.bitIsRegularProduct = false;
                            selectedItem.fltItemDiscount = 0;
                            selectedItem.bitIsNonStockedProduct = false;
                            selectedItem.fltItemRefund = 0;

                            selectedItem.varItemDescription = ((TextBox)grdInventorySearched.Rows[index].Cells[3].FindControl("txtTradeInDescription")).Text;
                            selectedItem.fltItemPrice = - discount;
                            InventoryManager IM = new InventoryManager();
                            Inventory newTrade = IM.ReturnInventoryItem(RM.AddTradeInToInventoryTable(selectedItem, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU), CU.terminal.intBusinessNumber)[0];

                            selectedItem.intInventoryID = newTrade.intInventoryID;
                            selectedItem.varSku = newTrade.varSku;
                            RM.AddingItemToTheSale(selectedItem, receipt.intTransactionTypeID, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                        }
                        else
                        {
                            selectedItem.varItemDescription = ((Label)grdInventorySearched.Rows[index].Cells[3].FindControl("Description")).Text;
                            selectedItem.fltItemRefund = 0;
                            selectedItem.fltItemPrice = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollPrice")).Text, NumberStyles.Currency);
                            selectedItem.fltItemAverageCostAtSale = double.Parse(((Label)grdInventorySearched.Rows[index].Cells[4].FindControl("rollCost")).Text, NumberStyles.Currency);
                            selectedItem.bitIsPercentageDiscount = ((CheckBox)grdInventorySearched.Rows[index].Cells[5].FindControl("chkIsPercentDiscount")).Checked;
                            selectedItem.bitIsTradeIn = ((CheckBox)grdInventorySearched.Rows[index].Cells[6].FindControl("chkTradeInSearch")).Checked;
                            selectedItem.bitIsRegularProduct = ((CheckBox)grdInventorySearched.Rows[index].Cells[8].FindControl("chkRegularProduct")).Checked;

                            //add item to table and remove the added qty from current inventory
                            RM.AddingItemToTheSale(selectedItem, receipt.intTransactionTypeID, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                            if (!nonStocked)
                            {
                                RM.RemoveQuantityFromInventory(selectedItem.intInventoryID, (currentQty - quantity), CU.terminal.intBusinessNumber);
                            }
                        }
                        grdInventorySearched.DataSource = null;
                        grdInventorySearched.DataBind();
                        //Recalculate the new subtotal
                        UpdateReceiptTotal();
                    }
                    else
                    {
                        MessageBox.ShowMessage("Item is already in the cart. Please update item in "
                         + "cart or process a second sale.", this);
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

        ////Currently used for Removing the row
        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "OnRowDeleting";
            try
            {
                lblInvalidQuantity.Visible = false;
                int intInventoryID = Convert.ToInt32(((Label)grdSaleCartItem.Rows[e.RowIndex].Cells[0].FindControl("lblInventoryID")).Text);

                RM.ReturnReceiptItemQuantityToInventory(intInventoryID, receipt.intReceiptID, CU.terminal.intBusinessNumber);

                //Remove the indexed pointer
                grdSaleCartItem.EditIndex = -1;

                UpdateReceiptTotal();
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
        //Currently used for Editing the row
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "OnRowEditing";
            try
            {
                lblInvalidQuantity.Visible = false;
                grdSaleCartItem.DataSource = receipt.lstReceiptItem;
                grdSaleCartItem.EditIndex = e.NewEditIndex;
                grdSaleCartItem.DataBind();
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
        //Currently used for cancelling the edit
        protected void ORowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "ORowCanceling";
            try
            {
                lblInvalidQuantity.Visible = false;
                //Clears the indexed row
                grdSaleCartItem.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdSaleCartItem.DataSource = receipt.lstReceiptItem;
                grdSaleCartItem.DataBind();
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
        //Currently used for updating the row
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "OnRowUpdating";
            try
            {
                lblInvalidQuantity.Visible = false;
                //Stores all the data for each element in the row
                ReceiptItem newItemInfo = new ReceiptItem();
                newItemInfo.intReceiptID = receipt.intReceiptID;

                newItemInfo.intInventoryID = Convert.ToInt32(((Label)grdSaleCartItem.Rows[e.RowIndex].Cells[0].FindControl("lblInventoryID")).Text);
                newItemInfo.fltItemDiscount = Convert.ToDouble(((TextBox)grdSaleCartItem.Rows[e.RowIndex].Cells[6].FindControl("txtDiscountEdit")).Text);
                newItemInfo.intItemQuantity = Convert.ToInt32(((TextBox)grdSaleCartItem.Rows[e.RowIndex].Cells[3].Controls[0]).Text);
                newItemInfo.bitIsPercentageDiscount = ((CheckBox)grdSaleCartItem.Rows[e.RowIndex].Cells[6].FindControl("ckbPercentageEdit")).Checked;
                newItemInfo.bitIsNonStockedProduct = ((CheckBox)grdSaleCartItem.Rows[e.RowIndex].Cells[8].FindControl("chkNonStockedProduct")).Checked;

                if (!newItemInfo.bitIsNonStockedProduct)
                {
                    if (!RM.ValidInventoryQuantity(newItemInfo, CU.terminal.intBusinessNumber))
                    {
                        //if it is less than 0 then there is not enough in invenmtory to sell
                        lblInvalidQuantity.Visible = true;
                    }
                    else
                    {
                        RM.UpdateItemFromCurrentSalesTable(newItemInfo, CU.terminal.intBusinessNumber);
                    }
                }
                else
                {
                    RM.UpdateItemFromCurrentSalesTable(newItemInfo, CU.terminal.intBusinessNumber);
                }

                //Clears the indexed row
                grdSaleCartItem.EditIndex = -1;
                //Recalculates the new subtotal and Binds cart items to grid view
                UpdateReceiptTotal();
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
        protected void btnExitSale_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnExitSale_Click";
            try
            {
                Response.Redirect("SalesHomePage.aspx", true);
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
        protected void btnProceedToCheckout_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProceedToCheckout_Click";
            try
            {
                if (RCM.CheckForItemsInTransaction(receipt))
                {
                    UpdateReceiptTotal();
                    lblInvalidQuantity.Visible = false;
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", receipt.customer.intCustomerID.ToString());
                    nameValues.Set("receipt", receipt.intReceiptID.ToString());
                    Response.Redirect("SalesCheckout.aspx?" + nameValues, true);
                }
                else
                {
                    MessageBox.ShowMessage("There are no items on this transaction.", this);
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
        protected void UpdateReceiptTotal()
        {
            string strMethod = "UpdateReceiptTotal";
            try
            {
                //Calculate new subtotal
                receipt = RM.CalculateNewReceiptTotalsToUpdate(receipt.intReceiptID, CU.terminal.intBusinessNumber)[0];
                grdSaleCartItem.DataSource = receipt.lstReceiptItem;
                grdSaleCartItem.DataBind();
                lblSubtotalDisplay.Text = (receipt.fltCartTotal - receipt.fltDiscountTotal + receipt.fltTradeInTotal).ToString("C");
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
        protected void btnRefreshCart_Click(object sender, EventArgs e)
        {
            string strMethod = "btnRefreshCart_Click";
            try
            {
                btnRefreshCart.Visible = false;
                UpdateReceiptTotal();
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
        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            string strMethod = "btnClearSearch_Click";
            try
            {
                grdInventorySearched.DataSource = null;
                grdInventorySearched.DataBind();
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