using Microsoft.AspNetCore.Hosting;
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

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}