using System.Collections.Generic;
using Microsoft.Owin.Security;


namespace Identity.UI.ViewModels.Account
{
    public class FirstTimeLoginViewModel
    {
        public string FirstName { get; set; }
        public IList<AuthenticationDescription> ExternalLogins { get; set; }
    }
}