using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace Identity.UI.ViewModels.Manage
{
    public class ConfigureTwoFactorViewModel
    {
        [Display(Name = "SelectedOAuthProvider",
            ResourceType = typeof (Resources.AccountResources))]
        public string SelectedProvider { get; set; }

        [Display(Name = "OAuthProviderList",
            ResourceType = typeof (Resources.AccountResources))]
        public ICollection<SelectListItem> Providers { get; set; }
    }
}