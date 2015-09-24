using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Account
{
    public class LoginViewModel
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

        [Display(Name = "Password",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "Password_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [DataType(DataType.Password,
            ErrorMessageResourceName = "Password_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Password { get; set; }

        [Display(Name = "RememberMe",
            ResourceType = typeof (Resources.AccountResources))]
        public bool RememberMe { get; set; }
    }
}