using Domain.Features.Authentication;
using Domain.Features.Products;
using Domain.Features.Sync;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public partial class AppDataContext
    {
        public required DbSet<Product> Products { get; set; }
        public required DbSet<User> Users { get; set; }
        public required DbSet<SyncLog> SyncLogs { get; set; }
    }
}
