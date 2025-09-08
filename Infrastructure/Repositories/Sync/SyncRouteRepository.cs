using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Domain.Features.Sync;

namespace Infrastructure.Repositories.Sync
{
    public class SyncRouteRepository : RepositoryBase<SyncRoute>, ISyncRouteRepository
    {
        public SyncRouteRepository(AppDataContext context) : base(context)
        {
        }
    }
}
