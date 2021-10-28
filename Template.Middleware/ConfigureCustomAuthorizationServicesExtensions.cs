using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureCustomAuthorizationServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("BookViewer", builder => builder.RequireClaim("books.view", "true"));
            });
        }
    }
}