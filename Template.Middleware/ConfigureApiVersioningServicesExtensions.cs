using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureApiVersioningServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            services.AddApiVersioning(x => {
                x.DefaultApiVersion = new ApiVersion(1, 1);
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                x.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        }
    }

}