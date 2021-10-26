using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.Middleware
{
    public static class ConfigureApiVersioningServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiVersioning(x => {
                x.DefaultApiVersion = new ApiVersion(1, 1);
                x.ReportApiVersions = true;
                x.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                // x.ApiVersionReader = new MediaTypeApiVersionReader("X-Version");
                x.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
        }
    }

}