using Infrastructure.Data;
using Infrastructure.Data.MySql;
using Infrastructure.Data.Oracle;
using Infrastructure.Data.SqlServer;
using Infrastructure.Repositories.Authentication;
using Infrastructure.Repositories.Base.Transactions;
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
        }

        private static void RegisterUnitsOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
