using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using UrfIdentity.Models;


namespace Identity.Extensions
{
    public static class ApplicationUserExtension
    {
        public static async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            this ApplicationUser user, UserManager<ApplicationUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in 
            // CookieAuthenticationOptions.AuthenticationType
            var userIdentity =
                await
                    manager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);

            // Add custom user claims here

            return userIdentity;
        }
    }
}