using System;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Execution;
using Microsoft.Extensions.Hosting; // Provides `IsDevelopment` for `IWebHostEnvironment`, see https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostenvironment?view=dotnet-plat-ext-3.1 and https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.iwebhostenvironment?view=aspnetcore-3.1
// using Microsoft.AspNetCore.Builder; // UseWebSockets
using IApplicationBuilder = Microsoft.AspNetCore.Builder.IApplicationBuilder;
using IServiceCollection = Microsoft.Extensions.DependencyInjection.IServiceCollection;
using IWebHostEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;
using HotChocolate.Execution.Configuration;
using GraphQlX = Infrastructure.GraphQl;

namespace Infrastructure.Configuration
{
    public abstract class GraphQl
    {
        public static void ConfigureServices(
            IServiceCollection services,
            Func<IRequestExecutorBuilder, IRequestExecutorBuilder> configureExecutorBuilder
            )
        {
          configureExecutorBuilder(
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
                options.IncludeExceptionDetails = true; // TODO Only in developer mode. Default is `Debugger.IsAttached`.
                /* options.QueryCacheSize = ...; */
                options.TracingPreference = HotChocolate.Execution.Options.TracingPreference.Always; // TODO Should we use `Never` (the default) or `OnDemand`?
                /* options.UseComplexityMultipliers = ...; */
                }
                )
            // TODO Configure `https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Validation/Options/ValidationOptions.cs`. But how?
            // TODO Subscriptions
            /* .AddInMemorySubscriptions() */
            // TODO Persisted queries
            /* .AddFileSystemQueryStorage("./persisted_queries") */
            /* .UsePersistedQueryPipeline(); */
            /* TODO services.AddDiagnosticObserver<GraphQlX.DiagnosticObserver>(); */
              );
        }
    }
}