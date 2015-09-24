using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Manage
{
    public class AddPhoneNumberViewModel
    {
        [Display(Name = "PhoneNumber",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "PhoneNumber_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [Phone(ErrorMessageResourceName = "PhoneNumber_Invalid",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Number { get; set; }
    }
}