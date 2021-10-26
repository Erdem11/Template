using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Template.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            // auto migration on start
            // using (var serviceScope = host.Services.CreateScope())
            // {
            //     var dbContext = serviceScope.ServiceProvider.GetRequiredService<TemplateContext>();
            //     dbContext.Database.Migrate();
            // }

            host.Run();
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
            configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = $"{context.Configuration["ApplicationName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
        }

        private static void LoadAppsettingsFiles(IWebHostBuilder webBuilder)
        {
            webBuilder.ConfigureAppConfiguration((webHostBuilderContext,
                configurationBuilder) => {
                var env = webHostBuilderContext.HostingEnvironment;
                configurationBuilder.SetBasePath(env.ContentRootPath);
                var configurationNameList = new[]
                {
                    "jwt", "logging", "mssql", "caching", "caching.redis", "caching.inmemory", "subscriber"
                };

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                environment = environment == null ? null : environment + ".";

                foreach (var s in configurationNameList)
                {
                    configurationBuilder.AddJsonFile($"appsettings.{environment + s}.json", false, true);
                }
                configurationBuilder.AddEnvironmentVariables();
            });
        }
    }
}