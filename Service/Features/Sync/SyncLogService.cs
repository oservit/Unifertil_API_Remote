using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;

namespace Service.Features.Sync
{
    public class SyncLogService : ServiceBase<SyncLog>, ISyncLogService
    {
        private readonly ISyncLogRepository _logRepository;

        public SyncLogService(ISyncLogRepository logRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(logRepository, httpUser, auditService)
        {
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
        }
    }
}
