// Inspired by
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-5.0#customize-webapplicationfactory
// https://www.thinktecture.com/en/entity-framework-core/isolation-of-integration-tests-in-2-1/

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using AppSettings = Infrastructure.AppSettings;
using IRelationalDatabaseCreator = Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator;
using IsolationLevel = System.Data.IsolationLevel;
using Startup = Metabase.Startup;
using Data = Metabase.Data;

namespace Metabase.Tests.Integration
{
  public sealed class CustomWebApplicationFactory
    : WebApplicationFactory<Startup>
    {
        private void Do(Action<IServiceProvider> what)
        {
            using (var scope = Services.CreateScope())
            {
                what(scope.ServiceProvider);
            }
        }

        private async Task DoAsync(Func<IServiceProvider, Task> what)
        {
            using (var scope = Services.CreateScope())
            {
                await what(scope.ServiceProvider).ConfigureAwait(false);
            }
        }

        private TResult Get<TResult>(Func<IServiceProvider, TResult> what)
        {
            using (var scope = Services.CreateScope())
            {
                return what(scope.ServiceProvider);
            }
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

        public CustomWebApplicationFactory()
        {
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("test");
            builder.ConfigureServices(serviceCollection =>
                {
                using (var scope = serviceCollection.BuildServiceProvider().CreateScope())
                {
                var appSettings = scope.ServiceProvider.GetRequiredService<AppSettings>();
                appSettings.Database.SchemaName += Guid.NewGuid().ToString().Replace("-", "");
                /* var appSettingsServiceDescriptor = */
                /* serviceCollection.SingleOrDefault( */
                /*     d => d.ServiceType == typeof(AppSettings) */
                /*     ); */
                /* if (appSettingsServiceDescriptor is not null) */
                /* { */
                /* serviceCollection.Remove(appSettingsServiceDescriptor); */
                /* } */
                /* serviceCollection.AddSingleton<AppSettings>(appSettings); */
                }
                }
            );
        }

        public void SetUp()
        {
            Do(
                services =>
                    SetUpDatabase(services.GetRequiredService<Data.ApplicationDbContext>())
            );
        }

        private void SetUpDatabase(DbContext dbContext)
        {
            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created#multiple-dbcontext-classes
            var databaseCreator = dbContext.Database.GetService<IRelationalDatabaseCreator>();
            if (databaseCreator.Exists())
            {
              DropDatabaseSchema(dbContext);
            }
            else {
              databaseCreator.Create();
            }
            databaseCreator.CreateTables();
        }

        public async Task SeedUsers()
        {
            /* await DoAsync( */
            /*     async services => */
            /*     await SeedData.SeedUsers( */
            /*       services.GetRequiredService<UserManager<Models.UserX>>() */
            /*       ) */
            /*     .ConfigureAwait(false) */
            /* ).ConfigureAwait(false); */
        }

        public void SeedAuth()
        {
            /* Do( */
            /*         services => SeedData.SeedAuth(services.GetRequiredService<ConfigurationDbContext>()) */
            /* ); */
        }

        public new void Dispose()
        {
            Do(
                services =>
                DropDatabaseSchema(
                  services.GetRequiredService<Data.ApplicationDbContext>()
                  )
            );
            base.Dispose();
        }

        private void DropDatabaseSchema(DbContext dbContext)
        {
          DropDatabaseSchema(
              dbContext.Model.GetDefaultSchema()
              );
        }

        private void DropDatabaseSchema(string schemaName)
        {
            using (var connection = new NpgsqlConnection(AppSettings.Database.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var command = connection.CreateCommand();
                    command.CommandText = $"DROP SCHEMA IF EXISTS {schemaName} CASCADE;";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }
    }
}
