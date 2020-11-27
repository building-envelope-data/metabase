// Inspired by https://hotchocolate.io/docs/aspnet#asp-net-core-

using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Execution;
using Infrastructure.ValueObjects;
using Microsoft.Extensions.Hosting; // Provides `IsDevelopment` for `IWebHostEnvironment`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment?view=dotnet-plat-ext-3.1 and https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment?view=aspnetcore-3.1
// using Microsoft.AspNetCore.Builder; // UseWebSockets
using GraphQlX = Metabase.GraphQl;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using QueryExecutionOptions = HotChocolate.Execution.Configuration.QueryExecutionOptions;

namespace Metabase.Configuration
{
    public sealed class GraphQl
      : Infrastructure.Configuration.GraphQl
    {
        private GraphQl() { }

        public static void ConfigureServices(IServiceCollection services)
        {
            Infrastructure.Configuration.GraphQl.ConfigureServices(
                services,
                schemaBuilder =>
                    schemaBuilder
                    .AddQueryType<GraphQlX.Query>()
                    .AddMutationType<GraphQlX.Mutation>()
                    // .AddSubscriptionType<SubscriptionType>()

                    .AddType<GraphQlX.StakeholderType>()

                    .AddType<GraphQlX.OpticalDataType>()
                    .AddType<GraphQlX.CalorimetricDataType>()
                    .AddType<GraphQlX.PhotovoltaicDataType>()
                    .AddType<GraphQlX.HygrothermalDataType>()
                );
        }

        public static new void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment environment
            )
        {
            Infrastructure.Configuration.GraphQl.Configure(app, environment);
        }
    }
}