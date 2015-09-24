using System;
using System.ComponentModel.DataAnnotations.Schema;
using Repository.Pattern.Infrastructure;


namespace UrfIdentity.Models
{
    public class ApplicationUserDetails : IObjectState
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; }

        // Navigation
        public virtual ApplicationUser User { get; set; }


        public string FullName => ($"{FirstName} {LastName}").Trim();
        public string ListName => ($"{LastName}, {FirstName}").Trim();
    }
}