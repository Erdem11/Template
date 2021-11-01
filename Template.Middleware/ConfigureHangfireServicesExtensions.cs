using System;
using System.Linq;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Hangfire.SQLite;
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
            if (settingsHolder.MyServices.Redis)
            {
                services.AddHangfire(x => x.UseRedisStorage(settingsHolder.RedisSettings.ConnectionString));
                services.AddHangfireServer();
                return;
            }

            var dbOptions = settingsHolder.SqlSettings.GetSecondary();
            switch (dbOptions.DbType)
            {
                case DbTypes.Mssql:
                    services.AddHangfire(x => x.UseSqlServerStorage(dbOptions.ConnectionString));
                    break;
                case DbTypes.Npgsql:
                    services.AddHangfire(x => x.UsePostgreSqlStorage(dbOptions.ConnectionString));
                    break;
                case DbTypes.Sqlite:
                    services.AddHangfire(x => x.UseSQLiteStorage(dbOptions.ConnectionString));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            var tokenModel = context.GetHttpContext().GetTokenModel();
       
            return tokenModel.CustomClaims.Contains(ClaimConstants.Hangfire);
        }
    }
}