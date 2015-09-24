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
    }
}