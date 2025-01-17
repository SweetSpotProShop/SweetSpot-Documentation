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
    public partial class PrintablePurchase : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        CustomerManager CM = new CustomerManager();
        InvoiceManager IM = new InvoiceManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PrintablePurchase.aspx";
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
                        //Store in Customer class
                        Invoice invoice = IM.ReturnInvoice(Convert.ToInt32(Request.QueryString["invoice"].ToString()), CU.terminal.intBusinessNumber)[0];
                        lblInvoiceNumber.Text = invoice.varInvoiceNumber.ToString();
                        lblInvoiceCompletionDate.Text = invoice.dtmInvoiceCompletionDate.ToShortDateString();
                        lblInvoiceCompletionTime.Text = invoice.dtmInvoiceCompletionTime.ToString("h:mm tt");

                        //display information on receipt
                        lblCustomerName.Text = invoice.customer.varLastName.ToString() + ", " + invoice.customer.varFirstName.ToString();
                        if (invoice.customer.intCustomerID != CM.ReturnGuestCustomerForLocation(CU.currentStoreLocation.intStoreLocationID, CU.terminal.intBusinessNumber))
                        {
                            lblCustomerAddress.Text = invoice.customer.varAddress.ToString();
                            lblCustomerPostalCode.Text = invoice.customer.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.customer.intProvinceID) + " " + invoice.customer.varPostalCode.ToString();
                            lblCustomerPhoneNumber.Text = "H: " + invoice.customer.varHomePhone.ToString() + ", M: " + invoice.customer.varMobilePhone.ToString();
                        }
                        
                        //Display the location information
                        lblStoreName.Text = invoice.storeLocation.varStoreName.ToString();
                        lblAddress.Text = invoice.storeLocation.varAddress.ToString();
                        lblPostalCode.Text = invoice.storeLocation.varCityName.ToString() + ", " + LM.ReturnProvinceName(invoice.storeLocation.intProvinceID) + " " + invoice.storeLocation.varPostalCode.ToString();
                        lblPhoneNumber.Text = invoice.storeLocation.varPhoneNumber.ToString();

                        //Binds the cart to the grid view
                        grdItemsPurchasedList.DataSource = invoice.lstInvoiceItem;
                        grdItemsPurchasedList.DataBind();

                        //Displays the total amount ppaid
                        lblSubtotalDisplay.Text = invoice.fltCostTotal.ToString("C");
                        lblTotalPaidDisplay.Text = invoice.fltCostTotal.ToString("C");
                        //Binds the payment methods to a gridview
                        grdMOPS.DataSource = invoice.lstInvoicePayment;
                        grdMOPS.DataBind();
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