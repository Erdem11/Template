using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureCustomServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            // add custom services
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}