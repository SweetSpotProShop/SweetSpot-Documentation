using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class PurchaseOrderCart : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        PurchaseOrderManager POM = new PurchaseOrderManager();
        ReceiptCalculationManager RCM = new ReceiptCalculationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        TaxManager TM = new TaxManager();
        CurrentUser CU;
        private static PurchaseOrder purchaseOrder = new PurchaseOrder();

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PurchaseOrderCart.aspx";
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
                    txtVendorDisplay.Focus();
                    if (!Page.IsPostBack)
                    {

                        if (Request.QueryString["purchO"].ToString() == "-10")
                        {
                            if(Request.QueryString["suppliedPO"].ToString() == "")
                            {
                                purchaseOrder = POM.CreateNewPurchaseOrder(Convert.ToInt32(Request.QueryString["vendor"].ToString()), Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU)[0];
                            }
                            else
                            {
                                purchaseOrder = POM.CreateNewPurchaseOrderPOSupplied(Convert.ToInt32(Request.QueryString["vendor"].ToString()),
                                    Request.QueryString["suppliedPO"].ToString(), Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU)[0];
                            }
                        }
                        else
                        {
                            int purchOrderID = 0;
                            if (int.TryParse(Request.QueryString["purchO"].ToString(), out purchOrderID))
                            {
                                purchaseOrder = POM.ReturnPurchaseOrderCurrent(purchOrderID, CU)[0];
                            }
                        }
                        txtVendorDisplay.Text = purchaseOrder.vendorSupplier.varVendorSupplierName.ToString();
                        txtVendorDisplay.Enabled = false;
                        lblPurchaseOrderNumberDisplay.Text = purchaseOrder.varPurchaseOrderNumber.ToString();
                        lblDateDisplay.Text = purchaseOrder.dtmPurchaseOrderCreationDate.ToString("yyyy-MM-dd");
                        UpdatePurchaseOrderTotal();
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
        protected void grdVendorSupplierItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdVendorSupplierItems_RowCommand";
            try
            {
                //lblInvalidQuantity.Visible = false;
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                
                //Stores the info about the item in that index
                PurchaseOrderItem selectedSku = POM.ReturnPurchaseOrderItemForPurchaseOrder(Convert.ToInt32(e.CommandArgument.ToString()), CU.terminal.intBusinessNumber)[0];
                selectedSku.intPurchaseOrderID = purchaseOrder.intPurchaseOrderID;
                int poQuantity = 1;
                selectedSku.intReceivedQuantity = 0;
                string poQuantityAtPurchase = ((TextBox)grdVendorSupplierItems.Rows[index].Cells[3].FindControl("txtQuantity")).Text;
                if (poQuantityAtPurchase != "")
                {
                    if (int.TryParse(poQuantityAtPurchase, out poQuantity))
                    {
                        poQuantity = Convert.ToInt32(poQuantityAtPurchase);
                    }
                }
                selectedSku.intPurchaseOrderQuantity = poQuantity;
                POM.AddItemIntoPurchaseOrderCart(selectedSku, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                UpdatePurchaseOrderTotal();
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
        protected void grdPurchaseOrderItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdPurchaseOrderItems_RowCommand";
            try
            {
                POM.RemoveSelectedItemFromPurchaseOrderCart(Convert.ToInt32(e.CommandArgument.ToString()), CU);
                UpdatePurchaseOrderTotal();
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
        private void UpdatePurchaseOrderTotal()
        {
            //Collects current method for error tracking
            string strMethod = "UpdatePurchaseOrderTotal";
            try
            {
                //recalculate the return total
                purchaseOrder = POM.ReturnPurchaseOrderTotals(purchaseOrder.intPurchaseOrderID, CU)[0];

                //store items available for return in session
                grdVendorSupplierItems.DataSource = POM.ListOfAvailableVendorSupplierItems(purchaseOrder, CU);
                grdVendorSupplierItems.DataBind();

                grdPurchaseOrderItems.DataSource = purchaseOrder.lstPurchaseOrderItem;
                grdPurchaseOrderItems.DataBind();

                int charged = 0;
                lblGSTTotalDisplay.Text = charged.ToString("C");
                if (purchaseOrder.bitGSTCharged)
                {
                    charged = 1;
                    lblGSTTotalDisplay.Text = purchaseOrder.fltGSTTotal.ToString("C");
                }
                rdbGSTIncorp.SelectedValue = charged.ToString();
                charged = 0;
                lblPSTTotalDisplay.Text = charged.ToString("C");
                if (purchaseOrder.bitPSTCharged)
                {
                    charged = 1;
                    lblPSTTotalDisplay.Text = purchaseOrder.fltPSTTotal.ToString("C");
                }
                rdbPSTIncorp.SelectedValue = charged.ToString();

                lblPurchaseOrderSubtotalDisplay.Text = purchaseOrder.fltCostSubTotal.ToString("C");
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
        protected void btnCancelPurchaseOrder_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancelPurchaseOrder_Click";
            try
            {
                POM.RemovePurchaseOrderFromCurrentTable(purchaseOrder, CU.terminal.intBusinessNumber);
                Response.Redirect("PurchaseOrderHomePage.aspx", true);
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
        protected void btnSavePurchaseForProcessing_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnSavePurchaseForProcessing_Click";
            try
            {
                if (RCM.CheckForItemsInTransaction(purchaseOrder))
                {
                    //Changes page to the Purchases Home page
                    Response.Redirect("PurchaseOrderHomePage.aspx", true);
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
        protected void rdbPSTIncorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "rdbPSTIncorp_SelectedIndexChanged";
            try
            {
                if (Convert.ToInt32(rdbPSTIncorp.SelectedValue) == 0)
                {
                    purchaseOrder.bitPSTCharged = false;
                }
                else
                {
                    purchaseOrder.bitPSTCharged = true;
                }
                POM.SavePurchaseOrderInformationCurrentNonReturn(purchaseOrder, CU);
                TM.UpdateListOfPSTPurchaseOrderTaxes(purchaseOrder, CU);
                UpdatePurchaseOrderTotal();
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
        protected void rdbGSTIncorp_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "rdbGSTIncorp_SelectedIndexChanged";
            try
            {
                if (Convert.ToInt32(rdbGSTIncorp.SelectedValue) == 0)
                {
                    purchaseOrder.bitGSTCharged = false;
                }
                else
                {
                    purchaseOrder.bitGSTCharged = true;
                }
                POM.SavePurchaseOrderInformationCurrentNonReturn(purchaseOrder, CU);
                TM.UpdateListOfGSTPurchaseOrderTaxes(purchaseOrder, CU);
                UpdatePurchaseOrderTotal();
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