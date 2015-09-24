using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UrfIdentity.DAL.Db;
using UrfIdentity.Models;


namespace Identity
{
    public class ApplicationUserStore : UserStore<ApplicationUser, ApplicationRole, Guid,
        ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUserStore(IUrfIdentityDataContextAsync context)
            : base(context as DbContext)
        {
        }
    }
}