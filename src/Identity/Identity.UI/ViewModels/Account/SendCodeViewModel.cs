using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Identity.UI.ViewModels.Account
{
    public class SendCodeViewModel
    {
        [Display(Name = "SelectedOAuthProvider",
            ResourceType = typeof (Resources.AccountResources))]
        public string SelectedProvider { get; set; }

        [Display(Name = "OAuthProviderList",
            ResourceType = typeof (Resources.AccountResources))]
        public ICollection<SelectListItem> Providers { get; set; }

        [Display(Name = "ReturnUrl",
            ResourceType = typeof (Resources.AccountResources))]
        public string ReturnUrl { get; set; }

        [Display(Name = "RememberMe",
            ResourceType = typeof (Resources.AccountResources))]
        public bool RememberMe { get; set; }
    }
}