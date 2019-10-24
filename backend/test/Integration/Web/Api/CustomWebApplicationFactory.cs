// Inspired by
// https://github.com/mmacneil/ApiIntegrationTestSamples/blob/master/tests/Web.Api.IntegrationTests/CustomWebApplicationFactory.cs
// https://github.com/oskardudycz/EventSourcing.NetCore/blob/master/Marten.Integration.Tests/TestsInfrasructure/MartenTest.cs
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0#customize-webapplicationfactory

using PersistedGrantDbContext = IdentityServer4.EntityFramework.DbContexts.PersistedGrantDbContext;
using ConfigurationDbContext = IdentityServer4.EntityFramework.DbContexts.ConfigurationDbContext;
using Microsoft.Extensions.Configuration;
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

        public CustomWebApplicationFactory SetUp()
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
            return this;
        }

        private void SetUpDatabase(DbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }

        private void SetUpEventStore()
        {
            DisposeEventStore();
            // The schema is re-created on the fly
        }

        public void SeedUsers()
        {
            Do(
                    services =>
                    {
                        SeedData.PopulateUsers(services.GetRequiredService<UserManager<User>>());
                    }
            );
        }

        public void SeedTestData()
        {
            Do(
                    services =>
                    {
                        SeedData.PopulateTestData(services.GetRequiredService<ApplicationDbContext>());
                    }
            );
        }

        public new void Dispose()
        {
            Do(
                    services =>
                    {
                        DisposeDatabase(services.GetRequiredService<ApplicationDbContext>());
                        DisposeDatabase(services.GetRequiredService<PersistedGrantDbContext>());
                        DisposeDatabase(services.GetRequiredService<ConfigurationDbContext>());
                        DisposeEventStore();
                    }
            );
						base.Dispose();
        }

        private void DisposeDatabase(DbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
        }

        private void DisposeEventStore()
        {
            using (var connection = new NpgsqlConnection(AppSettings.Database.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var command = connection.CreateCommand();
                    command.CommandText = $"DROP SCHEMA IF EXISTS {AppSettings.Database.SchemaName.EventStore} CASCADE;";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

    }
}