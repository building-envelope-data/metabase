// Inspired by https://hotchocolate.io/docs/aspnet#asp-net-core-

using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.AspNetCore.GraphiQL;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.Subscriptions;
using QueryExecutionOptions = HotChocolate.Execution.Configuration.QueryExecutionOptions;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using Microsoft.AspNetCore.Builder; // UseWebSockets

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

          services.AddGraphQL(serviceProvider =>
              SchemaBuilder.New()
                .AddServices(serviceProvider)

                // Adds the authorize directive and
                // enable the authorization middleware.
                .AddAuthorizeDirectiveType()

                /* .AddQueryType<QueryType>() */
                /* .AddMutationType<MutationType>() */
                /* .AddSubscriptionType<SubscriptionType>() */
                /* .AddType<HumanType>() */
                /* .AddType<DroidType>() */
                /* .AddType<EpisodeType>() */
                .Create(),
                new QueryExecutionOptions
                {
                    MaxOperationComplexity = 10,
                    UseComplexityMultipliers = true
                });
            services.AddGraphQL(
                SchemaBuilder.New());
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