using NSwag.Generation.Processors.Security;
using IdentityServer4.AccessTokenValidation;
using Icon.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using Microsoft.OpenApi;
using NSwag;
using IdentityServer4.AspNetIdentity;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Command = Icon.Infrastructure.Command;
using Query = Icon.Infrastructure.Query;
using Event = Icon.Events;
using Aggregate = Icon.Infrastructure.Aggregate;
using Models = Icon.Models;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Marten.NodaTime;
using Events = Icon.Events;
using Aggregates = Icon.Aggregates;

namespace Icon.Configuration
{
    public sealed class EventStore
    {
        public static void ConfigureServices(IServiceCollection services, IWebHostEnvironment environment, AppSettings.DatabaseSettings databaseSettings)
        {
            services.AddScoped( // TODO Using `AddSingleton` here makes Marten not create schema objects in tests. Why?
                typeof(Marten.IDocumentStore),
                serviceProvider =>
                BuildDocumentStore(
                  environment,
                  databaseSettings,
                  serviceProvider.GetRequiredService<ILogger<EventStore>>()
                  )
                );
            services.AddScoped<Aggregate.IAggregateRepository, Aggregate.AggregateRepository>();
        }

        public static Marten.IDocumentStore BuildDocumentStore(IWebHostEnvironment environment, AppSettings.DatabaseSettings databaseSettings, ILogger<EventStore> logger)
        {
            var martenLogger = new MartenLogger(logger);
            // TODO Declare `creatorId` of events as foreign key to `User`, see https://jasperfx.github.io/marten/documentation/documents/customizing/foreign_keys/
            return Marten.DocumentStore.For(_ =>
                  {
                      _.Connection(databaseSettings.ConnectionString);
                      _.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                      _.Events.DatabaseSchemaName = databaseSettings.SchemaName.EventStore;
                      /* _.UseNodaTime(); */
                      // For a full list auf auto-create options, see
                      // https://jasperfx.github.io/marten/documentation/schema/
                      if (environment.IsDevelopment() || environment.IsEnvironment("Test"))
                      {
                          _.AutoCreateSchemaObjects = Marten.AutoCreate.All;
                      }
                      else
                      {
                          _.AutoCreateSchemaObjects = Marten.AutoCreate.CreateOrUpdate;
                      }
                      _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePrivateApply);

                      _.Logger(martenLogger);
                      _.Listeners.Add(martenLogger);

                      _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentAggregate>();
                      _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVersionAggregate>();
                      _.Events.InlineProjections.AggregateStreamsWith<Aggregates.ComponentVersionManufacturerAggregate>();

                      _.Events.AddEventType(typeof(Events.ComponentCreated));
                      _.Events.AddEventType(typeof(Events.ComponentVersionCreated));
                      _.Events.AddEventType(typeof(Events.ComponentVersionManufacturerCreated));
                  });
        }
    }
}