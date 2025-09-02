using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Domain.Features.Sync;

namespace Infrastructure.Repositories.Sync
{
    public class SyncBatchRepository : RepositoryBase<SyncBatch>, ISyncBatchRepository
    {
        public SyncBatchRepository(AppDataContext context) : base(context)
        {
        }
    }
}
