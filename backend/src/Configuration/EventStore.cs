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
using Event = Icon.Infrastructure.Event;
using Aggregate = Icon.Infrastructure.Aggregate;
using Domain = Icon.Domain;
using Component = Icon.Domain.Component;
using ComponentVersion = Icon.Domain.ComponentVersion;
using ComponentVersionOwnership = Icon.Domain.ComponentVersionOwnership;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;
using Marten.NodaTime;

namespace Icon.Configuration
{
    class EventStore
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString, string schemaName, bool isProductionEnvironment)
        {
            services.AddScoped(typeof(Marten.IDocumentSession), serviceProvider =>
                {
                    var logger = serviceProvider.GetRequiredService<ILogger<EventStore>>();
                    // The dependency injection container will take care of
                    // closing the session, because it implements the
                    // `IDisposable` interface, see
                    // https://andrewlock.net/four-ways-to-dispose-idisposables-in-asp-net-core/#automatically-disposing-services-leveraging-the-built-in-di-container
                    return GetDocumentStore(connectionString, schemaName, isProductionEnvironment, logger).OpenSession();
                });
            services.AddScoped<Aggregate.IAggregateRepository, Aggregate.AggregateRepository>();
        }

        private static Marten.IDocumentStore GetDocumentStore(string connectionString, string schemaName, bool isProductionEnvironment, ILogger<EventStore> logger)
        {
            // TODO Declare `creatorId` of events as foreign key to `User`, see https://jasperfx.github.io/marten/documentation/documents/customizing/foreign_keys/
            return Marten.DocumentStore.For(_ =>
                  {
                      _.Connection(connectionString);
                      _.DatabaseSchemaName = schemaName;
                      _.Events.DatabaseSchemaName = schemaName;
                      /* _.UseNodaTime(); */
                      // For a full list auf auto-create options, see
                      // https://jasperfx.github.io/marten/documentation/schema/
                      if (isProductionEnvironment)
                      {
                          _.AutoCreateSchemaObjects = Marten.AutoCreate.CreateOrUpdate;
                      }
                      else
                      {
                          _.AutoCreateSchemaObjects = Marten.AutoCreate.All;
                      }
                      _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePrivateApply);
                      _.Logger(new MartenLogger(logger));

                      _.Events.InlineProjections.AggregateStreamsWith<Domain.ComponentAggregate>();
                      _.Events.InlineProjections.AggregateStreamsWith<Domain.ComponentVersionAggregate>();
                      _.Events.InlineProjections.AggregateStreamsWith<Domain.ComponentVersionOwnershipAggregate>();

                      _.Events.InlineProjections.Add(new Domain.ComponentViewProjection());
                      _.Events.InlineProjections.Add(new Domain.ComponentVersionViewProjection());
                      _.Events.InlineProjections.Add(new Domain.ComponentVersionOwnershipViewProjection());

                      _.Events.AddEventType(typeof(Component.Create.Event));
                      _.Events.AddEventType(typeof(ComponentVersion.Create.Event));
                      _.Events.AddEventType(typeof(ComponentVersionOwnership.Create.Event));
                  });
        }
    }
}