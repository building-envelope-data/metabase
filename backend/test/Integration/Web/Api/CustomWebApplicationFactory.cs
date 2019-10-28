// Inspired by
// https://github.com/mmacneil/ApiIntegrationTestSamples/blob/master/tests/Web.Api.IntegrationTests/CustomWebApplicationFactory.cs
// https://github.com/oskardudycz/EventSourcing.NetCore/blob/master/Marten.Integration.Tests/TestsInfrasructure/MartenTest.cs
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0#customize-webapplicationfactory

using IRelationalDatabaseCreator = Microsoft.EntityFrameworkCore.Storage.IRelationalDatabaseCreator;
using PersistedGrantDbContext = IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext;
using ConfigurationDbContext = IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Guid = System.Guid;
using System;
using Exception = System.Exception;
using Npgsql;
using IsolationLevel = System.Data.IsolationLevel;
using IDisposable = System.IDisposable;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ApplicationDbContext = Icon.Data.ApplicationDbContext;
using Microsoft.AspNetCore.Identity;
using User = Icon.Domain.User;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Configuration = Icon.Configuration;
using Startup = Icon.Startup;
using AppSettings = Icon.AppSettings;

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

				private TResult Get<TResult>(Func<IServiceProvider, TResult> what)
				{
            using (var scope = Services.CreateScope())
            {
                return what(scope.ServiceProvider);
            }
				}

				private AppSettings AppSettings {
					get {
						return Get(
								services =>
									services.GetRequiredService<AppSettings>()
							);
					}
				}

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            /* builder.UseStartup<Startup>().UseEnvironment("Test"); */
            builder.UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
            });
        }

        public void SetUp()
        {
            Do(
                services =>
                    {
                        SetUpDatabase(services.GetRequiredService<ApplicationDbContext>());
                        SetUpDatabase(services.GetRequiredService<PersistedGrantDbContext>());
                        SetUpDatabase(services.GetRequiredService<ConfigurationDbContext>());
                        SetUpEventStore();
                    }
            );
        }

        private void SetUpDatabase(DbContext dbContext)
        {
            // https://docs.microsoft.com/en-us/ef/core/managing-schemas/ensure-created#multiple-dbcontext-classes
            var databaseCreator = dbContext.Database.GetService<IRelationalDatabaseCreator>();
            if (!databaseCreator.Exists()) databaseCreator.Create();
            DropDatabaseSchema(dbContext.Model.GetDefaultSchema());
            databaseCreator.CreateTables();
        }

        private void SetUpEventStore()
        {
            DropDatabaseSchema(AppSettings.Database.SchemaName.EventStore);
            // The schema is re-created on the fly
        }

        public void SeedUsers()
        {
            Do(
                    services =>
                    {
                        SeedData.SeedUsers(services.GetRequiredService<UserManager<User>>());
                    }
            );
        }

        public void SeedAuth()
        {
            Do(
                    services =>
                    {
                        SeedData.SeedAuth(services.GetRequiredService<ConfigurationDbContext>());
                    }
            );
        }

        public new void Dispose()
        {
            // We could alternatively just drop the one and only database the connection string refers to
            Do(
                services =>
                    {
                        DropDatabaseSchema(services.GetRequiredService<ApplicationDbContext>());
                        DropDatabaseSchema(services.GetRequiredService<PersistedGrantDbContext>());
                        DropDatabaseSchema(services.GetRequiredService<ConfigurationDbContext>());
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