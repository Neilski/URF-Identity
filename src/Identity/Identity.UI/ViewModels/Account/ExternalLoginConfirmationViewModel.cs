using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Account
{
    public class ExternalLoginConfirmationViewModel
    {
        [Display(Name = "Email",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "Email_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [EmailAddress(ErrorMessageResourceName = "Email_Invalid",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Email { get; set; }
    }
}