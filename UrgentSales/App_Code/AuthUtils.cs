using System.Web;
using System.Web.Security;

public static class AuthUtils
{
    public static bool IsUserLoggedIn()
    {
        // Optionally, validate the FormsAuthenticationTicket if needed
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        if (authCookie != null)
        {
            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket != null && !ticket.Expired)
                {
                    // The user is logged in and the ticket is valid
                    return true;
                }
            }
            catch
            {
                // Handle any exceptions related to ticket decryption
            }
        }

        // The user is not authenticated or the ticket is invalid/expired
        return false;
    }

    public static string GetUserName()
    {
        HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
        if (ticket != null && !ticket.Expired)
        {
            return ticket.UserData;
        }

        return "";
    }
}
