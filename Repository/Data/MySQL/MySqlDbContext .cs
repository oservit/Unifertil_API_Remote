using Domain.Base;
using Domain.Base.Enums;
using Domain.Settings;
using Infrastructure.Data.Base;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Data.MySql
{
    public class MySqlDbContext : BaseDbContext
    {
        public MySqlDbContext(IOptions<AppSettings> settings, string connectionName = "MySql")
            : base(settings, connectionName, SgbdType.MySql)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(_connection.ConnectionString, ServerVersion.AutoDetect(_connection.ConnectionString));

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
