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
using Microsoft.AspNetCore.Builder; // UseWebSockets
using GraphQlX = Icon.GraphQl;

namespace Icon.Configuration
{
    class GraphQl
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Add in-memory event provider
            services.AddInMemorySubscriptionProvider();

            // https://hotchocolate.io/docs/dataloaders
            services.AddDataLoaderRegistry();

            services.AddGraphQL(serviceProvider =>
                SchemaBuilder.New()
                  .AddServices(serviceProvider)
                  /* .EnableRelaySupport() */
                  // Adds the authorize directive and
                  // enable the authorization middleware.
                  .AddAuthorizeDirectiveType()
                  .AddQueryType<GraphQlX.Query>()
                  .AddMutationType<GraphQlX.Mutation>()
                  /* .AddSubscriptionType<SubscriptionType>() */
                  .AddType<GraphQlX.INode>()
                  .AddType<GraphQlX.ComponentResolvers>()
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
                GraphQlX.Timestamp.StoreRequest(
                    ValueObjects.Timestamp.Now,
                    requestBuilder
                    );
                return Task.CompletedTask;
            });

            services.AddDiagnosticObserver<GraphQlX.DiagnosticObserver>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment _environment)
        {
            // TODO Do we want `UseWebSockets` here?
            var graphQl = app
                  .UseWebSockets()
                  .UseGraphQL("/graphql");
            // TODO Where does `InDevelopment` hide? It must be some kind of extension method.
            /* if (_environment.InDevelopment()) */
            /* { */
            graphQl
              .UseGraphiQL("/graphql")
            .UsePlayground("/graphql")
            .UseVoyager("/graphql");
            /* } */
        }
    }
}