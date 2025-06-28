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
    public partial class PurchasesCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        CustomerManager CM = new CustomerManager();
        InvoiceManager IM = new InvoiceManager();
        ReceiptCalculationManager RCM = new ReceiptCalculationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        InventoryManager InvM = new InventoryManager();
        CurrentUser CU;
        private static Invoice invoice;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PurchasesCart.aspx";
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
                    btnAddPurchase.Focus();
                    if (!Page.IsPostBack)
                    {
                        DateTime createDateTime = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                        lblDateDisplay.Text = createDateTime.ToString("yyyy-MM-dd");
                        if (Request.QueryString["invoice"].ToString() == "-10")
                        {
                            invoice = IM.CreateNewPurchaseInvoice(Convert.ToInt32(Request.QueryString["customer"].ToString()), createDateTime, CU)[0];
                        }
                        else
                        {
                            invoice = IM.ReturnInvoiceCurrent(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.terminal.intBusinessNumber)[0];
                        }
                        lblInvoiceNumberDisplay.Text = invoice.varInvoiceNumber;
                        UpdateInvoiceTotal();
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
        protected void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string strMethod = "btnAddCustomer_Click";
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
                invoice.customer = customer;

                UpdateInvoiceTotal();

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("invoice", Request.QueryString["invoice"].ToString());
                nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
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
                    //if command argument is SwitchCustomer, set the new key
                    invoice.customer = CM.ReturnCustomer(Convert.ToInt32(e.CommandArgument.ToString()), CU.terminal.intBusinessNumber)[0];
                    UpdateInvoiceTotal();
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                    nameValues.Set("invoice", invoice.varInvoiceNumber.ToString());
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
        protected void btnAddPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method error tracking
            string strMethod = "btnAddPurchase_Click";
            try
            {
                InvoiceItem selectedItem = new InvoiceItem();

                selectedItem.intInvoiceID = invoice.intInvoiceID;
                selectedItem.varItemDescription = "";
                selectedItem.fltItemCost = 0.00;
                selectedItem.intItemQuantity = 1;

                Inventory newTrade = InvM.ReturnInventoryItem(IM.AddPurchaseTradeInToInventoryTable(selectedItem, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU), CU.terminal.intBusinessNumber)[0];

                selectedItem.intInventoryID = newTrade.intInventoryID;
                selectedItem.varItemSku = newTrade.varSku;

                IM.InsertItemIntoInvoiceCart(selectedItem, CU.terminal.intBusinessNumber);
                //Bind items in cart to grid view
                UpdateInvoiceTotal();
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
                //it's available columns
                grdPurchasedItems.DataSource = invoice.lstInvoiceItem;
                grdPurchasedItems.EditIndex = e.NewEditIndex;
                grdPurchasedItems.DataBind();
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
        protected void OnRowCanceling(object sender, GridViewCancelEditEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "ORowCanceling";
            try
            {
                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds gridview to Session items in cart
                grdPurchasedItems.DataSource = invoice.lstInvoiceItem;
                grdPurchasedItems.DataBind();
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
                //creates a temp item with the new updates
                InvoiceItem selectedItem = new InvoiceItem();

                selectedItem.intInvoiceItemID = Convert.ToInt32(((Label)grdPurchasedItems.Rows[e.RowIndex].Cells[0].FindControl("lblInvoiceItemID")).Text);
                selectedItem.varItemDescription = ((TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
                TextBox txtcost = (TextBox)grdPurchasedItems.Rows[e.RowIndex].Cells[3].Controls[0];
                string cost = (txtcost).Text;
                selectedItem.fltItemCost = Convert.ToDouble(cost);
                IM.UpdateInvoiceItemCurrent(selectedItem, CU.terminal.intBusinessNumber);

                //Clears the indexed row
                grdPurchasedItems.EditIndex = -1;
                //Binds cart items to grid view
                UpdateInvoiceTotal();
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
        protected void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelSale_Click";
            try
            {
                invoice.bitIsInvoiceCancelled = true;
                IM.CancelPurchaseInvoice(invoice, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                //Change to Home Page
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
        protected void btnProceedToPayOut_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProceedToPayOut_Click";
            try
            {
                if (RCM.CheckForItemsInTransaction(invoice))
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("invoice", invoice.intInvoiceID.ToString());
                    nameValues.Set("customer", invoice.customer.intCustomerID.ToString());
                    //Changes to Sales Checkout page
                    Response.Redirect("PurchasesCheckout.aspx?" + nameValues, true);
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
        protected void UpdateInvoiceTotal()
        {
            //Collects current method for error tracking
            string strMethod = "UpdateInvoiceTotal";
            try
            {
                invoice = IM.CalculateInvoiceTotal(invoice, CU.terminal.intBusinessNumber)[0];
                grdPurchasedItems.DataSource = invoice.lstInvoiceItem;
                grdPurchasedItems.DataBind();

                txtCustomer.Text = invoice.customer.varLastName + ", " + invoice.customer.varFirstName;
                lblPurchaseAmountDisplay.Text = invoice.fltCostTotal.ToString("C");
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