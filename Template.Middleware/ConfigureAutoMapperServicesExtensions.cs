using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Mappers;

namespace Template.Middleware
{
    public static class ConfigureAutoMapperServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(RequestToDomainProfile));
            services.AddAutoMapper(typeof(DomainToResponseProfile));
        }
    }
}