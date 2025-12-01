// Inspired by
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?customize-webapplicationfactory
// https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Metabase.Data;
using Metabase.Services;

namespace Metabase.Tests.Integration;

public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Metabase.Program>
{
    private bool _disposed;

    public CustomWebApplicationFactory()
    {
        EmailSender = new CollectingEmailSender();
    }

    public CollectingEmailSender EmailSender { get; }

    private T Get<T>(Func<IServiceProvider, T> what)
    {
        using var scope = Services.CreateScope();
        return what(scope.ServiceProvider);
    }

    private void Do(Action<IServiceProvider> what)
    {
        using var scope = Services.CreateScope();
        what(scope.ServiceProvider);
    }

    private Task DoAsync(Func<IServiceProvider, Task> what)
    {
        using var scope = Services.CreateScope();
        return what(scope.ServiceProvider);
    }

    public Task DoAsync(Func<ApplicationDbContext, Task> what)
    {
        return DoAsync(services =>
            what(services.GetRequiredService<ApplicationDbContext>())
        );
    }

    // private TResult Get<TResult>(Func<IServiceProvider, TResult> what)
    // {
    //     using var scope = Services.CreateScope();
    //     return what(scope.ServiceProvider);
    // }

    public AppSettings AppSettings
    {
        get
        {
            return Get(
                    services =>
                        services.GetRequiredService<AppSettings>()
                );
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var connectionString = $"Host=database; Port=5432; Database=xbase_test_{Guid.NewGuid().ToString().Replace("-", "")}; User Id=postgres; Password=+iUv9drsFZuuFIK/3WuBEd0HshaPqz8qks62cWwF3BM=; Maximum Pool Size=90;";
        // var schemaName = $"metabase_{Guid.NewGuid().ToString().Replace("-", "")}";
        // builder.ConfigureAppConfiguration(_ => _.AddInMemoryCollection([new KeyValuePair<string, string?>("Database__ConnectionString", connectionString)])); // "Database__SchemaName"
        builder.UseEnvironment(Metabase.Program.TestEnvironment);
        builder.ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
            {
                configurationBuilder.Sources.Clear();
                Metabase.Program.ConfigureAppConfiguration(
                    configurationBuilder,
                    webHostBuilderContext.HostingEnvironment,
                    []
                );
            }
        );
        builder.ConfigureServices(serviceCollection =>
            {
                using var scope = serviceCollection.BuildServiceProvider().CreateScope();
                // Configure `AppSettings`
                var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                appSettings.Database.ConnectionString = connectionString;
                // `appSettings.Database.ConnectionString` should readonly.
                // However, the commented code below together with
                // `AddInMemoryCollection` above, does not configure the
                // `ConnectionString` in such a way, that Npgsql knows about it.
                // It uses the empty connection string from the original app
                // settings in `Startup.cs`. Why?
                // var oldAppSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                // var newAppSettings = oldAppSettings with
                // {
                //     Database = oldAppSettings.Database with
                //     {
                //         ConnectionString = connectionString
                //         // SchemaName = schemaName
                //     }
                // }
                // ;
                // var appSettingsServiceDescriptor =
                //     serviceCollection.SingleOrDefault(d =>
                //         d.ServiceType == typeof(AppSettings)
                //     );
                // if (appSettingsServiceDescriptor is not null)
                // {
                //     serviceCollection.Remove(appSettingsServiceDescriptor);
                // }
                // serviceCollection.AddSingleton(newAppSettings);
                // Configure `IEmailSender`
                var emailSenderServiceDescriptor =
                    serviceCollection.SingleOrDefault(d =>
                        d.ServiceType == typeof(IEmailSender)
                    );
                if (emailSenderServiceDescriptor is not null)
                {
                    serviceCollection.Remove(emailSenderServiceDescriptor);
                }
                serviceCollection.AddTransient<IEmailSender>(_ => EmailSender);
            }
        );
    }

    // https://docs.microsoft.com/en-us/dotnet/standard/managed-code
    // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    ~CustomWebApplicationFactory()
    {
        Dispose(false);
    }

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            Do(
                services =>
                {
                    services
                        .GetRequiredService<ApplicationDbContext>()
                        .Database
                        .EnsureDeleted();
                }
            );
            _disposed = true;
        }

        base.Dispose(disposing);
    }
}