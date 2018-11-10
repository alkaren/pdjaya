using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Master_Main : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //bool IsLoggedIn = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
        //if (!IsLoggedIn) Response.Redirect("/Pages/Public/Login.aspx");
    }
}
