using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.BackgroundTasks;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureBackgroundTasksServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            if (!settings.RedisSettings.Enabled)
                return;

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(settings.RedisSettings.ConnectionString));
            services.AddHostedService<RedisSubscriber>();
        }
    }
}