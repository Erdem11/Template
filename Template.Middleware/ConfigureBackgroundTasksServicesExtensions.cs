using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Template.BackgroundTasks;

namespace Template.Middleware
{
    public static class ConfigureBackgroundTasksServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            var redisSubscriberSettings = new RedisSubscriberSettings();
            configuration.GetSection(nameof(RedisSubscriberSettings)).Bind(redisSubscriberSettings);
            services.AddSingleton(redisSubscriberSettings);
            
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(redisSubscriberSettings.ConnectionString));
            services.AddHostedService<RedisSubscriber>();
        }
    }
}