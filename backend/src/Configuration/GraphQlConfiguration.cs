using HotChocolate;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using GraphQlX = Metabase.GraphQl;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;

namespace Metabase.Configuration
{
    public static class GraphQlConfiguration
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment
            )
        {
              services.AddGraphQLServer()
              // Scalars
              .BindRuntimeType<uint, GraphQlX.Common.UIntType>()
              // Types
              .AddType<GraphQlX.Common.OpenEndedDateTimeRangeType>()
              // Extensions
              .AddProjections()
              .AddFiltering()
              .AddSorting()
              .AddAuthorization()
              .EnableRelaySupport()
              .ModifyOptions(options =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Configuration/Contracts/ISchemaOptions.cs
                    options.StrictValidation = true;
                    options.UseXmlDocumentation = false;
                    options.SortFieldsByName = true;
                    options.RemoveUnreachableTypes = false;
                    options.DefaultBindingBehavior = HotChocolate.Types.BindingBehavior.Implicit;
                    /* options.FieldMiddleware = ... */
                }
                )
              .ModifyRequestOptions(options =>
                  {
                      // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Execution/Options/RequestExecutorOptions.cs
                      /* options.ExecutionTimeout = ...; */
                      options.IncludeExceptionDetails = environment.IsDevelopment(); // Default is `Debugger.IsAttached`.
                      /* options.QueryCacheSize = ...; */
                      options.TracingPreference = HotChocolate.Execution.Options.TracingPreference.Always; // TODO Should we use `Never` (the default) or `OnDemand`?
                      /* options.UseComplexityMultipliers = ...; */
                  }
                  )
                // TODO Configure `https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Validation/Options/ValidationOptions.cs`. But how?
                // Subscriptions
                /* .AddInMemorySubscriptions() */
                // TODO Persisted queries
                /* .AddFileSystemQueryStorage("./persisted_queries") */
                /* .UsePersistedQueryPipeline(); */
                /* TODO services.AddDiagnosticObserver<GraphQlX.DiagnosticObserver>(); */
                    .AddHttpRequestInterceptor(async (httpContext, requestExecutor, requestBuilder, cancellationToken) =>
                    {
                        // HotChocolate uses the default cookie authentication
                        // scheme `IdentityConstants.ApplicationScheme` by
                        // default. We want it to use the JavaScript Web Token
                        // (JWT), aka, Access Token, provided as `Authorization`
                        // HTTP header with the prefix `Bearer` as issued by
                        // OpenIddict though. This Access Token includes Scopes
                        // and Claims.
                        var authenticateResult = await httpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme).ConfigureAwait(false);
                        if (authenticateResult.Succeeded && authenticateResult.Principal is not null)
                        {
                            httpContext.User = authenticateResult.Principal;
                        }
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
                    );
        }
    }
}