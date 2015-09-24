using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;


namespace UrfIdentity.Models
{
    public class ApplicationRole : IdentityRole<Guid, ApplicationUserRole>, IObjectState
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();
        }

        public ApplicationRole(string name)
            : this()
        {
            Name = name;
        }

        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}