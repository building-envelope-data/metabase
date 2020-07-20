using System;
using Infrastructure.Aggregates;
using Infrastructure.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Configuration
{
    public abstract class EventStore
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment,
            AppSettings.DatabaseSettings databaseSettings,
            Action<Marten.StoreOptions> projectAggregatesInline,
            Action<Marten.StoreOptions> addEventTypes
            )
        {
            services.AddScoped(typeof(Marten.IDocumentStore), serviceProvider =>
                BuildDocumentStore(
                  environment,
                  databaseSettings,
                  serviceProvider.GetRequiredService<ILogger<EventStore>>(),
                  projectAggregatesInline,
                  addEventTypes
                  )
                );
            services.AddScoped<IModelRepository, ModelRepository>();
        }

        public static Marten.IDocumentStore BuildDocumentStore(
            IWebHostEnvironment environment,
            AppSettings.DatabaseSettings databaseSettings,
            ILogger<EventStore> logger,
            Action<Marten.StoreOptions> projectAggregatesInline,
            Action<Marten.StoreOptions> addEventTypes
            )
        {
            var martenLogger = new MartenLogger(logger);
            return Marten.DocumentStore.For(_ =>
                {
                    _.Connection(databaseSettings.ConnectionString);
                    _.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                    _.Events.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                    /* _.UseNodaTime(); */
                    // For a full list auf auto-create options, see
                    // https://jasperfx.github.io/marten/documentation/schema/
                    if (environment.IsDevelopment() || environment.IsEnvironment("test"))
                    {
                        _.AutoCreateSchemaObjects = Marten.AutoCreate.All;
                        // TODO Why does this not work?
                        // https://martendb.io/getting_started/#sec3
                        // _.CreateDatabasesForTenants(c =>
                        //     {
                        //     // Specify a db to which to connect in case database needs to be created.
                        //     // If not specified, defaults to 'postgres' on the connection for a tenant.
                        //     c.MaintenanceDatabase(databaseSettings.ConnectionString);
                        //     c.ForTenant()
                        //     .CheckAgainstPgDatabase()
                        //     .WithOwner("postgres")
                        //     .WithEncoding("UTF-8")
                        //     .ConnectionLimit(-1)
                        //     .OnDatabaseCreated(_ =>
                        //         {
                        //         /* dbCreated = true; */
                        //         });
                        //     });
                    }
                    else
                    {
                        _.AutoCreateSchemaObjects = Marten.AutoCreate.CreateOrUpdate;
                    }
                    _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePublicApply);
                    _.UseDefaultSerialization( // https://martendb.io/documentation/documents/json/newtonsoft/
                        enumStorage: Marten.EnumStorage.AsString,
                        collectionStorage: Marten.CollectionStorage.AsArray
                        );

                    _.Logger(martenLogger);
                    _.Listeners.Add(martenLogger);

                    projectAggregatesInline(_);
                    addEventTypes(_);
                });
        }
    }
}