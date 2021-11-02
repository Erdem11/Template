using System;
using System.Linq;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Hangfire.SQLite;
using Hangfire.SqlServer;
using Microsoft.Extensions.DependencyInjection;
using Template.BackgroundTasks;
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
                    JobStorage.Current = new SqlServerStorage(dbOptions.ConnectionString);
                    break;
                case DbTypes.Npgsql:
                    services.AddHangfire(x => x.UsePostgreSqlStorage(dbOptions.ConnectionString));
                    JobStorage.Current = new PostgreSqlStorage(dbOptions.ConnectionString);
                    break;
                case DbTypes.Sqlite:
                    services.AddHangfire(x => x.UseSQLiteStorage(dbOptions.ConnectionString));
                    JobStorage.Current = new SQLiteStorage(dbOptions.ConnectionString);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            services.AddHangfireServer();
            
            RecurringJob.AddOrUpdate<HangfireCronJobs>(x=>x.UnPauseApp(),  Cron.Monthly);
        }
    }

    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        { 
            var tokenModel = context.GetHttpContext().GetTokenModel();
       
            return tokenModel.CustomClaims.Contains(ClaimConstants.Hangfire);
        }
    }
}