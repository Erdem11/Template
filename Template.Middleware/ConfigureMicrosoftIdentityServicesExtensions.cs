using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Data;
using Template.Domain.Dto.IdentityModels;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureMicrosoftIdentityServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            // add MicrosoftIdentity with EntityFramework
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TemplateContext>();

            // add identity service di
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}