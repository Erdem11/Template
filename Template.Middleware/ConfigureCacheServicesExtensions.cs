using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.Caching;
using Template.Caching.RedisCaching;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureCacheServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
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
            
            // services.AddStackExchangeRedisCache(x => x.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }

}