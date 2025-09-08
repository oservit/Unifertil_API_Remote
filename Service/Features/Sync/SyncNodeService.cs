using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;

namespace Service.Features.Sync
{
    public class SyncNodeService : ServiceBase<SyncNode>, ISyncNodeService
    {
        private readonly ISyncNodeRepository _nodeRepository;

        public SyncNodeService(ISyncNodeRepository nodeRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(nodeRepository, httpUser, auditService)
        {
            _nodeRepository = nodeRepository ?? throw new ArgumentNullException(nameof(nodeRepository));
        }
    }
}
