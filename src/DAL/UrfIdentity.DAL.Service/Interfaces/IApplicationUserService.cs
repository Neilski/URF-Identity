using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using UrfIdentity.Models;
using UrfIdentity.Models.ViewModels;


namespace UrfIdentity.DAL.Service.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetUserDetailsAsync(Guid userId);
        IEnumerable<ApplicationUserViewModel> GetUsersInRole(Guid roleId);

        ApplicationUser Find(params object[] keyValues);
        IQueryable<ApplicationUser> SelectQuery(string query, params object[] parameters);
        void Insert(ApplicationUser entity);
        void InsertRange(IEnumerable<ApplicationUser> entities);
        void InsertOrUpdateGraph(ApplicationUser entity);
        void InsertGraphRange(IEnumerable<ApplicationUser> entities);
        void Update(ApplicationUser entity);
        void Delete(object id);
        void Delete(ApplicationUser entity);
        IQueryFluent<ApplicationUser> Query();
        IQueryFluent<ApplicationUser> Query(IQueryObject<ApplicationUser> queryObject);
        IQueryFluent<ApplicationUser> Query(Expression<Func<ApplicationUser, bool>> query);
        Task<ApplicationUser> FindAsync(params object[] keyValues);

        Task<ApplicationUser> FindAsync(CancellationToken cancellationToken,
            params object[] keyValues);

        Task<bool> DeleteAsync(params object[] keyValues);

        Task<bool> DeleteAsync(CancellationToken cancellationToken,
            params object[] keyValues);

        IQueryable<ApplicationUser> Queryable();
    }
}