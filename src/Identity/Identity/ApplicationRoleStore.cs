using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Repository.Pattern.Infrastructure;
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


        public override Task CreateAsync(ApplicationRole role)
        {
            role.ObjectState = ObjectState.Added;
            return base.CreateAsync(role);
        }


        public override Task UpdateAsync(ApplicationRole role)
        {
            role.ObjectState = ObjectState.Modified;
            return base.UpdateAsync(role);
        }


        public override Task DeleteAsync(ApplicationRole role)
        {
            role.ObjectState = ObjectState.Deleted;
            return base.DeleteAsync(role);
        }
    }
}