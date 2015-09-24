using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Repository.Pattern.Repositories;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Service.Interfaces
{
    public interface IApplicationRoleService
    {
        ApplicationRole Find(params object[] keyValues);
        IQueryable<ApplicationRole> SelectQuery(string query, params object[] parameters);
        void Insert(ApplicationRole entity);
        void InsertRange(IEnumerable<ApplicationRole> entities);
        void InsertOrUpdateGraph(ApplicationRole entity);
        void InsertGraphRange(IEnumerable<ApplicationRole> entities);
        void Update(ApplicationRole entity);
        void Delete(object id);
        void Delete(ApplicationRole entity);
        IQueryFluent<ApplicationRole> Query();
        IQueryFluent<ApplicationRole> Query(IQueryObject<ApplicationRole> queryObject);
        IQueryFluent<ApplicationRole> Query(Expression<Func<ApplicationRole, bool>> query);
        Task<ApplicationRole> FindAsync(params object[] keyValues);

        Task<ApplicationRole> FindAsync(CancellationToken cancellationToken,
            params object[] keyValues);

        Task<bool> DeleteAsync(params object[] keyValues);

        Task<bool> DeleteAsync(CancellationToken cancellationToken,
            params object[] keyValues);

        IQueryable<ApplicationRole> Queryable();
    }
}