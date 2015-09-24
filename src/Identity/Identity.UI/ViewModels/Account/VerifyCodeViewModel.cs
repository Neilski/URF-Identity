using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Account
{
    public class VerifyCodeViewModel
    {
        [Display(Name = "SelectedOAuthProvider",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "OAuthProvider_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Provider { get; set; }

        [Display(Name = "SecurityCode",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "SecurityCode_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Code { get; set; }

        [Display(Name = "ReturnUrl",
            ResourceType = typeof (Resources.AccountResources))]
        public string ReturnUrl { get; set; }

        [Display(Name = "RememberBrowser",
            ResourceType = typeof (Resources.AccountResources))]
        public bool RememberBrowser { get; set; }

        [Display(Name = "RememberMe",
            ResourceType = typeof (Resources.AccountResources))]
        public bool RememberMe { get; set; }
    }
}