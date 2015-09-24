using System.ComponentModel.DataAnnotations;
using Abl;


namespace Identity.UI.ViewModels.Admin
{
    public class AddRoleViewModel
    {
        [Display(Name = "Role Name")]
        [Required(ErrorMessage = "The {0} must be specified!")]
        [StringLength(128, MinimumLength = 2,
            ErrorMessage =
                "{0} must be between {1} and {2} charcaters in length!")]
        [RegularExpression(RegexPatterns.PlainStringPattern,
            ErrorMessageResourceName = "PlainString_Format",
            ErrorMessageResourceType = typeof (Resources.AccountResources),
            ErrorMessage = null)
        ]
        public string RoleName { get; set; }
    }
}