using System;
using System.Linq;
using Repository.Pattern.Ef6;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Repository.Queries
{
    public class ApplicationUserQuery : QueryObject<ApplicationUser>
    {
        public ApplicationUserQuery SelectByRole(Guid roleId)
        {
            And(x => (
                (x.EmailConfirmed) &&
                (x.Roles.Any(r => r.RoleId == roleId))
                ));

            return this;
        }
    }
}