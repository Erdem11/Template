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
            app.UseMiddleware<ResolveTokenModel>();

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
                    new MyAuthorizationFilter(identityService)
                }
            });
        }
    }

    public class ResolveTokenModel
    {
        private readonly RequestDelegate _next;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public ResolveTokenModel(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task Invoke(HttpContext context)
        {
            var tokenString = context.GetToken();
            if (!string.IsNullOrWhiteSpace(tokenString))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = tokenHandler.ValidateToken(tokenString, _tokenValidationParameters, out _).Claims.ToList();

                var tokenModel = SolveTokenModel(claims);

                context.Items.Add(nameof(TokenModel), tokenModel);
            }

            await _next(context);
        }

        private static TokenModel SolveTokenModel(IReadOnlyCollection<Claim> claims)
        {
            var name = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var jtiString = claims.FirstOrDefault(x => x.Type == "jti")?.Value;
            _ = Guid.TryParse(jtiString, out var jti);

            var email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var idString = claims.FirstOrDefault(x => x.Type == "id")?.Value;
            _ = Guid.TryParse(idString, out var id);

            var roles = claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();

            var customClaims = claims.Where(x => string.Equals(x.Type, nameof(TokenModel.CustomClaims), StringComparison.CurrentCultureIgnoreCase)).Select(x => x.Value).ToList();

            var nbfString = claims.FirstOrDefault(x => x.Type == "nbf")?.Value;
            _ = int.TryParse(nbfString, out var nbf);

            var expString = claims.FirstOrDefault(x => x.Type == "exp")?.Value;
            _ = int.TryParse(expString, out var exp);

            var iatString = claims.FirstOrDefault(x => x.Type == "iat")?.Value;
            _ = int.TryParse(iatString, out var iat);

            var tokenModel = new TokenModel
            {
                Email = email,
                Exp = exp,
                Iat = iat,
                Id = id,
                Jti = jti,
                Nbf = nbf,
                Roles = roles,
                Sub = name,
                CustomClaims = customClaims
            };

            return tokenModel;
        }
    }
}