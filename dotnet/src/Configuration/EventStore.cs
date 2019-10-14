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
using Domain = Icon.Domain;
using Component = Icon.Domain.Component;
using System.Threading.Tasks;
using System;
using WebPWrecover.Services;
using IdentityServer4.Validation;

namespace Icon.Configuration {
  class EventStore {
    public static void ConfigureServices(IServiceCollection services, string connectionString, string schemaName) {
      services.AddScoped(sp =>
          {
          var documentStore = Marten.DocumentStore.For(_ =>
              {
              _.Connection(connectionString);
              _.AutoCreateSchemaObjects = Marten.AutoCreate.None;
              _.Events.DatabaseSchemaName = schemaName;
              _.DatabaseSchemaName = schemaName;

              _.Events.InlineProjections.AggregateStreamsWith<Domain.ComponentAggregate>();
              /* options.Events.InlineProjections.Add(new AllAccountsSummaryViewProjection()); */
              /* options.Events.InlineProjections.Add(new AccountSummaryViewProjection()); */
              /* options.Events.InlineProjections.Add(new ClientsViewProjection()); */

              _.Events.AddEventType(typeof(Component.Create.Event));
              });
          return documentStore.OpenSession();
          });
    }
  }
}
