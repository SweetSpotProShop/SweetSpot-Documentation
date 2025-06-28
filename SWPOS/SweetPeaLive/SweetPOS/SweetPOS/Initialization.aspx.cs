using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SweetPOS
{
    public partial class Initialization : System.Web.UI.Page
    {
        LocationManager LM = new LocationManager();
        LicenceFiles LF = new LicenceFiles();
        TerminalManager TM = new TerminalManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CreateCompanyDatabase CCD = new CreateCompanyDatabase();
        DeleteCompanyDatabase DCD = new DeleteCompanyDatabase();
        private static StoreLocation storeLocation;
        private static Terminal terminal;

        CurrentUser CU = new CurrentUser();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCountry.DataSource = LM.ReturnCountryDropDown();
                ddlCountry.DataBind();
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlProvince.DataBind();
                pnlLocationSetup.Visible = true;

                //create a drop down and on code 4 all you do is 
                //change the drop down to another location,
                //There will be an option in the drop down for a new location
                //either way we can then proceed to the next step.
                //Collects setup location data to add to database

                int businessNumber = Convert.ToInt32(Request.QueryString["businessNumber"]);
                int code = Convert.ToInt32(Request.QueryString["code"]);

                if (code == 4)
                {
                    Session["locations"] = LF.ChooseLocationForTerminal(businessNumber);
                }
                else if(code == 1)
                {
                    //set table creation here
                    //with table creation create all default data
                    CCD.CreateAllTableData(businessNumber);
                    Session["locations"] = LF.CreateLocationForTerminal();
                }

                ddlStoreLocationName.DataSource = LF.ReturnDropDownForLocations((DataTable)Session["locations"]);
                ddlStoreLocationName.DataBind();
                ddlStoreLocationName.Focus();

                txtStoreLocationName.Enabled = true;
                txtStoreCode.Enabled = true;
                txtPhoneNumber.Enabled = true;
                txtTaxNumber.Enabled = true;
                txtEmailAddress.Enabled = true;
                chkIsRetailLocation.Enabled = true;
                txtAddress.Enabled = true;
                txtCity.Enabled = true;
                txtPostalCode.Enabled = true;
                ddlProvince.Enabled = true;
                ddlCountry.Enabled = true;
            }
        }
        protected void btnSaveStoreLocation_Click(object sender, EventArgs e)
        {
            try
            {
                StoreLocation sLocation = new StoreLocation();
                if (Convert.ToInt32(ddlStoreLocationName.SelectedValue) == 0)
                {
                    sLocation.varStoreName = txtStoreLocationName.Text;
                    sLocation.varStoreCode = txtStoreCode.Text;
                    sLocation.varAddress = txtAddress.Text;
                    sLocation.varPhoneNumber = txtPhoneNumber.Text;
                    sLocation.varTaxNumber = txtTaxNumber.Text;
                    sLocation.bitIsRetailLocation = chkIsRetailLocation.Checked;
                    sLocation.varEmailAddress = txtEmailAddress.Text;
                    sLocation.varCityName = txtCity.Text;
                    sLocation.intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue);
                    sLocation.intCountryID = Convert.ToInt32(ddlCountry.SelectedValue);
                    sLocation.varPostalCode = txtPostalCode.Text;

                    storeLocation = sLocation;
                    Session["terminals"] = LF.CreateTerminalFromLocation();
                }
                else
                {
                    storeLocation = LM.ReturnLocation(Convert.ToInt32(ddlStoreLocationName.SelectedValue), Convert.ToInt32(Request.QueryString["businessNumber"]))[0];
                    ddlTillNumber.Visible = true;
                    ddlTillNumber.Enabled = true;
                    Session["terminals"] = LF.ChooseTerminalFromLocation(storeLocation.intStoreLocationID, Convert.ToInt32(Request.QueryString["businessNumber"]));
                }

                ddlTillNumber.DataSource = LF.ReturnDropDownForTerminals((DataTable)Session["terminals"]);
                ddlTillNumber.DataBind();
                ddlTillNumber.Focus();

                pnlLocationSetup.Visible = false;
                pnlTerminalSetup.Visible = true;

                txtLicenceNumber.Enabled = true;
                txtDrawerFloat.Enabled = true;

            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Display message box
                MessageBox.ShowMessage("An Error has occurred during setup. "
                    + "Please contact program administrator.", this);
            }
        }
        protected void btnSaveTerminalInformation_Click(object sender, EventArgs e)
        {
            try
            {
                Terminal tempTerminal = new Terminal();
                if(Convert.ToInt32(ddlTillNumber.SelectedValue) == 0)
                {
                    tempTerminal.intBusinessNumber = Convert.ToInt32(Request.QueryString["businessNumber"]);
                    tempTerminal.intTillNumber = 0;
                    tempTerminal.varLicenceNumber = txtLicenceNumber.Text;
                    tempTerminal.fltDrawerFloatAmount = Convert.ToDouble(txtDrawerFloat.Text);
                    terminal = tempTerminal;
                }
                else
                {
                    terminal = TM.ReturnTerminal(Convert.ToInt32(ddlTillNumber.SelectedValue), Convert.ToInt32(Request.QueryString["businessNumber"]))[0];
                    terminal.intBusinessNumber = Convert.ToInt32(Request.QueryString["businessNumber"]);
                };
                
                object[] setupFiles = { Convert.ToInt32(Request.QueryString["businessNumber"]), storeLocation, terminal, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), Convert.ToInt32(Request.QueryString["code"]) };
                Terminal returnData = TM.SetupInitialFiles(setupFiles);

                HttpCookie cookieTerminalID = new HttpCookie("terminalID");
                cookieTerminalID.Value = returnData.intTerminalID.ToString();
                cookieTerminalID.Expires = DateTime.Now.AddYears(1);
                HttpCookie cookieBusinessNumber = new HttpCookie("businessNumber");
                cookieBusinessNumber.Value = returnData.intBusinessNumber.ToString();
                cookieBusinessNumber.Expires = DateTime.Now.AddYears(1);
                HttpCookie cookieLicenceID = new HttpCookie("licenceID");
                cookieLicenceID.Value = returnData.intLicenceID.ToString();
                cookieLicenceID.Expires = DateTime.Now.AddYears(1);

                var response = HttpContext.Current.Response;
                response.Cookies.Add(cookieTerminalID);
                response.Cookies.Add(cookieBusinessNumber);
                response.Cookies.Add(cookieLicenceID);

                Response.Redirect("LoginPage.aspx", true);
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Display message box
                MessageBox.ShowMessage("An Error has occurred during setup. "
                    + "Please contact program administrator. " + ex.Message.ToString(), this);
            }
        }
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlProvince.DataBind();
            }
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                //Display message box
                MessageBox.ShowMessage("An Error has occurred during setup. "
                    + "Please contact program administrator.", this);
            }
        }
        protected void btnRemoveTables_Click(object sender, EventArgs e)
        {
            DCD.DELETEAllTableData(Convert.ToInt32(Request.QueryString["businessNumber"]));
        }
        protected void ddlStoreLocationName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Convert.ToInt32(ddlStoreLocationName.SelectedValue) == 0)
            {
                txtStoreLocationName.Visible = true;
                txtStoreLocationName.Text = "";
                txtStoreCode.Enabled = true;
                txtStoreCode.Text = "";
                txtPhoneNumber.Enabled = true;
                txtPhoneNumber.Text = "";
                txtTaxNumber.Enabled = true;
                txtTaxNumber.Text = "";
                txtEmailAddress.Enabled = true;
                txtEmailAddress.Text = "";
                chkIsRetailLocation.Enabled = true;
                chkIsRetailLocation.Checked = false;
                txtAddress.Enabled = true;
                txtAddress.Text = "";
                txtCity.Enabled = true;
                txtCity.Text = "";
                txtPostalCode.Enabled = true;
                txtPostalCode.Text = "";
                ddlProvince.Enabled = true;
                ddlCountry.Enabled = true;
            }
            else
            {
                StoreLocation thisLocation = LM.ReturnLocation(Convert.ToInt32(ddlStoreLocationName.SelectedValue), Convert.ToInt32(Request.QueryString["businessNumber"]))[0];
                txtStoreLocationName.Visible = false;
                txtStoreCode.Enabled = false;
                txtStoreCode.Text = thisLocation.varStoreCode.ToString();
                txtPhoneNumber.Enabled = false;
                txtPhoneNumber.Text = thisLocation.varPhoneNumber.ToString();
                txtTaxNumber.Enabled = false;
                txtTaxNumber.Text = thisLocation.varTaxNumber.ToString();
                txtEmailAddress.Enabled = false;
                txtEmailAddress.Text = thisLocation.varEmailAddress.ToString();
                chkIsRetailLocation.Enabled = false;
                chkIsRetailLocation.Checked = thisLocation.bitIsRetailLocation;
                txtAddress.Enabled = false;
                txtAddress.Text = thisLocation.varAddress.ToString();
                txtCity.Enabled = false;
                txtCity.Text = thisLocation.varCityName.ToString();
                txtPostalCode.Enabled = false;
                txtPostalCode.Text = thisLocation.varPostalCode.ToString();
                ddlProvince.Enabled = false;
                ddlCountry.SelectedValue = thisLocation.intCountryID.ToString();
                ddlProvince.DataSource = LM.ReturnProvinceDropDown(thisLocation.intCountryID);
                ddlProvince.SelectedValue = thisLocation.intProvinceID.ToString();
                ddlProvince.DataBind();
                ddlCountry.Enabled = false;
            }
        }
        protected void ddlTillNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(ddlTillNumber.SelectedValue) == 0)
            {
                txtLicenceNumber.Enabled = true;
                txtLicenceNumber.Text = "";
                txtDrawerFloat.Enabled = true;
                txtDrawerFloat.Text = "";                
            }
            else
            {
                Terminal thisTerminal = TM.ReturnTerminal(Convert.ToInt32(ddlTillNumber.SelectedValue), Convert.ToInt32(Request.QueryString["businessNumber"]))[0];
                txtLicenceNumber.Enabled = false;
                txtLicenceNumber.Text = thisTerminal.varLicenceNumber.ToString();
                txtDrawerFloat.Enabled = false;
                txtDrawerFloat.Text = thisTerminal.fltDrawerFloatAmount.ToString();
            }
        }
        //private string settingFolderPath()
        //{
        //    string strLicenceFilePath = "";
        //    CommonOpenFileDialog dialog = new CommonOpenFileDialog
        //    {
        //        DefaultFileName = string.Empty,
        //        InitialDirectory = "C:\\Users",
        //        IsFolderPicker = true
        //    };
        //    CommonFileDialogResult result = dialog.ShowDialog();
        //    if (result == CommonFileDialogResult.Ok)
        //    {
        //        strLicenceFilePath = dialog.FileName;
        //    }
        //    else
        //    {
        //        strLicenceFilePath = dialog.InitialDirectory;
        //    }
        //    return strLicenceFilePath;
        //}
    }
}