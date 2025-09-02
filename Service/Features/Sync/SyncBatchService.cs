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
        private readonly IHttpUserAccessor _httpUser;
        private readonly IAuditService _auditService;

        public SyncBatchService(ISyncBatchRepository batchRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(batchRepository, httpUser, auditService)
        {
            _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
            _httpUser = httpUser ?? throw new ArgumentNullException(nameof(httpUser));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }
    }
}
