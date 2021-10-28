using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;

namespace Template.Middleware
{
    public static class ConfigureEntityFrameworkServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            // add EntityFramework
            services.AddDbContext<TemplateContext>(options => {
                // options.EnableSensitiveDataLogging();
                options.UseSqlServer(settings.MsSqlSettings.ConnectionString,
                b => b.MigrationsAssembly("Template.Data"));
            });
        }
    }
}