using OfficeOpenXml;
using OfficeOpenXml.Style;
using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class SettingsHomePage : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InventoryManager IM = new InventoryManager();
        EmployeeManager EM = new EmployeeManager();
        TaxManager TM = new TaxManager();
        LocationManager LM = new LocationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "SettingsHomePage.aspx";
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
                    lblCurrentDate.Text = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())).ToString("yyyy-MM-dd");

                    if (txtImplimentationDate.Text == "")
                    {
                        txtImplimentationDate.Text = Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())).ToString("yyyy-MM-dd");
                    }
                    //Checks if the user is an SuDo
                    if (EM.ReturnSuDoIDForLocation(CU))
                    {
                        lblImportFiles.Visible = true;
                        tblImports.Visible = true;
                        //btnAddNewEmployee.Enabled = false;
                        //btnSaveTheTax.Enabled = false;
                        //btnAddBrand.Enabled = false;
                    }
                    if (!IsPostBack)
                    {
                        ddlProvince.DataSource = LM.ReturnProvinceDropDown(CU.currentStoreLocation.intCountryID);
                        ddlProvince.SelectedValue = CU.currentStoreLocation.intProvinceID.ToString();
                        ddlProvince.DataBind();
                        ddlTax.DataSource = TM.ReturnTaxListBasedOnDateAndProvinceForUpdate(CU.currentStoreLocation.intProvinceID, Convert.ToDateTime(lblCurrentDate.Text));
                        ddlTax.DataBind();
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
        protected void btnAddNewEmployee_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddNewEmployee_Click";
            try
            {
                //Change to Employee add new page
                Response.Redirect("EmployeeAddNew.aspx?employee=-10", true);
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
        protected void grdEmployeesSearched_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdEmployeesSearched_RowCommand";
            try
            {
                //Checks if the string is view profile
                if (e.CommandName == "ViewProfile")
                {
                    //Changes page to Employee Add New
                    Response.Redirect("EmployeeAddNew.aspx?employee=" + e.CommandArgument.ToString(), true);
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
        protected void btnEmployeeSearch_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnEmployeeSearch_Click";
            try
            {
                grdEmployeesSearched.Visible = true;
                //Binds the employee list to grid view
                if (EM.ReturnSuDoIDForLocation(CU))
                {
                    grdEmployeesSearched.DataSource = EM.ReturnEmployeeAllBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                }
                else
                {
                    grdEmployeesSearched.DataSource = EM.ReturnEmployeeLessThanAdminBasedOnText(txtSearch.Text, CU.terminal.intBusinessNumber);
                }
                grdEmployeesSearched.DataBind();
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

        protected void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMethod = "ddlProvince_SelectedIndexChanged";
            try
            {
                ddlTax.DataSource = TM.ReturnTaxListBasedOnDateAndProvinceForUpdate(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToDateTime(lblCurrentDate.Text));
                ddlTax.DataBind();
            }
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
        protected void ddlTax_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strMethod = "ddlTax_SelectedIndexChanged";
            try
            {
                List<ProvinceTax> t = TM.ReturnProvinceTaxListBasedOnDate(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToDateTime(lblCurrentDate.Text), CU.terminal.intBusinessNumber);
                foreach (var tax in t)
                {
                    if (tax.intTaxTypeID == Convert.ToInt32(ddlTax.SelectedValue))
                    {
                        lblCurrentDisplay.Text = tax.fltTaxRate.ToString("P");
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
        protected void btnSaveTheTax_Click(object sender, EventArgs e)
        {
            string strMethod = "btnSaveTheTax_Click";
            try
            {
                TM.InsertNewTaxRate(Convert.ToInt32(ddlProvince.SelectedValue), Convert.ToInt32(ddlTax.SelectedValue),
                    Convert.ToDateTime(txtImplimentationDate.Text), Convert.ToDouble(txtNewTaxRate.Text), CU.terminal.intBusinessNumber);
                txtImplimentationDate.Text = "";
                txtNewTaxRate.Text = "";
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

        protected void btnAddBrand_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnAddBrand_Click";
            try
            {
                if (txtNewBrandName.Text != "" && txtNewBrandConfirmName.Text != "")
                {
                    if (txtNewBrandName.Text.Equals(txtNewBrandConfirmName.Text))
                    {
                        IM.AddNewBrandName(txtNewBrandName.Text, CU.terminal.intBusinessNumber);
                        txtNewBrandName.Text = "";
                        txtNewBrandConfirmName.Text = "";
                    }
                    else
                    {
                        MessageBox.ShowMessage("The Brands Names do not match. "
                                + "Please review your entries.", this);
                    }
                }
                else
                {
                    MessageBox.ShowMessage("Both fields must match to create new Brand Name.", this);
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

        //Importing
        protected void btnImportInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnImportInventory_Click";
            try
            {
                FullInventoryUpload();
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
        private void FullInventoryUpload()
        {
            //Collects current method for error tracking
            string strMethod = "FullInventoryUpload";
            try
            {
                //Verifies file has been selected
                if (fupInventorySheet.HasFile)
                {
                    ImportFile IF = new ImportFile();
                    using (MemoryStream stream = new MemoryStream(fupInventorySheet.FileBytes))
                    //Lets the server know to use the excel package
                    using (ExcelPackage xlPackage = new ExcelPackage(stream))
                    {
                        bool bolHasNulls = false;
                        //Gets the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                        //Gets the row count
                        var rowCountInWorksheet = worksheet.Dimension.End.Row;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCountInWorksheet; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 3, 5, 6, 8, 9, 10, 11, 12, 13, 14 };
                            foreach (int column in cells)
                            {
                                //If there is no value in the column, proceed
                                if (worksheet.Cells[i, column].Value == null)
                                {
                                    worksheet.Cells[i, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, column].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    bolHasNulls = true;
                                }
                            }
                        }
                        string nameFile = (fupInventorySheet.FileName).Replace(".xlsx", "");

                        if (bolHasNulls)
                        {
                            //Sets the attributes and writes file                        
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment; filename=" + nameFile + "_NullCheck.xls");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            //Response.Write(xlPackage);
                            Response.BinaryWrite(xlPackage.GetAsByteArray());
                            Response.End();
                        }
                        else
                        {
                            //Calls method to import the requested file
                            DataTable errorList = IF.ErrorCheckInventoryList(fupInventorySheet, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                            if (errorList.Rows.Count != 0)
                            {
                                //Loop through the Excel sheet
                                for (int i = rowCountInWorksheet; i >= 2; i--)
                                {
                                    bool errorItemFound = false;
                                    //Loop through the error array
                                    for (int j = 0; j < errorList.Rows.Count; j++)
                                    {
                                        //Column 3 = SKU
                                        if (worksheet.Cells[i, 5].Value.ToString().Equals(errorList.Rows[j][0].ToString()))
                                        {
                                            errorItemFound = true;
                                            worksheet.Cells[i, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            //If brand caused an error
                                            if (Convert.ToInt32(errorList.Rows[j][1]) == 1)
                                            {
                                                worksheet.Cells[i, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 3].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If storeLocation caused an error
                                            if (Convert.ToInt32(errorList.Rows[j][2]) == 1)
                                            {
                                                worksheet.Cells[i, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 6].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            break;
                                        }
                                    }
                                    if (!errorItemFound)
                                    {
                                        worksheet.DeleteRow(i);
                                    }
                                }
                                worksheet.Cells[rowCountInWorksheet + 1, 1].Value = "Errors Found. The skus that are highlighted in red have an "
                                    + "issue with either their brand or model. This could be a spelling mistake or the brand "
                                    + "and/or model are not in the database.";

                                //Sets the attributes and writes file
                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment; filename=" + nameFile + "_Errors.xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                //Response.Write(xlPackage);
                                Response.BinaryWrite(xlPackage.GetAsByteArray());
                                Response.End();
                            }
                            else
                            {
                                MessageBox.ShowMessage("Importing Complete.", this);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.ShowMessage("No file Loaded.", this);
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
        protected void btnImportCustomer_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnImportCustomer_Click";
            try
            {
                FullCustomerUpLoad();
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
        private void FullCustomerUpLoad()
        {
            //Collects current method for error tracking
            string strMethod = "FullCustomerUpLoad";
            try
            {
                //Verifies file has been selected
                if (fupCustomerSheet.HasFile)
                {
                    ImportFile IF = new ImportFile();
                    //load the uploaded file into the memorystream
                    using (MemoryStream stream = new MemoryStream(fupCustomerSheet.FileBytes))
                    //Lets the server know to use the excel package
                    using (ExcelPackage xlPackage = new ExcelPackage(stream))
                    {
                        bool bolHasNulls = false;
                        //Gets the first worksheet in the workbook
                        ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];
                        //Gets the row count
                        var rowCountInWorksheet = worksheet.Dimension.End.Row;
                        //Beginning the loop for data gathering
                        for (int i = 2; i <= rowCountInWorksheet; i++) //Starts on 2 because excel starts at 1, and line 1 is headers
                        {
                            //Array of the cells that will need to be checked
                            int[] cells = { 2, 3, 9, 10, 12 };
                            foreach (int column in cells)
                            {
                                //If there is no value in the column, proceed
                                if (worksheet.Cells[i, column].Value == null)
                                {
                                    worksheet.Cells[i, column].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[i, column].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                    bolHasNulls = true;
                                }
                            }
                        }
                        string nameFile = (fupCustomerSheet.FileName).Replace(".xlsx", "");
                        if (bolHasNulls)
                        {
                            //Sets the attributes and writes file
                            Response.Clear();
                            Response.AddHeader("content-disposition", "attachment; filename=" + nameFile + "_NullCheck.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.BinaryWrite(xlPackage.GetAsByteArray());
                            Response.End();
                        }
                        else
                        {
                            //Calls method to import the requested file
                            DataTable errorList = IF.ErrorCheckCustomerList(fupCustomerSheet, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU.terminal.intBusinessNumber);
                            if (errorList.Rows.Count != 0)
                            {
                                //Loop through the Excel sheet
                                for (int i = rowCountInWorksheet; i >= 2; i--)
                                {
                                    bool errorItemFound = false;
                                    //Loop through the error array
                                    for (int j = 0; j < errorList.Rows.Count; j++)
                                    {
                                        //Column 3 = FirstName && LastName
                                        if (worksheet.Cells[i, 2].Value.ToString().Equals(errorList.Rows[j][0].ToString()) && worksheet.Cells[i, 3].Value.ToString().Equals(errorList.Rows[j][1].ToString()))
                                        {
                                            errorItemFound = true;
                                            worksheet.Cells[i, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            worksheet.Cells[i, 2].Style.Fill.BackgroundColor.SetColor(Color.Red);
                                            //If province caused an error
                                            if (Convert.ToInt32(errorList.Rows[j][2]) == 1)
                                            {
                                                worksheet.Cells[i, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 9].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            //If country caused an error
                                            if (Convert.ToInt32(errorList.Rows[j][3]) == 1)
                                            {
                                                worksheet.Cells[i, 10].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                                worksheet.Cells[i, 10].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                                            }
                                            break;
                                        }                                        
                                    }
                                    if (!errorItemFound)
                                    {
                                        worksheet.DeleteRow(i);
                                    }
                                }
                                worksheet.Cells[rowCountInWorksheet + 1, 1].Value = "Error: The Customer that is highlighted in red has an "
                                    + "Error in the data. This could be a spelling mistake with the province or the "
                                    + "country. This can include the province or country not in the database.";

                                //Sets the attributes and writes file
                                Response.Clear();
                                Response.AddHeader("content-disposition", "attachment; filename=" + nameFile + "_Errors.xlsx");
                                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                                Response.BinaryWrite(xlPackage.GetAsByteArray());
                                Response.End();
                            }
                            else
                            {
                                MessageBox.ShowMessage("Importing Complete.", this);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.ShowMessage("No file Loaded.", this);
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


        //Exporting
        protected void btnExportInventory_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnExportInventory_Click";
            try
            {
                //ExportFile EF = new ExportFile();
                //EF.InventoryExport();
                MessageBox.ShowMessage("This process is still in progress", this);
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
        //This needs date range attached***
        protected void btnExportReceipt_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnExportReceipt_Click";
            try
            {
                MessageBox.ShowMessage("This process is still in progress", this);
                ////Selects everything form the invoice table
                //DataTable dtim = new DataTable();
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    //Executing the SP
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da.Fill(dtim);
                //}
                //DataColumnCollection dcimHeaders = dtim.Columns;
                ////Selects everything form the invoice item table
                //DataTable dtii = new DataTable();
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    //Executing the SP
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da.Fill(dtii);
                //}
                //DataColumnCollection dciiHeaders = dtii.Columns;
                ////Selects everything form the invoice mop table
                //DataTable dtimo = new DataTable();
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    //Executing the SP
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da.Fill(dtimo);
                //}
                //DataColumnCollection dcimoHeaders = dtimo.Columns;
                ////Sets path and file name to download report to
                //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                //string pathDownload = (pathUser + "\\Downloads\\");
                //FileInfo newFile = new FileInfo(pathDownload + "InvoiceReport.xlsx");
                //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                //{
                //    //Creates a seperate sheet for each data table
                //    ExcelWorksheet invoiceMain = xlPackage.Workbook.Worksheets.Add("Invoice Main");
                //    ExcelWorksheet invoiceItems = xlPackage.Workbook.Worksheets.Add("Invoice Items");
                //    ExcelWorksheet invoiceMOPS = xlPackage.Workbook.Worksheets.Add("Invoice MOPS");
                //    // write to sheet                  

                //    //Export main invoice
                //    for (int i = 1; i <= dtim.Rows.Count; i++)
                //    {
                //        for (int j = 1; j < dtim.Columns.Count + 1; j++)
                //        {
                //            if (i == 1)
                //            {
                //                invoiceMain.Cells[i, j].Value = dcimHeaders[j - 1].ToString();
                //            }
                //            else
                //            {
                //                invoiceMain.Cells[i, j].Value = dtim.Rows[i - 1][j - 1];
                //            }
                //        }
                //    }
                //    //Export item invoice
                //    for (int i = 1; i <= dtii.Rows.Count; i++)
                //    {
                //        for (int j = 1; j < dtii.Columns.Count + 1; j++)
                //        {
                //            if (i == 1)
                //            {
                //                invoiceItems.Cells[i, j].Value = dciiHeaders[j - 1].ToString();
                //            }
                //            else
                //            {
                //                invoiceItems.Cells[i, j].Value = dtii.Rows[i - 1][j - 1];
                //            }
                //        }
                //    }
                //    //Export mop invoice
                //    for (int i = 1; i <= dtimo.Rows.Count; i++)
                //    {
                //        for (int j = 1; j < dtimo.Columns.Count + 1; j++)
                //        {
                //            if (i == 1)
                //            {
                //                invoiceMOPS.Cells[i, j].Value = dcimoHeaders[j - 1].ToString();
                //            }
                //            else
                //            {
                //                invoiceMOPS.Cells[i, j].Value = dtimo.Rows[i - 1][j - 1];
                //            }
                //        }
                //    }
                //    Response.Clear();
                //    Response.AddHeader("content-disposition", "attachment; filename=InvoiceReport.xlsx");
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.BinaryWrite(xlPackage.GetAsByteArray());
                //    Response.End();
                //}
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
        protected void btnExportCustomerEmail_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnExportCustomerEmail_Click";
            try
            {
                MessageBox.ShowMessage("This process is still in progress", this);
                ////Sets up database connection
                ////Selects everything form the invoice table
                //DataTable emailTable = new DataTable();
                //using (var da = new SqlDataAdapter(cmd))
                //{
                //    //Executing the SP
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da.Fill(emailTable);
                //}
                //DataColumnCollection headers = emailTable.Columns;
                ////Sets path and file name to download report to
                //string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                //string pathDownload = (pathUser + "\\Downloads\\");
                //FileInfo newFile = new FileInfo(pathDownload + "Email List.xlsx");
                //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
                //{
                //    //Creates a seperate sheet for each data table
                //    ExcelWorksheet emailExport = xlPackage.Workbook.Worksheets.Add("Email List");
                //    // write to sheet                  
                //    //Export main invoice
                //    for (int i = 1; i < emailTable.Rows.Count; i++)
                //    {
                //        for (int j = 1; j < emailTable.Columns.Count + 1; j++)
                //        {
                //            if (i == 1)
                //            {
                //                emailExport.Cells[i, j].Value = headers[j - 1].ToString();
                //            }
                //            else
                //            {
                //                emailExport.Cells[i, j].Value = emailTable.Rows[i - 1][j - 1];
                //            }
                //        }
                //    }
                //    Response.Clear();
                //    Response.AddHeader("content-disposition", "attachment; filename=Email List.xlsx");
                //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //    Response.BinaryWrite(xlPackage.GetAsByteArray());
                //    Response.End();
                //}
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

        protected void btnVendorManagement_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnVendorManagement_Click";
            try
            {
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
    }
}