using HotChocolate;
using HotChocolate.Data.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using GraphQlX = Metabase.GraphQl;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;

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
                    .AddHttpRequestInterceptor(async (httpContext, requestExecutor, requestBuilder, cancellationToken) =>
                    {
                        var authenticateResult = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme).ConfigureAwait(false);
                        if (authenticateResult.Succeeded && authenticateResult.Principal is not null)
                            httpContext.User = authenticateResult.Principal;
                    })
                    .AddQueryType(d => d.Name(nameof(GraphQlX.Query)))
                        .AddType<GraphQlX.Users.UserQueries>()
                        .AddType<GraphQlX.Components.ComponentQueries>()
                        .AddType<GraphQlX.Databases.DatabaseQueries>()
                        .AddType<GraphQlX.Institutions.InstitutionQueries>()
                        .AddType<GraphQlX.Methods.MethodQueries>()
                    .AddMutationType(d => d.Name(nameof(GraphQlX.Mutation)))
                        .AddType<GraphQlX.Users.UserMutations>()
                        .AddType<GraphQlX.Components.ComponentMutations>()
                        .AddType<GraphQlX.Databases.DatabaseMutations>()
                        .AddType<GraphQlX.Institutions.InstitutionMutations>()
                        .AddType<GraphQlX.Methods.MethodMutations>()
                    /* .AddSubscriptionType(d => d.Name(nameof(GraphQl.Subscription))) */
                    /*     .AddType<ComponentSubscriptions>() */
                    // Object Types
                    .AddType<GraphQlX.Users.UserType>()
                    .AddType<GraphQlX.Components.ComponentType>()
                    .AddType<GraphQlX.Databases.DatabaseType>()
                    .AddType<GraphQlX.Institutions.InstitutionType>()
                    .AddType<GraphQlX.Methods.MethodType>()
                    .AddType<GraphQlX.Persons.PersonType>()
                    // Data Loaders
                    .AddDataLoader<GraphQlX.Components.ComponentByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Databases.DatabaseByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Institutions.InstitutionByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Institutions.InstitutionRepresentativesByInstitutionIdDataLoader>()
                    .AddDataLoader<GraphQlX.Methods.MethodByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Persons.PersonByIdDataLoader>()
                    // Filtering
                    .AddConvention<IFilterConvention>(
                     new FilterConventionExtension(descriptor =>
                       {
                           descriptor.BindRuntimeType<Data.Component, GraphQlX.Components.ComponentFilterType>();
                           descriptor.BindRuntimeType<Data.Database, GraphQlX.Databases.DatabaseFilterType>();
                           descriptor.BindRuntimeType<Data.Institution, GraphQlX.Institutions.InstitutionFilterType>();
                           descriptor.BindRuntimeType<Data.Method, GraphQlX.Methods.MethodFilterType>();
                       }
                       )
                     )
                );
        }
    }
}