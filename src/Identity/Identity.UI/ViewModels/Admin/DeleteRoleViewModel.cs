using System;
using System.ComponentModel.DataAnnotations;


namespace Identity.UI.ViewModels.Admin
{
    public class DeleteRoleViewModel
    {
        [Display(Name = "Role Id")]
        [Required(ErrorMessage = "The {0} must be specified!")]
        public Guid RoleId { get; set; }
    }
}