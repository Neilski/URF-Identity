using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Account
{
    public class ResetPasswordViewModel
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
        [StringLength(100,
            ErrorMessageResourceName = "Password_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null,
            MinimumLength = 8)]
        [DataType(DataType.Password,
            ErrorMessageResourceName = "Password_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Password { get; set; }

        [Display(Name = "ConfirmPassword",
            ResourceType = typeof (Resources.AccountResources))]
        [Compare("Password",
            ErrorMessageResourceName = "ConfirmPassword_Compare",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [DataType(DataType.Password,
            ErrorMessageResourceName = "Password_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string ConfirmPassword { get; set; }

        [Display(Name = "SecurityCode",
            ResourceType = typeof (Resources.AccountResources))]
        public string Code { get; set; }
    }
}