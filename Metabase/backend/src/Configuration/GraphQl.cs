using HotChocolate;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
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
                        // TODO Remove when we do not use cookies in the web frontend anymore (except for the OIDC login)
                        var authenticateResult = await httpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme).ConfigureAwait(false);
                        if (authenticateResult.Succeeded && authenticateResult.Principal is not null)
                            httpContext.User = authenticateResult.Principal;
                    })
                    .AddQueryType(d => d.Name(nameof(GraphQlX.Query)))
                        .AddType<GraphQlX.Components.ComponentQueries>()
                        .AddType<GraphQlX.DataFormats.DataFormatQueries>()
                        .AddType<GraphQlX.Databases.DatabaseQueries>()
                        .AddType<GraphQlX.Institutions.InstitutionQueries>()
                        .AddType<GraphQlX.Methods.MethodQueries>()
                        .AddType<GraphQlX.OpenIdConnect.OpendIdConnectQueries>()
                        .AddType<GraphQlX.Users.UserQueries>()
                    .AddMutationType(d => d.Name(nameof(GraphQlX.Mutation)))
                        .AddType<GraphQlX.Components.ComponentMutations>()
                        .AddType<GraphQlX.DataFormats.DataFormatMutations>()
                        .AddType<GraphQlX.Databases.DatabaseMutations>()
                        .AddType<GraphQlX.Institutions.InstitutionMutations>()
                        .AddType<GraphQlX.Methods.MethodMutations>()
                        .AddType<GraphQlX.Users.UserMutations>()
                    /* .AddSubscriptionType(d => d.Name(nameof(GraphQl.Subscription))) */
                    /*     .AddType<ComponentSubscriptions>() */
                    // Object Types
                    .AddType<GraphQlX.Components.ComponentType>()
                    .AddType<GraphQlX.DataFormats.DataFormatType>()
                    .AddType<GraphQlX.Databases.DatabaseType>()
                    .AddType<GraphQlX.Institutions.InstitutionType>()
                    .AddType<GraphQlX.Methods.MethodType>()
                    .AddType<GraphQlX.Numerations.NumerationType>()
                    .AddType<GraphQlX.OpenIdConnect.OpenIdConnectApplicationType>()
                    .AddType<GraphQlX.OpenIdConnect.OpenIdConnectAuthorizationType>()
                    .AddType<GraphQlX.OpenIdConnect.OpenIdConnectScopeType>()
                    .AddType<GraphQlX.OpenIdConnect.OpenIdConnectTokenType>()
                    .AddType<GraphQlX.Publications.PublicationType>()
                    .AddType<GraphQlX.References.ReferenceType>()
                    .AddType<GraphQlX.Stakeholders.StakeholderType>()
                    .AddType<GraphQlX.Standards.StandardType>()
                    .AddType<GraphQlX.Users.UserType>()
                    // Data Loaders
                    .AddDataLoader<GraphQlX.Components.ComponentByIdDataLoader>()
                    .AddDataLoader<GraphQlX.DataFormats.DataFormatByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Databases.DatabaseByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Institutions.InstitutionByIdDataLoader>()
                    .AddDataLoader<GraphQlX.Institutions.InstitutionRepresentativesByInstitutionIdDataLoader>()
                    .AddDataLoader<GraphQlX.Methods.MethodByIdDataLoader>()
                    // Filtering
                    .AddConvention<IFilterConvention>(
                     new FilterConventionExtension(descriptor =>
                       {
                           descriptor.BindRuntimeType<Data.Component, GraphQlX.Components.ComponentFilterType>();
                           descriptor.BindRuntimeType<Data.DataFormat, GraphQlX.DataFormats.DataFormatFilterType>();
                           descriptor.BindRuntimeType<Data.Database, GraphQlX.Databases.DatabaseFilterType>();
                           descriptor.BindRuntimeType<Data.Institution, GraphQlX.Institutions.InstitutionFilterType>();
                           descriptor.BindRuntimeType<Data.Method, GraphQlX.Methods.MethodFilterType>();
                       }
                       )
                     )
                    // Paging
                    .SetPagingOptions(
                        new PagingOptions
                        {
                            // MaxPageSize = ...,
                            // DefaultPageSize = ...,
                            IncludeTotalCount = true
                        }
                    )
                );
        }
    }
}