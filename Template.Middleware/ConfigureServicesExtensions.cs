using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.HealthChecks;

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
            ConfigureRateLimitServicesExtensions.Configure(services, settings);

            
            services.AddSignalR().AddStackExchangeRedis(settings.RedisSettings.ConnectionString, options => {
                options.Configuration.ChannelPrefix = "Template";
            });
            
            services.AddHealthChecks()
                .AddCheck<RedisHealthCheck>("redis")
                .AddCheck<DbContextHealthCheck>("database");

            return settings;
        }


        private static SettingsHolder LoadSettings(IServiceCollection services, IConfiguration configuration)
        {
            var settings = new SettingsHolder();
            settings.Configuration = configuration;
            configuration.GetSection(nameof(MyServices)).Bind(settings.MyServices);
            configuration.GetSection(nameof(CacheSettings)).Bind(settings.CacheSettings);
            configuration.GetSection(nameof(JwtSettings)).Bind(settings.JwtSettings);
            configuration.GetSection(nameof(LoggingSettings)).Bind(settings.LoggingSettings);
            configuration.GetSection(nameof(SqlSettings)).Bind(settings.SqlSettings);
            configuration.GetSection(nameof(RedisSettings)).Bind(settings.RedisSettings);
            configuration.GetSection(nameof(ClientRateLimitOptions)).Bind(settings.ClientRateLimitOptions);
            configuration.GetSection(nameof(ClientRateLimitPolicies)).Bind(settings.ClientRateLimitPolicies);
            configuration.GetSection(nameof(IpRateLimitOptions)).Bind(settings.IpRateLimitOptions);
            configuration.GetSection(nameof(IpRateLimitPolicies)).Bind(settings.IpRateLimitPolicies);

            services.AddSingleton(settings);
            return settings;
        }
    }

}