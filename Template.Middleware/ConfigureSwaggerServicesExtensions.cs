using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Template.Middleware
{
    public static class ConfigureSwaggerServicesExtensions
    {
        public static void Configure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c => {
                // c.SwaggerDoc("v1", new OpenApiInfo
                // {
                //     Title = "Template.Api",
                //     Version = "v1"
                // });
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
        }
    }
}