// Inspired by
// https://github.com/mmacneil/ApiIntegrationTestSamples/blob/master/tests/Web.Api.IntegrationTests/CustomWebApplicationFactory.cs
// https://github.com/oskardudycz/EventSourcing.NetCore/blob/master/Marten.Integration.Tests/TestsInfrasructure/MartenTest.cs
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0#customize-webapplicationfactory

using System;
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
using Models = Metabase.Models;
using Startup = Metabase.Startup;

namespace Test.Integration.Web.Api
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
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

        // To run tests in parallel without conflicting each other by working
        // on the same tables, each test must have its own database or schemas.
        // How can the database or schemas be changed?
        // private readonly string _applicationSchemaName;
        // private readonly string _eventStoreSchemaName;
        // private readonly string _identityServerPersistedGrantSchemaName;
        // private readonly string _identityServerConfigurationSchemaName;

        public CustomWebApplicationFactory()
        {
            // var prefix = Guid.NewGuid().ToString();
            // _applicationSchemaName = prefix + AppSettings.Database.SchemaName.Application;
            // _eventStoreSchemaName = prefix + AppSettings.Database.SchemaName.EventStore;
            // _identityServerPersistedGrantSchemaName = prefix + AppSettings.Database.SchemaName.IdentityServerPersistedGrant;
            // _identityServerConfigurationSchemaName = prefix + AppSettings.Database.SchemaName.IdentityServerConfiguration;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            /* builder.UseStartup<Startup>().UseEnvironment("test"); */
            builder.UseEnvironment("test");
            builder.ConfigureServices(services =>
            {
            });
        }

        public void SetUp()
        {
            Do(
                services =>
                    {
                        SetUpEventStore();
                    }
            );
        }

        private void SetUpEventStore()
        {
            DropDatabaseSchema(AppSettings.Database.SchemaName.EventStore);
            // The schema is re-created on the fly
        }

        public async Task SeedUsers()
        {
            await DoAsync(
                    async services => await SeedData.SeedUsers(services.GetRequiredService<UserManager<Models.UserX>>()).ConfigureAwait(false)
            ).ConfigureAwait(false);
        }

        public void SeedAuth()
        {
            /* Do( */
            /*         services => SeedData.SeedAuth(services.GetRequiredService<ConfigurationDbContext>()) */
            /* ); */
        }

        public new void Dispose()
        {
            // We could alternatively just drop the one and only database the connection string refers to
            Do(
                services =>
                    {
                        DropDatabaseSchema(AppSettings.Database.SchemaName.EventStore);
                    }
            );
            base.Dispose();
        }

        private void DropDatabaseSchema(DbContext dbContext)
        {
            DropDatabaseSchema(dbContext.Model.GetDefaultSchema());
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