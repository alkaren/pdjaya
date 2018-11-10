using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Public_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Login1.LoggedIn += Login1_LoggedIn;
    }

    private void Login1_LoggedIn(object sender, EventArgs e)
    {
        Response.Redirect("/Pages/Private/Index.aspx");
    }
}