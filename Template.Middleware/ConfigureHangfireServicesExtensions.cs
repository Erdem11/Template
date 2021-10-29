using System.Linq;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.Extensions.DependencyInjection;
using Template.Common;
using Template.Common.SettingsConfigurationFiles;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureHangfireServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settingsHolder)
        {
            if (settingsHolder.RedisSettings.Enabled)
            {
                services.AddHangfire(x => x.UseRedisStorage(settingsHolder.RedisSettings.ConnectionString));
                services.AddHangfireServer();
                return;
            }

            if (settingsHolder.SqlSettings.SecondaryDb.Enabled)
            {
                if (settingsHolder.SqlSettings.SecondaryDb.Mssql)
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(settingsHolder.SqlSettings.SecondaryDb.ConnectionString));
                    return;
                }

                services.AddHangfire(x => x.UsePostgreSqlStorage(settingsHolder.SqlSettings.SecondaryDb.ConnectionString));
                return;
            }
            
            if (settingsHolder.SqlSettings.PrimaryDb.Enabled)
            {
                if (settingsHolder.SqlSettings.PrimaryDb.Mssql)
                {
                    services.AddHangfire(x => x.UseSqlServerStorage(settingsHolder.SqlSettings.PrimaryDb.ConnectionString));
                    return;
                }

                services.AddHangfire(x => x.UsePostgreSqlStorage(settingsHolder.SqlSettings.PrimaryDb.ConnectionString));
                return;
            }

            services.AddHangfireServer();
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly IIdentityService _identityService;

        public MyAuthorizationFilter(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault(x => x.ToLower().Contains("bearer "));
            token = token?.Replace("Bearer ", "");

            var principalsToken = _identityService.GetPrincipalFromToken(token);

            var canSeeHangfireValue = principalsToken?.Claims.FirstOrDefault(x => x.Type == ClaimConstants.Hangfire)?.Value;
            return bool.TryParse(canSeeHangfireValue, out var canSeeHangfire) && canSeeHangfire;
        }
    }
}