using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;

namespace Template.Middleware
{
    public static class ConfigureEntityFrameworkServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settingsHolder)
        {
            // primary db configuration
            services.AddDbContext<TemplateContext>(options => {
                var dbOptions = settingsHolder.SqlSettings.GetPrimary();

                switch (dbOptions.DbType)
                {
                    case DbTypes.Mssql:
                        options.UseSqlServer(dbOptions.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        break;
                    case DbTypes.Npgsql:
                        options.UseNpgsql(dbOptions.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });

            // secondary db configuration
            services.AddDbContext<MessagingContext>(options => {
                var dbOptions = settingsHolder.SqlSettings.GetSecondary();
                
                switch (dbOptions.DbType)
                {
                    case DbTypes.Mssql:
                        options.UseSqlServer(dbOptions.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        break;
                    case DbTypes.Npgsql:
                        options.UseNpgsql(dbOptions.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}