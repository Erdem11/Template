using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Middleware
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            ConfigureSwaggerServicesExtensions.Configure(services, configuration);
            ConfigureJwtAuthServicesExtensions.Configure(services, configuration);
            ConfigureCustomAuthorizationServicesExtensions.Configure(services, configuration);
            ConfigureMicrosoftIdentityServicesExtensions.Configure(services, configuration);
            ConfigureEntityFrameworkServicesExtensions.Configure(services, configuration);
            ConfigureCustomServicesExtensions.Configure(services, configuration);
            ConfigureAutoMapperServicesExtensions.Configure(services, configuration);
            ConfigureCacheServicesExtensions.Configure(services, configuration);
            ConfigureApiVersioningServicesExtensions.Configure(services, configuration);
        }
    }
}