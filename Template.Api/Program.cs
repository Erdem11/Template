using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();

                    LoadAppsettingsFiles(webBuilder);
                });
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