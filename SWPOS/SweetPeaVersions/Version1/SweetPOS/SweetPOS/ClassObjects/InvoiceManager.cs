using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace SweetPOS.ClassObjects
{
    public class InvoiceManager
    {
        DatabaseCalls DBC = new DatabaseCalls();

        private List<Invoice> InsertIntoInvoiceCurrent(Invoice invoice, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoiceCurrent VALUES(@varInvoiceNumber, "
                + "@dtmInvoiceCreationDate, @dtmInvoiceCreationTime, @intCustomerID, "
                + "@intEmployeeID, @intStoreLocationID, @intTerminalID, @fltCostTotal, "
                + "@intTransactionTypeID, @varInvoiceComments)";

            object[][] parms =
            {
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@dtmInvoiceCreationDate", invoice.dtmInvoiceCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceCreationTime", invoice.dtmInvoiceCreationTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", invoice.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", invoice.intTerminalID },
                new object[] { "@fltCostTotal", invoice.fltCostTotal },
                new object[] { "@intTransactionTypeID", invoice.intTransactionTypeID },
                new object[] { "@varInvoiceComments", invoice.varInvoiceComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            return ReturnNewInvoiceFromInvoiceNumber(parms, businessNumber);
        }
        private List<Invoice> ReturnNewInvoiceFromInvoiceNumber(object[][] parms, int businessNumber)
        {
            string sqlCmd = "SELECT intInvoiceID FROM tbl" + businessNumber + "InvoiceCurrent WHERE "
                + "varInvoiceNumber = @varInvoiceNumber AND dtmInvoiceCreationDate "
                + "= @dtmInvoiceCreationDate AND dtmInvoiceCreationTime = "
                + "@dtmInvoiceCreationTime AND intCustomerID = @intCustomerID  AND "
                + "intEmployeeID = @intEmployeeID AND intStoreLocationID = "
                + "@intStoreLocationID AND intTerminalID = @intTerminalID AND fltCostTotal = @fltCostTotal AND "
                + "intTransactionTypeID = @intTransactionTypeID AND "
                + "varInvoiceComments = @varInvoiceComments";
            return ReturnInvoiceCurrent(DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms), businessNumber);
        }
        private List<Invoice> ConvertFromDataTableToInvoiceCurrent(DataTable dt, int businessNumber)
        {
            EmployeeManager EM = new EmployeeManager();
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            List<Invoice> r = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                dtmInvoiceCreationDate = row.Field<DateTime>("dtmInvoiceCreationDate"),
                dtmInvoiceCreationTime = row.Field<DateTime>("dtmInvoiceCreationTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostTotal = row.Field<double>("fltCostTotal"),
                lstInvoiceItem = ReturnInvoiceInventoryCurrentInPurchaseCart(row.Field<int>("intInvoiceID"), businessNumber),
                lstInvoicePayment = ReturnInvoicePaymentCurrentInPurchaseCart(row.Field<int>("intInvoiceID"), businessNumber),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varInvoiceComments = row.Field<string>("varInvoiceComments")
            }).ToList();
            return r;
        }
        private List<Invoice> ConvertFromDataTableToInvoice(DataTable dt, int businessNumber)
        {
            EmployeeManager EM = new EmployeeManager();
            CustomerManager CM = new CustomerManager();
            LocationManager LM = new LocationManager();
            List<Invoice> r = dt.AsEnumerable().Select(row =>
            new Invoice
            {
                intInvoiceID = row.Field<int>("intInvoiceID"),
                varInvoiceNumber = row.Field<string>("varInvoiceNumber"),
                dtmInvoiceCompletionDate = row.Field<DateTime>("dtmInvoiceCompletionDate"),
                dtmInvoiceCompletionTime = row.Field<DateTime>("dtmInvoiceCompletionTime"),
                customer = CM.ReturnCustomer(row.Field<int>("intCustomerID"), businessNumber)[0],
                employee = EM.ReturnEmployee(row.Field<int>("intEmployeeID"), businessNumber)[0],
                storeLocation = LM.ReturnLocation(row.Field<int>("intStoreLocationID"), businessNumber)[0],
                intTerminalID = row.Field<int>("intTerminalID"),
                fltCostTotal = row.Field<double>("fltCostTotal"),
                lstInvoiceItem = ReturnInvoiceInventoryInPurchaseCart(row.Field<int>("intInvoiceID"), businessNumber),
                lstInvoicePayment = ReturnInvoicePaymentInPurchaseCart(row.Field<int>("intInvoiceID"), businessNumber),
                intTransactionTypeID = row.Field<int>("intTransactionTypeID"),
                varInvoiceComments = row.Field<string>("varInvoiceComments")
            }).ToList();
            return r;
        }

        public Invoice SaveNewInvoiceTotal(Invoice invoice)
        {
            double cost = 0;
            foreach (InvoiceItem invoiceItem in invoice.lstInvoiceItem)
            {
                cost += invoiceItem.fltItemCost * invoiceItem.intItemQuantity;
            }
            invoice.fltCostTotal = cost;
            return invoice;
        }

        public List<Invoice> CreateNewPurchaseInvoice(int customerID, DateTime createDateTime, CurrentUser cu)
        {
            CustomerManager CM = new CustomerManager();
            Invoice invoice = new Invoice();
            invoice.varInvoiceNumber = ReturnNextInvoiceNumberForNewInvoice(cu);
            invoice.dtmInvoiceCreationDate = createDateTime;
            invoice.dtmInvoiceCreationTime = createDateTime;
            invoice.customer = CM.ReturnCustomer(customerID, cu.terminal.intBusinessNumber)[0];
            invoice.employee = cu.employee;
            invoice.storeLocation = cu.currentStoreLocation;
            invoice.intTerminalID = cu.terminal.intTerminalID;
            invoice.fltCostTotal = 0;
            invoice.intTransactionTypeID = 7;
            invoice.varInvoiceComments = "";
            return InsertIntoInvoiceCurrent(invoice, cu.terminal.intBusinessNumber);
        }
        public List<Invoice> ReturnInvoiceCurrent(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, dtmInvoiceCreationDate, CAST(dtmInvoiceCreationTime AS "
                + "DATETIME) AS dtmInvoiceCreationTime, intCustomerID, intEmployeeID, intStoreLocationID, intTerminalID, fltCostTotal, "
                + "intTransactionTypeID, varInvoiceComments FROM tbl" + businessNumber + "InvoiceCurrent WHERE intInvoiceID "
                + "= @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoiceCurrent(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Invoice> ReturnInvoice(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT intInvoiceID, varInvoiceNumber, dtmInvoiceCompletionDate, CAST(dtmInvoiceCompletionTime AS DATETIME) "
                + "AS dtmInvoiceCompletionTime, intCustomerID, intEmployeeID, intStoreLocationID, intTerminalID, fltCostTotal, "
                + "intTransactionTypeID, varInvoiceComments FROM tbl" + businessNumber + "Invoice WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                 new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoice(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms), businessNumber);
        }
        public List<Invoice> CalculateInvoiceTotal(Invoice invoice, int businessNumber)
        {
            UpdateInvoiceCurrent(SaveNewInvoiceTotal(ReturnInvoiceCurrent(invoice.intInvoiceID, businessNumber)[0]), businessNumber);
            return ReturnInvoiceCurrent(invoice.intInvoiceID, businessNumber);
        }

        private List<InvoiceItem> ReturnInvoiceInventoryCurrentInPurchaseCart(int invoiceID, int businessNumber)
        {
            return ConvertFromDataTableToInvoiceItem(ReturnInvoiceInventoryCurrentDataTable(invoiceID, businessNumber));
        }
        private List<InvoiceItem> ReturnInvoiceInventoryInPurchaseCart(int invoiceID, int businessNumber)
        {
            return ConvertFromDataTableToInvoiceItem(ReturnInvoiceInventoryDataTable(invoiceID, businessNumber));
        }
        private List<InvoiceItem> ConvertFromDataTableToInvoiceItem(DataTable dt)
        {
            List<InvoiceItem> invoiceItem = dt.AsEnumerable().Select(row =>
            new InvoiceItem
            {
                intInvoiceItemID = row.Field<int>("intInvoiceItemID"),
                intInvoiceID = row.Field<int>("intInvoiceID"),
                intInventoryID = row.Field<int>("intInventoryID"),
                varItemSku = row.Field<string>("varItemSku"),
                intItemQuantity = row.Field<int>("intItemQuantity"),
                fltItemCost = row.Field<double>("fltItemCost"),
                varItemDescription = row.Field<string>("varItemDescription")
            }).ToList();
            return invoiceItem;
        }

        private List<InvoicePayment> ReturnInvoicePaymentCurrentInPurchaseCart(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT IPC.intPaymentID, IPC.intInvoiceID, IPC.intMethodOfPaymentID, MOP.varMethodOfPaymentName, "
                + "IPC.fltAmountReceived, intChequeNumber FROM tbl" + businessNumber + "InvoicePaymentCurrent IPC JOIN "
                + "tblMethodOfPayment MOP ON MOP.intMethodOfPaymentID = IPC.intMethodOfPaymentID WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoicePayment(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<InvoicePayment> ReturnInvoicePaymentInPurchaseCart(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT IPC.intPaymentID, IPC.intInvoiceID, IPC.intMethodOfPaymentID, MOP.varMethodOfPaymentName, "
                + "IPC.fltAmountReceived, IPC.intChequeNumber FROM tbl" + businessNumber + "InvoicePayment IPC JOIN "
                + "tblMethodOfPayment MOP ON MOP.intMethodOfPaymentID = IPC.intMethodOfPaymentID WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            return ConvertFromDataTableToInvoicePayment(DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms));
        }
        private List<InvoicePayment> ConvertFromDataTableToInvoicePayment(DataTable dt)
        {
            List<InvoicePayment> invoicePayment = dt.AsEnumerable().Select(row =>
            new InvoicePayment
            {
                intPaymentID = row.Field<int>("intPaymentID"),
                intInvoiceID = row.Field<int>("intInvoiceID"),
                intMethodOfPaymentID = row.Field<int>("intMethodOfPaymentID"),
                varMethodOfPaymentName = row.Field<string>("varMethodOfPaymentName"),
                fltAmountReceived = row.Field<double>("fltAmountReceived"),
                intChequeNumber = row.Field<int>("intChequeNumber")
            }).ToList();
            return invoicePayment;
        }

        private DataTable GatherOpenBulkPurchases(CurrentUser cu)
        {
            string sqlCmd = "SELECT IC.intInvoiceID, IC.dtmInvoiceCreationDate, IC.varInvoiceNumber, CONCAT(C.varLastName, ', ', "
                + "C.varFirstName) AS varCustomerName, CONCAT(E.varLastName, ', ', E.varFirstName) AS varEmployeeName, IC.fltCostTotal, "
                + "IC.varInvoiceComments FROM tbl" + cu.terminal.intBusinessNumber + "InvoiceCurrent IC JOIN tbl"
                + cu.terminal.intBusinessNumber + "Customer C ON C.intCustomerID = IC.intCustomerID JOIN tbl"
                + cu.terminal.intBusinessNumber + "Employee E ON E.intEmployeeID = IC.intEmployeeID WHERE IC.intStoreLocationID = "
                + "@intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }

        public DataTable ReturnInvoiceInventoryCurrentDataTable(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, varItemSku, intItemQuantity, fltItemCost, "
                + "varItemDescription FROM tbl" + businessNumber + "InvoiceItemCurrent WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnInvoiceInventoryDataTable(int invoiceID, int businessNumber)
        {
            string sqlCmd = "SELECT intInvoiceItemID, intInvoiceID, intInventoryID, varItemSku, intItemQuantity, fltItemCost, "
                + "varItemDescription FROM tbl" + businessNumber + "InvoiceItem WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            return DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public DataTable ReturnOpenBulkPurchases(CurrentUser cu)
        {
            return GatherOpenBulkPurchases(cu);
        }

        private string ReturnNextInvoiceNumberForNewInvoice(CurrentUser cu)
        {
            string sqlCmd = "SELECT intInvoiceStoreLocationID, intInvoiceNumberSystem, CONCAT(varStoreCodeInvoice, CASE WHEN "
                + "LEN(CAST(intInvoiceNumberSystem AS INT)) < 6 THEN RIGHT(RTRIM('000000' + CAST(intInvoiceNumberSystem AS "
                + "VARCHAR(6))),6) ELSE CAST(intInvoiceNumberSystem AS VARCHAR(MAX)) END) AS varInvoiceNumber FROM tbl" 
                + cu.terminal.intBusinessNumber + "StoredInvoiceNumber WHERE intStoreLocationID = @intStoreLocationID";
            object[][] parms =
            {
                new object[] { "@intStoreLocationID", cu.currentStoreLocation.intStoreLocationID }
            };
            //Creates the new receipt number
            CreateInvoiceNumberForNextInvoice(DBC.MakeDatabaseCallToReturnSecondColumnAsInt(sqlCmd, parms) + 1, cu);
            return DBC.MakeDatabaseCallToReturnThirdColumnAsString(sqlCmd, parms);
        }

        public int AddPurchaseTradeInToInventoryTable(InvoiceItem invoiceItem, DateTime createDateTime, CurrentUser cu)
        {
            InventoryManager IM = new InventoryManager();
            return IM.NewPurchaseTradeIn(invoiceItem, createDateTime, cu);
        }

        public bool VerifyPaymentOnInvoice(int invoiceID, int businessNumber)
        {
            bool mopsAdded = false;
            string sqlCmd = "SELECT COUNT(intPaymentID) AS countPayment FROM tbl" + businessNumber + "InvoicePaymentCurrent "
                + "WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoiceID }
            };

            if (DBC.MakeDatabaseCallToReturnInt(sqlCmd, parms) > 0)
            {
                mopsAdded = true;
            }
            return mopsAdded;
        }

        private void CreateInvoiceNumberForNextInvoice(int invoiceNumber, CurrentUser cu)
        {
            string sqlCmd = "UPDATE tbl" + cu.terminal.intBusinessNumber + "StoredInvoiceNumber SET intInvoiceNumberSystem = "
                + "@invoiceNumber WHERE intStoreLocationID = @storeLocationID";
            object[][] parms =
            {
                new object[] { "@invoiceNumber", invoiceNumber },
                new object[] { "@storeLocationID", cu.currentStoreLocation.intStoreLocationID }

            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void UpdateInvoiceCurrent(Invoice invoice, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "InvoiceCurrent SET intCustomerID = @intCustomerID, intEmployeeID = "
                + "@intEmployeeID, intTerminalID = @intTerminalID, fltCostTotal = @fltCostTotal, varInvoiceComments = "
                + "@varInvoiceComments WHERE intInvoiceID = @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intTerminalID", invoice.intTerminalID },
                new object[] { "@fltCostTotal", invoice.fltCostTotal },
                new object[] { "@varInvoiceComments", invoice.varInvoiceComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertInvoiceIntoInvoiceTable(Invoice invoice, DateTime completeDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "Invoice VALUES(@intInvoiceID, @varInvoiceNumber, "
                + "@dtmInvoiceCreationDate, @dtmInvoiceCreationTime, @dtmInvoiceCompletionDate, "
                + "@dtmInvoiceCompletionTime, @intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, "
                + "@fltCostTotal, @intTransactionTypeID, @varInvoiceComments)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@dtmInvoiceCreationDate", invoice.dtmInvoiceCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceCreationTime", invoice.dtmInvoiceCreationTime.ToString("HH:mm:ss") },
                new object[] { "@dtmInvoiceCompletionDate", completeDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceCompletionTime", completeDateTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", invoice.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", invoice.intTerminalID },
                new object[] { "@fltCostTotal", invoice.fltCostTotal },
                new object[] { "@intTransactionTypeID", invoice.intTransactionTypeID },
                new object[] { "@varInvoiceComments", invoice.varInvoiceComments }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertInvoiceItemIntoInvoiceItemTable(Invoice invoice, CurrentUser cu)
        {
            InventoryManager IM = new InventoryManager();
            foreach (InvoiceItem item in invoice.lstInvoiceItem)
            {
                string sqlCmd = "INSERT INTO tbl" + cu.terminal.intBusinessNumber + "InvoiceItem VALUES(@intInvoiceItemID, @intInvoiceID, "
                    + "@intInventoryID, @varItemSku, @intItemQuantity, @fltItemCost, @varItemDescription)";

                object[][] parms =
                {
                    new object[] { "@intInvoiceItemID", item.intInvoiceItemID },
                    new object[] { "@intInvoiceID", item.intInvoiceID },
                    new object[] { "@intInventoryID", item.intInventoryID },
                    new object[] { "@varItemSku", item.varItemSku },
                    new object[] { "@intItemQuantity", item.intItemQuantity },
                    new object[] { "@fltItemCost", item.fltItemCost },
                    new object[] { "@varItemDescription", item.varItemDescription }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
                IM.SetUpPurchasesForProcessing(item, cu);
                IM.UpdateInventoryFromInvoiceItem(item, cu);
            }
        }
        private void InsertInvoicePaymentIntoInvoicePaymentTable(Invoice invoice, int businessNumber)
        {
            foreach (InvoicePayment payment in invoice.lstInvoicePayment)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoicePayment VALUES(@intPaymentID, "
                    + "@intInvoiceID, @intMethodOfPaymentID, @fltAmountReceived, @intChequeNumber)";

                object[][] parms =
                {
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@intInvoiceID", payment.intInvoiceID },
                    new object[] { "@intMethodOfPaymentID", payment.intMethodOfPaymentID },
                    new object[] { "@fltAmountReceived", payment.fltAmountReceived },
                    new object[] { "@intChequeNumber", payment.intChequeNumber }

                };
                DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            }
        }
        private void InsertInvoiceIntoInvoiceVoidCancel(Invoice invoice, DateTime cancelDateTime, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoiceVoidCancel VALUES(@intInvoiceID, @varInvoiceNumber, "
                + "@dtmInvoiceCreationDate, @dtmInvoiceCreationTime, @dtmInvoiceVoidCancelDate, "
                + "@dtmInvoiceVoidCancelTime, @intCustomerID, @intEmployeeID, @intStoreLocationID, @intTerminalID, "
                + "@fltCostTotal, @intTransactionTypeID, @varInvoiceComments, @bitIsInvoiceVoided, "
                + "@bitIsInvoiceCancelled)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID },
                new object[] { "@varInvoiceNumber", invoice.varInvoiceNumber },
                new object[] { "@dtmInvoiceCreationDate", invoice.dtmInvoiceCreationDate.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceCreationTime", invoice.dtmInvoiceCreationTime.ToString("HH:mm:ss") },
                new object[] { "@dtmInvoiceVoidCancelDate", cancelDateTime.ToString("yyyy-MM-dd") },
                new object[] { "@dtmInvoiceVoidCancelTime", cancelDateTime.ToString("HH:mm:ss") },
                new object[] { "@intCustomerID", invoice.customer.intCustomerID },
                new object[] { "@intEmployeeID", invoice.employee.intEmployeeID },
                new object[] { "@intStoreLocationID", invoice.storeLocation.intStoreLocationID },
                new object[] { "@intTerminalID", invoice.intTerminalID },
                new object[] { "@fltCostTotal", invoice.fltCostTotal },
                new object[] { "@intTransactionTypeID", invoice.intTransactionTypeID },
                new object[] { "@varInvoiceComments", invoice.varInvoiceComments },
                new object[] { "@bitIsInvoiceVoided", invoice.bitIsInvoiceVoided },
                new object[] { "@bitIsInvoiceCancelled", invoice.bitIsInvoiceCancelled }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void InsertInvoiceItemIntoInvoiceItemVoidCancel(Invoice invoice, int businessNumber)
        {
            foreach (InvoiceItem item in invoice.lstInvoiceItem)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoiceItemVoidCancel VALUES(@intInvoiceItemID, @intInvoiceID, "
                    + "@intInventoryID, @varItemSku, @intItemQuantity, @fltItemCost, @varItemDescription)";

                object[][] parms =
                {
                new object[] { "@intInvoiceItemID", item.intInvoiceItemID },
                new object[] { "@intInvoiceID", item.intInvoiceID },
                new object[] { "@intInventoryID", item.intInventoryID },
                new object[] { "@varItemSku", item.varItemSku },
                new object[] { "@intItemQuantity", item.intItemQuantity },
                new object[] { "@fltItemCost", item.fltItemCost },
                new object[] { "@varItemDescription", item.varItemDescription }
                };
                DBC.ExecuteNonReturnQuery(sqlCmd, parms);
            }
        }
        private void InsertInvoicePaymentIntoInvoicePaymentVoidCancel(Invoice invoice, int businessNumber)
        {
            foreach (InvoicePayment payment in invoice.lstInvoicePayment)
            {
                string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoicePaymentVoidCancel VALUES(@intPaymentID, "
                    + "@intInvoiceID, @intMethodOfPaymentID, @fltAmountReceived, @intChequeNumber)";

                object[][] parms =
                {
                    new object[] { "@intPaymentID", payment.intPaymentID },
                    new object[] { "@intInvoiceID", payment.intInvoiceID },
                    new object[] { "@intMethodOfPaymentID", payment.intMethodOfPaymentID },
                    new object[] { "@fltAmountReceived", payment.fltAmountReceived },
                    new object[] { "@intChequeNumber", payment.intChequeNumber }

                };
                DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
            }
        }
        private void RemoveInvoiceFromInvoiceCurrentTable(Invoice invoice, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "InvoiceCurrent WHERE intInvoiceID = "
                + "@intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveInvoiceItemFromInvoiceItemCurrentTable(Invoice invoice, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "InvoiceItemCurrent WHERE intInvoiceID "
                + "= @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        private void RemoveInvoicePaymentFromInvoicePaymentCurrentTable(Invoice invoice, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "InvoicePaymentCurrent WHERE intInvoiceID "
                + "= @intInvoiceID";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", invoice.intInvoiceID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }

        public void InsertItemIntoInvoiceCart(InvoiceItem invoiceItem, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoiceItemCurrent VALUES("
                + "@intInvoiceID, @intInventoryID, @varItemSku, @intItemQuantity, @fltItemCost, "
                + "@varItemDescription)";
            object[][] parms =
            {
                new object[] {"@intInvoiceID", invoiceItem.intInvoiceID },
                new object[] {"@intInventoryID", invoiceItem.intInventoryID },
                new object[] {"@varItemSku", invoiceItem.varItemSku },
                new object[] {"@intItemQuantity", invoiceItem.intItemQuantity },
                new object[] {"@fltItemCost", invoiceItem.fltItemCost },
                new object[] {"@varItemDescription", invoiceItem.varItemDescription }
            };
            DBC.MakeDatabaseCallToReturnDataTable(sqlCmd, parms);
        }
        public void UpdateInvoiceItemCurrent(InvoiceItem invoiceItem, int businessNumber)
        {
            string sqlCmd = "UPDATE tbl" + businessNumber + "InvoiceItemCurrent SET fltItemCost = @fltItemCost, varItemDescription = "
                + "@varItemDescription WHERE intInvoiceItemID = @intInvoiceItemID";

            object[][] parms =
            {
                new object[] { "@intInvoiceItemID", invoiceItem.intInvoiceItemID },
                new object[] { "@fltItemCost", invoiceItem.fltItemCost },
                new object[] { "@varItemDescription", invoiceItem.varItemDescription }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void CancelPurchaseInvoice(Invoice invoice, DateTime cancelDateTime, int businessNumber)
        {
            InsertInvoiceIntoInvoiceVoidCancel(invoice, cancelDateTime, businessNumber);
            InsertInvoiceItemIntoInvoiceItemVoidCancel(invoice, businessNumber);
            InsertInvoicePaymentIntoInvoicePaymentVoidCancel(invoice, businessNumber);

            RemoveInvoicePaymentFromInvoicePaymentCurrentTable(invoice, businessNumber);
            RemoveInvoiceItemFromInvoiceItemCurrentTable(invoice, businessNumber);
            RemoveInvoiceFromInvoiceCurrentTable(invoice, businessNumber);
        }
        public void AddPaymentToInvoice(object[] paymentCriteria, int businessNumber)
        {
            string sqlCmd = "INSERT INTO tbl" + businessNumber + "InvoicePaymentCurrent VALUES(@intInvoiceID, "
                + "@intMethodOfPaymentID, @fltAmountReceived, @intChequeNumber)";

            object[][] parms =
            {
                new object[] { "@intInvoiceID", Convert.ToInt32(paymentCriteria[0]) },
                new object[] { "@intMethodOfPaymentID", Convert.ToInt32(paymentCriteria[1]) },
                new object[] { "@fltAmountReceived", Convert.ToDouble(paymentCriteria[2]) },
                new object[] { "@intChequeNumber", Convert.ToInt32(paymentCriteria[3]) }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void RemovePaymentFromInvoice(int paymentID, int businessNumber)
        {
            string sqlCmd = "DELETE tbl" + businessNumber + "InvoicePaymentCurrent WHERE intPaymentID = @intPaymentID";

            object[][] parms =
            {
                new object[] { "@intPaymentID", paymentID }
            };
            DBC.ExecuteNonReturnQuery(sqlCmd, parms);
        }
        public void FinalizeInvoice(Invoice invoice, DateTime completeDateTime, CurrentUser cu)
        {
            //Step 1: Save New Invoice to the Final Invoice Table
            InsertInvoiceIntoInvoiceTable(invoice, completeDateTime, cu.terminal.intBusinessNumber);

            //Step 2: Save New Invoice Mops to the Final Invoice Mops Table
            InsertInvoicePaymentIntoInvoicePaymentTable(invoice, cu.terminal.intBusinessNumber);
            //Step 3: Remove Invoice Mops from the Current Invoice Mops Table
            RemoveInvoicePaymentFromInvoicePaymentCurrentTable(invoice, cu.terminal.intBusinessNumber);

            //Step 4: Save New Invoice Items to the Final Invoice Items Table
            InsertInvoiceItemIntoInvoiceItemTable(invoice, cu);
            //Step 5: Remove Invoice Items from the Current Invoice Items Table
            RemoveInvoiceItemFromInvoiceItemCurrentTable(invoice, cu.terminal.intBusinessNumber);
            
            //Step 6: Remove Invoice from the Current Invoice Table
            RemoveInvoiceFromInvoiceCurrentTable(invoice, cu.terminal.intBusinessNumber);
        }
    }
}