using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Types;
using HotChocolate.Data.Filters;
using Microsoft.Extensions.Hosting; // Provides `IsDevelopment` for `IWebHostEnvironment`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment?view=dotnet-plat-ext-3.1 and https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment?view=aspnetcore-3.1
// using Microsoft.AspNetCore.Builder; // UseWebSockets
using GraphQlX = Metabase.GraphQl;
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

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
                executorBuilder =>
                    executorBuilder
                    .AddQueryType(d => d.Name(nameof(GraphQlX.Query)))
                        .AddType<GraphQlX.Users.UserQueries>()
                        .AddType<GraphQlX.Components.ComponentQueries>()
                    .AddMutationType(d => d.Name(nameof(GraphQlX.Mutation)))
                        .AddType<GraphQlX.Users.UserMutations>()
                        .AddType<GraphQlX.Components.ComponentMutations>()
                    /* .AddSubscriptionType(d => d.Name(nameof(GraphQl.Subscription))) */
                    /*     .AddType<ComponentSubscriptions>() */
                    // Object Types
                    .AddType<GraphQlX.Users.UserType>()
                    .AddType<GraphQlX.Components.ComponentType>()
                    // Data Loaders
                    .AddDataLoader<GraphQlX.Components.ComponentByIdDataLoader>()
                    // Filtering
                    /* .AddConvention<IFilterConvention>( */
                    /*   new FilterConventionExtension(descriptor => */
                    /*     { */
                    /*     descriptor.BindRuntimeType<Data.Component, GraphQlX.Components.ComponentFilterType>(); */
                    /*     } */
                    /*     ) */
                    /*   ) */
                );
        }
    }
}
