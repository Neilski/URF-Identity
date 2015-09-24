using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;


namespace UrfIdentity.Models
{
    public class ApplicationUserRole : IdentityUserRole<Guid>, IObjectState
    {
        [NotMapped]
        public ObjectState ObjectState { get; set; }
    }
}