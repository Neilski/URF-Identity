using System;
using Microsoft.AspNet.Identity;
using UrfIdentity.Models;


namespace Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole, Guid>
    {
        public ApplicationRoleManager(
            IRoleStore<ApplicationRole, Guid> roleStore)
            : base(roleStore)
        {
        }
    }
}