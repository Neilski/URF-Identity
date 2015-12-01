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


        /*
        ** Uncomment this code if you do not want to modify the core URF library
        ** (Repository.Pattern.Ef6.DataContext.SyncObjectsStatePreCommit() method).
        **
        ** If you do this however, note that some of the core Identity functionality
        ** such as UserManager.AddToRoleAsync() will not work as the ApplicationUserStore
        ** will not correctly set the URF ObjectState on child entities.  If you need
        ** (and you probably do!) this functionality then you would need to replicate 
        ** it - probably in an appropriate URF Service.
        **
        ** public override Task CreateAsync(ApplicationUser user)
        ** {
        **     user.ObjectState = ObjectState.Added;
        **     return base.CreateAsync(user);
        ** }
        ** 
        ** 
        ** public override Task UpdateAsync(ApplicationUser user)
        ** {
        **     user.ObjectState = ObjectState.Modified;
        **     return base.UpdateAsync(user);
        ** }
        ** 
        ** 
        ** public override Task DeleteAsync(ApplicationUser user)
        ** {
        **     user.ObjectState = ObjectState.Deleted;
        **     return base.DeleteAsync(user);
        ** }
        ** 
        ** 
        ** public override Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        ** {
        **     user.ObjectState = ObjectState.Added;
        **     return base.AddLoginAsync(user, login);
        ** }
        */
    }
}
 