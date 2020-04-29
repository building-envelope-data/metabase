// Inspired by https://hotchocolate.io/docs/aspnet#asp-net-core-

using ValueObjects = Icon.ValueObjects;
using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.AspNetCore.GraphiQL;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Subscriptions;
using HotChocolate.Execution;
using QueryExecutionOptions = HotChocolate.Execution.Configuration.QueryExecutionOptions;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Microsoft.Extensions.Hosting; // Provides `IsDevelopment` for `IWebHostEnvironment`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment?view=dotnet-plat-ext-3.1 and https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment?view=aspnetcore-3.1
using Microsoft.AspNetCore.Builder; // UseWebSockets
using GraphQlX = Icon.GraphQl;

namespace Icon.Configuration
{
    public sealed class GraphQl
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Add in-memory event provider
            services.AddInMemorySubscriptionProvider();

            // https://hotchocolate.io/docs/dataloaders
            services.AddDataLoaderRegistry();

            services.AddGraphQL(serviceProvider =>
                SchemaBuilder.New()
                  /* .EnableRelaySupport() */
                  .AddServices(serviceProvider)
                  // Adds the authorize directive and
                  // enable the authorization middleware.
                  .AddAuthorizeDirectiveType()

                  .BindClrType<ValueObjects.Id, GraphQlX.NonEmptyUuidType>()
                  .BindClrType<ValueObjects.Timestamp, GraphQlX.TimestampType>()

                  .AddQueryType<GraphQlX.Query>()
                  .AddMutationType<GraphQlX.Mutation>()
                  /* .AddSubscriptionType<SubscriptionType>() */

                  .AddType<GraphQlX.Node>()
                  .AddType<GraphQlX.StakeholderBase>()

                  .Create(),
                  new QueryExecutionOptions
                  {
                      // https://hotchocolate.io/docs/options#members
                      // https://github.com/ChilliCream/hotchocolate/blob/master/src/Core/Core/Execution/Configuration/QueryExecutionOptions.cs
                      /* MaxExecutionDepth = 50, // https://hotchocolate.io/docs/security#query-depth */
                      /* MaxOperationComplexity = 50, // https://hotchocolate.io/docs/security#query-complexity */
                      /* UseComplexityMultipliers = true, // https://hotchocolate.io/docs/security#query-complexity */
                      /* TracingPreference = TracingPreference.Always */
                  });

            services.AddQueryRequestInterceptor(
                (httpContext, requestBuilder, cancellationToken) =>
            {
                /* var identity = new ClaimsIdentity("abc"); */
                /* identity.AddClaim(new Claim(ClaimTypes.Country, "us")); */
                /* ctx.User = new ClaimsPrincipal(identity); */
                /* builder.SetProperty(nameof(ClaimsPrincipal), ctx.User); */
                GraphQlX.TimestampHelpers.StoreRequest(
                    ValueObjects.Timestamp.Now,
                    requestBuilder
                    );
                return Task.CompletedTask;
            });

            services.AddDiagnosticObserver<GraphQlX.DiagnosticObserver>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            // TODO Do we want `UseWebSockets` here?
            var graphQl = app
                  .UseWebSockets()
                  .UseGraphQL(
                      new QueryMiddlewareOptions
                      {
                          Path = "/graphql",
                          EnableSubscriptions = false
                      }
                      );
            if (environment.IsDevelopment())
            {
                graphQl
                  .UseGraphiQL("/graphql")
                .UsePlayground("/graphql")
                .UseVoyager("/graphql");
            }
        }
    }
}