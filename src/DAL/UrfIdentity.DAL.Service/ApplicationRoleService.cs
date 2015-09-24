using Service.Pattern;
using UrfIdentity.DAL.Repository.Infrastructure;
using UrfIdentity.DAL.Service.Interfaces;
using UrfIdentity.Models;


namespace UrfIdentity.DAL.Service
{
    public class ApplicationRoleService
        : Service<ApplicationRole>, IApplicationRoleService
    {
        private readonly IUrfIdentityRepositoryAsync<ApplicationRole> _repository;

        public ApplicationRoleService(
            IUrfIdentityRepositoryAsync<ApplicationRole> repository)
            : base(repository)
        {
            _repository = repository;
        }
    }
}