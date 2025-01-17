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
    public partial class EmployeeAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        EmployeeManager EM = new EmployeeManager();
        LocationManager LM = new LocationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "EmployeeAddNew.aspx";
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
                    btnEditEmployee.Focus();
                    if (EM.ReturnSuDoIDForLocation(CU))
                    {
                        //If user is not an admin then disable the edit employee button
                        //btnEditEmployee.Enabled = false;
                    }
                    //Check to see if an employee was selected
                    if (Convert.ToInt32(Request.QueryString["employee"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create an employee class
                            Employee employee = EM.ReturnEmployee(Convert.ToInt32(Request.QueryString["employee"].ToString()), CU.terminal.intBusinessNumber)[0];

                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(employee.intCountryID);
                            ddlProvince.DataBind();

                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.DataBind();

                            ddlJob.DataSource = EM.ReturnJobListings(CU.terminal.intBusinessNumber);
                            ddlJob.DataBind();

                            ddlLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                            ddlLocation.DataBind();

                            //Fill asll lables with current selected employee info
                            txtFirstName.Text = employee.varFirstName.ToString();
                            txtLastName.Text = employee.varLastName.ToString();
                            ddlJob.SelectedValue = employee.intJobCodeID.ToString();
                            ddlLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
                            txtEmail.Text = employee.varEmailAddress.ToString();
                            txtHomePhone.Text = employee.varHomePhone.ToString();
                            txtMobilePhone.Text = employee.varMobilePhone.ToString();
                            txtAddress.Text = employee.varAddress.ToString();
                            txtCity.Text = employee.varCityName.ToString();
                            txtPostalCode.Text = employee.varPostalCode.ToString();
                            ddlProvince.SelectedValue = employee.intProvinceID.ToString();
                            ddlCountry.SelectedValue = employee.intCountryID.ToString();
                        }
                    }
                    else
                    {
                        if (!IsPostBack)
                        {
                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.DataBind();
                            ddlCountry.SelectedValue = CU.currentStoreLocation.intCountryID.ToString();

                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(CU.currentStoreLocation.intCountryID);
                            ddlProvince.DataBind();
                            ddlProvince.SelectedValue = CU.currentStoreLocation.intProvinceID.ToString();

                            ddlJob.DataSource = EM.ReturnJobListings(CU.terminal.intBusinessNumber);
                            ddlJob.DataBind();
                            ddlJob.SelectedValue = EM.ReturnDefaultJobCode(CU.terminal.intBusinessNumber).ToString();

                            ddlLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                            ddlLocation.DataBind();
                            ddlLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
                        }
                        //With no employee selected display text boxes and drop downs to add employee
                        txtFirstName.Enabled = true;
                        txtLastName.Enabled = true;
                        ddlJob.Enabled = true;
                        ddlLocation.Enabled = true;

                        txtEmail.Enabled = true;
                        txtHomePhone.Enabled = true;
                        txtMobilePhone.Enabled = true;
                        txtAddress.Enabled = true;
                        txtCity.Enabled = true;
                        txtPostalCode.Enabled = true;
                        ddlProvince.Enabled = true;
                        ddlCountry.Enabled = true;

                        //hides and displays the proper buttons for access
                        btnSaveEmployee.Visible = false;
                        btnAddEmployee.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddEmployee";
                        btnEditEmployee.Visible = false;
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
        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddEmployee_Click";
            try
            {
                //Collects new employee data to add to database
                Employee employee = new Employee();
                employee.varFirstName = txtFirstName.Text;
                employee.varLastName = txtLastName.Text;
                employee.intJobCodeID = Convert.ToInt32(ddlJob.SelectedValue);
                employee.varEmailAddress = txtEmail.Text;
                employee.varHomePhone = txtHomePhone.Text;
                employee.varMobilePhone = txtMobilePhone.Text;
                employee.varAddress = txtAddress.Text;
                employee.varCityName = txtCity.Text;
                employee.varPostalCode = txtPostalCode.Text;
                employee.dtmCreationDate = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                employee.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                employee.intCountryID = Convert.ToInt32(ddlCountry.SelectedValue);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("employee", EM.AddEmployee(employee, CU.terminal.intBusinessNumber).ToString());
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
        protected void btnEditEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnEditEmployee_Click";
            try
            {
                //transfers data from label into textbox for editing
                txtFirstName.Enabled = true;
                txtLastName.Enabled = true;
                txtEmail.Enabled = true;
                txtHomePhone.Enabled = true;
                txtMobilePhone.Enabled = true;
                txtAddress.Enabled = true;
                txtCity.Enabled = true;
                txtPostalCode.Enabled = true;
                ddlJob.Enabled = true;
                ddlLocation.Enabled = true;
                ddlProvince.Enabled = true;
                ddlCountry.Enabled = true;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveEmployee";
                btnEditEmployee.Visible = false;
                btnAddEmployee.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
                //Add or Update the password for employee
                lblNewPassword.Visible = true;
                txtNewPassword.Visible = true;
                lblPasswordFormat.Visible = true;
                lblNewPassword2.Visible = true;
                txtNewPassword2.Visible = true;
                btnSavePassword.Visible = true;
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
        protected void btnSaveEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnSaveEmployee_Click";
            try
            {
                //Collects employee data to add to database
                Employee employee = new Employee();
                employee.intEmployeeID = Convert.ToInt32(Request.QueryString["employee"].ToString());
                employee.varFirstName = txtFirstName.Text;
                employee.varLastName = txtLastName.Text;
                employee.intJobCodeID = Convert.ToInt32(ddlJob.SelectedValue);
                employee.varEmailAddress = txtEmail.Text;
                employee.varHomePhone = txtHomePhone.Text;
                employee.varMobilePhone = txtMobilePhone.Text;
                employee.varAddress = txtAddress.Text;
                employee.varCityName = txtCity.Text;
                employee.varPostalCode = txtPostalCode.Text;
                employee.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                employee.intCountryID = Convert.ToInt32(ddlCountry.SelectedValue);

                //changes all text boxes and dropdowns to labels
                txtFirstName.Enabled = false;
                txtLastName.Enabled = false;
                ddlJob.Enabled = false;
                ddlLocation.Enabled = false;
                txtEmail.Enabled = false;
                txtHomePhone.Enabled = false;
                txtMobilePhone.Enabled = false;
                txtAddress.Enabled = false;
                txtCity.Enabled = false;
                txtPostalCode.Enabled = false;
                ddlProvince.Enabled = false;
                ddlCountry.Enabled = false;

                //hides and displays the proper buttons for access
                btnSaveEmployee.Visible = false;
                btnEditEmployee.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditEmployee";
                btnCancel.Visible = false;
                btnAddEmployee.Visible = false;
                btnBackToSearch.Visible = true;
                lblNewPassword.Visible = false;
                txtNewPassword.Visible = false;
                lblPasswordFormat.Visible = false;
                lblNewPassword2.Visible = false;
                txtNewPassword2.Visible = false;
                btnSavePassword.Visible = false;

                EM.UpdateEmployee(employee, CU.terminal.intBusinessNumber);
                //reloads current page
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("employee", employee.intEmployeeID.ToString());
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancel_Click";
            try
            {
                //no changes saved, refreshes current page
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnBackToSearch_Click";
            try
            {
                //Changes page to the settings page
                Response.Redirect("SettingsHomePage.aspx", true);
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
        protected void btnSavePassword_Click(object sender, EventArgs e)
        {
            string strMethod = "btnSavePassword_Click";
            try
            {
                //Compare the 2 passwords entered to make sure they are identical
                if (Convert.ToInt32(txtNewPassword.Text) == Convert.ToInt32(txtNewPassword2.Text))
                {
                    //Call method to add the new password
                    bool bolAdded = EM.SaveNewPassword(Convert.ToInt32(Request.QueryString["employee"].ToString()), Convert.ToInt32(txtNewPassword.Text), CU.terminal.intBusinessNumber);
                    //Check if the password was added or not
                    if (!bolAdded)
                    {
                        //The password was not added because it is already in use by employee
                        MessageBox.ShowMessage("The password supplied is not available. "
                            + "Please try another password.", this);
                    }
                    else
                    {
                        //The password was added, advise user and return to employee viewing
                        MessageBox.ShowMessage("New password for employee saved.", this);
                        btnCancel_Click(sender, e);
                    }
                }
                else
                {
                    //Passwords do not match
                    MessageBox.ShowMessage("The passwords do not match. "
                            + "Please retype the passwords again.", this);
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