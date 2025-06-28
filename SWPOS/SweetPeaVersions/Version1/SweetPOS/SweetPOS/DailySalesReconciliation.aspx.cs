using SweetPOS.ClassObjects;
using System;
using System.Threading;

namespace SweetPOS
{
    public partial class DailySalesReconciliation : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        LocationManager L = new LocationManager();
        SalesReconciliationManager SRM = new SalesReconciliationManager();
        CurrentUserManager CUM = new CurrentUserManager();
        private static SalesReconciliation salesReconciliation;
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Collects current method and page for error tracking
            string strMethod = "Page_Load";
            Session["currentPage"] = "DailySalesReconciliation.aspx";
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
                    txtReceiptTalliesAmericanExpress.Focus();
                    if (!IsPostBack)
                    {
                        //Gathering the start and end dates
                        DateTime selectedDate = DateTime.Parse(Request.QueryString["reconcilaitionDate"].ToString());
                        StoreLocation storeLocation = L.ReturnLocation(Convert.ToInt32(Request.QueryString["storeLocation"]), CU.terminal.intBusinessNumber)[0];
                        lblReconciliationDate.Text = "Daily Sales Reconciliation on: " + selectedDate.ToString("d") + " for " + storeLocation.varStoreName.ToString();
                        if (SRM.DailyReconciliationExists(selectedDate, storeLocation.intStoreLocationID, CU))
                        {
                            salesReconciliation = SRM.ReturnSelectedSalesReconciliation(storeLocation.intStoreLocationID, selectedDate, CU);
                        }
                        else
                        {
                            //Creating a cashout list and calling a method that grabs all mops and amounts paid
                            salesReconciliation = SRM.CreateNewSalesReconciliation(selectedDate, storeLocation.intStoreLocationID, Convert.ToDateTime(CUM.ConvertDate(DateTime.Now.ToLocalTime())), CU)[0];
                        }

                        //need to add in all MOP types
                        lblRecordedSalesAmericanExpressDisplay.Text = salesReconciliation.fltAmericanExpressSales.ToString("C");
                        txtReceiptTalliesAmericanExpress.Text = salesReconciliation.fltAmericanExpressCounted.ToString("C");

                        lblRecordedSalesCashDisplay.Text = salesReconciliation.fltCashSales.ToString("C");
                        lblReceiptTalliesCashManagement.Text = salesReconciliation.fltCashCounted.ToString("C");

                        lblRecordedSalesChequeDisplay.Text = salesReconciliation.fltChequeSales.ToString("C");
                        txtReceiptTalliesCheque.Text = salesReconciliation.fltChequeCounted.ToString("C");

                        lblRecordedSalesDebitDisplay.Text = salesReconciliation.fltDebitSales.ToString("C");
                        txtReceiptTalliesDebit.Text = salesReconciliation.fltDebitCounted.ToString("C");

                        lblRecordedSalesDiscoverDisplay.Text = salesReconciliation.fltDiscoverSales.ToString("C");
                        txtReceiptTalliesDiscover.Text = salesReconciliation.fltDiscoverCounted.ToString("C");

                        lblRecordedSalesGiftCardDisplay.Text = salesReconciliation.fltGiftCardSales.ToString("C");
                        txtReceiptTalliesGiftCard.Text = salesReconciliation.fltGiftCardCounted.ToString("C");

                        lblRecordedSalesMastercardDisplay.Text = salesReconciliation.fltMastercardSales.ToString("C");
                        txtReceiptTalliesMastercard.Text = salesReconciliation.fltMastercardCounted.ToString("C");

                        lblRecordedSalesTradeInDisplay.Text = salesReconciliation.fltTradeInSales.ToString("C");
                        txtReceiptTalliesTradeIn.Text = salesReconciliation.fltTradeInCounted.ToString("C");

                        lblRecordedSalesVisaDisplay.Text = salesReconciliation.fltVisaSales.ToString("C");
                        txtReceiptTalliesVisa.Text = salesReconciliation.fltVisaCounted.ToString("C");

                        lblGovernmentTaxTotal.Text = salesReconciliation.fltGovernmentTaxTotal.ToString("C");
                        lblProvincialTaxTotal.Text = salesReconciliation.fltProvincialTaxTotal.ToString("C");
                        lblPreTaxSalesTotal.Text = salesReconciliation.fltPreTaxSalesTotal.ToString("C");
                        lblSalesAndTaxTotal.Text = (salesReconciliation.fltPreTaxSalesTotal + salesReconciliation.fltGovernmentTaxTotal + salesReconciliation.fltProvincialTaxTotal).ToString("C");


                        lblRecordedSalesTotal.Text = (salesReconciliation.fltAmericanExpressSales + salesReconciliation.fltCashSales + salesReconciliation.fltChequeSales
                            + salesReconciliation.fltDebitSales + salesReconciliation.fltDiscoverSales + salesReconciliation.fltGiftCardSales
                            + salesReconciliation.fltMastercardSales + salesReconciliation.fltVisaSales).ToString("C");
                        lblReceiptTallyTotal.Text = (salesReconciliation.fltAmericanExpressCounted + salesReconciliation.fltCashCounted + salesReconciliation.fltChequeCounted
                            + salesReconciliation.fltDebitCounted + salesReconciliation.fltDiscoverCounted + salesReconciliation.fltGiftCardCounted
                            + salesReconciliation.fltMastercardCounted + salesReconciliation.fltVisaSales).ToString("C");
                        lblCashPurchasesDislay.Text = salesReconciliation.fltCashPurchases.ToString("C");
                        lblOverShortDisplay.Text = salesReconciliation.fltOverShort.ToString("C");
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
        //Calculating the cashout
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnCalculate_Click";
            try
            {
                SalesReconciliationCalculation();
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
        //Clearing the entered amounts
        protected void btnClear_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnClear_Click";
            try
            {
                salesReconciliation.fltAmericanExpressCounted = 0;
                salesReconciliation.fltCashCounted = 0;
                salesReconciliation.fltChequeCounted = 0;
                salesReconciliation.fltDebitCounted = 0;
                salesReconciliation.fltDiscoverCounted = 0;
                salesReconciliation.fltGiftCardCounted = 0;
                salesReconciliation.fltMastercardCounted = 0;
                salesReconciliation.fltTradeInCounted = 0;
                salesReconciliation.fltVisaCounted = 0;
                //Blanking the textboxes
                txtReceiptTalliesAmericanExpress.Text = "0.00";
                txtReceiptTalliesCheque.Text = "0.00";
                txtReceiptTalliesDebit.Text = "0.00";
                txtReceiptTalliesDiscover.Text = "0.00";
                txtReceiptTalliesGiftCard.Text = "0.00";
                txtReceiptTalliesMastercard.Text = "0.00";
                txtReceiptTalliesTradeIn.Text = "0.00";
                txtReceiptTalliesVisa.Text = "0.00";
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
        protected void btnProcessSalesReconciliation_Click(object sender, EventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "btnProcessSalesReconciliation_Click";
            try
            {
                SalesReconciliationCalculation();
                salesReconciliation.bitIsProcessed = true;
                SRM.UpdateSalesReconciliation(salesReconciliation, CU);                
                txtReceiptTalliesAmericanExpress.Enabled = false;
                txtReceiptTalliesCheque.Enabled = false;
                txtReceiptTalliesDebit.Enabled = false;
                txtReceiptTalliesDiscover.Enabled = false;
                txtReceiptTalliesGiftCard.Enabled = false;
                txtReceiptTalliesMastercard.Enabled = false;
                txtReceiptTalliesTradeIn.Enabled = false;
                txtReceiptTalliesVisa.Enabled = false;
                
                MessageBox.ShowMessage("Cashout has been processed", this);
                btnPrint.Enabled = true;
                btnCalculate.Enabled = false;
                btnProcessSalesReconciliation.Enabled = false;
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
        private void SalesReconciliationCalculation()
        {
            //Collects current method for error tracking
            string strMethod = "SalesReconciliationCalculation";
            try
            {
                //If nothing is entered, setting text to 0.00 and the total to 0
                if (txtReceiptTalliesAmericanExpress.Text == "")
                {
                    salesReconciliation.fltAmericanExpressCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesAmericanExpress.Text.Replace("$", ""), out result);
                    salesReconciliation.fltAmericanExpressCounted = result;
                }
                if (txtReceiptTalliesCheque.Text == "")
                {
                    salesReconciliation.fltChequeCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesCheque.Text.Replace("$", ""), out result);
                    salesReconciliation.fltChequeCounted = result;
                }
                if (txtReceiptTalliesDebit.Text == "")
                {
                    salesReconciliation.fltDebitCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesDebit.Text.Replace("$", ""), out result);
                    salesReconciliation.fltDebitCounted = result;
                }
                if (txtReceiptTalliesDiscover.Text == "")
                {
                    salesReconciliation.fltDiscoverCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesDiscover.Text.Replace("$", ""), out result);
                    salesReconciliation.fltDiscoverCounted = result;
                }
                if (txtReceiptTalliesGiftCard.Text == "")
                {
                    salesReconciliation.fltGiftCardCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesGiftCard.Text.Replace("$", ""), out result);
                    salesReconciliation.fltGiftCardCounted = result;
                }
                if (txtReceiptTalliesMastercard.Text == "")
                {
                    salesReconciliation.fltMastercardCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesMastercard.Text.Replace("$", ""), out result);
                    salesReconciliation.fltMastercardCounted = result;
                }
                if (txtReceiptTalliesTradeIn.Text == "")
                {
                    salesReconciliation.fltTradeInCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesTradeIn.Text.Replace("$", ""), out result);
                    salesReconciliation.fltTradeInCounted = result;
                }
                if (txtReceiptTalliesVisa.Text == "")
                {
                    salesReconciliation.fltVisaCounted = 0;
                }
                else
                {
                    double result = 0;
                    double.TryParse(txtReceiptTalliesVisa.Text.Replace("$", ""), out result);
                    salesReconciliation.fltVisaCounted = result;
                }

                //The calculation of the application totals
                double receiptSalesTotal = salesReconciliation.fltAmericanExpressSales + salesReconciliation.fltCashSales + salesReconciliation.fltChequeSales
                    + salesReconciliation.fltDebitSales + salesReconciliation.fltDiscoverSales + salesReconciliation.fltGiftCardSales
                    + salesReconciliation.fltMastercardSales + salesReconciliation.fltVisaSales;
                lblRecordedSalesTotal.Text = receiptSalesTotal.ToString("C");

                //the calculation of hand tallied receipts
                double receiptSalesTallies = salesReconciliation.fltAmericanExpressCounted + salesReconciliation.fltCashCounted + salesReconciliation.fltChequeCounted
                    + salesReconciliation.fltDebitCounted + salesReconciliation.fltDiscoverCounted + salesReconciliation.fltGiftCardCounted
                    + salesReconciliation.fltMastercardCounted + salesReconciliation.fltVisaCounted;
                lblReceiptTallyTotal.Text = receiptSalesTallies.ToString("C");

                salesReconciliation.fltOverShort = receiptSalesTallies - receiptSalesTotal + salesReconciliation.fltCashPurchases;

                lblOverShortDisplay.Text = salesReconciliation.fltOverShort.ToString("C");
                //Checking over or under
                if (salesReconciliation.fltOverShort < 0)
                {
                    lblOverShortDisplay.ForeColor = System.Drawing.Color.Red;
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
    }
}