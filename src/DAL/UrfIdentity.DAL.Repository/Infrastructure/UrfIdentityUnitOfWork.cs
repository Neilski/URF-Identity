using Repository.Pattern.Ef6;
using UrfIdentity.DAL.Db;


namespace UrfIdentity.DAL.Repository.Infrastructure
{
    public class UrfIdentityUnitOfWork : UnitOfWork, IUrfIdentityUnitOfWorkAsync
    {
        public UrfIdentityUnitOfWork(IUrfIdentityDataContextAsync dataContextAsync)
            : base(dataContextAsync)
        {
        }
    }
}