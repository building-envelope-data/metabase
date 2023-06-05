using System;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Data.Filters;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Validation.AspNetCore;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using Microsoft.Extensions.Logging;
using HotChocolate.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Metabase.Authorization;

namespace Metabase.Configuration
{
    public static class GraphQlConfiguration
    {
        public static void ConfigureServices(
            IServiceCollection services,
            IWebHostEnvironment environment
            )
        {
            services
            .AddMemoryCache() // Needed by the automatic persisted query pipeline
            .AddSha256DocumentHashProvider(HotChocolate.Language.HashFormat.Hex) // Needed by the automatic persisted query pipeline
            .AddGraphQLServer()
            // Services https://chillicream.com/docs/hotchocolate/v13/integrations/entity-framework#registerdbcontext
            .RegisterDbContext<Data.ApplicationDbContext>(DbContextKind.Pooled)
            .AddMutationConventions(new MutationConventionOptions { ApplyToAllMutations = false })
            // Extensions
            .AddProjections()
            .AddFiltering()
            .AddSorting()
            .AddAuthorization()
            .AddApolloTracing(
                HotChocolate.Execution.Options.TracingPreference.OnDemand
            )
            .AddGlobalObjectIdentification()
            .AddQueryFieldToMutationPayloads()
            .ModifyOptions(options =>
              {
                  // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Configuration/Contracts/ISchemaOptions.cs
                  options.StrictValidation = true;
                  options.UseXmlDocumentation = false;
                  options.SortFieldsByName = true;
                  options.RemoveUnreachableTypes = false;
                  options.DefaultBindingBehavior = BindingBehavior.Implicit;
                  /* options.FieldMiddleware = ... */
              }
              )
            .ModifyRequestOptions(options =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Execution/Options/RequestExecutorOptions.cs
                    /* options.ExecutionTimeout = ...; */
                    options.IncludeExceptionDetails = environment.IsDevelopment(); // Default is `Debugger.IsAttached`.
                    /* options.QueryCacheSize = ...; */
                    /* options.UseComplexityMultipliers = ...; */
                }
                )
                  // Subscriptions
                  /* .AddInMemorySubscriptions() */
                  // Persisted queries
                  /* .AddFileSystemQueryStorage("./persisted_queries") */
                  /* .UsePersistedQueryPipeline(); */
                  // HotChocolate uses the default authentication scheme,
                  // which we set to `null` in `AuthConfiguration` to force
                  // users to be explicit about what scheme to use when
                  // making it easier to grasp the various authentication
                  // flows.
                  .AddHttpRequestInterceptor(async (httpContext, requestExecutor, requestBuilder, cancellationToken) =>
                  {
                      try
                      {
                          await HttpContextAuthentication.Authenticate(httpContext);
                      }
                      catch (Exception e)
                      {
                          // TODO Log to a `ILogger<GraphQlConfiguration>` instead.
                          Console.WriteLine(e);
                      }
                  })
                  .AddDiagnosticEventListener(_ =>
                      new GraphQl.LoggingDiagnosticEventListener(
                          _.GetApplicationService<ILogger<GraphQl.LoggingDiagnosticEventListener>>()
                      )
                  )
                  // Scalar Types
                  .AddType(new UuidType("Uuid", defaultFormat: 'D')) // https://chillicream.com/docs/hotchocolate/defining-a-schema/scalars#uuid-type
                  .AddType(new UrlType("Url"))
                  .AddType(new JsonType("Any", bind: BindingBehavior.Implicit)) // https://chillicream.com/blog/2023/02/08/new-in-hot-chocolate-13#json-scalar
                  // .BindRuntimeType<Guid, MyUuidType>()
                  // Query Types
                  .AddQueryType(d => d.Name(nameof(GraphQl.Query)))
                      .AddType<GraphQl.Components.ComponentQueries>()
                      .AddType<GraphQl.DataFormats.DataFormatQueries>()
                      .AddType<GraphQl.Databases.DatabaseQueries>()
                      .AddType<GraphQl.Institutions.InstitutionQueries>()
                      .AddType<GraphQl.Methods.MethodQueries>()
                      .AddType<GraphQl.OpenIdConnect.OpendIdConnectQueries>()
                      .AddType<GraphQl.Users.UserQueries>()
                  // Mutation Types
                  .AddMutationType(d => d.Name(nameof(GraphQl.Mutation)))
                      .AddType<GraphQl.ComponentAssemblies.ComponentAssemblyMutations>()
                      .AddType<GraphQl.ComponentGeneralizations.ComponentGeneralizationMutations>()
                      .AddType<GraphQl.ComponentManufacturers.ComponentManufacturerMutations>()
                      .AddType<GraphQl.ComponentVariants.ComponentVariantMutations>()
                      .AddType<GraphQl.Components.ComponentMutations>()
                      .AddType<GraphQl.DataFormats.DataFormatMutations>()
                      .AddType<GraphQl.Databases.DatabaseMutations>()
                      .AddType<GraphQl.InstitutionMethodDevelopers.InstitutionMethodDeveloperMutations>()
                      .AddType<GraphQl.InstitutionRepresentatives.InstitutionRepresentativeMutations>()
                      .AddType<GraphQl.Institutions.InstitutionMutations>()
                      .AddType<GraphQl.Methods.MethodMutations>()
                      .AddType<GraphQl.UserMethodDevelopers.UserMethodDeveloperMutations>()
                      .AddType<GraphQl.Users.UserMutations>()
                  /* .AddSubscriptionType(d => d.Name(nameof(GraphQl.Subscription))) */
                  /*     .AddType<ComponentSubscriptions>() */
                  // Object Types
                  .AddType<GraphQl.Common.OpenEndedDateTimeRangeType>()
                  .AddType<GraphQl.Components.ComponentType>()
                  .AddType<GraphQl.DataFormats.DataFormatType>()
                  .AddType<GraphQl.DataX.CalorimetricData>()
                  .AddType<GraphQl.DataX.DataApproval>()
                  .AddType<GraphQl.DataX.GetHttpsResourceTreeNonRootVertex>()
                  .AddType<GraphQl.DataX.GetHttpsResourceTreeRoot>()
                  .AddType<GraphQl.DataX.HygrothermalData>()
                  .AddType<GraphQl.DataX.OpticalData>()
                  .AddType<GraphQl.DataX.PhotovoltaicData>()
                  .AddType<GraphQl.DataX.ResponseApproval>()
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
                         descriptor.BindRuntimeType<Data.User, GraphQl.Users.UserFilterType>();
                     }
                     )
                   )
                  // Paging
                  .SetPagingOptions(
                      new PagingOptions
                      {
                          MaxPageSize = int.MaxValue,
                          DefaultPageSize = int.MaxValue,
                          IncludeTotalCount = true
                      }
                  )
                  .UseAutomaticPersistedQueryPipeline()
                  .AddInMemoryQueryStorage(); // Needed by the automatic persisted query pipeline
        }

        private sealed class MyUuidType : UuidType
        {
            private const string _specifiedBy = "https://tools.ietf.org/html/rfc4122";

            public MyUuidType(
                string name,
                string? description = null,
                char defaultFormat = '\0',
                bool enforceFormat = false,
                BindingBehavior bind = BindingBehavior.Explicit
            )
                : base(name, description: description, defaultFormat: defaultFormat, enforceFormat: enforceFormat, bind: bind)
            {
                SpecifiedBy = new Uri(_specifiedBy);
            }
        }

        private sealed class MyUrlType : UrlType
        {
            private const string _specifiedBy = "https://tools.ietf.org/html/rfc3986";

            public MyUrlType(
                string name,
                string? description = null,
                BindingBehavior bind = BindingBehavior.Explicit)
                : base(name, description: description, bind: bind)
            {
                SpecifiedBy = new Uri(_specifiedBy);
            }
        }
    }
}