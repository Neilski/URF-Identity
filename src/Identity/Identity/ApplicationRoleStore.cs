using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using UrfIdentity.DAL.Db;
using UrfIdentity.Models;


namespace Identity
{
    public class ApplicationRoleStore :
        RoleStore<ApplicationRole, Guid, ApplicationUserRole>
    {
        public ApplicationRoleStore(IUrfIdentityDataContextAsync context)
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
        ** public override Task CreateAsync(ApplicationRole role)
        ** {
        **     role.ObjectState = ObjectState.Added;
        **     return base.CreateAsync(role);
        ** }
        ** 
        ** 
        ** public override Task UpdateAsync(ApplicationRole role)
        ** {
        **     role.ObjectState = ObjectState.Modified;
        **     return base.UpdateAsync(role);
        ** }
        ** 
        ** 
        ** public override Task DeleteAsync(ApplicationRole role)
        ** {
        **     role.ObjectState = ObjectState.Deleted;
        **     return base.DeleteAsync(role);
        ** }
        */
    }
}