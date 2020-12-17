using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Directory = System.IO.Directory;

namespace Metabase
{
    public class Program
    {
        public static void Main(string[] commandLineArguments)
        {
            var host = CreateHostBuilder(commandLineArguments).Build();
            CreateAndSeedDbIfNotExists(host);
            host.Run();
        }

        public static void CreateAndSeedDbIfNotExists(IHost host)
        {
          // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro?view=aspnetcore-5.0#initialize-db-with-test-data
          using (var scope = host.Services.CreateScope())
          {
            var services = scope.ServiceProvider;
            try
            {
              using var dbContext = services.GetRequiredService<IDbContextFactory<Data.ApplicationDbContext>>().CreateDbContext();
              if (dbContext.Database.EnsureCreated())
              {
                /* TODO SeedData.Initialize(services); */
              }
            }
            catch (Exception exception)
            {
              var logger = services.GetRequiredService<ILogger<Program>>();
              logger.LogError(exception, "An error occurred creating and seeding the database.");
            }
          }
        }

        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
        public static IHostBuilder CreateHostBuilder(string[] commandLineArguments)
        {
            return Host.CreateDefaultBuilder(commandLineArguments)
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel() // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-3.1
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup<Startup>(webHostBuilderContext =>
                    new Startup(
                      webHostBuilderContext.HostingEnvironment,
                      commandLineArguments
                      )
                    )
                  );
        }
    }
}