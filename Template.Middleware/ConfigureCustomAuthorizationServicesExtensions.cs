using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Middleware
{
    public static class ConfigureCustomAuthorizationServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("BookViewer", builder => builder.RequireClaim("books.view", "true"));
            });
        }
    }
}