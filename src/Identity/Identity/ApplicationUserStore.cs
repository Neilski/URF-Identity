using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;
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


        public override Task CreateAsync(ApplicationUser user)
        {
            user.ObjectState = ObjectState.Added;
            return base.CreateAsync(user);
        }


        public override Task UpdateAsync(ApplicationUser user)
        {
            user.ObjectState = ObjectState.Modified;
            return base.UpdateAsync(user);
        }


        public override Task DeleteAsync(ApplicationUser user)
        {
            user.ObjectState = ObjectState.Deleted;
            return base.DeleteAsync(user);
        }
    }
}