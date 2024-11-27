using System;
using System.Web;
using System.Web.UI;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;

public partial class Account_Login : Page
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (AuthUtils.IsUserLoggedIn())
        { 
            Response.Redirect("~"); 
        }
    }
    private string _username = "Default User";

    protected void LogIn(object sender, EventArgs e)
    {
        if (IsValid)
        {
            // Validate the user password against the APTIV domain
            string validationMessage;
            if (ValidateUser(UserName.Text, Password.Text, out validationMessage))
            {
                // Create an authentication ticket
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                                  // Ticket version
                    UserName.Text,                      // Username associated with the ticket
                    DateTime.Now,                       // Date/time issued
                    DateTime.Now.AddDays(30),           // Date/time to expire
                    RememberMe.Checked,                 // "true" for a persistent user cookie
                    _username,                          // User data (could be roles, etc.)
                    FormsAuthentication.FormsCookiePath // Cookie path
                );

                // Encrypt the ticket
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                Response.Cookies.Add(cookie);

                // Log the user in
                Response.Redirect("~");
            }
            else
            {
                FailureText.Text = validationMessage;
                ErrorMessage.Visible = true;
            }
        }
    }

    private bool ValidateUser(string username, string password, out string validationMessage)
    {
        validationMessage = string.Empty;

        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "APTIV"))
        {
            // Check if the user exists in the domain
            UserPrincipal user = UserPrincipal.FindByIdentity(pc, username);
            if (user == null)
            {
                validationMessage = "User does not exist in the domain.";
                return false;
            }

            _username = user.DisplayName;

            // Validate the credentials
            if (pc.ValidateCredentials(username, password))
            {
                return true;
            }
            else
            {
                validationMessage = "Incorrect password.";
                return false;
            }
        }
    }
}
