using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.MSSqlServer;
using Template.Common;
using Template.Common.SettingsConfigurationFiles;
using Template.Data;
using Template.Domain.Dto.IdentityModels;

namespace Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var messagingContext = serviceScope.ServiceProvider.GetRequiredService<MessagingContext>();
                messagingContext.Database.EnsureCreated();

                var templateContext = serviceScope.ServiceProvider.GetRequiredService<TemplateContext>();
                templateContext.Database.EnsureCreated();

                // auto migration on start
                // dbContext.Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                AddAdminRole(roleManager, userManager);
            }

            host.Run();
        }

        private static void AddAdminRole(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            if (roleManager.RoleExistsAsync(RoleConstants.Admin).Result)
                return;

            var admin = new User
            {
                Email = "admin@template.com",
                UserName = "admin@template.com",
            };

            _ = userManager.CreateAsync(admin, "123456").Result;
            admin = userManager.FindByEmailAsync(admin.Email).Result;

            var adminRole = new Role
            {
                Name = RoleConstants.Admin,
            };
            roleManager.CreateAsync(adminRole).Wait();
            var editorRole = new Role
            {
                Name = RoleConstants.Editor,
            };
            roleManager.CreateAsync(editorRole).Wait();

            adminRole = roleManager.FindByNameAsync(RoleConstants.Admin).Result;

            userManager.AddToRoleAsync(admin, RoleConstants.Admin).Wait();
            userManager.AddToRoleAsync(admin, RoleConstants.Editor).Wait();

            roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, RoleConstants.Admin)).Wait();
            roleManager.AddClaimAsync(adminRole, new Claim(ClaimTypes.Role, RoleConstants.Editor)).Wait();
            roleManager.AddClaimAsync(adminRole, new Claim(nameof(TokenModel.CustomClaims), ClaimConstants.Hangfire)).Wait();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) => {
                    ConfigureSerilog(configuration, context);
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();

                    LoadAppsettingsFiles(webBuilder);
                });
        }

        private static void ConfigureSerilog(LoggerConfiguration configuration, HostBuilderContext context)
        {
            var connectionString = LoadSettingFiles(new ConfigurationBuilder());
            var settingsHolder = new SettingsHolder();
            connectionString.Bind(settingsHolder);

            configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteToDb(settingsHolder, context)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
        }

        private static void LoadAppsettingsFiles(IWebHostBuilder webBuilder)
        {
            webBuilder.ConfigureAppConfiguration((webHostBuilderContext,
                configurationBuilder) => {
                var env = webHostBuilderContext.HostingEnvironment;
                configurationBuilder.SetBasePath(env.ContentRootPath);
                LoadSettingFiles(configurationBuilder);
            });
        }
        private static IConfiguration LoadSettingFiles(IConfigurationBuilder configurationBuilder)
        {
            var configurationNameList = new[]
            {
                "cache", "jwt", "logging", "sql", "redis", "clientratelimiting", "clientratelimitpolicies",
            };

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            environment = environment == null ? null : environment + ".";

            // for MyServices file
            configurationBuilder.AddJsonFile($"appsettings.{environment}json", false, true);
            foreach (var s in configurationNameList)
            {
                configurationBuilder.AddJsonFile($"appsettings.{environment + s}.json", false, true);
            }
            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
    }

    public static class SerilogHelpers
    {
        public static LoggerConfiguration WriteToDb(this LoggerConfiguration loggerConfiguration, SettingsHolder settingsHolder, HostBuilderContext context)
        {
            if (settingsHolder.MyServices.ElasticSearch)
            {
                return loggerConfiguration.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                });
            }

            var dbOptions = settingsHolder.SqlSettings.GetAlternate();
            return dbOptions.DbType switch
            {
                DbTypes.Mssql => loggerConfiguration.WriteTo.MSSqlServer(dbOptions.ConnectionString, new MSSqlServerSinkOptions
                {
                    TableName = "TemplateLogs",
                    AutoCreateSqlTable = true,
                    SchemaName = "serilog"
                }),
                DbTypes.Npgsql => loggerConfiguration.WriteTo.PostgreSQL(dbOptions.ConnectionString, "TemplateLogs", needAutoCreateTable: true),
                DbTypes.Sqlite => loggerConfiguration.WriteTo.SQLite(dbOptions.ConnectionString, "TemplateLogs"),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}