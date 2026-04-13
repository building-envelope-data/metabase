using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Metabase.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.OpenTelemetry;

namespace Metabase;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "An error occurred creating the database.")]
    public static partial void FailedToCreateDatabase(
        this ILogger<Program> logger,
        // The first exception is implicitly taken care of as detailed in
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator#log-method-anatomy
        Exception exception
    );

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "An error occurred seeding the database.")]
    public static partial void FailedToSeedDatabase(
        this ILogger<Program> logger,
        // The first exception is implicitly taken care of as detailed in
        // https://learn.microsoft.com/en-us/dotnet/core/extensions/logger-message-generator#log-method-anatomy
        Exception exception
    );
}

public sealed class Program
{
    public const string TestEnvironment = "test";
    private const string ProductionEnvironment = "production";
    private const string LogsPath = "./logs/serilog.json";

    public static async Task<int> Main(
        string[] commandLineArguments
    )
    {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            ?? throw new ArgumentException("Unknown enrivornment.");
        var openTelemetryHost = new UriBuilder(
            scheme: "http",
            host: Environment.GetEnvironmentVariable("XBASE_OpenTelemetry__Host")
                ?? throw new ArgumentException("Unknown OpenTelemetry host."),
            portNumber: int.Parse(
                Environment.GetEnvironmentVariable("XBASE_OpenTelemetry__GrpcPort")
                ?? throw new ArgumentException("Unknown OpenTelemetry gRPC port."),
                CultureInfo.InvariantCulture
            )
        )
        .Uri;
        // https://github.com/serilog/serilog-aspnetcore#two-stage-initialization
        ConfigureBootstrapLogging(environment, openTelemetryHost);
        try
        {
            Serilog.Log.Information("Starting web host");
            // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/webapplication
            var builder = CreateWebApplicationBuilder(commandLineArguments, openTelemetryHost);
            var startup = new Startup(builder.Environment, builder.Configuration);
            startup.ConfigureServices(builder.Services);
            var application = builder.Build();
            startup.Configure(application);
            using (var scope = application.Services.CreateScope())
            {
                if (!builder.Environment.IsEnvironment(TestEnvironment))
                {
                    EnsureDatabaseIsUpToDate(scope.ServiceProvider);
                    // Inspired by https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/intro#initialize-db-with-test-data
                }
                if (builder.Environment.IsEnvironment(TestEnvironment))
                {
                    await CreateDatabase(scope.ServiceProvider);
                }
                await SeedDatabase(scope.ServiceProvider);
            }

            await application.RunAsync();
            return 0;
        }
        catch (Exception exception) when (exception is not HostAbortedException && exception.Source != "Microsoft.EntityFrameworkCore.Design") // see https://github.com/dotnet/efcore/issues/29923
        {
            Serilog.Log.Fatal(exception, "Host terminated unexpectedly");
            return 1;
        }
        finally
        {
            Serilog.Log.CloseAndFlush();
        }
    }

    private static void ConfigureBootstrapLogging(
        string environment,
        Uri openTelemetryHost
    )
    {
        var configuration = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information);
        ConfigureLogging(configuration, environment, openTelemetryHost);
        Serilog.Log.Logger = configuration.CreateBootstrapLogger();
    }

    private static void ConfigureLogging(
        LoggerConfiguration configuration,
        string environment,
        Uri openTelemetryHost
    )
    {
        configuration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProperty("Environment", environment)
            .Enrich.WithSpan() // add trace context
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            // inspired by https://last9.io/blog/serilog-and-opentelemetry/
            .WriteTo.OpenTelemetry(_ =>
            {
                _.Endpoint = openTelemetryHost.AbsoluteUri;
                _.Protocol = OtlpProtocol.Grpc;
                _.OnBeginSuppressInstrumentation =
                    OpenTelemetry.SuppressInstrumentationScope.Begin;
                _.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = "backend",
                };
            })
            .WriteTo.File(
                new CompactJsonFormatter(),
                LogsPath,
                fileSizeLimitBytes: 1073741824, // 1 GB
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: 7
            );
        if (environment != ProductionEnvironment)
        {
            configuration.WriteTo.Debug(formatProvider: CultureInfo.InvariantCulture);
        }
    }

    private static void EnsureDatabaseIsUpToDate(
        IServiceProvider services
    )
    {
        using var dbContext =
            services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
                .CreateDbContext();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();
        if (pendingMigrations.Any())
        {
            throw new InvalidOperationException($"The database is not up to date. The pending migrations are: {string.Join(", ", pendingMigrations)}");
        }
    }

    private static async Task CreateDatabase(
        IServiceProvider services
    )
    {
        try
        {
            using var dbContext =
                services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>()
                    .CreateDbContext();
            await dbContext.Database.EnsureCreatedAsync();
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.FailedToCreateDatabase(exception);
        }
    }

    private static async Task SeedDatabase(
        IServiceProvider services
    )
    {
        try
        {
            await DbSeeder.DoAsync(services);
        }
        catch (Exception exception)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.FailedToSeedDatabase(exception);
        }
    }

    // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host
    private static WebApplicationBuilder CreateWebApplicationBuilder(
        string[] commandLineArguments,
        Uri openTelemetryHost
    )
    {
        var builder = WebApplication.CreateBuilder(
            new WebApplicationOptions
            {
                Args = commandLineArguments,
                ContentRootPath = Directory.GetCurrentDirectory() // PlatformServices.Default.Application.ApplicationBasePath
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
                webHostBuilderContext.HostingEnvironment.EnvironmentName,
                openTelemetryHost
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
                false,
                !environment.IsEnvironment(TestEnvironment)
            )
            .AddJsonFile(
                $"appsettings.{environment.EnvironmentName}.json",
                false,
                !environment.IsEnvironment(TestEnvironment)
            )
            .AddEnvironmentVariables()
            .AddEnvironmentVariables(
                "XBASE_") // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#environment-variables
            .AddCommandLine(commandLineArguments);
    }
}