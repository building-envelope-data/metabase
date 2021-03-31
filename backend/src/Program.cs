using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                    await Data.DbSeeder.DoAsync(services).ConfigureAwait(false);
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
                // .ConfigureHostConfiguration(_ =>
                //     ...
                // )
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
                .ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                    ConfigureAppConfiguration(
                        configurationBuilder,
                        webHostBuilderContext.HostingEnvironment,
                        commandLineArguments
                        )
                )
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
                // .ConfigureLogging(_ =>
                //     _.ClearProviders()
                // )
                // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder
                    .UseKestrel() // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    );
        }

        public static void ConfigureAppConfiguration(
            IConfigurationBuilder configuration,
            IHostEnvironment environment,
            string[] commandLineArguments
            )
        {
            configuration.Sources.Clear();
            configuration
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: !environment.IsEnvironment("test"))
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: false, reloadOnChange: !environment.IsEnvironment("test"))
                .AddEnvironmentVariables(prefix: "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
                .AddCommandLine(commandLineArguments);
        }
    }
}