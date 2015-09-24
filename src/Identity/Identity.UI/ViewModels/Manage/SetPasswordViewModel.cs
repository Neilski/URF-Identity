using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Manage
{
    public class SetPasswordViewModel
    {
        [Display(Name = "NewPassword",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "NewPassword_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [StringLength(100,
            ErrorMessageResourceName = "NewPassword_Length",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null,
            MinimumLength = 8)]
        public string NewPassword { get; set; }

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
    }
}