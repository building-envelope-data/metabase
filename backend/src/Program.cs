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
using Serilog.Formatting.Compact;
using Microsoft.AspNetCore.Builder;

namespace Metabase
{
    public sealed class Program
    {
        public const string TestEnvironment = "test";

        public static async Task<int> Main(
            string[] commandLineArguments
            )
        {
            var environment =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? throw new ArgumentException("Unknown enrivornment.");
            // https://github.com/serilog/serilog-aspnetcore#two-stage-initialization
            ConfigureBootstrapLogging(environment);
            try
            {
                Log.Information("Starting web host");
                // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/webapplication
                var builder = CreateWebApplicationBuilder(commandLineArguments);
                var startup = new Startup(builder.Environment, builder.Configuration);
                startup.ConfigureServices(builder.Services);
                var application = builder.Build();
                startup.Configure(application);
                using (var scope = application.Services.CreateScope())
                {
                    if (application.Environment.IsDevelopment())
                    {
                        // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro#initialize-db-with-test-data
                        await CreateAndSeedDbIfNotExists(scope.ServiceProvider).ConfigureAwait(false);
                    }
                }
                application.Run();
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
                    formatter: new CompactJsonFormatter(),
                    path: "./logs/serilog.json",
                    fileSizeLimitBytes: 1073741824, // 1 GB
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 7);
            if (environment != "production")
            {
                configuration.WriteTo.Debug();
            }
        }

        private static async Task CreateAndSeedDbIfNotExists(
            IServiceProvider services
            )
        {
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
        private static WebApplicationBuilder CreateWebApplicationBuilder(
            string[] commandLineArguments
            )
        {
            var builder = WebApplication.CreateBuilder(
                new WebApplicationOptions
                {
                    Args = commandLineArguments,
                    ContentRootPath = Directory.GetCurrentDirectory()
                }
            );
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
            ConfigureAppConfiguration(
                builder.Configuration,
                builder.Environment,
                commandLineArguments
            );
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
            // https://github.com/dotnet/aspnetcore/issues/38334#issuecomment-967709919
            builder.Host.UseDefaultServiceProvider(_ =>
            {
                _.ValidateScopes = true;
                _.ValidateOnBuild = true;
            });
            // https://github.com/serilog/serilog-aspnetcore#instructions
            builder.Host.UseSerilog((webHostBuilderContext, loggerConfiguration) =>
            {
                ConfigureLogging(
                    loggerConfiguration,
                    webHostBuilderContext.HostingEnvironment.EnvironmentName
                    );
                loggerConfiguration
                    .ReadFrom.Configuration(webHostBuilderContext.Configuration);
            });
            return builder;
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
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: !environment.IsEnvironment(TestEnvironment)
                    )
                .AddJsonFile(
                    $"appsettings.{environment.EnvironmentName}.json",
                    optional: false,
                    reloadOnChange: !environment.IsEnvironment(TestEnvironment)
                    )
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
                .AddCommandLine(commandLineArguments);
        }
    }
}