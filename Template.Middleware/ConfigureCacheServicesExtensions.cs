using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.Caching;
using Template.Caching.InMemoryCaching;
using Template.Caching.RedisCaching;

namespace Template.Middleware
{
    public static class ConfigureCacheServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            var cachingSettings = new CacheSettings();
            configuration.GetSection(nameof(CacheSettings)).Bind(cachingSettings);
            services.AddSingleton(cachingSettings);

            if (!cachingSettings.Enabled)
            {
                return;
            }

            if (cachingSettings.Redis)
            {
                ConfigureRedis(services, configuration);
                return;
            }

            if (cachingSettings.InMemory)
            {
                ConfigureInMemory(services, configuration);
                return;
            }
        }

        private static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled)
            {
                return;
            }

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));
            services.AddSingleton<ICacheService, RedisCacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

        private static void ConfigureInMemory(this IServiceCollection services, IConfiguration configuration)
        {
            var inMemoryCacheSettings = new InMemoryCacheSettings();
            configuration.GetSection(nameof(InMemoryCacheSettings)).Bind(inMemoryCacheSettings);
            services.AddSingleton(inMemoryCacheSettings);

            if (!inMemoryCacheSettings.Enabled)
            {
                return;
            }

            services.AddSingleton<ICacheService, InMemoryCacheService>();
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}