using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Directory = System.IO.Directory;

namespace Metabase
{
    public class Program
    {
        public static async Task Main(string[] commandLineArguments)
        {
            var host = CreateHostBuilder(commandLineArguments).Build();
            await CreateAndSeedDbIfNotExists(host).ConfigureAwait(false);
            host.Run();
        }

        public static async Task CreateAndSeedDbIfNotExists(IHost host)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro#initialize-db-with-test-data
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                using var dbContext =
                 services.GetRequiredService<IDbContextFactory<Data.ApplicationDbContext>>()
                 .CreateDbContext();
                if (dbContext.Database.EnsureCreated())
                {
                    await Data.DbSeeder.DoAsync(services, testEnvironment: false).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(exception, "An error occurred creating and seeding the database.");
            }
        }

        // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
        public static IHostBuilder CreateHostBuilder(string[] commandLineArguments)
        {
            return Host.CreateDefaultBuilder(commandLineArguments)
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel() // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup(webHostBuilderContext =>
                    new Startup(
                      webHostBuilderContext.HostingEnvironment,
                      commandLineArguments
                      )
                    )
                  );
        }
    }
}