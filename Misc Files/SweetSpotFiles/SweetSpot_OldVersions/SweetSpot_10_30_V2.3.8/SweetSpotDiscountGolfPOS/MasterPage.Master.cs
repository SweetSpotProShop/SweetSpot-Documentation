﻿using SweetSpotDiscountGolfPOS.ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SweetSpotDiscountGolfPOS
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session["currentUser"] = null;
            Server.Transfer("LoginPage.aspx", false);
        }
    }
}