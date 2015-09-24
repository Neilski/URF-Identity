using System;


namespace UrfIdentity.Models.ViewModels
{
    public class ApplicationUserViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}