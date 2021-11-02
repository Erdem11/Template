using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Template.Common.SettingsConfigurationFiles;
using Template.Contracts;
using Template.Contracts.V1.Books.Requests;

namespace Template.Middleware
{
    public static class ConfigureValidationServicesExtensions
    {
        public static void Configure(this IServiceCollection services, SettingsHolder settings)
        {

            services.AddMvc(options => {
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddBookRequestValidator>());
            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();
        }
    }
}