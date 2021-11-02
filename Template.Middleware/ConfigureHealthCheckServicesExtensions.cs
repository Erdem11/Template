using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.HealthChecks;

namespace Template.Middleware
{
    public static class ConfigureHealthCheckServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settingsHolder)
        {
            var health = services.AddHealthChecks()
                .AddCheck<DbContextHealthCheck>("database");

            if (settingsHolder.MyServices.Redis)
            {
                health.AddCheck<RedisHealthCheck>("redis");
            }
        }
    }
}