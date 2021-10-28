using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Template.Common.SettingsConfigurationFiles;

namespace Template.Middleware
{
    public static class ConfigureSwaggerServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {
            services.AddSwaggerGen(c => {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                var apiXmlFile = $"Template.Api.xml";
                var apiXmlPath = Path.Combine(AppContext.BaseDirectory, apiXmlFile);
                c.IncludeXmlComments(apiXmlPath);

                var commonXmlFile = $"Template.Common.xml";
                var commonXmlPath = Path.Combine(AppContext.BaseDirectory, commonXmlFile);
                c.IncludeXmlComments(commonXmlPath);
            });

            // Configure swagger for multiple descriptions i.e api versioning
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }

    /// <summary>
    /// Configure swagger for multiple descriptions i.e api versioning
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion))
            {
                options.SwaggerDoc(
                description.GroupName,
                new OpenApiInfo()
                {
                    Title = $"Template API {description.ApiVersion}",
                    Version = description.ApiVersion.ToString(),
                });
            }
        }
    }
}