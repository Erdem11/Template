using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Template.Api.Hubs;
using Template.Common;
using Template.Common.SettingsConfigurationFiles;
using Template.Common.Structs;
using Template.Contracts;
using Template.Contracts.V1.Books.Requests;
using Template.HealthChecks.HealthCheckResponseModels;
using Template.Middleware;
using Template.Service;

namespace Template.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureServices(Configuration);
            // services.AddMvc(x => x.EnableEndpointRouting = false);
            services.AddMvc(options => {
                    options.Filters.Add(typeof(ValidateModelStateAttribute));
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddBookRequestValidator>());
            services.Configure<ApiBehaviorOptions>(options => {
                options.SuppressModelStateInvalidFilter = true;
            });
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();

            services.AddScoped<LocalizationInfo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, IIdentityService identityService, SettingsHolder settingsHolder)
        {
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) => {
                    context.Response.ContentType = HttpConstants.ApplicationJson;

                    var response = new HealthCheckResponse
                    {
                        Status = report.Status.ToString(),
                        Duration = report.TotalDuration,
                        HealthChecks = report.Entries
                            .Select(x
                                => new HealthCheck
                                {
                                    Component = x.Key,
                                    Description = x.Value.Description,
                                    Status = x.Value.Status.ToString(),
                                }).ToList()
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
                }
            });

            app.UseMiddleware<ResolveTokenModel>();
            if (settingsHolder.MyServices.ApiRateLimit)
                app.UseClientRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                options => {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                        $"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                    }
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapHub<MessagingHub>("/MessagingHub");
            });

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter()
                }
            });
        }
    }
}