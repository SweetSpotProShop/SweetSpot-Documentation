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
    public partial class HomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        ReportTools RT = new ReportTools();
        CurrentUserManager CUM = new CurrentUserManager();
        LocationManager LM = new LocationManager();
        CurrentUser CU;

        int totalSales = 0;
        double totalDiscounts = 0;
        double totalTradeIns = 0;
        double totalSubtotals = 0;
        double totalGST = 0;
        double totalPST = 0;
        double totalBalancePaid = 0;
        double totalMOPAmount = 0;
        string oldInvoice = string.Empty;
        string newInvoice = string.Empty;
        int currentRow = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strMethod = "Page_Load";
            Session["currentPage"] = "HomePage.aspx";
            try
            {
                if (Session["currentUser"] == null)
                {
                    Response.Redirect("SweetPea.aspx", true);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    if (!IsPostBack)
                    {
                        ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                        ddlStoreLocation.DataBind();
                        ddlStoreLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
                        ddlStoreLocation.Focus();
                        EmployeeManager EM = new EmployeeManager();
                        //This will need to be changed for securty access
                        if (EM.ReturnSuDoIDForLocation(CU))
                        {
                            ddlStoreLocation.Enabled = true;
                            //lblUserAccess.Text = "You have Admin access";
                            //lblUserAccess.Visible = true;
                        }
                    }
                    grdSameDaySales.DataSource = RT.ReturnSameDaySales(Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU);
                    grdSameDaySales.DataBind();
                }
            }
            catch(ThreadAbortException tae) { }
            catch(Exception ex)
            {
                ER.logError(ex, CU, Convert.ToString(Session["currentPage"]), strMethod, this);
                MessageBox.ShowMessage("An Error has occurred and been logged. "
                    + "If you continue to receive this message please contact "
                    + "your system administrator.", this);
            }
        }
        protected void grdSameDaySales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Problems with looping 
            //Collects current method for error tracking
            string strMethod = "grdSameDaySales_RowDataBound";
            //Current method does nothing
            try
            {
                LinkButton lb = (LinkButton)e.Row.FindControl("lbtnReceiptNumber");
                if (lb != null)
                {
                    oldInvoice = lb.Text;
                    if (oldInvoice != newInvoice)
                    {
                        //This is where the totals at the bottom are calculated
                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //This triggers when the row is not the footer(Not the end)
                            //Need to determine if the cell is empty
                            totalSales += 1;
                            totalDiscounts += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltDiscountTotal"));
                            totalTradeIns += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltTradeInTotal"));
                            totalSubtotals += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltSubTotal"));
                            totalGST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltGovernmentTax"));
                            totalPST += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltProvincialTax"));
                            totalBalancePaid += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltReceiptTotal"));
                        }
                        newInvoice = oldInvoice;
                    }
                }
                //This is separate because every line will have a MOP on it
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    totalMOPAmount += Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "fltAmountPaid"));
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    //Triggers when the row is the footer(End)
                    e.Row.Cells[1].Text = totalSales.ToString();
                    e.Row.Cells[3].Text = String.Format("{0:C}", totalDiscounts);
                    e.Row.Cells[4].Text = String.Format("{0:C}", totalTradeIns);
                    e.Row.Cells[5].Text = String.Format("{0:C}", totalSubtotals);
                    e.Row.Cells[6].Text = String.Format("{0:C}", totalGST);
                    e.Row.Cells[7].Text = String.Format("{0:C}", totalPST);
                    e.Row.Cells[8].Text = String.Format("{0:C}", totalBalancePaid);
                    e.Row.Cells[10].Text = String.Format("{0:C}", totalMOPAmount);
                }
                currentRow++;
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
        protected void grdSameDaySales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdSameDaySales_RowCommand";
            try
            {
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int tranType = Convert.ToInt32(((TextBox)grdSameDaySales.Rows[index].Cells[11].FindControl("txtTransactionTypeID")).Text);
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
        protected void grdSameDaySales_DataBound(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdSameDaySales_DataBound";
            try
            {
                for (int rowIndex = grdSameDaySales.Rows.Count - 2; rowIndex >= 0; rowIndex--)
                {
                    GridViewRow gvRow = grdSameDaySales.Rows[rowIndex];
                    GridViewRow gvPreviousRow = grdSameDaySales.Rows[rowIndex + 1];
                    LinkButton lbtnRow = (LinkButton)gvRow.FindControl("lbtnReceiptNumber");
                    LinkButton lbtnPreviousRow = (LinkButton)gvPreviousRow.FindControl("lbtnReceiptNumber");
                    for (int cellCount = 0; cellCount < gvRow.Cells.Count; cellCount++)
                    {
                        if (lbtnRow.Text == lbtnPreviousRow.Text)
                        {
                            if (gvRow.Cells[cellCount].Text == gvPreviousRow.Cells[cellCount].Text)
                            {
                                if (gvPreviousRow.Cells[cellCount].RowSpan < 2)
                                {
                                    gvRow.Cells[cellCount].RowSpan = 2;
                                }
                                else
                                {
                                    gvRow.Cells[cellCount].RowSpan = gvPreviousRow.Cells[cellCount].RowSpan + 1;
                                }
                                gvPreviousRow.Cells[cellCount].Visible = false;
                            }
                        }
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
    }
}