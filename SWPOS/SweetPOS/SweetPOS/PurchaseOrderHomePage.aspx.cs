using OfficeOpenXml;
using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class PurchaseOrderHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        PurchaseOrderManager POM = new PurchaseOrderManager();
        VendorSupplierManager VSM = new VendorSupplierManager();
        CurrentUserManager CUM = new CurrentUserManager();
        InvoiceManager IM = new InvoiceManager();
        CustomerManager CM = new CustomerManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PurchaseOrderHomePage";
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
                    txtNewPurchaseOrderNumber.Focus();
                    if (!IsPostBack)
                    {
                        ddlVendor.DataSource = VSM.ReturnVendorSupplierList(CU.terminal.intBusinessNumber);
                        ddlVendor.DataBind();
                        populateGridview();
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
        protected void btnProcessTradeIn_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessTradeIn_Click";
            try
            {
                //Changes page to the inventory add new page
                Response.Redirect("TradeInManagement.aspx", true);
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
        protected void btnBulkPurchase_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnBulkPurchase_Click";
            try
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", CM.ReturnGuestCustomerForLocation(CU.currentStoreLocation.intStoreLocationID, CU.terminal.intBusinessNumber).ToString());
                nameValues.Set("invoice", "-10");
                //Changes page to the inventory add new page
                Response.Redirect("PurchasesCart.aspx?" + nameValues, true);
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
        protected void btnCreatePO_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCreatePO_Click";
            try
            {
                string suppliedPO = txtNewPurchaseOrderNumber.Text;
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                //Change the vendorID to the drop down box
                nameValues.Set("vendor", ddlVendor.SelectedValue.ToString());
                nameValues.Set("purchO", "-10");
                nameValues.Set("suppliedPO", suppliedPO);
                Response.Redirect("PurchaseOrderCart.aspx?" + nameValues, true);
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
        protected void grdOpenPurchaseOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdOpenPurchaseOrders_RowCommand";
            try
            {
                //Change to Inventory Add new page to display selected item
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                //Change the vendorID to the drop down box
                nameValues.Set("purchO", e.CommandArgument.ToString());

                if (e.CommandName == "update")
                {
                    Response.Redirect("PurchaseOrderCart.aspx?" + nameValues, true);
                }
                else if(e.CommandName == "receive")
                {
                    Response.Redirect("PurchaseOrderReceivingCart.aspx?" + nameValues, true);
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
        protected void grdOpenBulkPurchases_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdOpenBulkPurchases_RowCommand";
            try
            {
                if (e.CommandName == "update")
                {
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("invoice", e.CommandArgument.ToString());
                    //Changes page to the inventory add new page
                    Response.Redirect("PurchasesCart.aspx?" + nameValues, true);
                }
                else if (e.CommandName == "delete")
                {
                    Invoice invoice = IM.ReturnInvoiceCurrent(Convert.ToInt32(e.CommandArgument), CU.terminal.intBusinessNumber)[0];
                    invoice.bitIsInvoiceVoided = true;
                    IM.CancelPurchaseInvoice(invoice, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                    Response.Redirect(Request.Url.AbsolutePath, true);
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
        protected void populateGridview()
        {
            string strMethod = "populateGridview";
            try
            {
                //Binds returned items to gridview for display
                grdOpenBulkPurchases.DataSource = IM.ReturnOpenBulkPurchases(CU);
                grdOpenBulkPurchases.DataBind();
                grdOpenPurchaseOrders.DataSource = POM.ReturnOpenPurchaseOrders(CU);
                grdOpenPurchaseOrders.DataBind();
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