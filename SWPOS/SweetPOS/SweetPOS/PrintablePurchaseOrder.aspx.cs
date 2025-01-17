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
    public partial class PrintablePurchaseOrder : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        VendorSupplierManager VSM = new VendorSupplierManager();
        PurchaseOrderManager POM = new PurchaseOrderManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PrintablePurchaseOrder.aspx";
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
                    btnHome.Focus();
                    if (!IsPostBack)
                    {
                        PurchaseOrder purchaseOrder = POM.ReturnPurchaseOrder(Convert.ToInt32(Request.QueryString["purchO"]), CU)[0];
                        lblPurchaseOrderNumber.Text = purchaseOrder.varPurchaseOrderNumber.ToString();
                        lblPurchaseOrderCompletionDate.Text = purchaseOrder.dtmPurchaseOrderCompletionDate.ToShortDateString();
                        lblPurchaseOrderCompletionTime.Text = purchaseOrder.dtmPurchaseOrderCompletionTime.ToString("h:mm tt");

                        //display information on receipt
                        lblVendorName.Text = purchaseOrder.vendorSupplier.varVendorSupplierName.ToString();
                        lblVendorAddress.Text = purchaseOrder.vendorSupplier.varAddress.ToString();
                        lblVendorPostalCode.Text = purchaseOrder.vendorSupplier.varCityName.ToString() + ", " + LM.ReturnProvinceName(purchaseOrder.vendorSupplier.intProvinceID) + " " + purchaseOrder.vendorSupplier.varPostalCode.ToString();
                        lblVendorPhoneNumber.Text = purchaseOrder.vendorSupplier.varMainPhoneNumber.ToString();
                        //Display the location information
                        lblStoreName.Text = purchaseOrder.storeLocation.varStoreName.ToString();
                        lblAddress.Text = purchaseOrder.storeLocation.varAddress.ToString();
                        lblPostalCode.Text = purchaseOrder.storeLocation.varCityName.ToString() + ", " + LM.ReturnProvinceName(purchaseOrder.storeLocation.intProvinceID) + " " + purchaseOrder.storeLocation.varPostalCode.ToString();
                        lblPhoneNumber.Text = purchaseOrder.storeLocation.varPhoneNumber.ToString();

                        //Display the totals
                        lblGSTDisplay.Text = purchaseOrder.fltGSTTotal.ToString("C");
                        lblPSTDisplay.Text = purchaseOrder.fltPSTTotal.ToString("C");
                        lblSubtotalDisplay.Text = (purchaseOrder.fltCostSubTotal - (purchaseOrder.fltGSTTotal + purchaseOrder.fltPSTTotal)).ToString("C");
                        lblTotalPaidDisplay.Text = purchaseOrder.fltCostSubTotal.ToString("C");
                        

                        //Binds the cart to the grid view
                        grdExpectedItemsList.DataSource = purchaseOrder.lstPurchaseOrderItem;
                        grdExpectedItemsList.DataBind();

                        //Displays the total amount ppaid
                        //Binds the payment methods to a gridview
                        grdReceivedItems.DataSource = purchaseOrder.lstPurchaseOrderItem;
                        grdReceivedItems.DataBind();
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
        protected void btnHome_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnHome_Click";
            try
            {
                //Change to the Home Page
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
    }
}