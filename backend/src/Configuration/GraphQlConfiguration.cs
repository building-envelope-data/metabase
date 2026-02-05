using System;
using HotChocolate.AspNetCore;
using HotChocolate.Configuration;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Sorting;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.NodaTime;
using Metabase.Authentication;
using Metabase.Data;
using Metabase.GraphQl;
using Metabase.GraphQl.DataX;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace Metabase.Configuration;

public static class GraphQlConfiguration
{
    public static void ConfigureServices(
        IServiceCollection services,
        IWebHostEnvironment environment
    )
    {
        // Automatic-Persisted-Queries Services
        services
            .AddMemoryCache()
            .AddSha256DocumentHashProvider(HashFormat.Hex); // https://chillicream.com/docs/hotchocolate/v15/security/#fips-compliance
        // GraphQL Server
        services
            .AddGraphQLServer()
            // TODO add warmup task once we upgrade to version 16: https://chillicream.com/docs/hotchocolate/v16/server/warmup
            // .AddWarmupTask(async (executor, cancellationToken) =>
            // {
            //     await executor.ExecuteAsync("{ __typename }", cancellationToken);
            // })
            .DisableIntrospection(false) // if the introspection result becomes too big we need to disable it in production
            .BindRuntimeType<uint, NonNegativeIntType>()
            // Services https://chillicream.com/docs/hotchocolate/v13/integrations/entity-framework#registerdbcontext
            .RegisterDbContextFactory<ApplicationDbContext>()
            .AddMutationConventions(new MutationConventionOptions { ApplyToAllMutations = false })
            // Extensions
            .AddProjections()
            .AddFiltering<CustomFilterConvention>()
            .AddSorting<CustomSortConvention>()
            .AddQueryContext()
            .AddAuthorization()
            .AddGlobalObjectIdentification()
            .AddQueryFieldToMutationPayloads()
            .ModifyOptions(options =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Configuration/Contracts/ISchemaOptions.cs
                    options.StrictValidation = true;
                    options.UseXmlDocumentation = false;
                    options.SortFieldsByName = true;
                    options.RemoveUnreachableTypes = false;
                    options.RemoveUnusedTypeSystemDirectives = true;
                    options.DefaultBindingBehavior = BindingBehavior.Implicit;
                    // options.DefaultFieldBindingFlags = FieldBindingFlags.InstanceAndStatic;
                    options.EnableDirectiveIntrospection = true;
                    options.DefaultDirectiveVisibility = DirectiveVisibility.Public;
                    options.DefaultResolverStrategy = ExecutionStrategy.Parallel;
                    options.ValidatePipelineOrder = true;
                    options.StrictRuntimeTypeValidation = true;
                    options.EnableOneOf = true;
                    options.EnsureAllNodesCanBeResolved = true;
                    options.EnableFlagEnums = false;
                    options.EnableDefer = false;
                    options.EnableStream = false;
                    options.EnableSemanticNonNull = false;
                    options.StripLeadingIFromInterface = false;
                    options.EnableTag = true;
                    options.PublishRootFieldPagesToPromiseCache = true;
                }
            )
            .ModifyRequestOptions(options =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Execution/Options/RequestExecutorOptions.cs
                    options.ExecutionTimeout = TimeSpan.FromSeconds(120);
                    options.IncludeExceptionDetails = !environment.IsProduction(); // Default is `Debugger.IsAttached`.
                    /* options.QueryCacheSize = ...; */
                    /* options.UseComplexityMultipliers = ...; */
                    options.EnableSchemaFileSupport = true;
                }
            )
            // Configure
            // `https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Validation/Options/ValidationOptions.cs`.
            // .AddMaxExecutionDepthRule(5)
            // .SetIntrospectionAllowedDepth(maxAllowedOfTypeDepth: 16, maxAllowedListRecursiveDepth: 1)
            // .SetMaxAllowedValidationErrors(5)
            // Subscriptions
            /* .AddInMemorySubscriptions() */
            // Persisted queries
            /* .AddFileSystemOperationDocumentStorage("./persisted_operations") */
            /* .UsePersistedOperationPipeline(); */
            // HotChocolate uses the default authentication scheme,
            // which we set to `null` in `AuthConfiguration` to force
            // users to be explicit about what scheme to use when
            // making it easier to grasp the various authentication
            // flows.
            .AddHttpRequestInterceptor(async (httpContext, requestExecutor, requestBuilder, cancellationToken) =>
            {
                await httpContext.RequestServices
                    .GetRequiredService<GraphQlAuthenticationAndAntiforgeryHandler>()
                    .HandleAsync(httpContext, cancellationToken);
            })
            .AddDiagnosticEventListener(_ =>
                new ErrorLoggingDiagnosticEventListener(
                    _.GetRequiredService<ILogger<ErrorLoggingDiagnosticEventListener>>()
                )
            )
            // Scalar Types
            // TODO Use `MyUuidType` and `MyUrlType` (see code below)
            .AddType(new UuidType("Uuid", defaultFormat: 'D')) // https://chillicream.com/docs/hotchocolate/defining-a-schema/scalars#uuid-type
            .AddType(new UrlType("Url"))
            .AddType(new JsonType("Any", BindingBehavior.Implicit)) // https://chillicream.com/blog/2023/02/08/new-in-hot-chocolate-13#json-scalar
            .AddType<LocaleType>()
            .AddType<DurationType>()
            .AddType<DateTimeZoneType>()
            // .AddType<OffsetDateTimeType>()
            // Register converters between NodaTime's `OffsetDateTime` and .NET's
            // `DateTimeOffset` to reuse the existing `DateTimeType`
            // https://chillicream.com/docs/hotchocolate/v15/defining-a-schema/scalars#custom-converters
            .BindRuntimeType<OffsetDateTime, DateTimeType>()
            .AddTypeConverter<OffsetDateTime, DateTimeOffset>(
                _ => _.ToDateTimeOffset()
            )
            .AddTypeConverter<DateTimeOffset, OffsetDateTime>(
                _ => OffsetDateTime.FromDateTimeOffset(_)
            )
            // Object Types
            .AddType<DataConnection>()
            // Query, Mutation, Subscription, Object, and Input Types
            .AddQueryType(_ => _.Name(nameof(Query)))
            .AddMutationType(_ => _.Name(nameof(Mutation)))
            // .AddSubscriptionType(_ => _.Name(nameof(Subscription)))
            // auto-discover using `HotChocolate.Types.Analyzers`
            .AddTypes()
            // Paging
            .AddDbContextCursorPagingProvider()
            .ModifyPagingOptions(_ =>
                {
                    _.MaxPageSize = int.MaxValue - 1;
                    _.DefaultPageSize = 100;
                    _.IncludeTotalCount = true;
                    _.IncludeNodesField = false;
                    _.InferConnectionNameFromField = true;
                }
            )
            // Automatic Peristed Queries
            .UseAutomaticPersistedOperationPipeline()
            .AddInMemoryOperationDocumentStorage(); // Needed by the automatic persisted operation pipeline
    }

    // private sealed class MyUuidType : UuidType
    // {
    //     private const string SpecifiedByString = "https://tools.ietf.org/html/rfc4122";
    //
    //     public MyUuidType(
    //         string name,
    //         string? description = null,
    //         char defaultFormat = '\0',
    //         bool enforceFormat = false,
    //         BindingBehavior bind = BindingBehavior.Explicit
    //     )
    //         : base(name, description, defaultFormat, enforceFormat,
    //             bind)
    //     {
    //         SpecifiedBy = new Uri(SpecifiedByString, UriKind.Absolute);
    //     }
    // }

    // private sealed class MyUrlType : UrlType
    // {
    //     private const string SpecifiedByString = "https://tools.ietf.org/html/rfc3986";
    //
    //     public MyUrlType(
    //         string name,
    //         string? description = null,
    //         BindingBehavior bind = BindingBehavior.Explicit)
    //         : base(name, description, bind)
    //     {
    //         SpecifiedBy = new Uri(SpecifiedByString, UriKind.Absolute);
    //     }
    // }
}

// Inspired by https://chillicream.com/docs/hotchocolate/v15/api-reference/extending-filtering
public static class CustomFilterOperations
{
    // public const int InClosedInterval = 1025;
}

// internal record ClosedIntervalInput<T>(
//     T LowerBound,
//     T UpperBound
// );

// internal sealed class ClosedIntervalInputType<TSchemaType, TRuntimeType>
//     : InputObjectType<ClosedIntervalInput<TRuntimeType>>
//     where TSchemaType : class, IInputType
// {
//     protected override void Configure(
//         IInputObjectTypeDescriptor<ClosedIntervalInput<TRuntimeType>> descriptor
//     )
//     {
//         descriptor.BindFieldsExplicitly();
//         descriptor
//             .Field(f => f.LowerBound)
//             .Type<TSchemaType>();
//         descriptor
//             .Field(f => f.UpperBound)
//             .Type<TSchemaType>();
//     }
// }

// public sealed class QueryableComparableInClosedIntervalHandler : QueryableComparableOperationHandler
// {
//     public QueryableComparableInClosedIntervalHandler(
//         ITypeConverter typeConverter,
//         InputParser inputParser)
//         : base(typeConverter, inputParser)
//     {
//         CanBeNull = false;
//     }
// 
//     // This is used to match the handler to all `inClosedInterval` fields
//     protected override int Operation => CustomFilterOperations.InClosedInterval;
// 
//     public override Expression HandleOperation(
//         QueryableFilterContext context,
//         IFilterOperationField field,
//         IValueNode value,
//         object? parsedValue
//     )
//     {
//         // We get the instance of the context. This is the expression path to the property
//         // e.g. ~> y.gValue
//         var property = context.GetInstance();
//         // the parsed value is what was specified in the query
//         // e.g. ~> inClosedInterval: { lowerBound: 0.0, upperBound: 1.0 }
//         parsedValue = ParseValue(value, parsedValue, field.Type, context);
//         ArgumentNullException.ThrowIfNull(parsedValue);
//         if (parsedValue is ClosedIntervalInput<double> closedIntervalInput)
//         {
//             // Creates and returns the LINQ operation
//             // e.g. ~> 0.0 >= y.gValue && y.gValue <= 1.0
//             return Expression.And(
//                 FilterExpressionBuilder.GreaterThanOrEqual(property, closedIntervalInput.LowerBound),
//                 FilterExpressionBuilder.LowerThanOrEqual(property, closedIntervalInput.UpperBound)
//             );
//         }
//         // Something went wrong 😱
//         throw new InvalidOperationException();
//     }
// 
//     private new object? ParseValue(
//         IValueNode node,
//         object? parsedValue,
//         IType type,
//         QueryableFilterContext context
//     )
//     {
//         if (parsedValue is null)
//         {
//             return parsedValue;
//         }
//         var returnType = context.RuntimeTypes.Peek().Source;
//         return parsedValue;
//     }
// }

// public sealed class ExtendedComparableOperationFilterInputType<T>
//     : ComparableOperationFilterInputType<T>
// {
//     protected override void Configure(IFilterInputTypeDescriptor descriptor)
//     {
//         base.Configure(descriptor);
//         descriptor
//             .Operation(CustomFilterOperations.InClosedInterval)
//             .Type(typeof(ClosedIntervalInput<T>))
//             .MakeNullable();
//     }
// }

// See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#filter-conventions
public partial class CustomFilterConvention : FilterConvention
{
    protected override void Configure(IFilterConventionDescriptor descriptor)
    {
        descriptor.AddDefaults();
        // Use argument name `where`
        descriptor.ArgumentName("where");
        // Allow conjunction and disjunction
        descriptor.AllowAnd();
        descriptor.AllowOr();
        // descriptor.BindRuntimeType<JsonElement, JsonElementFilterType>();
        // descriptor.Operation(CustomFilterOperations.InClosedInterval).Name("inClosedInterval");
        // descriptor.AddProviderExtension(
        //     new QueryableFilterProviderExtension(filterProviderDescriptor =>
        //         filterProviderDescriptor
        //             .AddFieldHandler<QueryableComparableInClosedIntervalHandler>()
        //     )
        // );
    }
}

public static class FilterConventionDescriptorExtensions
{
    // Inspired by FilterConventionDescriptorExtensions#AddDefaults
    // https://github.com/ChilliCream/hotchocolate/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L16
    public static IFilterConventionDescriptor AddDefaults(
        this IFilterConventionDescriptor descriptor)
    {
        return descriptor
            .AddDefaultOperations()
            .BindDefaultTypes()
            .UseQueryableProvider();
    }

    // Inspired by FilterConventionDescriptorExtensions#AddDefaultOperations
    // https://github.com/ChilliCream/hotchocolate/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L28
    public static IFilterConventionDescriptor AddDefaultOperations(
        this IFilterConventionDescriptor descriptor)
    {
        descriptor.Operation(DefaultFilterOperations.Equals).Name("equalTo");
        descriptor.Operation(DefaultFilterOperations.NotEquals).Name("notEqualTo");
        descriptor.Operation(DefaultFilterOperations.Contains).Name("contains");
        descriptor.Operation(DefaultFilterOperations.NotContains).Name("doesNotContain");
        descriptor.Operation(DefaultFilterOperations.In).Name("in");
        descriptor.Operation(DefaultFilterOperations.NotIn).Name("notIn");
        descriptor.Operation(DefaultFilterOperations.StartsWith).Name("startsWith");
        descriptor.Operation(DefaultFilterOperations.NotStartsWith).Name("doesNotStartWith");
        descriptor.Operation(DefaultFilterOperations.EndsWith).Name("endsWith");
        descriptor.Operation(DefaultFilterOperations.NotEndsWith).Name("doesNotEndWith");
        descriptor.Operation(DefaultFilterOperations.And).Name("and");
        descriptor.Operation(DefaultFilterOperations.Or).Name("or");
        descriptor.Operation(DefaultFilterOperations.GreaterThan).Name("greaterThan");
        descriptor.Operation(DefaultFilterOperations.NotGreaterThan).Name("notGreaterThan");
        descriptor.Operation(DefaultFilterOperations.GreaterThanOrEquals).Name("greaterThanOrEqualTo");
        descriptor.Operation(DefaultFilterOperations.NotGreaterThanOrEquals).Name("notGreaterThanOrEqualTo");
        descriptor.Operation(DefaultFilterOperations.LowerThan).Name("lessThan");
        descriptor.Operation(DefaultFilterOperations.NotLowerThan).Name("notLessThan");
        descriptor.Operation(DefaultFilterOperations.LowerThanOrEquals).Name("lessThanOrEqualTo");
        descriptor.Operation(DefaultFilterOperations.NotLowerThanOrEquals).Name("notLessThanOrEqualTo");
        descriptor.Operation(DefaultFilterOperations.Some).Name("some");
        descriptor.Operation(DefaultFilterOperations.All).Name("all");
        descriptor.Operation(DefaultFilterOperations.None).Name("none");
        descriptor.Operation(DefaultFilterOperations.Any).Name("any");
        descriptor.Operation(DefaultFilterOperations.Like).Name("like");
        descriptor.Operation(DefaultFilterOperations.Data).Name("data");
        // TODO `descriptor.Operation(AdditionalFilterOperations.Not).Name("not");` as in the project `database`
        // `inClosedInterval`
        return descriptor;
    }

    // Inspired by FilterConventionDescriptorExtensions#BindDefaultTypes
    // https://github.com/ChilliCream/hotchocolate/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L73
    public static IFilterConventionDescriptor BindDefaultTypes(
        this IFilterConventionDescriptor descriptor)
    {
        descriptor
            .BindRuntimeType<string, StringOperationFilterInputType>()
            .BindRuntimeType<bool, BooleanOperationFilterInputType>()
            .BindRuntimeType<bool?, BooleanOperationFilterInputType>()
            .BindComparableType<byte>("BytePropositionInput")
            .BindComparableType<short>("ShortPropositionInput")
            .BindComparableType<int>("IntPropositionInput")
            .BindComparableType<long>("LongPropositionInput")
            .BindComparableType<float>("FloatXPropositionInput")
            .BindComparableType<double>("FloatPropositionInput")
            .BindComparableType<decimal>("DecimalPropositionInput")
            .BindComparableType<sbyte>("SignedBytePropositionInput")
            .BindComparableType<ushort>("UnsignedShortPropositionInput")
            .BindComparableType<uint>("UnsignedIntPropositionInput")
            .BindComparableType<ulong>("UnsigendLongPropositionInput")
            .BindComparableType<Guid>("UuidPropositionInput")
            .BindComparableType<DateTime>("DateTimePropositionInput")
            .BindComparableType<DateTimeOffset>("DateTimeOffsetPropositionInput")
            .BindComparableType<TimeSpan>("TimeSpanPropositionInput");
        // TODO Why does this not work?
        // descriptor
        //     .Configure<StringOperationFilterInputType>(x => x.Name("StringPropositionInput"))
        //     .Configure<BooleanOperationFilterInputType>(x => x.Name("BooleanPropositionInput"));
        return descriptor;
    }

    // Inspired by FilterConventionDescriptorExtensions#FilterConventionDescriptorExtensions
    // https://github.com/ChilliCream/hotchocolate/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L102
    private static IFilterConventionDescriptor BindComparableType<T>(
        this IFilterConventionDescriptor descriptor,
        string? name = null)
        where T : struct
    {
        descriptor
            .BindRuntimeType<T, ComparableOperationFilterInputType<T>>()
            .BindRuntimeType<T?, ComparableOperationFilterInputType<T?>>();
        // .BindRuntimeType<T, ExtendedComparableOperationFilterInputType<T>>()
        // .BindRuntimeType<T?, ExtendedComparableOperationFilterInputType<T?>>();
        // TODO Why does this not work?
        // if (name is not null)
        // {
        //     descriptor
        //         .Configure<ComparableOperationFilterInputType<T>>(x => x.Name(name))
        //         .Configure<ComparableOperationFilterInputType<T?>>(x => x.Name($"Maybe{name}"));
        // }
        return descriptor;
    }
}

// See https://chillicream.com/docs/hotchocolate/fetching-data/sorting/#sorting-conventions
public partial class CustomSortConvention : SortConvention
{
    protected override void Configure(ISortConventionDescriptor descriptor)
    {
        descriptor.AddDefaults();
    }
}