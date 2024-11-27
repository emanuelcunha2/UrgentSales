using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Account_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        // Log the user out
        FormsAuthentication.SignOut();

        // Clear the authentication cookie
        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
        cookie.Expires = DateTime.Now.AddYears(-1);
        Response.Cookies.Add(cookie);

        // Clear session state
        Session.Clear();

        // Redirect to the login page
        Response.Redirect("~/Account/Login.aspx");
    }
}