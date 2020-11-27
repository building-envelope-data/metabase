using System;
using System.Data;
using Marten;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;
using Aggregates = Metabase.Aggregates;
using AppSettings = Infrastructure.AppSettings;

namespace Test.Integration.EventStore
{
    [Collection("EventStore")]
    public abstract class TestBase : IDisposable
    {
        protected string ConnectionString { get; }
        protected string SchemaName { get; }
        protected IDocumentSession Session { get; }

        protected TestBase()
        {
            var appSettings = new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: false)
              .AddJsonFile("appsettings.Test.json", optional: false)
              .Build()
              .Get<AppSettings>();
            ConnectionString = appSettings.Database.ConnectionString;
            SchemaName = appSettings.Database.SchemaName.EventStore + Guid.NewGuid().ToString().Replace("-", "_");
            Session = CreateSession();
        }

        protected virtual IDocumentSession CreateSession(Action<StoreOptions>? setStoreOptions = null)
        {
            return DocumentStore.For(_ =>
            {
                _.Connection(ConnectionString);
                _.AutoCreateSchemaObjects = AutoCreate.All;
                _.DatabaseSchemaName = SchemaName;
                _.Events.DatabaseSchemaName = SchemaName;
                _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePrivateApply);

                // TODO Add those inline projections to the tests where they are needed.
                _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentAggregate>();
                _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVersionAggregate>();
                _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentManufacturerAggregate>();

                if (!(setStoreOptions is null))
                {
                    setStoreOptions(_);
                }
            }).OpenSession();
        }

        public void Dispose()
        {
            Session?.Dispose();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var command = connection.CreateCommand();
                    command.CommandText = $"DROP SCHEMA {SchemaName} CASCADE;";
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
            }
        }
    }
}