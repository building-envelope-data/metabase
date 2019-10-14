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
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;

namespace Icon.Configuration
{
    class EventStore
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString, string schemaName)
        {
            services.AddScoped(typeof(Marten.IDocumentStore), serviceProvider =>
                {
                    return Marten.DocumentStore.For(_ =>
                  {
                      _.Connection(connectionString);
                      _.AutoCreateSchemaObjects = Marten.AutoCreate.All; // TODO If we set it to `None`, we have to set-up some tables manually, but also have more control. Which option is better?
                      _.Events.UseAggregatorLookup(Marten.Services.Events.AggregationLookupStrategy.UsePrivateApply);
                      _.Events.DatabaseSchemaName = schemaName;
                      _.DatabaseSchemaName = schemaName;

                      _.Events.InlineProjections.AggregateStreamsWith<Domain.ComponentAggregate>();
                      /* _.Events.InlineProjections.Add(new AllAccountsSummaryViewProjection()); */
                      /* _.Events.InlineProjections.Add(new AccountSummaryViewProjection()); */
                      /* _.Events.InlineProjections.Add(new ClientsViewProjection()); */

                      _.Events.AddEventType(typeof(Component.Create.Event));
                  });
                });
            services.AddScoped<Aggregate.IAggregateRepository, Aggregate.AggregateRepository>();
        }
    }
}