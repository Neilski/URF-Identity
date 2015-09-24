using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;


// ReSharper disable DoNotCallOverridableMethodsInConstructor

namespace UrfIdentity.Models
{
    // You can add profile data for the user by adding more properties to your
    // ApplicationUser class, please visit 
    // http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser
        :
            IdentityUser
                <Guid, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>,
            IObjectState
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        public ApplicationUser(string username)
            : this()
        {
            UserName = username;
        }

        #region Extended Properties
        public DateTime Created { get; set; }

        public DateTime? LastLogin { get; set; }

        public virtual ApplicationUserDetails UserDetails { get; set; }

        [NotMapped]
        public ObjectState ObjectState { get; set; }
        #endregion Extended Properties

    }
}