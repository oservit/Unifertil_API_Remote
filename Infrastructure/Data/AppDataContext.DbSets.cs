using Domain.Features.Authentication;
using Domain.Features.Products;
using Domain.Features.Sync;
using Domain.Features.Sync.Views;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class AppDataContext
    {
        public required DbSet<Product> Products { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<SyncLog> SyncLogs { get; set; }
        public required DbSet<SyncHash> SyncHashs { get; set; }
        public required DbSet<SyncBatch> SyncBatchs { get; set; }
        public required DbSet<SyncNode> SyncNodes { get; set; }
        public required DbSet<SyncRoute> SyncRoutes { get; set; }
        public required DbSet<SyncViewRouteUser> SyncViewRoutesUsers { get; set; }

    }
}
