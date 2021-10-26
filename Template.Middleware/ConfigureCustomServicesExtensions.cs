using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureCustomServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            // add custom services
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ITagService, TagService>();
        }
    }
}