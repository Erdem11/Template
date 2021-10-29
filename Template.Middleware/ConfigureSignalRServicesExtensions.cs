using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureSignalRServicesExtensions
    {
        public static void Configure(IServiceCollection services, SettingsHolder settings)
        {
            if (settings.RedisSettings.Enabled)
            {
                services.AddSignalR()
                    .AddStackExchangeRedis(settings.RedisSettings.ConnectionString, options => {
                        options.Configuration.ChannelPrefix = "SignalRTemplateMessaging";
                    });
                return;
            }

            services.AddSignalR();
        }
    }
}