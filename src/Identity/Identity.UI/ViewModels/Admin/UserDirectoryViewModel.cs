using System;


namespace Identity.UI.ViewModels.Admin
{
    public class UserDirectoryViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool LockedOut { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}