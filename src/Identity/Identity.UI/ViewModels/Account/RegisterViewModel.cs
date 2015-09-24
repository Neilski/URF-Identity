using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abl;


namespace Identity.UI.ViewModels.Account
{
    public class RegisterViewModel
    {
        // These properties are used if the user initiates the registration
        // process from their oAuth Provider Account
        public string ProviderEmailAddress { get; set; }


        /*
        ** Application User
        */

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


        /*
        ** ApplicationUserDetails
        */

        [Display(Name = "FirstName",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "FirstName_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [RegularExpression(RegexPatterns.PlainStringPattern,
            ErrorMessageResourceName = "PlainString_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string FirstName { get; set; }

        [Display(Name = "LastName",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "LastName_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [RegularExpression(RegexPatterns.PlainStringPattern,
            ErrorMessageResourceName = "PlainString_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string LastName { get; set; }

        [Display(Name = "JobTitle",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "JobTitle_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [RegularExpression(RegexPatterns.PlainStringPattern,
            ErrorMessageResourceName = "PlainString_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string JobTitle { get; set; }

        [Display(Name = "Company",
            ResourceType = typeof (Resources.AccountResources))]
        [Required(ErrorMessageResourceName = "Company_Required",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        [StringLength(100,
            ErrorMessageResourceName = "Company_Length",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null,
            MinimumLength = 8)]
        [RegularExpression(RegexPatterns.SafeCharacterPattern,
            ErrorMessageResourceName = "PlainString_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string Company { get; set; }

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
        public string Phone { get; set; }
    }
}