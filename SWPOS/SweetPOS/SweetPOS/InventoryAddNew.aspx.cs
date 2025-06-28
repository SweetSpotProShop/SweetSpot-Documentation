using SweetPOS.ClassObjects;
using System;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class InventoryAddNew : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager LM = new LocationManager();
        InventoryManager IM = new InventoryManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strMethod = "Page_Load";
            Session["currentPage"] = "InventoryAddNew.aspx";
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
                    btnEditItem.Focus();
                    EmployeeManager EM = new EmployeeManager();
                    if (EM.ReturnSuDoIDForLocation(CU))
                    {
                        //If user is not an admin then disable the edit item button
                        //btnEditItem.Enabled = false;
                    }
                    //Check to see if an item was selected
                    if (Request.QueryString["inventory"].ToString() != "-10")
                    {
                        if (!IsPostBack)
                        {
                            ddlBrand.DataSource = IM.ReturnDropDownForBrand(CU.terminal.intBusinessNumber);
                            ddlBrand.DataBind();
                            ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                            ddlStoreLocation.DataBind();
                            Inventory inventory = IM.ReturnInventoryItem(Convert.ToInt32(Request.QueryString["inventory"]), CU.terminal.intBusinessNumber, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())))[0];
                            lblSKUDisplay.Text = inventory.varSku.ToString();
                            txtAverageCost.Text = inventory.fltAverageCost.ToString();
                            ddlBrand.SelectedValue = inventory.intBrandID.ToString();
                            txtPrice.Text = inventory.fltPrice.ToString();
                            txtQuantity.Text = inventory.intQuantity.ToString();
                            ddlStoreLocation.SelectedValue = inventory.intStoreLocationID.ToString();
                            txtCreationDate.Text = inventory.dtmCreationDate.ToString("yyyy-MM-dd");
                            txtModelName.Text = inventory.varModelName.ToString();
                            txtUPCcode.Text = inventory.varUPCcode.ToString();
                            if (inventory.bitIsRegularProduct)
                            {
                                rdbIsRegularProduct.SelectedValue = "0";
                            }
                            else
                            {
                                rdbIsRegularProduct.SelectedValue = "1";
                            }
                            txtDescription.Text = inventory.varDescription.ToString();
                            txtAdditionalInformation.Text = inventory.varAdditionalInformation.ToString();
                            grdInventoryTaxes.DataSource = inventory.lstTaxTypePerInventoryItem;
                            grdInventoryTaxes.DataBind();
                            chkUsedProduct.Checked = inventory.bitIsUsedProduct;
                            chkActiveProduct.Checked = inventory.bitIsActiveProduct;
                            btnCreateSimilar.Visible = true;
                        }
                    }
                    else
                    {
                        //When no item was selected display drop downs and text boxes
                        if (!IsPostBack)
                        {
                            ddlBrand.DataSource = IM.ReturnDropDownForBrand(CU.terminal.intBusinessNumber);
                            ddlBrand.DataBind();
                            ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                            ddlStoreLocation.DataBind();
                            ddlBrand.Enabled = true;
                            txtPrice.Enabled = true;
                            ddlStoreLocation.Enabled = true;
                            ddlStoreLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
                            txtCreationDate.Text = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())).ToString("yyyy-MM-dd");
                            txtModelName.Enabled = true;
                            txtUPCcode.Enabled = true;
                            rdbIsRegularProduct.Enabled = true;
                            chkActiveProduct.Enabled = true;
                            txtDescription.Enabled = true;
                            txtAdditionalInformation.Enabled = true;
                            btnCreateSimilar.Visible = false;
                        }
                        //hides and displays the proper buttons for access
                        btnSaveItem.Visible = false;
                        btnAddItem.Visible = true;
                        pnlDefaultButton.DefaultButton = "btnAddItem";
                        btnEditItem.Visible = false;
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
        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddItem_Click";
            try
            {
                Inventory inventory = new Inventory
                {
                    intBrandID = Convert.ToInt32(ddlBrand.SelectedValue),
                    varModelName = txtModelName.Text,
                    varDescription = txtDescription.Text,
                    intStoreLocationID = Convert.ToInt32(ddlStoreLocation.SelectedValue),
                    varUPCcode = txtUPCcode.Text,
                    intQuantity = Convert.ToInt32(txtQuantity.Text),
                    fltPrice = Convert.ToDouble(txtPrice.Text),
                    fltAverageCost = Convert.ToDouble(txtAverageCost.Text),
                    bitIsUsedProduct = chkUsedProduct.Checked,
                    bitIsActiveProduct = chkActiveProduct.Checked,
                    dtmCreationDate = Convert.ToDateTime(txtCreationDate.Text),
                    varAdditionalInformation = txtAdditionalInformation.Text
                };

                if (rdbIsRegularProduct.SelectedValue == "0")
                {
                    inventory.bitIsRegularProduct = true;
                    inventory.bitIsNonStockedProduct = false;
                }
                else
                {
                    inventory.bitIsRegularProduct = false;
                    inventory.bitIsNonStockedProduct = true;
                }
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", IM.AddNewInventoryItem(inventory, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU).ToString());
                //Refreshes current page
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
        protected void btnEditItem_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnEditItem_Click";
            try
            {
                ddlBrand.Enabled = true;
                txtPrice.Enabled = true;
                ddlStoreLocation.Enabled = true;
                txtModelName.Enabled = true;
                txtUPCcode.Enabled = true;
                txtDescription.Enabled = true;
                txtAdditionalInformation.Enabled = true;
                chkActiveProduct.Enabled = true;

                //hides and displays the proper buttons for access
                btnSaveItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnSaveItem";
                btnEditItem.Visible = false;
                btnAddItem.Visible = false;
                btnCancel.Visible = true;
                btnBackToSearch.Visible = false;
                btnCreateSimilar.Visible = false;
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
        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            string strMethod = "btnSaveItem_Click";
            try
            {
                txtAverageCost.Enabled = false;
                ddlBrand.Enabled = false;
                txtPrice.Enabled = false;
                txtQuantity.Enabled = false;
                ddlStoreLocation.Enabled = false;
                txtModelName.Enabled = false;
                txtUPCcode.Enabled = false;
                rdbIsRegularProduct.Enabled = false;
                txtDescription.Enabled = false;
                chkUsedProduct.Enabled = false;
                chkActiveProduct.Enabled = false;
                txtAdditionalInformation.Enabled = false;

                Inventory inventory = new Inventory
                {
                    intInventoryID = Convert.ToInt32(Request.QueryString["inventory"].ToString()),
                    fltAverageCost = Convert.ToDouble(txtAverageCost.Text),
                    intBrandID = Convert.ToInt32(ddlBrand.SelectedValue),
                    fltPrice = Convert.ToDouble(txtPrice.Text),
                    intQuantity = Convert.ToInt32(txtQuantity.Text),
                    intStoreLocationID = Convert.ToInt32(ddlStoreLocation.SelectedValue),
                    varModelName = txtModelName.Text,
                    varUPCcode = txtUPCcode.Text,
                    varDescription = txtDescription.Text,
                    bitIsUsedProduct = chkUsedProduct.Checked,
                    bitIsActiveProduct = chkActiveProduct.Checked,
                    varAdditionalInformation = txtAdditionalInformation.Text
                };
                if (rdbIsRegularProduct.SelectedValue == "0")
                {
                    inventory.bitIsRegularProduct = true;
                    inventory.bitIsNonStockedProduct = false;
                }
                else
                {
                    inventory.bitIsRegularProduct = false;
                    inventory.bitIsNonStockedProduct = true;
                }

                //hides and displays the proper buttons for access
                btnSaveItem.Visible = false;
                btnEditItem.Visible = true;
                pnlDefaultButton.DefaultButton = "btnEditItem";
                btnCancel.Visible = false;
                btnAddItem.Visible = false;
                btnBackToSearch.Visible = true;
                btnCreateSimilar.Visible = true;

                IM.UpdateInventory(inventory, CU.terminal.intBusinessNumber);

                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", inventory.intInventoryID.ToString());
                //Refreshes current page
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
            //Collects current method for error tracking
            string strMethod = "btnBackToSearch_Click";
            try
            {
                //Changes page to the inventory home page
                Response.Redirect("InventoryHomePage.aspx", true);
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
        protected void btnCreateSimilar_Click(object sender, EventArgs e)
        {
            string strMethod = "btnCreateSimilar_Click";
            try
            {
                Inventory inventory = new Inventory
                {
                    intBrandID = Convert.ToInt32(ddlBrand.SelectedValue),
                    varModelName = txtModelName.Text,
                    varDescription = txtDescription.Text,
                    intStoreLocationID = Convert.ToInt32(ddlStoreLocation.SelectedValue),
                    varUPCcode = txtUPCcode.Text,
                    intQuantity = Convert.ToInt32(txtQuantity.Text),
                    fltPrice = Convert.ToDouble(txtPrice.Text),
                    fltAverageCost = Convert.ToDouble(txtAverageCost.Text),
                    bitIsUsedProduct = chkUsedProduct.Checked,
                    bitIsActiveProduct = chkActiveProduct.Checked,
                    dtmCreationDate = Convert.ToDateTime(txtCreationDate.Text),
                    varAdditionalInformation = txtAdditionalInformation.Text
                };

                if (rdbIsRegularProduct.SelectedValue == "0")
                {
                    inventory.bitIsRegularProduct = true;
                    inventory.bitIsNonStockedProduct = false;
                }
                else
                {
                    inventory.bitIsRegularProduct = false;
                    inventory.bitIsNonStockedProduct = true;
                }
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("inventory", IM.AddNewInventoryItem(inventory, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU).ToString());
                //Refreshes current page
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

        protected void grdInventoryTaxes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                if(!Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "bitChargeTax")))
                {
                    LinkButton lCharge = (LinkButton)e.Row.FindControl("lbtnChangeCharged");
                    lCharge.Text = "Charge";
                }
            }
        }

        protected void grdInventoryTaxes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
            bool chargeTax = Convert.ToBoolean(((CheckBox)grdInventoryTaxes.Rows[index].Cells[3].FindControl("chkChargeTax")).Checked);
            if (chargeTax)
            {
                chargeTax = false;
            }
            else
            {
                chargeTax = true;
            }
            TaxManager TM = new TaxManager();
            TM.UpdateTaxChargedForInventory(CU.terminal.intBusinessNumber, Convert.ToInt32(Request.QueryString["inventory"].ToString()), Convert.ToInt32(e.CommandArgument.ToString()), chargeTax);
            var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            nameValues.Set("inventory", Request.QueryString["inventory"].ToString());
            //Refreshes current page
            Response.Redirect(Request.Url.AbsolutePath + "?" + nameValues, true);
        }
    }
}