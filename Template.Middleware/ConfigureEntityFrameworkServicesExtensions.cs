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
                if (settingsHolder.SqlSettings.PrimaryDb.Enabled)
                {
                    if (settingsHolder.SqlSettings.PrimaryDb.Mssql)
                    {
                        options.UseSqlServer(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        return;
                    }

                    options.UseNpgsql(settingsHolder.SqlSettings.PrimaryDb.ConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                }
                
                if (settingsHolder.SqlSettings.SecondaryDb.Enabled)
                {
                    if (settingsHolder.SqlSettings.SecondaryDb.Mssql)
                    {
                        options.UseSqlServer(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        return;
                    }

                    options.UseNpgsql(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                    
                    return;
                }
            });
            
            // secondary db configuration
            services.AddDbContext<MessagingContext>(options => {
                if (settingsHolder.SqlSettings.SecondaryDb.Enabled)
                {
                    if (settingsHolder.SqlSettings.SecondaryDb.Mssql)
                    {
                        options.UseSqlServer(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        return;
                    }

                    options.UseNpgsql(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                    
                    return;
                }
                
                if (settingsHolder.SqlSettings.PrimaryDb.Enabled)
                {
                    if (settingsHolder.SqlSettings.PrimaryDb.Mssql)
                    {
                        options.UseSqlServer(settingsHolder.SqlSettings.SecondaryDb.ConnectionString,
                        b => b.MigrationsAssembly("Template.Data"));
                        return;
                    }

                    options.UseNpgsql(settingsHolder.SqlSettings.PrimaryDb.ConnectionString,
                    b => b.MigrationsAssembly("Template.Data"));
                }
            });
        }
    }
}