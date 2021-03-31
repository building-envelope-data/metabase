using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Serilog;
using Serilog.Events;

namespace Metabase
{
    public class Program
    {
        public static async Task<int> Main(
            string[] commandLineArguments
            )
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new Exception("Unknown enrivornment.");
            ConfigureBootstrapLogging(environment);
            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(commandLineArguments).Build();
                await CreateAndSeedDbIfNotExists(host).ConfigureAwait(false);
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureBootstrapLogging(
            string environment
        )
        {
            var configuration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
            ConfigureLogging(configuration, environment);
            Log.Logger = configuration.CreateBootstrapLogger();
        }

        private static void ConfigureLogging(
            LoggerConfiguration configuration,
            string environment
        )
        {
            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .WriteTo.Console()
                .WriteTo.File(
                    "seri.log",
                    fileSizeLimitBytes: 1073741824, // 1 GB
                    retainedFileCountLimit: 31,
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true
                    );
            if (environment != "production")
            {
                configuration.WriteTo.Debug();
            }
        }

        public static async Task CreateAndSeedDbIfNotExists(
            IHost host
            )
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
        public static IHostBuilder CreateHostBuilder(
            string[] commandLineArguments
            )
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
                .UseSerilog((webHostBuilderContext, loggerConfiguration) =>
                    {
                        ConfigureLogging(
                            loggerConfiguration,
                            webHostBuilderContext.HostingEnvironment.EnvironmentName
                            );
                        loggerConfiguration
                            .ReadFrom.Configuration(webHostBuilderContext.Configuration);
                    }
                )
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