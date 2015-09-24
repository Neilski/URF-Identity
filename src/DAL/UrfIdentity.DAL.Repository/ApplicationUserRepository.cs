using System;
using System.Collections.Generic;
using System.Linq;
using UrfIdentity.DAL.Repository.Infrastructure;
using UrfIdentity.DAL.Repository.Queries;
using UrfIdentity.Models;
using UrfIdentity.Models.ViewModels;


namespace UrfIdentity.DAL.Repository
{
    public static class ApplicationUserRepository
    {
        public static IEnumerable<ApplicationUserViewModel>
            GetUsersInRole(
            this IUrfIdentityRepositoryAsync<ApplicationUser> repository,
            Guid roleId)
        {
            var query = new ApplicationUserQuery().SelectByRole(roleId);

            return repository
                .Query(query)
                .OrderBy(
                    u =>
                        u.OrderBy(x => x.UserDetails.LastName)
                            .ThenBy(x => x.UserDetails.FirstName))
                .Select(
                    u =>
                        new ApplicationUserViewModel
                        {
                            UserId = u.Id,
                            Email = u.Email,
                            FirstName = u.UserDetails.FirstName,
                            LastName = u.UserDetails.LastName
                        });
        }
    }
}