using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Mappers;

namespace Template.Middleware
{
    public static class ConfigureAutoMapperServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            services.AddAutoMapper(typeof(RequestToDomainProfile));
            services.AddAutoMapper(typeof(DomainToResponseProfile));
        }
    }
}