using Domain;
using Domain.Base;
using Infrastructure.Repositories.Base;
using Infrastructure.Services.Audit;
using Infrastructure.Http;

namespace Service.Base
{
    public abstract class ServiceBase<T> : SelectService<T>, IDisposable, IServiceBase<T> where T : class, IEntityBase
    {
        private readonly new IRepositoryBase<T> _repository;
        private readonly IHttpUserAccessor _httpUser;
        private readonly IAuditService _auditService;

        public ServiceBase(IRepositoryBase<T> repository, IHttpUserAccessor httpUser, IAuditService auditService) : base(repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _httpUser= httpUser ?? throw new ArgumentNullException(nameof(httpUser));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }

        public virtual async Task<int> Save(T obj)
        {
            _auditService.SetCreated(obj);
            return await _repository.Save(obj);
        }

        public virtual async Task<int> SaveList(List<T> obj)
        {
            return await _repository.SaveList(obj);
        }

        public virtual async Task<int> Update(T obj)
        {
            _auditService.SetModified(obj);
            return await _repository.Update(obj);
        }
        public virtual async Task<int> Delete(long id)
        {
            return await _repository.Delete(id);
        }
    }
}