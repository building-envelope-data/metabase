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
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

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
                using var scope = host.Services.CreateScope();
                var services = scope.ServiceProvider;
                var webHostEnvironment = services.GetRequiredService<IWebHostEnvironment>();
                if (webHostEnvironment.IsDevelopment())
                {
                    // https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro#initialize-db-with-test-data
                    await CreateAndSeedDbIfNotExists(services).ConfigureAwait(false);
                }
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
                .WriteTo.Console(new RenderedCompactJsonFormatter())
                .WriteTo.File(
                    path: "./logs/seri.log",
                    formatter: new RenderedCompactJsonFormatter(),
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
        public static IHostBuilder CreateHostBuilder(
            string[] commandLineArguments
            )
        {
            return new HostBuilder()
            .UseContentRoot(
                Directory.GetCurrentDirectory()
                )
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host#host-configuration
            .ConfigureHostConfiguration(configuration =>
                {
                    configuration.AddEnvironmentVariables(prefix: "DOTNET_");
                    configuration.AddCommandLine(commandLineArguments);
                }
            )
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/
            .ConfigureAppConfiguration((hostingContext, configuration) =>
                ConfigureAppConfiguration(
                    configuration,
                    hostingContext.HostingEnvironment,
                    commandLineArguments
                    )
                )
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/
            .ConfigureLogging((_, loggingBuilder) =>
            {
                loggingBuilder.Configure(options =>
                {
                    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
                                                        | ActivityTrackingOptions.TraceId
                                                        | ActivityTrackingOptions.ParentId;
                });
            })
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection
            .UseDefaultServiceProvider((context, options) =>
            {
                var isDevelopment = context.HostingEnvironment.IsDevelopment();
                options.ValidateScopes = isDevelopment;
                options.ValidateOnBuild = isDevelopment;
            })
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
            // https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebHost.cs#L215
            .ConfigureWebHost(_ =>
                _
                .ConfigureAppConfiguration((context, _) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        StaticWebAssetsLoader.UseStaticWebAssets(context.HostingEnvironment, context.Configuration);
                    }
                })
                // Default web server https://docs.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel
                .UseKestrel((context, options) =>
                    options.Configure(context.Configuration.GetSection("Kestrel"), reloadOnChange: true)
                    )
                .ConfigureServices((context, services) =>
                    {
                        // Fallback
                        // services.PostConfigure<HostFilteringOptions>(options =>
                        // {
                        //     if (options.AllowedHosts == null || options.AllowedHosts.Count == 0)
                        //     {
                        //         // "AllowedHosts": "localhost;127.0.0.1;[::1]"
                        //         var hosts = context.Configuration["AllowedHosts"]?.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                        //         // Fall back to "*" to disable.
                        //         options.AllowedHosts = (hosts?.Length > 0 ? hosts : new[] { "*" });
                        //     }
                        // });
                        // Change notification
                        // services.AddSingleton<IOptionsChangeTokenSource<HostFilteringOptions>>(
                        //     new ConfigurationChangeTokenSource<HostFilteringOptions>(context.Configuration));
                        // services.AddTransient<IStartupFilter, HostFilteringStartupFilter>();
                        services.AddRouting();
                    })
                // .UseIIS()
                // .UseIISIntegration()
                .UseStartup<Startup>()
            );
        }

        public static void ConfigureAppConfiguration(
            IConfigurationBuilder configuration,
            IHostEnvironment environment,
            string[] commandLineArguments
            )
        {
            configuration
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
                .AddEnvironmentVariables()
                .AddEnvironmentVariables(prefix: "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
                .AddCommandLine(commandLineArguments);
        }
    }
}