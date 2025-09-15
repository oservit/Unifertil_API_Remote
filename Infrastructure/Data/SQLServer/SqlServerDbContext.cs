using Domain.Common;
using Domain.Common.Enums;
using Domain.Settings;
using Infrastructure.Data.Common;
using Infrastructure.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.SqlServer
{
    public class SqlServerDbContext : BaseDbContext
    {
        public SqlServerDbContext(IOptions<AppSettings> settings, string connectionName = "SqlServer")
            : base(settings, connectionName, SgbdType.SqlServer)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connection.ConnectionString);

#if DEBUG
                optionsBuilder
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(new SqlLoggingInterceptor());
#endif
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!string.IsNullOrWhiteSpace(_connection.Schema))
                modelBuilder.HasDefaultSchema(_connection.Schema);
        }
    }
}
