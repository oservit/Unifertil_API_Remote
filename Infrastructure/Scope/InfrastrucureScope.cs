using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scope
{
    public class InfrastrucureScope
    {

        public static void Register(IServiceCollection services)
        {
            ServiceScope.Register(services);
            RepositoryScope.Register(services);
            HttpScope.Register(services);
            LoggingScope.Register(services);
        }

    }
}
