using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class SweetPea : System.Web.UI.Page
    {
        private LicenceFiles LF = new LicenceFiles();

        protected void Page_Load(object sender, EventArgs e)
        {
            lblErrorOccurred.Visible = false;
            txtUserName.Focus();
            if (!IsPostBack)
            {
                HttpCookie cookieTerminalID = HttpContext.Current.Request.Cookies["terminalID"];
                HttpCookie cookieBusinessNumber = HttpContext.Current.Request.Cookies["businessNumber"];
                HttpCookie cookieLicenceID = HttpContext.Current.Request.Cookies["licenceID"];

                if (cookieTerminalID != null || cookieBusinessNumber != null || cookieLicenceID != null)
                {
                    int terminalID = Convert.ToInt32(cookieTerminalID.Value);
                    int businessNumber = Convert.ToInt32(cookieBusinessNumber.Value);
                    int licenceID = Convert.ToInt32(cookieLicenceID.Value);
                    //int systemCheck = LF.SystemCheck(businessNumber, terminalID);
                    //if (systemCheck == 0)
                    //{
                        //7. With outcome A, a store setup page will open to get the info of their first store location.
                        //here do nothing as we will stay on this page and use the login info
                    //}
                    //else if (systemCheck == 1)
                    //{
                        //8. With outcome B, this machine will try to setup as a valid till using a specific licence number and will need to enter which till # it is.
                        //new page select store location, enter till number and licence
                        //var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                        //nameValues.Set("businessNumber", businessNumber.ToString());
                        //nameValues.Set("code", "4");
                        //Response.Redirect("Initialization.aspx?" + nameValues, true);
                    //}
                    //else if (systemCheck == 2)
                    //{
                        //6. With outcome C, it will jump to their employee page to log in to the pos.
                        Response.Redirect("LoginPage.aspx", true);
                    //}
                    //else if (systemCheck == 3)
                    //{
                        //9. With outcome D, return what the error is and show main application page to choose what to do with selected options.
                        //Display error and stay on this page. Advise to contact S and G for additional Support.
                        //lblErrorOccurred.Visible = true;
                    //}
                }
            }
        }
        protected void btnSystemLogin_Click(object sender, EventArgs e)
        {
            object[] login = { txtUserName.Text, txtPassword.Text };

            if (LF.CheckBusinessPortfolioLogin(login))
            {
                var nameValues = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                nameValues.Set("businessNumber", login[0].ToString());
                int systemCheck = LF.SystemCheck2(Convert.ToInt32(login[0]));
                if (systemCheck == 1)
                {
                    //8. With outcome B, this machine will try to setup as a valid till using a specific licence number and will need to enter which till # it is.
                    //new page select store location, enter till number and licence
                    nameValues.Set("code", "4");
                    Response.Redirect("Initialization.aspx?" + nameValues, true);
                }
                else if (systemCheck == 2)
                {
                    nameValues.Set("code", "1");
                    Response.Redirect("Initialization.aspx?" + nameValues, true);
                }
                else if (systemCheck == 0)
                {
                    //9. With outcome D, return what the error is and show main application page to choose what to do with selected options.
                    //Display error and stay on this page. Advise to contact S and G for additional Support.
                    lblErrorOccurred.Visible = true;
                }
            }
            else
            {
                lblErrorOccurred.Text = "User name or password provided is incorrect. Please contact program administrator.";
                lblErrorOccurred.Visible = true;
            }
        }
    }
}