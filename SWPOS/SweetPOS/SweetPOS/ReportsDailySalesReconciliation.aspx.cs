using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class ReportsDailySalesReconciliation : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        LocationManager L = new LocationManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "ReportsDailySalesReconciliation";
            try
            {
                //checks if the user has logged in
                if (Session["currentUser"] == null)
                {
                    //Go back to Login to log in
                    Server.Transfer("SweetPea.aspx", true);
                }
                else
                {
                    CU = (CurrentUser)Session["currentUser"];
                    btnPrint.Focus();
                    if (!IsPostBack)
                    {
                        //Gathering the start and end dates

                        object[] reportCriteria = (object[])Session["reportCriteria"];
                        DateTime[] reportDates = (DateTime[]) reportCriteria[0];
                        lblReconciliationDates.Text = "Cashout report for: " + reportDates[0].ToString("d") + " to " + reportDates[1].ToString("d") + " for " + L.ReturnLocation(Convert.ToInt32(reportCriteria[1]), CU.terminal.intBusinessNumber)[0].varStoreName;
                        grdReconciliationDate.DataSource = SRM.ReturnSelectedRangeSalesReconciliation(reportCriteria);
                        grdReconciliationDate.DataBind();
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
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnDownload_Click";
            try
            {
                //Gathering the start and end dates
                ////DateTime startDate = DateTime.Parse(Request.QueryString["from"].ToString());
                ////DateTime endDate = DateTime.Parse(Request.QueryString["to"].ToString());
                ////DateTime[] rptDate = { startDate, endDate };
                ////int locationID = Convert.ToInt32(Request.QueryString["location"].ToString());
                ////object[] passing = { rptDate, locationID };
                ////dt = R.ReturnCashoutsForSelectedDates(passing);

                //////Sets path and file name to download report to
                ////string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                ////string pathDownload = (pathUser + "\\Downloads\\");
                ////string loc = L.locationName(Convert.ToInt32(Request.QueryString["location"].ToString()));
                ////string fileName = "CashOut Report by Date - " + loc + ".xlsx";

                ////FileInfo newFile = new FileInfo(pathDownload + fileName);
                ////using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                ////{
                ////    //Creates a seperate sheet for each data table
                ////    ExcelWorksheet salesExport = xlPackage.Workbook.Worksheets.Add("CashOut");
                ////    //Title
                ////    salesExport.Cells[1, 1, 1, 11].Merge = true;
                ////    salesExport.Cells[1, 1].Value = lblDates.Text;
                ////    salesExport.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ////    salesExport.Cells[1, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ////    //Headers
                ////    salesExport.Cells[2, 1].Value = "Date";
                ////    salesExport.Cells[2, 3].Value = "Trade-In";
                ////    salesExport.Cells[2, 4].Value = "Gift Card";
                ////    salesExport.Cells[2, 5].Value = "Cash";
                ////    salesExport.Cells[2, 6].Value = "Debit";
                ////    salesExport.Cells[2, 7].Value = "Mastercard";
                ////    salesExport.Cells[2, 8].Value = "Visa";
                ////    salesExport.Cells[2, 9].Value = "Over/Short";
                ////    salesExport.Cells[2, 10].Value = "Processed";
                ////    salesExport.Cells[2, 11].Value = "Finalized";
                ////    salesExport.Cells[2, 1, 2, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);

                ////    //salesExport.Cells[2, 2].Value = "Sales Dollars";
                ////    int recordIndex = 3;
                ////    foreach (DataRow row in dt.Rows)
                ////    {
                ////        //Date
                ////        DateTime d = (DateTimerow[0];
                ////        salesExport.Cells[recordIndex, 1, recordIndex + 1, 1].Merge = true;
                ////        salesExport.Cells[recordIndex, 1].Value = d.ToString("d");
                ////        salesExport.Cells[recordIndex, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ////        salesExport.Cells[recordIndex, 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ////        //Receipt over Sale
                ////        salesExport.Cells[recordIndex, 2].Value = "Receipts:";
                ////        salesExport.Cells[recordIndex + 1, 2].Value = "Sales:";
                ////        //TradeIn
                ////        salesExport.Cells[recordIndex, 3].Value = Convert.ToDouble(row[3].ToString());
                ////        salesExport.Cells[recordIndex + 1, 3].Value = Convert.ToDouble(row[2].ToString());
                ////        //GiftCard
                ////        salesExport.Cells[recordIndex, 4].Value = Convert.ToDouble(row[5].ToString());
                ////        salesExport.Cells[recordIndex + 1, 4].Value = Convert.ToDouble(row[4].ToString());
                ////        //Cash
                ////        salesExport.Cells[recordIndex, 5].Value = Convert.ToDouble(row[7].ToString());
                ////        salesExport.Cells[recordIndex + 1, 5].Value = Convert.ToDouble(row[6].ToString());
                ////        //Debit
                ////        salesExport.Cells[recordIndex, 6].Value = Convert.ToDouble(row[9].ToString());
                ////        salesExport.Cells[recordIndex + 1, 6].Value = Convert.ToDouble(row[8].ToString());
                ////        //Mastercard
                ////        salesExport.Cells[recordIndex, 7].Value = Convert.ToDouble(row[11].ToString());
                ////        salesExport.Cells[recordIndex + 1, 7].Value = Convert.ToDouble(row[10].ToString());
                ////        //Visa
                ////        salesExport.Cells[recordIndex, 8].Value = Convert.ToDouble(row[13].ToString());
                ////        salesExport.Cells[recordIndex + 1, 8].Value = Convert.ToDouble(row[12].ToString());
                ////        //Over/Short
                ////        salesExport.Cells[recordIndex, 9, recordIndex + 1, 9].Merge = true;
                ////        salesExport.Cells[recordIndex, 9].Value = Convert.ToDouble(row[14].ToString());
                ////        salesExport.Cells[recordIndex, 9].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ////        salesExport.Cells[recordIndex, 9].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ////        //Processed
                ////        salesExport.Cells[recordIndex, 10, recordIndex + 1, 10].Merge = true;
                ////        salesExport.Cells[recordIndex, 10].Value = row[15].ToString();
                ////        salesExport.Cells[recordIndex, 10].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ////        salesExport.Cells[recordIndex, 10].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ////        //Finalized
                ////        salesExport.Cells[recordIndex, 11, recordIndex + 1, 11].Merge = true;
                ////        salesExport.Cells[recordIndex, 11].Value = row[16].ToString();
                ////        salesExport.Cells[recordIndex, 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ////        salesExport.Cells[recordIndex, 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                ////        //Border
                ////        salesExport.Cells[recordIndex, 1, recordIndex + 1, 11].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Medium);
                ////        //Converting numbers to numbers
                ////        salesExport.Cells[recordIndex, 3, recordIndex + 1, 9].Style.Numberformat.Format = "#,##0.00";
                ////        //Autofit columns
                ////        salesExport.Cells[2, 1, recordIndex + 1, 11].AutoFitColumns();
                ////        //Incrementng the row
                ////        recordIndex = recordIndex + 2;
                ////    }
                ////    Response.Clear();
                ////    Response.AddHeader("content-disposition", "attachment; filename=\"" + fileName + "\"");
                ////    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                ////    Response.BinaryWrite(xlPackage.GetAsByteArray());
                ////    Response.End();
                ////}
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
        protected void grdReconciliationDate_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdReconciliationDate_RowCommand";
            try
            {
                if (e.CommandName == "EditDailyReconciliation")
                {
                    object[] reportCriteria = (object[])Session["reportCriteria"];
                    var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    nameValues.Set("reconcilaitionDate", e.CommandArgument.ToString());
                    nameValues.Set("storeLocation", reportCriteria[1].ToString());
                    //Changes to the Reports Cash Out page
                    Response.Redirect("DailySalesReconciliation.aspx?" + nameValues, true);
                }
                else if (e.CommandName == "FinalizeReconciliation")
                {
                    SRM.FinalizeDailyReconciliation(Convert.ToInt32(e.CommandArgument.ToString()), CU.terminal.intBusinessNumber);
                    object[] reportCriteria = (object[])Session["reportCriteria"];
                    grdReconciliationDate.DataSource = SRM.ReturnSelectedRangeSalesReconciliation(reportCriteria);
                    grdReconciliationDate.DataBind();
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
        protected void grdReconciliationDate_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "grdReconciliationDate_RowDataBound";
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "bitIsFinalized")) == 1)
                    {
                        Button edit = (Button)e.Row.FindControl("btnEdit");
                        edit.Enabled = false;
                        Button final = (Button)e.Row.FindControl("btnFinalize");
                        final.Enabled = false;
                    }
                    Label americanExpress = (Label)e.Row.FindControl("lblTradeInBalance");
                    Label cash = (Label)e.Row.FindControl("lblCashBalance");
                    Label cheque = (Label)e.Row.FindControl("lblGiftCardBalance");
                    Label debit = (Label)e.Row.FindControl("lblDebitBalance");
                    Label discover = (Label)e.Row.FindControl("lblCashBalance");
                    Label giftCard = (Label)e.Row.FindControl("lblGiftCardBalance");
                    Label master = (Label)e.Row.FindControl("lblMastercardBalance");
                    Label trade = (Label)e.Row.FindControl("lblTradeInBalance");
                    Label visa = (Label)e.Row.FindControl("lblVisaBalance");

                    if (americanExpress.Text == "Discrepancy")
                    {
                        americanExpress.ForeColor = System.Drawing.Color.Red;
                    }
                    if (cash.Text == "Discrepancy")
                    {
                        cash.ForeColor = System.Drawing.Color.Red;
                    }
                    if (cheque.Text == "Discrepancy")
                    {
                        cheque.ForeColor = System.Drawing.Color.Red;
                    }
                    if (debit.Text == "Discrepancy")
                    {
                        debit.ForeColor = System.Drawing.Color.Red;
                    }
                    if (discover.Text == "Discrepancy")
                    {
                        discover.ForeColor = System.Drawing.Color.Red;
                    }
                    if (giftCard.Text == "Discrepancy")
                    {
                        giftCard.ForeColor = System.Drawing.Color.Red;
                    }
                    if (master.Text == "Discrepancy")
                    {
                        master.ForeColor = System.Drawing.Color.Red;
                    }
                    if (trade.Text == "Discrepancy")
                    {
                        trade.ForeColor = System.Drawing.Color.Red;
                    }
                    if (visa.Text == "Discrepancy")
                    {
                        visa.ForeColor = System.Drawing.Color.Red;
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