using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class VendorAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        VendorSupplierManager VSM = new VendorSupplierManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "VendorAddNew.aspx";
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
                    btnEditVendor.Focus();
                    //Checks for a vendorsupplier Key
                    if (Convert.ToInt32(Request.QueryString["vendor"].ToString()) != -10)
                    {
                        if (!IsPostBack)
                        {
                            //Create vendorsupplier class and fill page with all info based in the vendorsupplier number 
                            VendorSupplier vendorSupplier = VSM.ReturnVendorSupplierWithInventoryAndPO(Convert.ToInt32(Request.QueryString["vendor"].ToString()), CU.terminal.intBusinessNumber)[0];

                            txtVendorSupplierName.Text = vendorSupplier.varVendorSupplierName.ToString();
                            txtVendorSupplierCode.Text = vendorSupplier.varVendorSupplierCode.ToString();
                            txtAddress.Text = vendorSupplier.varAddress.ToString();
                            txtMainPhoneNumber.Text = vendorSupplier.varMainPhoneNumber.ToString();
                            txtFaxNumber.Text = vendorSupplier.varFaxNumber.ToString();
                            txtEmailAddress.Text = vendorSupplier.varEmailAddress.ToString();
                            txtCity.Text = vendorSupplier.varCityName.ToString();
                            chkIsActive.Checked = vendorSupplier.bitIsActive;

                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.DataBind();
                            ddlCountry.SelectedValue = vendorSupplier.intCountryID.ToString();
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(vendorSupplier.intCountryID);
                            ddlProvince.SelectedValue = vendorSupplier.intProvinceID.ToString();
                            ddlProvince.DataBind();

                            txtPostalCode.Text = vendorSupplier.varPostalCode.ToString();

                            //Binds vendor items and pos list to the grid view
                            grdVendorSuppliedInventory.DataSource = vendorSupplier.lstVendorSupplierItems;
                            grdVendorSuppliedInventory.DataBind();
                            grdReceivedPurchaseOrders.DataSource = vendorSupplier.lstPurchaseOrders;
                            grdReceivedPurchaseOrders.DataBind();
                        }
                    }
                    else
                    {
                        //no vendor number
                        if (!IsPostBack)
                        {
                            ddlCountry.DataSource = LM.ReturnCountryDropDown();
                            ddlCountry.SelectedValue = CU.employee.intCountryID.ToString();
                            ddlCountry.DataBind();
                            ddlProvince.DataSource = LM.ReturnProvinceDropDown(Convert.ToInt32(ddlCountry.SelectedValue));
                            ddlProvince.SelectedValue = CU.employee.intProvinceID.ToString();
                            ddlProvince.DataBind();

                        }
                        //Displays text boxes instead of label for vendor creation info
                        txtVendorSupplierName.Enabled = true;
                        txtVendorSupplierCode.Enabled = true;
                        txtAddress.Enabled = true;
                        txtMainPhoneNumber.Enabled = true;
                        txtFaxNumber.Enabled = true;
                        txtEmailAddress.Enabled = true;
                        txtCity.Enabled = true;
                        ddlProvince.Enabled = true;
                        ddlCountry.Enabled = true;
                        txtPostalCode.Enabled = true;
                        chkIsActive.Enabled = true;
                        //hides and displays the proper buttons for access
                        btnSaveVendor.Visible = false;
                        btnAddVendor.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddVendor";
                        btnEditVendor.Visible = false;
                        btnCreatePurchaseOrder.Visible = false;
                        btnCancel.Visible = false;
                        btnBackToSearch.Visible = true;
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
        protected void btnAddVendor_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddVendor_Click";
            try
            {
                //Collects new vendor data to add to database
                VendorSupplier vendorSupplier = new VendorSupplier
                {
                    varVendorSupplierName = txtVendorSupplierName.Text,
                    varVendorSupplierCode = txtVendorSupplierCode.Text,
                    varAddress = txtAddress.Text,
                    varMainPhoneNumber = txtMainPhoneNumber.Text,
                    varFaxNumber = txtFaxNumber.Text,
                    varEmailAddress = txtEmailAddress.Text,
                    varCityName = txtCity.Text,
                    intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue),
                    intCountryID = Convert.ToInt32(ddlCountry.SelectedValue),
                    varPostalCode = txtPostalCode.Text,
                    bitIsActive = chkIsActive.Checked
                };

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("vendor", VSM.AddNewVendorSupplier(vendorSupplier, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber).ToString());
                Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, true);
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
        protected void btnEditVendor_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnEditVendor_Click";
            try
            {
                //transfers data from label into textbox for editing
                txtVendorSupplierName.Enabled = true;
                txtVendorSupplierCode.Enabled = true;
                txtAddress.Enabled = true;
                txtMainPhoneNumber.Enabled = true;
                txtFaxNumber.Enabled = true;
                txtEmailAddress.Enabled = true;
                txtCity.Enabled = true;
                ddlCountry.Enabled = true;
                ddlProvince.Enabled = true;
                txtPostalCode.Enabled = true;
                chkIsActive.Enabled = true;
                //hides and displays the proper buttons for access
                btnSaveVendor.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveVendor";
                btnEditVendor.Visible = false;
                btnAddVendor.Visible = false;
                btnCreatePurchaseOrder.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
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
        protected void btnSaveVendor_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnSaveVendor_Click";
            try
            {
                //Collects Vendor data to add to database
                VendorSupplier vendorSupplier = new VendorSupplier
                {
                    intVendorSupplierID = Convert.ToInt32(Request.QueryString["vendor"].ToString()),
                    varVendorSupplierName = txtVendorSupplierName.Text,
                    varVendorSupplierCode = txtVendorSupplierCode.Text,
                    varAddress = txtAddress.Text,
                    varMainPhoneNumber = txtMainPhoneNumber.Text,
                    varFaxNumber = txtFaxNumber.Text,
                    varEmailAddress = txtEmailAddress.Text,
                    varCityName = txtCity.Text,
                    intProvinceID = Convert.ToInt32(ddlProvince.SelectedValue),
                    intCountryID = Convert.ToInt32(ddlCountry.SelectedValue),
                    varPostalCode = txtPostalCode.Text,
                    bitIsActive = chkIsActive.Checked
                };
                //updates the vendor info in tables
                VSM.UpdateVendorInformation(vendorSupplier, CU.terminal.intBusinessNumber);
                //changes all text boxes and dropdowns to labels
                txtVendorSupplierName.Enabled = false;
                txtVendorSupplierCode.Enabled = false;
                txtAddress.Enabled = false;
                txtMainPhoneNumber.Enabled = false;
                txtFaxNumber.Enabled = false;
                txtEmailAddress.Enabled = false;
                txtCity.Enabled = false;
                ddlProvince.Enabled = false;
                ddlCountry.Enabled = false;
                txtPostalCode.Enabled = false;
                chkIsActive.Enabled = false;
                //hides and displays the proper buttons for access
                btnSaveVendor.Visible = false;
                btnEditVendor.Visible = true;
                btnCancel.Visible = false;
                btnAddVendor.Visible = false;
                btnBackToSearch.Visible = true;
                btnSaveVendor.Visible = false;
                btnEditVendor.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditVendor";
                btnCancel.Visible = false;
                btnCreatePurchaseOrder.Visible = true;
                btnAddVendor.Visible = false;
                btnBackToSearch.Visible = true;
                //reloads current page
                Response.Redirect(Request.RawUrl, true);
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
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCancel_Click";
            try
            {
                //no chnages saved and moves to vendor home page
                Response.Redirect(Request.RawUrl, true);
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
        protected void btnCreatePurchaseOrder_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnCreatePurchaseOrder_Click";
            try
            {
                DateTime dtmToday = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime()));
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("vendor", Request.QueryString["vendor"].ToString());
                nameValues.Set("purchO", "-10");
                nameValues.Set("suppliedPO", "");
                Response.Redirect("PurchaseOrderCart.aspx?" + nameValues, true);
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
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "btnBackToSearch_Click";
            try
            {
                //opens the vendor home page
                Response.Redirect("VendorHomePage.aspx", true);
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
        protected void grdVendorSuppliedInventory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdVendorSuppliedInventory_RowCommand";
            try
            {
                if(e.CommandName.ToString() == "viewInventory")
                {
                    Response.Redirect("InventoryAddNew.aspx?inventory=" + e.CommandArgument.ToString(), true);
                }
                else if(e.CommandName.ToString() == "addInventoryToVendor")
                {
                    Button btnClicked = (Button)grdVendorSuppliedInventory.FooterRow.FindControl("btnAddInventoryToVendor");
                    if (btnClicked.Text == "Set Inventory")
                    {
                        ((TextBox)grdVendorSuppliedInventory.FooterRow.FindControl("txtVendorSupplierProductCode")).Enabled = true;
                        ((DropDownList)grdVendorSuppliedInventory.FooterRow.FindControl("ddlInventoryList")).Enabled = true;
                        btnClicked.Text = "Commit";
                    }
                    else
                    {
                        TextBox vendorSupplierProductCode = (TextBox)grdVendorSuppliedInventory.FooterRow.FindControl("txtVendorSupplierProductCode");
                        DropDownList inventoryList = (DropDownList)grdVendorSuppliedInventory.FooterRow.FindControl("ddlInventoryList");

                        VendorSupplierProduct vendorSupplierProduct = new VendorSupplierProduct();
                        vendorSupplierProduct.intVendorSupplierID = Convert.ToInt32(Request.QueryString["vendor"].ToString());
                        vendorSupplierProduct.intInventoryID = Convert.ToInt32(inventoryList.SelectedValue);
                        vendorSupplierProduct.varVendorSupplierProductCode = vendorSupplierProductCode.Text;

                        VSM.AddVendorSupplierForInventoryItem(vendorSupplierProduct, CU.terminal.intBusinessNumber);

                        Response.Redirect(Request.RawUrl, true);
                    }
                }
                else if(e.CommandName.ToString() == "removeFromVendor")
                {
                    VSM.RemoveVendorSupplierForInventoryItem(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(Request.QueryString["vendor"]), CU.terminal.intBusinessNumber);
                    Response.Redirect(Request.RawUrl, true);
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
        protected void grdReceivedPurchaseOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdReceivedPurchaseOrders_RowCommand";
            try
            {
                Response.Redirect("PrintablePurchaseOrder.aspx?purchO=" + e.CommandArgument.ToString(), true);
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
        protected void grdVendorSuppliedInventory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.Footer)
            {
                DropDownList inventoryList = (DropDownList)e.Row.FindControl("ddlInventoryList");
                inventoryList.DataSource = VSM.ReturnInventoryNotSuppliedByVendor(Convert.ToInt32(Request.QueryString["vendor"].ToString()), CU.terminal.intBusinessNumber);
                inventoryList.DataBind();
            }
        }
    }
}