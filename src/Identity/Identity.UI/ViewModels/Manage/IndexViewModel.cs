using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;


namespace Identity.UI.ViewModels.Manage
{
    public class IndexViewModel
    {
        [Display(Name = "HasPassword",
            ResourceType = typeof (Resources.AccountResources))]
        public bool HasPassword { get; set; }

        [Display(Name = "RegisteredLoginList",
            ResourceType = typeof (Resources.AccountResources))]
        public IList<UserLoginInfo> Logins { get; set; }

        [Display(Name = "PhoneNumber",
            ResourceType = typeof (Resources.AccountResources))]
        public string PhoneNumber { get; set; }

        [Display(Name = "HasTwoFactorAuthentication",
            ResourceType = typeof (Resources.AccountResources))]
        public bool TwoFactor { get; set; }

        [Display(Name = "RememberBrowser",
            ResourceType = typeof (Resources.AccountResources))]
        public bool BrowserRemembered { get; set; }
    }
}