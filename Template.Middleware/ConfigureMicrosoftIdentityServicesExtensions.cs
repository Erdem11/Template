using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;
using Template.Domain.Dto.IdentityModels;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureMicrosoftIdentityServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            services.Configure<IdentityOptions>(x => {
                x.Password.RequireDigit = false;
                x.Password.RequiredLength = 1;
                x.Password.RequireLowercase = false;
                x.Password.RequireUppercase = false;
                x.Password.RequiredUniqueChars = 0;
                x.Password.RequireNonAlphanumeric = false;
            });
            
            // add MicrosoftIdentity with EntityFramework
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TemplateContext>();

            // add identity service di
            services.AddScoped<IIdentityService, IdentityService>();
        }
    }
}