using Infrastructure.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Scope
{
    public class LoggingScope
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<IAppLogger, ConsoleAppLogger>();
        }
    }
}
