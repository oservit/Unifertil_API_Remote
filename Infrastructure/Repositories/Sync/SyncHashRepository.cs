using Infrastructure.Data;
using Infrastructure.Repositories.Common;
using Domain.Features.Sync;

namespace Infrastructure.Repositories.Sync
{
    public class SyncHashRepository : RepositoryBase<SyncHash>, ISyncHashRepository
    {
        public SyncHashRepository(AppDataContext context) : base(context)
        {

        }
    }
}
