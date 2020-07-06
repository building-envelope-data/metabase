using System;
using Metabase.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Directory = System.IO.Directory;

namespace Metabase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    SeedData.Initialize(services);
                }
                catch (Exception exception)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(exception, "An error occurred seeding the database.");
                }
            }
            host.Run();
        }

        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel() // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup<Startup>()
                  );
        }
    }
}