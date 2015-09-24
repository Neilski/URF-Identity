using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Manage
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "CurrentPassword",
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
        public string OldPassword { get; set; }

        [Display(Name = "NewPassword",
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
        public string NewPassword { get; set; }

        [Display(Name = "ConfirmPassword",
            ResourceType = typeof (Resources.AccountResources))]
        [Compare("NewPassword",
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