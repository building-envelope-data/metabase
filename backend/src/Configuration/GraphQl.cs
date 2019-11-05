// Inspired by https://hotchocolate.io/docs/aspnet#asp-net-core-

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
            // Add the custom services like repositories etc ...
            /* services.AddSingleton<CharacterRepository>(); */
            /* services.AddSingleton<ReviewRepository>(); */

            // Add in-memory event provider
            services.AddInMemorySubscriptionProvider();

            // https://hotchocolate.io/docs/dataloaders
            services.AddDataLoaderRegistry();

            services.AddGraphQL(serviceProvider =>
                SchemaBuilder.New()
                  .AddServices(serviceProvider)
                  // Adds the authorize directive and
                  // enable the authorization middleware.
                  .AddAuthorizeDirectiveType()
                  .AddQueryType<GraphQlX.QueryType>()
                  .AddMutationType<GraphQlX.MutationType>()
                  /* .AddSubscriptionType<SubscriptionType>() */
                  .AddType<GraphQlX.ComponentType>()
                  .AddType<GraphQlX.ComponentVersionType>()
                  .AddType<GraphQlX.ComponentVersionInputType>()
                  .Create(),
                  new QueryExecutionOptions
                  {
                      // https://hotchocolate.io/docs/options#members
                      MaxOperationComplexity = 10,
                      UseComplexityMultipliers = true
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