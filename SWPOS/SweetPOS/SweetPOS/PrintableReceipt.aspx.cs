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
    public partial class PrintableReceipt : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        ReceiptManager RM = new ReceiptManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "PrintableReceipt.aspx";
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
                        Receipt receipt = RM.ReturnReceipt(Convert.ToInt32(Request.QueryString["receipt"]), CU.terminal.intBusinessNumber)[0];
                        lblReceiptNumber.Text = receipt.varReceiptNumber.ToString() + "-" + receipt.intReceiptSubNumber.ToString();
                        lblReceiptCompletionDate.Text = receipt.dtmReceiptCompletionDate.ToShortDateString();
                        lblReceiptCompletionTime.Text = receipt.dtmReceiptCompletionTime.ToString("h:mm tt");

                        //display information on receipt
                        CustomerManager CM = new CustomerManager();
                        lblCustomerName.Text = receipt.customer.varFirstName.ToString() + " " + receipt.customer.varLastName.ToString();
                        if (receipt.customer.intCustomerID != CM.ReturnGuestCustomerForLocation(CU.currentStoreLocation.intStoreLocationID, CU.terminal.intBusinessNumber))
                        {
                            lblCustomerAddress.Text = receipt.customer.varAddress.ToString();
                            lblCustomerPostalCode.Text = receipt.customer.varCityName.ToString() + ", " + LM.ReturnProvinceName(receipt.customer.intProvinceID) + " " + receipt.customer.varPostalCode.ToString();
                            lblCustomerPhoneNumber.Text = "H: " + receipt.customer.varHomePhone.ToString() + ", M: " + receipt.customer.varMobilePhone.ToString();
                        }
                        //Display the location information
                        lblStoreName.Text = receipt.storeLocation.varStoreName.ToString();
                        lblAddress.Text = receipt.storeLocation.varAddress.ToString();
                        lblPostalCode.Text = receipt.storeLocation.varCityName.ToString() + ", " + LM.ReturnProvinceName(receipt.storeLocation.intProvinceID) + " " + receipt.storeLocation.varPostalCode.ToString();
                        lblPhoneNumber.Text = receipt.storeLocation.varPhoneNumber.ToString();
                        lblTaxNumber.Text = receipt.storeLocation.varTaxNumber.ToString();

                        //Display the totals
                        lblDiscountsDisplay.Text = receipt.fltDiscountTotal.ToString("C");
                        lblTradeInsDisplay.Text = receipt.fltTradeInTotal.ToString("C");
                        lblShippingDisplay.Text = receipt.fltShippingTotal.ToString("C");
                        lblGSTDisplay.Text = receipt.fltGovernmentTaxTotal.ToString("C");
                        lblPSTDisplay.Text = receipt.fltProvincialTaxTotal.ToString("C");
                        lblSubtotalDisplay.Text = ((receipt.fltCartTotal - receipt.fltDiscountTotal + receipt.fltTradeInTotal) + receipt.fltShippingTotal).ToString("C");
                        lblTotalPaidDisplay.Text = receipt.fltBalanceDueTotal.ToString("C");
                        
                        lblTenderDisplay.Text = receipt.fltTenderedAmount.ToString("C");
                        lblChangeDisplay.Text = receipt.fltChangeAmount.ToString("C");

                        if (receipt.intReceiptSubNumber > 1)
                        {
                            //    //Changes headers if the invoice is return
                            grdItemsSoldList.Columns[2].HeaderText = "Sold At";
                            grdItemsSoldList.Columns[3].HeaderText = "Non Refundable";
                            grdItemsSoldList.Columns[5].HeaderText = "Returned At";
                        }

                        //Binds the cart to the grid view
                        grdItemsSoldList.DataSource = receipt.lstReceiptItem;
                        grdItemsSoldList.DataBind();

                        //Displays the total amount ppaid
                        //Binds the payment methods to a gridview
                        grdReceiptPayment.DataSource = receipt.lstReceiptPayment;
                        grdReceiptPayment.DataBind();
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