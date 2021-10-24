using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Template.Common;
using Template.Data;
using Template.Entities.Concrete;
using Template.Entities.Concrete.IdentityModels;
using Template.Mappers;
using Template.Service;

namespace Template.Middleware
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureCustomServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSwagger();

            services.ConfigureJwtAuthSettings(configuration);

            services.ConfigureCustomAuthorization();

            services.ConfigureMicrosoftIdentity();

            services.ConfigureEntityFramework(configuration);

            services.ConfigureCustomServiceDependencies();

            services.AddAutoMapper(typeof(RequestToEntityProfile));
        }

        private static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Template.Api",
                    Version = "v1"
                });
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
            });
        }

        private static void ConfigureJwtAuthSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // bind jwt settings
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });
        }

        private static void ConfigureMicrosoftIdentity(this IServiceCollection services)
        {
            // add MicrosoftIdentity with EntityFramework
            services.AddIdentity<User, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<TemplateContext>();

            // add identity service di
            services.AddScoped<IIdentityService, IdentityService>();
        }

        private static void ConfigureCustomServiceDependencies(this IServiceCollection services)
        {
            // add custom services
            services.AddScoped<IBookService, BookService>();
        }

        private static void ConfigureEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            // add EntityFramework
            services.AddDbContext<TemplateContext>(options => {
                // options.EnableSensitiveDataLogging();
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Template.Data"));
            });
        }

        private static void ConfigureCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => {
                options.AddPolicy("BookViewer", builder => builder.RequireClaim("books.view", "true"));
            });

        }
    }
}