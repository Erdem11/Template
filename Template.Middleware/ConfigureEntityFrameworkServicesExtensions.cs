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
            // add EntityFramework
            services.AddDbContext<TemplateContext>(options => {
                if (settingsHolder.SqlSettings.Mssql)
                {
                    options.UseSqlServer(settingsHolder.SqlSettings.MssqlConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                    return;
                }

                if (settingsHolder.SqlSettings.Npgsql)
                {
                    options.UseNpgsql(settingsHolder.SqlSettings.NpgsqlConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                    return;
                }
            });
        }
    }
}