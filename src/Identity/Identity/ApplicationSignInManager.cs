using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Identity.Extensions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using UrfIdentity.Models;


namespace Identity
{
    // Configure the application sign-in manager which is used in this 
    // application.
    public class ApplicationSignInManager :
        SignInManager<ApplicationUser, Guid>
    {
        public ApplicationSignInManager(
            ApplicationUserManager userManager,
            IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }


        public override Task<ClaimsIdentity> CreateUserIdentityAsync(
            ApplicationUser user)
        {
            return
                user.GenerateUserIdentityAsync(
                    (ApplicationUserManager) UserManager);
        }
    }
}