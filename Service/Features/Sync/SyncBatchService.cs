using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;

namespace Service.Features.Sync
{
    public class SyncBatchService : ServiceBase<SyncBatch>, ISyncBatchService
    {
        private readonly ISyncBatchRepository _batchRepository;

        public SyncBatchService(ISyncBatchRepository batchRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(batchRepository, httpUser, auditService)
        {
            _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        }
    }
}
