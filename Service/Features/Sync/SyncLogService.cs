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
        private readonly IHttpUserAccessor _httpUser;
        private readonly IAuditService _auditService;

        public SyncLogService(ISyncLogRepository logRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(logRepository, httpUser, auditService)
        {
            _logRepository = logRepository ?? throw new ArgumentNullException(nameof(logRepository));
            _httpUser = httpUser ?? throw new ArgumentNullException(nameof(httpUser));
            _auditService = auditService ?? throw new ArgumentNullException(nameof(auditService));
        }
    }
}
