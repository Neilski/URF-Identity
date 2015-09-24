using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Service.Pattern;
using UrfIdentity.DAL.Repository;
using UrfIdentity.DAL.Repository.Infrastructure;
using UrfIdentity.DAL.Service.Interfaces;
using UrfIdentity.Models;
using UrfIdentity.Models.ViewModels;


namespace UrfIdentity.DAL.Service
{
    public class ApplicationUserService
        : Service<ApplicationUser>, IApplicationUserService
    {
        private readonly IUrfIdentityRepositoryAsync<ApplicationUser> _repository;


        public ApplicationUserService(
            IUrfIdentityRepositoryAsync<ApplicationUser> repository)
            : base(repository)
        {
            _repository = repository;
        }


        public async Task<ApplicationUser>
            GetUserDetailsAsync(Guid userId)
        {
            return await _repository
                .Queryable()
                .Include(e => e.UserDetails)
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Id == userId);
        }

        public IEnumerable<ApplicationUserViewModel>
            GetUsersInRole(Guid roleId)
        {
            return _repository.GetUsersInRole(roleId);
        }
    }
}