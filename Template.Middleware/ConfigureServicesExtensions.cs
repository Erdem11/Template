using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureServicesExtensions
    {
        public static SettingsHolder ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = LoadSettings(services, configuration);

            services.AddControllers();
            ConfigureSwaggerServicesExtensions.Configure(services, settings);
            ConfigureJwtAuthServicesExtensions.Configure(services, settings);
            ConfigureCustomAuthorizationServicesExtensions.Configure(services, settings);
            ConfigureMicrosoftIdentityServicesExtensions.Configure(services, settings);
            ConfigureEntityFrameworkServicesExtensions.Configure(services, settings);
            ConfigureCustomServicesExtensions.Configure(services, settings);
            ConfigureAutoMapperServicesExtensions.Configure(services, settings);
            ConfigureCacheServicesExtensions.Configure(services, settings);
            ConfigureApiVersioningServicesExtensions.Configure(services, settings);
            ConfigureBackgroundTasksServicesExtensions.Configure(services, settings);
            ConfigureHangfireServicesExtensions.Configure(services, settings);

            return settings;
        }

        private static SettingsHolder LoadSettings(IServiceCollection services, IConfiguration configuration)
        {
            var settings = new SettingsHolder();
            settings.Configuration = configuration;
            configuration.GetSection(nameof(CacheSettings)).Bind(settings.CacheSettings);
            configuration.GetSection(nameof(JwtSettings)).Bind(settings.JwtSettings);
            configuration.GetSection(nameof(LoggingSettings)).Bind(settings.LoggingSettings);
            configuration.GetSection(nameof(MsSqlSettings)).Bind(settings.MsSqlSettings);
            configuration.GetSection(nameof(RedisSettings)).Bind(settings.RedisSettings);
            services.AddSingleton(settings);
            return settings;
        }
    }
}