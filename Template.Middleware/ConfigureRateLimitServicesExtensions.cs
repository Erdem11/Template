using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using AspNetCoreRateLimit.Redis;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public class ConfigureRateLimitServicesExtensions
    {
        public static void Configure(IServiceCollection services, SettingsHolder settings)
        {
            services.AddOptions();

            _ = settings.MyServices.Redis ? ConfigureRedis(services, settings) : ConfigureInMemory(services, settings);

            services.AddSingleton<IRateLimitConfiguration, CustomRateLimitingConfiguration>();
        }

        private static bool ConfigureRedis(IServiceCollection services, SettingsHolder settings)
        {
            services.AddDistributedMemoryCache();

            LoadConfiguration(services, settings);

            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(settings.RedisSettings.ConnectionString));
            services.AddRedisRateLimiting();

            return true;
        }

        private static bool ConfigureInMemory(IServiceCollection services, SettingsHolder settings)
        {
            services.AddMemoryCache();

            LoadConfiguration(services, settings);

            services.AddMemoryCache();
            services.AddInMemoryRateLimiting();

            return true;
        }

        private static void LoadConfiguration(IServiceCollection services, SettingsHolder settings)
        {
            services.Configure<ClientRateLimitOptions>(settings.Configuration.GetSection(nameof(ClientRateLimitOptions)));
            services.Configure<ClientRateLimitPolicies>(settings.Configuration.GetSection(nameof(ClientRateLimitPolicies)));
        }
    }

    public class CustomRateLimitingConfiguration : RateLimitConfiguration
    {
        public CustomRateLimitingConfiguration(IOptions<IpRateLimitOptions> ipOptions, IOptions<ClientRateLimitOptions> clientOptions) : base(ipOptions, clientOptions)
        {
        }

        public override void RegisterResolvers()
        {
            base.RegisterResolvers();
            ClientResolvers.Add(new CustomClientResolveContributor());
        }
    }

    public class CustomClientResolveContributor : IClientResolveContributor
    {
        public Task<string> ResolveClientAsync(HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault();

            var result = token ?? httpContext.Connection.RemoteIpAddress?.ToString();

            return Task.FromResult(result);
        }
    }
}