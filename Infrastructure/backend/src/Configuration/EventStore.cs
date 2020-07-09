using System;
using Infrastructure.Aggregates;
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
            services.AddScoped<IAggregateRepository, AggregateRepository>();
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