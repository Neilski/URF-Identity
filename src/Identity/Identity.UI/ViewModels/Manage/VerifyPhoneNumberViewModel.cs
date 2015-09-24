using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Manage
{
    public class VerifyPhoneNumberViewModel
    {
        [Display(Name = "SecurityCode",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "SecurityCode_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Code { get; set; }

        [Display(Name = "Phone",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "Phone_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [Phone(ErrorMessageResourceName = "PhoneNumber_Invalid",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string PhoneNumber { get; set; }
    }
}