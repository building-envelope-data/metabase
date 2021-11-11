// Inspired by
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0#customize-webapplicationfactory
// https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;

namespace Metabase.Tests.Integration
{
    public sealed class CustomWebApplicationFactory
      : WebApplicationFactory<Startup>
    {
        private bool _disposed = false;

        public CollectingEmailSender EmailSender { get; }

        public CustomWebApplicationFactory()
        {
            EmailSender = new CollectingEmailSender();
            Do(
                services =>
                    SetUpDatabase(services.GetRequiredService<Data.ApplicationDbContext>())
            );
        }

        private void Do(Action<IServiceProvider> what)
        {
            using var scope = Services.CreateScope();
            what(scope.ServiceProvider);
        }

        private async Task DoAsync(Func<IServiceProvider, Task> what)
        {
            using var scope = Services.CreateScope();
            await what(scope.ServiceProvider).ConfigureAwait(false);
        }

        private TResult Get<TResult>(Func<IServiceProvider, TResult> what)
        {
            using var scope = Services.CreateScope();
            return what(scope.ServiceProvider);
        }

        private AppSettings AppSettings
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
            builder.UseEnvironment("test");
            builder.ConfigureAppConfiguration((webHostBuilderContext, configurationBuilder) =>
                {
                    configurationBuilder.Sources.Clear();
                    Metabase.Program.ConfigureAppConfiguration(
                        configurationBuilder,
                        webHostBuilderContext.HostingEnvironment,
                        Array.Empty<string>()
                        );
                }
            );
            builder.ConfigureServices(serviceCollection =>
                {
                    using var scope = serviceCollection.BuildServiceProvider().CreateScope();
                    var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                    // appSettings.Database.SchemaName += Guid.NewGuid().ToString().Replace("-", ""); // does not work because enumeration types in public schema cannot be created when they already exist. We therefore create a whole new database for each test instead of just a new schema.
                    appSettings.Database.ConnectionString = $"Host=database; Port=5432; Database=xbase_test_{Guid.NewGuid().ToString().Replace("-", "")}; User Id=postgres; Password=postgres; Maximum Pool Size=90;";
                    // Configure `IEmailSender`
                    var emailSenderServiceDescriptor =
                    serviceCollection.SingleOrDefault(d =>
                     d.ServiceType == typeof(Services.IEmailSender)
                        );
                    if (emailSenderServiceDescriptor is not null)
                    {
                        serviceCollection.Remove(emailSenderServiceDescriptor);
                    }
                    serviceCollection.AddTransient<Services.IEmailSender>(_ => EmailSender);
                }
            );
        }

        private void SetUpDatabase(DbContext dbContext)
        {
            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created#multiple-dbcontext-classes
            var databaseCreator = dbContext.Database.GetService<IRelationalDatabaseCreator>();
            databaseCreator.EnsureDeleted();
            databaseCreator.Create();
            databaseCreator.CreateTables();
            Task.Run(async () =>
                await SeedDatabase().ConfigureAwait(false)
             ).GetAwaiter().GetResult();
        }

        private async Task SeedDatabase()
        {
            await DoAsync(
                async services =>
                 await Data.DbSeeder.DoAsync(services).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        // https://docs.microsoft.com/en-us/dotnet/standard/managed-code
        // https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
        ~CustomWebApplicationFactory() => Dispose(false);

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                Do(
                    services =>
                        {
                            services
                            .GetRequiredService<Data.ApplicationDbContext>()
                            .Database
                            .EnsureDeleted();
                        }
                );
                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}