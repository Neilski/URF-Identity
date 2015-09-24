using System;
using Microsoft.AspNet.Identity;
using UrfIdentity.Models;


namespace Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser, Guid>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, Guid> store)
            : base(store)
        {
        }
    }
}