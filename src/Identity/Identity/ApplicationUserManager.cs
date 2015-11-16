using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Repository.Pattern.Infrastructure;
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