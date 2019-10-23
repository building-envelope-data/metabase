// Inspired by
// https://github.com/mmacneil/ApiIntegrationTestSamples/blob/master/tests/Web.Api.IntegrationTests/CustomWebApplicationFactory.cs
// https://github.com/oskardudycz/EventSourcing.NetCore/blob/master/Marten.Integration.Tests/TestsInfrasructure/MartenTest.cs
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0#customize-webapplicationfactory

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

namespace Test.Integration.Web.Api
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IDisposable where TStartup : class
    {
        private static readonly string ConnectionString = "Host=database;Port=5432;Database=postgres;User Id=postgres;Password=postgres;";

        private readonly string _applicationDbName = "Application" + Guid.NewGuid().ToString().Replace("-", string.Empty);
        private readonly string _eventStoreSchemaName = "event_store_" + Guid.NewGuid().ToString().Replace("-", string.Empty);

        private void Do(Action<IServiceProvider> what)
        {
            using (var scope = Services.CreateScope())
            {
                what(scope.ServiceProvider);
            }
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // TODO builder.UseStartup<Startup>().UseEnvironment("Test");
            builder.ConfigureServices(services =>
            {
                ReplaceApplicationDbContext(services);
                ReplaceAuthStores(services);
                ReplaceEventStore(services);
            });
        }

        private void ReplaceApplicationDbContext(IServiceCollection services)
        {
            services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>((options, context) =>
            {
                context.UseInMemoryDatabase(_applicationDbName);
            });
        }

        private void ReplaceAuthStores(IServiceCollection services)
        {
            // TODO .AddInMemoryApiResources, .AddInMemoryClients, and more?
        }

        private void ReplaceEventStore(IServiceCollection services)
        {
            services.RemoveAll(typeof(Marten.IDocumentSession));
            services.AddScoped(typeof(Marten.IDocumentSession), serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<ILogger<Configuration.EventStore>>();
                return Configuration.EventStore.GetDocumentStore(ConnectionString, _eventStoreSchemaName, false, logger).OpenSession();
            });
        }

        public CustomWebApplicationFactory<TStartup> SetUp()
        {
            Do(
                services =>
                    {
                        SetUpApplicationDb(services.GetRequiredService<ApplicationDbContext>());
                        SetUpEventStore();
                    }
            );
            return this;
        }

        private void SetUpApplicationDb(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.Database.EnsureDeleted();
            applicationDbContext.Database.EnsureCreated();
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

        public void Dispose()
        {
            Do(
                    services =>
                    {
                        DisposeApplicationDb(services.GetRequiredService<ApplicationDbContext>());
                        DisposeEventStore();
                    }
            );
        }

        private void DisposeApplicationDb(ApplicationDbContext applicationDbContext)
        {
            applicationDbContext.Database.EnsureDeleted();
        }

        private void DisposeEventStore()
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var command = connection.CreateCommand();
                    command.CommandText = $"DROP SCHEMA IF EXISTS {_eventStoreSchemaName} CASCADE;";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }

    }
}