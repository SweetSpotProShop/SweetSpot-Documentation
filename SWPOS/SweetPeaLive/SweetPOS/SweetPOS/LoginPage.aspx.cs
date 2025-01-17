using SweetPOS.ClassObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetPOS
{
    public partial class LoginPage : System.Web.UI.Page
    {
        CurrentUserManager CUM = new CurrentUserManager();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtEFSHJEMVIFEntry.Focus();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LicenceFiles LF = new LicenceFiles();
            HttpCookie cookieBusinessNumber = HttpContext.Current.Request.Cookies["businessNumber"];
            int businessNumber = Convert.ToInt32(cookieBusinessNumber.Value);
            HttpCookie cookieTerminalID = HttpContext.Current.Request.Cookies["terminalID"];
            int terminalID = Convert.ToInt32(cookieTerminalID.Value);
            List<CurrentUser> CU = CUM.ReturnCurrentUserFromPassword(Convert.ToInt32(txtEFSHJEMVIFEntry.Text), businessNumber, terminalID);
            if(CU.Count > 0)
            {
                Session["CurrentUser"] = CU[0];
                Response.Redirect("HomePage.aspx", true);
            }
            else
            {
                lblError.Text = "Your password is incorrect";
            }
        }
    }
}