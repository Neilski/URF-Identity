using Repository.Pattern.Ef6;
using Repository.Pattern.Infrastructure;
using UrfIdentity.DAL.Db;


namespace UrfIdentity.DAL.Repository.Infrastructure
{
    public class UrfIdentityRepository<TEntity>
        : Repository<TEntity>, IUrfIdentityRepositoryAsync<TEntity>
        where TEntity : class, IObjectState
    {
        public UrfIdentityRepository(
            IUrfIdentityDataContextAsync context,
            IUrfIdentityUnitOfWorkAsync unitOfWork)
            : base(context, unitOfWork)
        {
        }
    }
}