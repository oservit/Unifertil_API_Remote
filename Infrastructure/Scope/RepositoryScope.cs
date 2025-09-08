using Domain.Features.Sync;
using Infrastructure.Data;
using Infrastructure.Data.MySql;
using Infrastructure.Data.Oracle;
using Infrastructure.Data.SqlServer;
using Infrastructure.Repositories.Authentication;
using Infrastructure.Repositories.Base.Transactions;
using Infrastructure.Repositories.Products;
using Infrastructure.Repositories.Sync;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scope
{
    public class RepositoryScope
    {
        public static void Register(IServiceCollection services)
        {
            RegisterContexts(services);
            RegisterRepositories(services);
            RegisterUnitsOfWork(services);
        }

        private static void RegisterContexts(IServiceCollection services)
        {
            services.AddScoped<AppDataContext>();
            services.AddScoped<ExternalDataContext>();

            services.AddScoped<OracleDbContext>();
            services.AddScoped<MySqlDbContext>();
            services.AddScoped<SqlServerDbContext>();
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProductRepositoryRepository, ProductRepository>();
            services.AddScoped<ISyncLogRepository, SyncLogRepository>();
            services.AddScoped<ISyncHashRepository, SyncHashRepository>();
            services.AddScoped<ISyncBatchRepository, SyncBatchRepository>();

            services.AddScoped<ISyncRouteRepository, SyncRouteRepository>();
            services.AddScoped<ISyncNodeRepository, SyncNodeRepository>();
            services.AddScoped<ISyncViewRouteUserRepository, SyncViewRouteUserRepository>();
        }

        private static void RegisterUnitsOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
