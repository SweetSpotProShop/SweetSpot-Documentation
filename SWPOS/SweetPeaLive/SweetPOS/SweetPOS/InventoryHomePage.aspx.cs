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
    public partial class InventoryHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        List<Inventory> searched = new List<Inventory>();
        InventoryManager IM = new InventoryManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "InventoryHomePage";
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
                    txtSearch.Focus();
                    EmployeeManager EM = new EmployeeManager();
                    if (EM.ReturnSuDoIDForLocation(CU))
                    {
                        //If user is not an admin then disable the add new item button
                        //btnAddNewInventory.Enabled = false;
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
        protected void btnInventorySearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnInventorySearch_Click";
            try
            {
                string[] headers = { "SKU", "Description ▼", "Store ▼", "Quantity ▼", "Stocked ▼", "Price ▼", "Cost ▼", "Comments ▼" };
                ViewState["headers"] = headers;

                searched = IM.ReturnInventoryFromSearchString(txtSearch.Text, chkIncludeZero.Checked, CU.terminal.intBusinessNumber);
                ViewState["listItems"] = searched;
                populateGridview(searched);
                grdInventorySearched.PageIndex = 0;
            }   
            //Exception catch
            catch (ThreadAbortException tae) { }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233086)
                {
                    MessageBox.ShowMessage("You have searched for an invalid SKU number. "
                       + "Please verify the SKU number you are looking for and try again. ", this);
                }
                else
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
        protected void btnAddNewInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddNewInventory_Click";
            try
            {
                //Changes page to the inventory add new page
                Response.Redirect("InventoryAddNew.aspx?inventory=-10", true);
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

        protected void btnPurchaseManagement_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnPurchaseManagement_Click";
            try
            {
                Response.Redirect("PurchaseOrderHomePage.aspx", true);
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

        protected void grdInventorySearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdInventorySearched_RowCommand";
            try
            {
                if (e.CommandName == "viewItem")
                {
                    //Change to Inventory Add new page to display selected item
                    Response.Redirect("InventoryAddNew.aspx?inventory=" + e.CommandArgument.ToString(), true);
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
        protected void grdInventorySearched_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string strMethod = "grdInventorySearched_PageIndexChanging";
            try
            {
                grdInventorySearched.PageIndex = e.NewPageIndex;
                searched = (List<Inventory>)ViewState["listItems"];
                populateGridview(searched);
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
        protected void populateGridview(List<Inventory> list)
        {
            string strMethod = "populateGridview";
            try
            {
                grdInventorySearched.Visible = true;
                //Binds returned items to gridview for display
                grdInventorySearched.DataSource = list;
                grdInventorySearched.DataBind();
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
        //Sorting Skus
        protected void btnSKU_Click(object sender, EventArgs e)
        {
            string strMethod = "btnSKU_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button sku = grdInventorySearched.HeaderRow.FindControl("btnSKU") as Button;
                string sort = sku.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "SKU":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varSku.CompareTo(y.varSku);
                        });
                        break;
                    case "SKU ▼":
                        headers[0] = "SKU ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varSku.CompareTo(y.varSku);
                        });
                        break;
                    case "SKU ▲":
                        headers[0] = "SKU ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.varSku.CompareTo(x.varSku);
                        });
                        break;
                }
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnDescription_Click(object sender, EventArgs e)
        {
            string strMethod = "btnDescription_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button desc = grdInventorySearched.HeaderRow.FindControl("btnDescription") as Button;
                string sort = desc.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Description":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varDescription.CompareTo(y.varDescription);
                        });
                        break;
                    case "Description ▼":
                        headers[1] = "Description ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varDescription.CompareTo(y.varDescription);
                        });
                        break;
                    case "Description ▲":
                        headers[1] = "Description ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.varDescription.CompareTo(x.varDescription);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnStore_Click(object sender, EventArgs e)
        {
            string strMethod = "btnStore_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button store = grdInventorySearched.HeaderRow.FindControl("btnStore") as Button;
                string sort = store.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Store":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.intStoreLocationID.CompareTo(y.intStoreLocationID);
                        });
                        break;
                    case "Store ▼":
                        headers[2] = "Store ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.intStoreLocationID.CompareTo(y.intStoreLocationID);
                        });
                        break;
                    case "Store ▲":
                        headers[2] = "Store ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.intStoreLocationID.CompareTo(x.intStoreLocationID);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnQuantity_Click(object sender, EventArgs e)
        {
            string strMethod = "btnQuantity_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button quantity = grdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button;
                string sort = quantity.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Quantity":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.intQuantity.CompareTo(y.intQuantity);
                        });
                        break;
                    case "Quantity ▼":
                        headers[3] = "Quantity ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.intQuantity.CompareTo(y.intQuantity);
                        });
                        break;
                    case "Quantity ▲":
                        headers[3] = "Quantity ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.intQuantity.CompareTo(x.intQuantity);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnStocked_Click(object sender, EventArgs e)
        {
            string strMethod = "btnStocked_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button stocked = grdInventorySearched.HeaderRow.FindControl("btnStocked") as Button;
                string sort = stocked.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Stocked":
                        headers[4] = "Stocked ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.bitIsRegularProduct.CompareTo(y.bitIsRegularProduct);
                        });
                        break;
                    case "Stocked ▼":
                        headers[4] = "Stocked ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.bitIsRegularProduct.CompareTo(y.bitIsRegularProduct);
                        });
                        break;
                    case "Stocked ▲":
                        headers[4] = "Stocked ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.bitIsRegularProduct.CompareTo(x.bitIsRegularProduct);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[5] = "Price";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnPrice_Click(object sender, EventArgs e)
        {
            string strMethod = "btnPrice_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button price = grdInventorySearched.HeaderRow.FindControl("btnPrice") as Button;
                string sort = price.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Price":
                        headers[5] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.fltPrice.CompareTo(y.fltPrice);
                        });
                        break;
                    case "Price ▼":
                        headers[5] = "Price ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.fltPrice.CompareTo(y.fltPrice);
                        });
                        break;
                    case "Price ▲":
                        headers[5] = "Price ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                        return y.fltPrice.CompareTo(x.fltPrice);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[6] = "Cost";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnCost_Click(object sender, EventArgs e)
        {
            string strMethod = "btnCost_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button cost = grdInventorySearched.HeaderRow.FindControl("btnCost") as Button;
                string sort = cost.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Cost":
                        headers[6] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.fltAverageCost.CompareTo(y.fltAverageCost);
                        });
                        break;
                    case "Cost ▼":
                        headers[6] = "Cost ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.fltAverageCost.CompareTo(y.fltAverageCost);
                        });
                        break;
                    case "Cost ▲":
                        headers[6] = "Cost ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.fltAverageCost.CompareTo(x.fltAverageCost);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[7] = "Comments";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void btnComments_Click(object sender, EventArgs e)
        {
            string strMethod = "btnComments_Click";
            try
            {
                //Grabbing the list
                searched = (List<Inventory>)ViewState["listItems"];
                Button comment = grdInventorySearched.HeaderRow.FindControl("btnComments") as Button;
                string sort = comment.Text;
                string[] headers = ViewState["headers"] as string[];
                switch (sort)
                {
                    case "Comments":
                        headers[7] = "Comments ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varAdditionalInformation.CompareTo(y.varAdditionalInformation);
                        });
                        break;
                    case "Comments ▼":
                        headers[7] = "Comments ▲";
                        //Ascending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return x.varAdditionalInformation.CompareTo(y.varAdditionalInformation);
                        });
                        break;
                    case "Comments ▲":
                        headers[7] = "Comments ▼";
                        //Descending Order
                        searched.Sort(delegate (Inventory x, Inventory y)
                        {
                            return y.varAdditionalInformation.CompareTo(x.varAdditionalInformation);
                        });
                        break;
                }
                headers[0] = "SKU";
                headers[1] = "Description";
                headers[2] = "Store";
                headers[3] = "Quantity";
                headers[4] = "Stocked";
                headers[5] = "Price";
                headers[6] = "Cost";
                ViewState["headers"] = headers;
                //Populating/Sorting the gridview
                populateGridview(searched);
                updateButtonText(headers);
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
        protected void updateButtonText(string[] headers)
        {
            string strMethod = "updateButtonText";
            try
            {
                (grdInventorySearched.HeaderRow.FindControl("btnSKU") as Button).Text = headers[0];
                (grdInventorySearched.HeaderRow.FindControl("btnDescription") as Button).Text = headers[1];
                (grdInventorySearched.HeaderRow.FindControl("btnStore") as Button).Text = headers[2];
                (grdInventorySearched.HeaderRow.FindControl("btnQuantity") as Button).Text = headers[3];
                (grdInventorySearched.HeaderRow.FindControl("btnStocked") as Button).Text = headers[4];
                (grdInventorySearched.HeaderRow.FindControl("btnPrice") as Button).Text = headers[5];
                (grdInventorySearched.HeaderRow.FindControl("btnCost") as Button).Text = headers[6];
                (grdInventorySearched.HeaderRow.FindControl("btnComments") as Button).Text = headers[7];
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
        //Downloading the search
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnDownload_Click";
            try
            {
                if (ViewState["listItems"] != null)
                {
                    string fileName = "ItemSearch_" + txtSearch.Text + ".xlsx";
                    object[] exportDetails = { ViewState["listItems"], fileName };
                    ExportFile EF = new ExportFile();
                    ExcelPackage packasgeDetails = EF.SearchedInventoryExport(exportDetails);
                    Response.Clear();
                    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.BinaryWrite(packasgeDetails.GetAsByteArray());
                    Response.End();
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
    }
}