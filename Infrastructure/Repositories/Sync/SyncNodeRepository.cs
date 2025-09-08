using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Domain.Features.Sync;

namespace Infrastructure.Repositories.Sync
{
    public class SyncNodeRepository : RepositoryBase<SyncNode>, ISyncNodeRepository
    {
        public SyncNodeRepository(AppDataContext context) : base(context)
        {
        }
    }
}
