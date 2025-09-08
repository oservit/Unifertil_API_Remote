using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;

namespace Service.Features.Sync
{
    public class SyncRouteService : ServiceBase<SyncRoute>, ISyncRouteService
    {
        private readonly ISyncRouteRepository _routeRepository;

        public SyncRouteService(ISyncRouteRepository routeRepository, IHttpUserAccessor httpUser, IAuditService auditService)
            : base(routeRepository, httpUser, auditService)
        {
            _routeRepository = routeRepository ?? throw new ArgumentNullException(nameof(routeRepository));
        }
    }
}
