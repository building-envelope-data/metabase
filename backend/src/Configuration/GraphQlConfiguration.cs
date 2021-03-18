using HotChocolate;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
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
            .BindRuntimeType<uint, GraphQl.Common.UIntType>()
            // Types
            .AddType<GraphQl.Common.OpenEndedDateTimeRangeType>()
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
                  /* TODO services.AddDiagnosticObserver<GraphQl.DiagnosticObserver>(); */
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
                  .AddQueryType(d => d.Name(nameof(GraphQl.Query)))
                      .AddType<GraphQl.Components.ComponentQueries>()
                      .AddType<GraphQl.DataFormats.DataFormatQueries>()
                      .AddType<GraphQl.Databases.DatabaseQueries>()
                      .AddType<GraphQl.Institutions.InstitutionQueries>()
                      .AddType<GraphQl.Methods.MethodQueries>()
                      .AddType<GraphQl.OpenIdConnect.OpendIdConnectQueries>()
                      .AddType<GraphQl.Users.UserQueries>()
                  .AddMutationType(d => d.Name(nameof(GraphQl.Mutation)))
                      .AddType<GraphQl.Components.ComponentMutations>()
                      .AddType<GraphQl.DataFormats.DataFormatMutations>()
                      .AddType<GraphQl.Databases.DatabaseMutations>()
                      .AddType<GraphQl.Institutions.InstitutionMutations>()
                      .AddType<GraphQl.Methods.MethodMutations>()
                      .AddType<GraphQl.Users.UserMutations>()
                  /* .AddSubscriptionType(d => d.Name(nameof(GraphQl.Subscription))) */
                  /*     .AddType<ComponentSubscriptions>() */
                  // Object Types
                  .AddType<GraphQl.Components.ComponentType>()
                  .AddType<GraphQl.DataFormats.DataFormatType>()
                  .AddType<GraphQl.Databases.DatabaseType>()
                  .AddType<GraphQl.Institutions.InstitutionType>()
                  .AddType<GraphQl.Methods.MethodType>()
                  .AddType<GraphQl.Numerations.NumerationType>()
                  .AddType<GraphQl.OpenIdConnect.OpenIdConnectApplicationType>()
                  .AddType<GraphQl.OpenIdConnect.OpenIdConnectAuthorizationType>()
                  .AddType<GraphQl.OpenIdConnect.OpenIdConnectScopeType>()
                  .AddType<GraphQl.OpenIdConnect.OpenIdConnectTokenType>()
                  .AddType<GraphQl.Publications.PublicationType>()
                  .AddType<GraphQl.References.ReferenceType>()
                  .AddType<GraphQl.Stakeholders.StakeholderType>()
                  .AddType<GraphQl.Standards.StandardType>()
                  .AddType<GraphQl.Users.UserType>()
                  // Data Loaders
                  .AddDataLoader<GraphQl.Components.ComponentByIdDataLoader>()
                  .AddDataLoader<GraphQl.DataFormats.DataFormatByIdDataLoader>()
                  .AddDataLoader<GraphQl.Databases.DatabaseByIdDataLoader>()
                  .AddDataLoader<GraphQl.Institutions.InstitutionByIdDataLoader>()
                  .AddDataLoader<GraphQl.Institutions.InstitutionRepresentativesByInstitutionIdDataLoader>()
                  .AddDataLoader<GraphQl.Methods.MethodByIdDataLoader>()
                  // Filtering
                  .AddConvention<IFilterConvention>(
                   new FilterConventionExtension(descriptor =>
                     {
                         descriptor.BindRuntimeType<Data.Component, GraphQl.Components.ComponentFilterType>();
                         descriptor.BindRuntimeType<Data.DataFormat, GraphQl.DataFormats.DataFormatFilterType>();
                         descriptor.BindRuntimeType<Data.Database, GraphQl.Databases.DatabaseFilterType>();
                         descriptor.BindRuntimeType<Data.Institution, GraphQl.Institutions.InstitutionFilterType>();
                         descriptor.BindRuntimeType<Data.Method, GraphQl.Methods.MethodFilterType>();
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