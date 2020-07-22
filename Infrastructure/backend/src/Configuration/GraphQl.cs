using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using Infrastructure.ValueObjects;
using Microsoft.Extensions.Hosting; // Provides `IsDevelopment` for `IWebHostEnvironment`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment?view=dotnet-plat-ext-3.1 and https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment?view=aspnetcore-3.1
// using Microsoft.AspNetCore.Builder; // UseWebSockets
using GraphQlX = Infrastructure.GraphQl;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using QueryExecutionOptions = HotChocolate.Execution.Configuration.QueryExecutionOptions;

namespace Infrastructure.Configuration
{
    public abstract class GraphQl
    {
        public static void ConfigureServices(
            IServiceCollection services,
            Func<ISchemaBuilder, ISchemaBuilder> configureSchemaBuilder
            )
        {
            // How subscriptions can be used is explained on
            // https://hotchocolate.io/docs/code-first-subscription
            // services.AddInMemorySubscriptionProvider();
            // services.AddRedisSubscriptionProvider(...);

            // https://hotchocolate.io/docs/dataloaders
            services.AddDataLoaderRegistry();

            services.AddGraphQL(serviceProvider =>
                configureSchemaBuilder(NewSchemaBuilder(serviceProvider)).Create(),
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
                GraphQlX.TimestampHelpers.Store(
                    Timestamp.Now,
                    requestBuilder
                    );
                return Task.CompletedTask;
            });

            services.AddDiagnosticObserver<GraphQlX.DiagnosticObserver>();
        }

        private static ISchemaBuilder NewSchemaBuilder(IServiceProvider serviceProvider)
        {
            return
                  SchemaBuilder.New()
                    /* .EnableRelaySupport() */
                    .AddServices(serviceProvider)
                    // Adds the authorize directive and
                    // enable the authorization middleware.
                    .AddAuthorizeDirectiveType()

                    .BindClrType<Id, GraphQlX.NonEmptyUuidType>()
                    .BindClrType<Timestamp, GraphQlX.TimestampType>()
                    .BindClrType<DateTime, GraphQlX.PreciseDateTimeType>()
                    .BindClrType<DateTimeOffset, GraphQlX.PreciseDateTimeType>()

                    .AddType<GraphQlX.NodeType>()
                    .AddType<HotChocolate.Types.Relay.PageInfoType>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            var graphQl = app
                  // .UseWebSockets()
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