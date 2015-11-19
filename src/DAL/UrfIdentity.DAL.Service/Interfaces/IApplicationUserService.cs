using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Service.Pattern;
using UrfIdentity.Models;
using UrfIdentity.Models.ViewModels;


namespace UrfIdentity.DAL.Service.Interfaces
{
    public interface IApplicationUserService
        :IService<ApplicationUser>
    {
        Task<ApplicationUser> GetUserDetailsAsync(Guid userId);
        IEnumerable<ApplicationUserViewModel> GetUsersInRole(Guid roleId);
    }
}