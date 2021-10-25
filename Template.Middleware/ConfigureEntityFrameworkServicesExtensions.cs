using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Data;

namespace Template.Middleware
{
    public static class ConfigureEntityFrameworkServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            // add EntityFramework
            services.AddDbContext<TemplateContext>(options => {
                // options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Template.Data"));
            });
        }
    }
}