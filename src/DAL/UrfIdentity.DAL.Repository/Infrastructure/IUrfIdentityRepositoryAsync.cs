using Repository.Pattern.Infrastructure;
using Repository.Pattern.Repositories;


namespace UrfIdentity.DAL.Repository.Infrastructure
{
    public interface IUrfIdentityRepositoryAsync<TEntity>
        : IRepositoryAsync<TEntity> where TEntity : class, IObjectState
    {
    }
}