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
    public partial class TradeInManagement : System.Web.UI.Page
    {
        ErrorReporting ER = new ErrorReporting();
        InventoryManager IM = new InventoryManager();
        LocationManager LM = new LocationManager();
        EmployeeManager EM = new EmployeeManager();
        CurrentUser CU;

        protected void Page_Load(object sender, EventArgs e)
        {
            string strMethod = "Page_Load";
            Session["currentPage"] = "TradeInManagement.aspx";
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
                        //This will be changed to a security level for processing
                        //if (EM.ReturnSuDoIDForLocation(CU))
                        //{
                        ddlStoreLocation.DataSource = LM.ReturnStoreLocationDropDown(CU.terminal.intBusinessNumber);
                        ddlStoreLocation.DataBind();
                        ddlStoreLocation.SelectedValue = CU.currentStoreLocation.intStoreLocationID.ToString();
                        ddlStoreLocation.Focus();
                        grdUnProcessedTradeIns.DataSource = IM.ReturnTradeInsForProcessing(CU);
                        grdUnProcessedTradeIns.DataBind();
                        if (EM.ReturnSuDoIDForLocation(CU))
                        {
                            ddlStoreLocation.Enabled = true;
                        }
                        //}
                    }
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
        protected void grdUnProcessedTradeIns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //Collects current method for error tracking
            string strMethod = "grdUnProcessedTradeIns_RowCommand";
            try
            {
                int index = ((GridViewRow)((LinkButton)e.CommandSource).NamingContainer).RowIndex;
                int purchasedInventoryID = Convert.ToInt32(e.CommandArgument.ToString());
                int tradeInAction = Convert.ToInt32(((RadioButtonList)grdUnProcessedTradeIns.Rows[index].Cells[5].FindControl("rblTradeInProcessAction")).SelectedValue);
                IM.FinalTradeInManagementProcess(purchasedInventoryID, tradeInAction, CU);

                grdUnProcessedTradeIns.DataSource = IM.ReturnTradeInsForProcessing(CU);
                grdUnProcessedTradeIns.DataBind();
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