using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.Caching;
using Template.Caching.InMemoryCaching;
using Template.Caching.RedisCaching;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureCacheServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            if (!settings.MyServices.Cache)
            {
                return;
            }

            if (settings.CacheSettings.Redis)
            {
                ConfigureRedis(services, settings);
                return;
            }

            if (settings.CacheSettings.InMemory)
            {
                ConfigureInMemory(services, settings);
                return;
            }
        }

        private static void ConfigureRedis(this IServiceCollection services, SettingsHolder settings)
        {
            if (!settings.RedisSettings.Enabled)
            {
                return;
            }

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(settings.RedisSettings.ConnectionString));
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

        private static void ConfigureInMemory(this IServiceCollection services, SettingsHolder settings)
        {
            services.AddSingleton<ICacheService, InMemoryCacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}