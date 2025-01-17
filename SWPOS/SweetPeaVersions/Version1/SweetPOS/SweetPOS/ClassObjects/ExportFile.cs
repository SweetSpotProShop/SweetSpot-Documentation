using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace SweetPOS.ClassObjects
{
    public class ExportFile
    {
        //Used - Called from InventoryHomePage 1
        public ExcelPackage SearchedInventoryExport(object[] exportDetails)
        {
            string pathUser = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string pathDownload = (pathUser + "\\Downloads\\");
            FileInfo newFile = new FileInfo(pathDownload + exportDetails[1].ToString());
            using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            {
                //Creates a seperate sheet for each data table

                ExcelWorksheet searchExport = xlPackage.Workbook.Worksheets.Add("Items");
                // write to sheet     
                searchExport.Cells[1, 1].Value = "SKU";
                searchExport.Cells[1, 2].Value = "Description";
                searchExport.Cells[1, 3].Value = "Store";
                searchExport.Cells[1, 4].Value = "Quantity";
                searchExport.Cells[1, 5].Value = "Price";
                searchExport.Cells[1, 6].Value = "Cost";
                searchExport.Cells[1, 7].Value = "AdditionalInfo";
                int recordIndex = 2;
                foreach (Inventory item in (exportDetails[0]) as List<Inventory>)
                {
                    searchExport.Cells[recordIndex, 1].Value = item.varSku;
                    searchExport.Cells[recordIndex, 2].Value = item.varDescription;
                    searchExport.Cells[recordIndex, 3].Value = item.intStoreLocationID;
                    searchExport.Cells[recordIndex, 4].Value = item.intQuantity;
                    searchExport.Cells[recordIndex, 5].Value = item.fltPrice;
                    searchExport.Cells[recordIndex, 6].Value = item.fltAverageCost;
                    searchExport.Cells[recordIndex, 7].Value = item.varAdditionalInformation;
                    recordIndex++;
                }
                searchExport.Cells[searchExport.Dimension.Address].AutoFitColumns();
                return xlPackage;
            }
        }
        //This may get used at some point
        public void InventoryExport()
        {
            //string fileName = "Inventory_" + Convert.ToDateTime(CU.ConvertDate(DateTime.Now.ToLocalTime())).ToString("DD.MM.yyyy") + ".xlsx";
            //FileInfo newFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads\\TotalInventory.xlsx");
            ////This is the table that has all of the information lined up the way Caspio needs it to be
            //DataTable exportTable = new DataTable();

            //exportTable = exportAllInventory();
            //using (ExcelPackage xlPackage = new ExcelPackage(newFile))
            //{
            //    //Add page to the work book called inventory
            //    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Inventory");
            //    worksheet.Cells[1, 1].Value = "Date Created: " + Convert.ToDateTime(CU.ConvertDate(DateTime.Now.ToLocalTime())).ToString("DD.MM.yyyy");
            //    //Sets the column headers
            //    for (int i = 0; i < itemExportColumns.Count(); i++)
            //    {
            //        worksheet.Cells[2, i + 1].Value = itemExportColumns[i].ToString();
            //    }
            //    DataColumnCollection dcCollection = exportTable.Columns;
            //    int recordIndex = 3;
            //    foreach (DataRow row in exportTable.Rows)
            //    {
            //        worksheet.Cells[recordIndex, 1].Value = row[0].ToString();
            //        worksheet.Cells[recordIndex, 2].Value = row[1].ToString();
            //        worksheet.Cells[recordIndex, 3].Value = Convert.ToDouble(row[2].ToString());
            //        worksheet.Cells[recordIndex, 4].Value = row[3].ToString();
            //        worksheet.Cells[recordIndex, 5].Value = row[4].ToString();
            //        worksheet.Cells[recordIndex, 6].Value = row[5].ToString();
            //        worksheet.Cells[recordIndex, 7].Value = row[6].ToString();
            //        worksheet.Cells[recordIndex, 8].Value = row[7].ToString();
            //        worksheet.Cells[recordIndex, 9].Value = row[8].ToString();
            //        worksheet.Cells[recordIndex, 10].Value = Convert.ToDouble(row[9].ToString());
            //        worksheet.Cells[recordIndex, 11].Value = Convert.ToDouble(row[10].ToString());
            //        worksheet.Cells[recordIndex, 12].Value = Convert.ToDouble(row[11].ToString());
            //        worksheet.Cells[recordIndex, 13].Value = Convert.ToDouble(row[12].ToString());
            //        worksheet.Cells[recordIndex, 14].Value = Convert.ToDouble(row[13].ToString());
            //        worksheet.Cells[recordIndex, 15].Value = Convert.ToDouble(row[14].ToString());
            //        worksheet.Cells[recordIndex, 16].Value = row[15].ToString();
            //        worksheet.Cells[recordIndex, 17].Value = row[16].ToString();
            //        worksheet.Cells[recordIndex, 18].Value = row[17].ToString();
            //        worksheet.Cells[recordIndex, 19].Value = row[18].ToString();
            //        worksheet.Cells[recordIndex, 20].Value = row[19].ToString();
            //        worksheet.Cells[recordIndex, 21].Value = row[20].ToString();
            //        worksheet.Cells[recordIndex, 22].Value = row[21].ToString();
            //        worksheet.Cells[recordIndex, 23].Value = row[22].ToString();
            //        worksheet.Cells[recordIndex, 24].Value = Convert.ToDouble(row[23].ToString());
            //        worksheet.Cells[recordIndex, 25].Value = row[24].ToString();
            //        worksheet.Cells[recordIndex, 26].Value = row[25].ToString();
            //        recordIndex++;
            //    }
            //    HttpContext.Current.Response.Clear();
            //    HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            //    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //    HttpContext.Current.Response.BinaryWrite(xlPackage.GetAsByteArray());
            //    HttpContext.Current.Response.End();
            //}
        }
    }
}