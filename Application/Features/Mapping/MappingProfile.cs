using Microsoft.Extensions.DependencyInjection;

namespace Application.Features.Mapping
{
    public static class ApplicationMappingScope
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(
                typeof(Application.Features.Authentication.Mapping.AuthenticationMappingProfile)
            // Adicione outros profiles aqui
            );
        }
    }
}

