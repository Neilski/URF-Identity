using System;
using System.Collections.Generic;


namespace Identity.UI.ViewModels.Admin
{
    public class RoleAssignmentViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IList<RoleAssignmentItemViewModel> Roles { get; set; }
    }


    public class RoleAssignmentPostViewModel
    {
        public Guid UserId { get; set; }
        public string[] AssignedRoles { get; set; }
    }

    public class RoleAssignmentItemViewModel
    {
        public string RoleName { get; set; }
        public bool Assigned { get; set; }
    }
}