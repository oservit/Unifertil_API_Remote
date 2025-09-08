using Domain.Features.Sync;
using Infrastructure.Services.Audit;
using Infrastructure.Http;
using Service.Common;
using Infrastructure.Repositories.Sync;
using Domain.Features.Sync.Views;

namespace Service.Features.Sync
{
    public class SyncViewRouteUserService : SelectService<SyncViewRouteUser>, ISyncViewRouteUserService
    {
        private readonly ISyncViewRouteUserRepository _viewRepository;

        public SyncViewRouteUserService(ISyncViewRouteUserRepository viewRepository, IHttpUserAccessor httpUser)
            : base(viewRepository, httpUser)
        {
            _viewRepository = viewRepository ?? throw new ArgumentNullException(nameof(viewRepository));
        }
    }
}
