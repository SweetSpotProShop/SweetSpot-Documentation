using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class CustomerAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        CustomerManager CM = new CustomerManager();
        EmployeeManager EM = new EmployeeManager();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        ReceiptManager RM = new ReceiptManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "CustomerAddNew.aspx";
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
                    btnEditCustomer.Focus();
                    //Checks for a Customer Key
                    if (Convert.ToInt32(Request.QueryString["customer"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create customer class and fill page with all info based in the customer number 
                            Customer customer = CM.ReturnCustomerWithReceiptList(Convert.ToInt32(Request.QueryString["customer"].ToString()), CU.terminal.intBusinessNumber)[0];

                            txtFirstName.Text = customer.varFirstName.ToString();
                            txtLastName.Text = customer.varLastName.ToString();
                            txtAddress.Text = customer.varAddress.ToString();
                            txtHomePhone.Text = customer.varHomePhone.ToString();
                            txtMobilePhone.Text = customer.varMobilePhone.ToString();
                            txtEmailAddress.Text = customer.varEmailAddress.ToString();
                            txtCity.Text = customer.varCityName.ToString();

                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.DataBind();
                            ddlCountry.SelectedValue = customer.intCountryID.ToString();
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(customer.intCountryID);
                            ddlProvince.SelectedValue = customer.intProvinceID.ToString();
                            ddlProvince.DataBind();

                            txtPostalCode.Text = customer.varPostalCode.ToString();
                            chkAllowMarketing.Checked = customer.bitAllowMarketing;

                            //Binds invoice list to the grid view
                            grdReceiptSelection.DataSource = customer.lstReceipt;
                            grdReceiptSelection.DataBind();
                        }
                    }
                    else
                    {
                        //no cust number
                        if (!IsPostBack)
                        {
                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.SelectedValue = CU.employee.intCountryID.ToString();
                            ddlCountry.DataBind();
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                            ddlProvince.SelectedValue = CU.employee.intProvinceID.ToString();
                            ddlProvince.DataBind();

                        }
                        //Displays text boxes instead of label for customer creation info
                        txtFirstName.Enabled = true;
                        txtLastName.Enabled = true;
                        txtAddress.Enabled = true;
                        txtHomePhone.Enabled = true;
                        txtMobilePhone.Enabled = true;
                        txtEmailAddress.Enabled = true;
                        txtCity.Enabled = true;
                        ddlProvince.Enabled = true;
                        ddlCountry.Enabled = true;
                        chkAllowMarketing.Enabled = true;
                        txtPostalCode.Enabled = true;
                        //hides and displays the proper buttons for access
                        btnSaveCustomer.Visible = false;
                        btnAddCustomer.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddCustomer";
                        btnEditCustomer.Visible = false;
                        btnStartSale.Visible = false;
                        btnCancel.Visible = false;
                        btnBackToSearch.Visible = true;
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
            //Collects current method for error tracking
            string strMethod = "btnAddCustomer_Click";
            try
            {
                //Collects new customer data to add to database
                Customer c = new Customer();
                c.varFirstName = txtFirstName.Text;
                c.varLastName = txtLastName.Text;
                c.varAddress = txtAddress.Text;
                c.varHomePhone = txtHomePhone.Text;
                c.varMobilePhone = txtMobilePhone.Text;
                c.bitAllowMarketing = chkAllowMarketing.Checked;
                c.varEmailAddress = txtEmailAddress.Text;
                c.varCityName = txtCity.Text;
                c.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                c.intCountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                c.varPostalCode = txtPostalCode.Text;

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("customer", CM.AddNewCustomer(c, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber).ToString());
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
        protected void btnEditCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnEditCustomer_Click";
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtAddress.Enabled = true;
                txtHomePhone.Enabled = true;
                txtMobilePhone.Enabled = true;
                txtEmailAddress.Enabled = true;
                chkAllowMarketing.Enabled = true;
                txtCity.Enabled = true;
                ddlCountry.Enabled = true;
                ddlProvince.Enabled = true;
                txtPostalCode.Enabled = true;
                //hides and displays the proper buttons for access
                btnSaveCustomer.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveCustomer";
                btnEditCustomer.Visible = false;
                btnAddCustomer.Visible = false;
                btnStartSale.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
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
        protected void btnSaveCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnSaveCustomer_Click";
            try
            {
                //Collects customer data to add to database
                Customer c = new Customer();
                c.intCustomerID = Convert.ToInt32(Request.QueryString["customer"].ToString());
                c.varFirstName = txtFirstName.Text;
                c.varLastName = txtLastName.Text;
                c.varAddress = txtAddress.Text;
                c.varHomePhone = txtHomePhone.Text;
                c.varMobilePhone = txtMobilePhone.Text;
                c.bitAllowMarketing = chkAllowMarketing.Checked;
                c.varEmailAddress = txtEmailAddress.Text;
                c.varCityName = txtCity.Text;
                c.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                c.intCountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                c.varPostalCode = txtPostalCode.Text;
                //updates the customer info in tables
                CM.UpdateCurrentCustomer(c, CU.terminal.intBusinessNumber);
                //changes all text boxes and dropdowns to labels
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                txtAddress.Enabled = false;
                txtHomePhone.Enabled = false;
                txtMobilePhone.Enabled = false;
                txtEmailAddress.Enabled = false;
                chkAllowMarketing.Enabled = false;
                txtCity.Enabled = false;
                ddlProvince.Enabled = false;
                ddlCountry.Enabled = false;
                txtPostalCode.Enabled = false;
                //hides and displays the proper buttons for access
                btnSaveCustomer.Visible = false;
                btnEditCustomer.Visible = true;
                btnCancel.Visible = false;
                btnAddCustomer.Visible = false;
                btnBackToSearch.Visible = true;
                btnSaveCustomer.Visible = false;
                btnEditCustomer.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditCustomer";
                btnCancel.Visible = false;
                btnStartSale.Visible = true;
                btnAddCustomer.Visible = false;
                btnBackToSearch.Visible = true;
                //reloads current page
                Response.Redirect(Request.RawUrl, true);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancel_Click";
            try
            {
                //no chnages saved and moves to customer home page
                Response.Redirect(Request.RawUrl, true);
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
        protected void btnStartSale_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnStartSale_Click";
            try
            {
                DateTime dtmToday = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                if (!SRM.TillAlreadyCashedOut(dtmToday, CU))
                {
                    if (SRM.TillReconciliationProcessCheck(dtmToday, CU) == 0)
                    {
                        TillCashout tillCashout = new TillCashout();
                        tillCashout.intTerminalID = CU.terminal.intTerminalID;
                        tillCashout.dtmTillCashoutDate = dtmToday;
                        tillCashout.dtmTillCashoutProcessedDate = dtmToday;
                        tillCashout.dtmTillCashoutProcessedTime = dtmToday;
                        tillCashout.intHundredDollarBillCount = 0;
                        tillCashout.intFiftyDollarBillCount = 0;
                        tillCashout.intTwentyDollarBillCount = 0;
                        tillCashout.intTenDollarBillCount = 0;
                        tillCashout.intFiveDollarBillCount = 0;
                        tillCashout.intToonieCoinCount = 0;
                        tillCashout.intLoonieCoinCount = 0;
                        tillCashout.intQuarterCoinCount = 0;
                        tillCashout.intDimeCoinCount = 0;
                        tillCashout.intNickelCoinCount = 0;
                        tillCashout.fltCashDrawerTotal = 0;
                        tillCashout.fltCountedTotal = 0;
                        tillCashout.fltCashDrawerFloat = CU.terminal.fltDrawerFloatAmount;
                        tillCashout.fltCashDrawerCashDrop = 0;
                        SRM.ProcessTerminalReconciliation(tillCashout, dtmToday, CU.terminal.intBusinessNumber);
                    }
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("customer", Request.QueryString["customer"].ToString());
                    nameValues.Set("receipt", "-10");
                    Response.Redirect("SalesCart.aspx?" + nameValues, true);
                }
                else
                {
                    MessageBox.ShowMessage("This till has already been cashed out. Unprocess to continue.", this);
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnBackToSearch_Click";
            try
            {
                //opens the Customer home page
                Response.Redirect("CustomerHomePage.aspx", true);
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
        protected void grdReceiptSelection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdReceiptSelection_RowCommand";
            try
            {
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int tranType = RM.ReturnTransactionTypeFromReceiptNumber(Convert.ToInt32(e.CommandArgument), CU.terminal.intBusinessNumber);

                //Changes page to display a printable invoice
                if (tranType == 1)
                {
                    Response.Redirect("PrintableReceipt.aspx?receipt=" + e.CommandArgument.ToString(), true);
                }
                else if (tranType == 2)
                {
                    Response.Redirect("PrintableReceiptReturn.aspx?receipt=" + e.CommandArgument.ToString(), true);
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
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "ddlCountry_SelectedIndexChanged";
            try
            {
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlProvince.DataBind();
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