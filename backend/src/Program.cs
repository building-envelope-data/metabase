using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Directory = System.IO.Directory;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Metabase
{
    public class Program
    {
        public static async Task<int> Main(
            string[] commandLineArguments
            )
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? throw new Exception("Unknown enrivornment.");
            var elasticsearchUri = new Uri(
                Environment.GetEnvironmentVariable("XBASE_ElasticsearchUri")
                ?? throw new Exception("Unknown Elasticsearch URI")
                );
            ConfigureBootstrapLogging(environment, elasticsearchUri);
            try
            {
                Log.Information("Starting web host");
                var host = CreateHostBuilder(commandLineArguments, elasticsearchUri).Build();
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
            string environment,
            Uri elasticsearchUri
        )
        {
            var configuration = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
            ConfigureLogging(configuration, environment, elasticsearchUri);
            Log.Logger = configuration.CreateBootstrapLogger();
        }

        private static void ConfigureLogging(
            LoggerConfiguration configuration,
            string environment,
            Uri elasticsearchUri
        )
        {
            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environment)
                .WriteTo.Console()
                .WriteTo.Debug()
                .WriteTo.Elasticsearch(
                    ConfigureElasticSink(environment, elasticsearchUri)
                    );
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(
            string environment,
            Uri elasticsearchUri
            )
        {
            return new ElasticsearchSinkOptions(
                elasticsearchUri
                )
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    // https://github.com/serilog/serilog-sinks-elasticsearch/blob/6cafc3e94551e81b2e9e3bf33ecf095764e8b0c4/src/Serilog.Sinks.Elasticsearch/Sinks/ElasticSearch/ElasticsearchSinkOptions.cs#L282
                    IndexFormat = $"metabase-{environment.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}"
                };
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
        public static IHostBuilder CreateHostBuilder(
            string[] commandLineArguments,
            Uri elasticsearchUri
            )
        {
            return Host.CreateDefaultBuilder(commandLineArguments)
              .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
                  ConfigureConfigurationBuilder(
                      configurationBuilder,
                      hostBuilderContext.HostingEnvironment,
                      commandLineArguments
                      )
              )
              .ConfigureWebHostDefaults(webBuilder =>
                  webBuilder
                  .UseKestrel() // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
                  .UseContentRoot(Directory.GetCurrentDirectory())
                  .UseStartup<Startup>()
                  )
              .UseSerilog((webHostBuilderContext, loggerConfiguration) =>
                  {
                    ConfigureLogging(
                        loggerConfiguration,
                        webHostBuilderContext.HostingEnvironment.EnvironmentName,
                        elasticsearchUri
                        );
                    loggerConfiguration
                        .ReadFrom.Configuration(webHostBuilderContext.Configuration);
                        }
              );
        }

        private static void ConfigureConfigurationBuilder(
            IConfigurationBuilder builder,
            IHostEnvironment environment,
            string[] commandLineArguments
            )
        {
            builder.Sources.Clear();
            builder
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile(
                    "appsettings.json",
                    optional: false,
                    reloadOnChange: !environment.IsEnvironment("test")
                    )
                .AddJsonFile(
                    $"appsettings.{environment.EnvironmentName}.json",
                    optional: false,
                    reloadOnChange: !environment.IsEnvironment("test")
                    )
                .AddEnvironmentVariables(prefix: "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
                .AddCommandLine(commandLineArguments);
        }
    }
}