using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Template.Common;

namespace Template.Middleware
{
    public static class ConfigureServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            ConfigureSwaggerServicesExtensions.Configure(services, configuration);
            ConfigureJwtAuthServicesExtensions.Configure(services, configuration);
            ConfigureCustomAuthorizationServicesExtensions.Configure(services, configuration);
            ConfigureMicrosoftIdentityServicesExtensions.Configure(services, configuration);
            ConfigureEntityFrameworkServicesExtensions.Configure(services, configuration);
            ConfigureCustomServicesExtensions.Configure(services, configuration);
            ConfigureAutoMapperServicesExtensions.Configure(services, configuration);
        }
    }
}