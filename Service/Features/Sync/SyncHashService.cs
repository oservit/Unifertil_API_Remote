using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;

namespace Service.Features.Sync
{
    public class SyncHashService : ServiceBase<SyncHash>, ISyncHashService
    {
        private readonly ISyncHashRepository _hashRepository;
        private readonly IHttpUserAccessor _httpUser;
        private readonly IAuditService _auditService;

        public SyncHashService(ISyncHashRepository hashRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(hashRepository, httpUser, auditService)
        {
            _hashRepository = hashRepository ?? throw new ArgumentNullException(nameof(hashRepository));
            _httpUser = httpUser ?? throw new ArgumentNullException(nameof(httpUser));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }

        public override async Task<int> SaveOrUpdate(SyncHash obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var existing = await _hashRepository.Get(x => x.RecordId == obj.RecordId && x.EntityId == obj.EntityId);

            if (existing != null)
            {
                existing.HashValue = obj.HashValue;
                existing.OperationId = obj.OperationId;
                existing.OperationDate = obj.OperationDate;

                _auditService.SetModified(existing);

                return await _hashRepository.Update(existing);
            }
            else
            {
                _auditService.SetCreated(obj);

                return await _hashRepository.Save(obj);
            }
        }
    }
}
