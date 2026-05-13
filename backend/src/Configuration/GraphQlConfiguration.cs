using System;
using HotChocolate.AspNetCore;
using HotChocolate.Configuration;
using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;
using HotChocolate.Data.Sorting;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Configurations;
using HotChocolate.Types.NodaTime;
using Metabase.Authentication;
using Metabase.Data;
using Metabase.GraphQl;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.Filters;
using Metabase.GraphQl.Scalars;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            .AddMemoryCache();
        // GraphQL Server
        var serverBuilder = services
            .AddGraphQLServer();
        if (environment.IsDevelopment())
        {
            // to debug exceptions like `System.InvalidOperationException: The name becomes immutable once it was assigned.`
            serverBuilder.TryAddTypeInterceptor<LoggingTypeInterceptor>();
        }
        serverBuilder
            .AddSha256DocumentHashProvider(HashFormat.Hex) // https://chillicream.com/docs/hotchocolate/v15/security/#fips-compliance
            .AddApplicationService<IHttpContextAccessor>() // for `AddHttpRequestInterceptor`
            .AddApplicationService<ILogger<ErrorLoggingDiagnosticEventListener>>() // for `AddDiagnosticEventListener`
            .DisableIntrospection(false) // if the introspection result becomes too big we need to disable it in production
            .RegisterDbContextFactory<ApplicationDbContext>()
            // .AddInstrumentation()
            .AddMutationConventions(new MutationConventionOptions { ApplyToAllMutations = false })
            // Extensions
            .AddNodaTime()
            // .AddTypeConverter<OffsetDateTime, DateTimeOffset>(
            //     _ => _.ToDateTimeOffset()
            // )
            // .AddTypeConverter<DateTimeOffset, OffsetDateTime>(
            //     _ => OffsetDateTime.FromDateTimeOffset(_)
            // )
            // .AddProjections()
            .AddFiltering<CustomFilterConvention>()
            .AddSorting<CustomSortConvention>()
            .AddConvention<INamingConventions, CustomNamingConventions>()
            .AddQueryContext()
            .AddAuthorization()
            .AddQueryFieldToMutationPayloads()
            .AddGlobalObjectIdentification(_ =>
                {
                    // _.MaxAllowedNodeBatchSize = 100;
                    _.EnsureAllNodesCanBeResolved = true;
                }
            )
            .ModifyOptions(_ =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Types/Configuration/Contracts/ISchemaOptions.cs
                    _.StrictValidation = true;
                    _.UseXmlDocumentation = false;
                    _.SortFieldsByName = true;
                    _.RemoveUnreachableTypes = false;
                    _.RemoveUnusedTypeSystemDirectives = true;
                    _.DefaultBindingBehavior = BindingBehavior.Implicit;
                    // options.DefaultFieldBindingFlags = FieldBindingFlags.InstanceAndStatic;
                    _.EnableDirectiveIntrospection = true;
                    _.EnableOptInFeatures = true;
                    _.DefaultDirectiveVisibility = DirectiveVisibility.Public;
                    _.DefaultResolverStrategy = ExecutionStrategy.Parallel;
                    _.ValidatePipelineOrder = true;
                    _.StrictRuntimeTypeValidation = true;
                    _.EnableFlagEnums = false;
                    _.EnableDefer = false;
                    _.EnableStream = false;
                    _.StripLeadingIFromInterface = false;
                    _.EnableTag = true;
                    _.PublishRootFieldPagesToPromiseCache = true;
                    // options.OperationDocumentCacheSize = 200;
                    // options.PreparedOperationCacheSize = 100;
                }
            )
            .ModifyServerOptions(_ =>
                {
                    _.AllowedGetOperations = AllowedGetOperations.Query;
                    _.Batching = AllowedBatching.All;
                    _.EnableGetRequests = false;
                    _.EnableMultipartRequests = true;
                    _.EnableSchemaRequests = true;
                    // Nitro
                    _.Tool.DisableTelemetry = true;
                    _.Tool.Enable = true; // environment.IsDevelopment()
                    _.Tool.GraphQLEndpoint = GraphQlConstants.EndpointPath;
                    _.Tool.IncludeCookies = false;
                    _.Tool.Title = "GraphQL";
                    _.Tool.UseBrowserUrlAsGraphQLEndpoint = false;
                    _.Tool.UseGet = false;
                }
            )
            .ModifyRequestOptions(_ =>
                {
                    // https://github.com/ChilliCream/hotchocolate/blob/main/src/HotChocolate/Core/src/Execution/Options/RequestExecutorOptions.cs
                    _.ExecutionTimeout = TimeSpan.FromSeconds(120);
                    _.IncludeExceptionDetails = !environment.IsProduction(); // Default is `Debugger.IsAttached`.
                    _.AllowErrorHandlingModeOverride = true;
                    // options.QueryCacheSize = ...;
                    // options.UseComplexityMultipliers = ...;
                    // options.EnableSchemaFileSupport = true;
                }
            )
            .ModifyCostOptions(_ =>
                {
                    _.MaxFieldCost = 10000;
                    _.MaxTypeCost = 10000;
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
            // TODO Add `MyUuidType` based on https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Core/src/Types/Types/Scalars/UuidType.cs
            .AddType(new UuidType("Uuid", defaultFormat: 'D')) // https://chillicream.com/docs/hotchocolate/defining-a-schema/scalars#uuid-type
            .AddType(new MyUriType())
            .AddType(new AnyType("Any"))
            .AddType<LocaleType>()
            .BindRuntimeType<uint, NonNegativeIntType>()
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
            // .AddCursorKeySerializer(new OffsetDateTimeCursorKeySerializer())
            .ModifyPagingOptions(_ =>
                {
                    _.MaxPageSize = (int)GraphQlConstants.MaximumPageSize;
                    _.DefaultPageSize = (int)GraphQlConstants.MaximumPageSize;
                    _.IncludeTotalCount = true;
                    _.IncludeNodesField = false;
                    _.InferConnectionNameFromField = true;
                }
            )
            // Automatic Peristed Queries
            .UseAutomaticPersistedOperationPipeline()
            .AddInMemoryOperationDocumentStorage(); // Needed by the automatic persisted operation pipeline
    }

    // 
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

}

// https://github.com/ChilliCream/graphql-platform/blob/main/src/HotChocolate/Core/src/Types/Configuration/TypeInterceptor.cs
public sealed class LoggingTypeInterceptor
: TypeInterceptor
{
    public override void OnBeforeInitialize(ITypeDiscoveryContext discoveryContext)
    {
        Console.WriteLine($"[INIT] Discovered type '{discoveryContext.Type.GetType().Name}'");
    }

    public override void OnBeforeCompleteName(ITypeCompletionContext completionContext, TypeSystemConfiguration configuration)
    {
        Console.WriteLine($"[NAME] Finalizing name '{configuration.Name}' for type '{completionContext.Type.GetType().Name}'");
    }

    public override void OnAfterCompleteType(ITypeCompletionContext completionContext, TypeSystemConfiguration configuration)
    {
        Console.WriteLine($"[DONE] Completed type '{completionContext.Type.GetType().Name}' with name '{configuration.Name}'");
    }
}

public sealed class CustomNamingConventions
: DefaultNamingConventions
{
}

// See https://chillicream.com/docs/hotchocolate/fetching-data/filtering/#filter-conventions
public partial class CustomFilterConvention : FilterConvention
{

    private const string InputPostFix = "FilterInput";
    private const string InputTypePostFix = "FilterInputType";

    private readonly CustomNamingConventions _namingConventions = new();

    protected override void Configure(IFilterConventionDescriptor descriptor)
    {
        descriptor.AddDefaults();
        // Use argument name `where`
        descriptor.ArgumentName("where");
        // Allow conjunction and disjunction
        descriptor.AllowAnd();
        descriptor.AllowOr();
        // TODO negation "AllowNot" and "UseNot". See `NotField` and `EntityFilterType.OnCompleteFields`
        // Add in-closed-interval operation
        descriptor
            .Operation(AdditionalFilterOperations.InClosedInterval)
            .Name("inClosedInterval");
        descriptor.Configure<FloatFilterInputType>(_ => _
            .Operation(AdditionalFilterOperations.InClosedInterval)
            .Type<InputObjectType<ClosedIntervalInput<double>>>()
        );
        descriptor.Provider(
            new QueryableFilterProvider(_ => _
                .AddDefaultFieldHandlers()
                .AddFieldHandler<QueryableComparableInClosedIntervalHandler<double>>(context =>
                    new QueryableComparableInClosedIntervalHandler<double>(context.TypeConverter, context.InputParser)
                )
            )
        );
    }

    // For the base implementation see https://github.com/ChilliCream/hotchocolate/blob/f0dff93a14cb7ddecc7b3a0530a687a5bc4bad71/src/HotChocolate/Data/src/Data/Filters/Convention/FilterConvention.cs#L129
    public override string GetTypeName(Type runtimeType)
    {
        // return base.GetTypeName(runtimeType);
        return GetTypeName(runtimeType, plural: false);
    }

    public string GetTypeName(Type runtimeType, bool plural)
    {
        ArgumentNullException.ThrowIfNull(runtimeType);
        var pluralSuffix = plural ? "s" : "";
        if (typeof(IEnumOperationFilterInputType).IsAssignableFrom(runtimeType)
            && runtimeType.GenericTypeArguments.Length == 1
            && runtimeType.GetGenericTypeDefinition() == typeof(EnumOperationFilterInputType<>))
        {
            var genericName = _namingConventions.GetTypeName(runtimeType.GenericTypeArguments[0]);
            return $"{genericName}{pluralSuffix}{GraphQlConstants.FilterInputSuffix}"; ;
            // return genericName + "OperationFilterInput";
        }
        if (typeof(IComparableOperationFilterInputType).IsAssignableFrom(runtimeType)
            && runtimeType.GenericTypeArguments.Length == 1
            && runtimeType.GetGenericTypeDefinition()
            == typeof(ComparableOperationFilterInputType<>))
        {
            var genericName = _namingConventions.GetTypeName(runtimeType.GenericTypeArguments[0]);
            return $"Comparable{genericName}{pluralSuffix}{GraphQlConstants.FilterInputSuffix}";
            // return $"Comparable{genericName}OperationFilterInput";
        }
        if (typeof(IListFilterInputType).IsAssignableFrom(runtimeType)
            && runtimeType.GenericTypeArguments.Length == 1)
        {
            var genericType = runtimeType.GenericTypeArguments[0];
            var genericName = typeof(FilterInputType).IsAssignableFrom(genericType)
                ? GetTypeName(genericType, plural: true)
                : "List" + _namingConventions.GetTypeName(genericType);
            return $"{genericName}";
            // return "List" + genericName;
        }
        var name = _namingConventions.GetTypeName(runtimeType);
        var isInputObjectType = typeof(FilterInputType).IsAssignableFrom(runtimeType);
        var isEndingInput = name.EndsWith(InputPostFix, StringComparison.Ordinal);
        var isEndingInputType = name.EndsWith(InputTypePostFix, StringComparison.Ordinal);
        if (isInputObjectType && isEndingInputType)
        {
            return $"{name[..^"FilterInputType".Length]}{pluralSuffix}{GraphQlConstants.FilterInputSuffix}";
        }
        if (isInputObjectType && !isEndingInput && !isEndingInputType)
        {
            return $"{name}{pluralSuffix}{GraphQlConstants.FilterInputSuffix}";
        }
        if (!isInputObjectType && !isEndingInput)
        {
            return $"{name}{pluralSuffix}{GraphQlConstants.FilterInputSuffix}";
        }
        return name;
    }
}

public static class CustomFilterConventionExtensions
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
    // https://github.com/ChilliCream/graphql-platform/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L28
    public static IFilterConventionDescriptor AddDefaultOperations(
        this IFilterConventionDescriptor descriptor
    )
    {
        // Use speaking names for operations
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
        return descriptor;
    }

    // Inspired by FilterConventionDescriptorExtensions#BindDefaultTypes
    // https://github.com/ChilliCream/hotchocolate/blob/ee5813646fdfea81035c681989793514f33b5d94/src/HotChocolate/Data/src/Data/Filters/Convention/Extensions/FilterConventionDescriptorExtensions.cs#L73
    public static IFilterConventionDescriptor BindDefaultTypes(
        this IFilterConventionDescriptor descriptor
    )
    {
        return descriptor
            .BindRuntimeType<string, StringFilterInputType>()
            .BindRuntimeType<bool, BooleanFilterInputType>()
            .BindRuntimeType<bool?, BooleanFilterInputType>()
            .BindRuntimeType<byte, ByteFilterInputType>()
            .BindRuntimeType<byte?, ByteFilterInputType>()
            .BindRuntimeType<sbyte, ByteFilterInputType>()
            .BindRuntimeType<sbyte?, ByteFilterInputType>()
            .BindRuntimeType<short, ShortFilterInputType>()
            .BindRuntimeType<short?, ShortFilterInputType>()
            .BindRuntimeType<int, IntFilterInputType>()
            .BindRuntimeType<int?, IntFilterInputType>()
            .BindRuntimeType<long, LongFilterInputType>()
            .BindRuntimeType<long?, LongFilterInputType>()
            .BindRuntimeType<float, FloatFilterInputType>()
            .BindRuntimeType<float?, FloatFilterInputType>()
            .BindRuntimeType<double, FloatFilterInputType>()
            .BindRuntimeType<double?, FloatFilterInputType>()
            .BindRuntimeType<decimal, DecimalFilterInputType>()
            .BindRuntimeType<decimal?, DecimalFilterInputType>()
            .BindRuntimeType<Guid, UuidFilterInputType>()
            .BindRuntimeType<Guid?, UuidFilterInputType>()
            .BindRuntimeType<DateTime, DateTimeFilterInputType>()
            .BindRuntimeType<DateTime?, DateTimeFilterInputType>()
            .BindRuntimeType<OffsetDateTime, DateTimeFilterInputType>()
            .BindRuntimeType<OffsetDateTime?, DateTimeFilterInputType>()
            .BindRuntimeType<DateTimeOffset, DateTimeFilterInputType>()
            .BindRuntimeType<DateTimeOffset?, DateTimeFilterInputType>()
            .BindRuntimeType<Instant, DateTimeFilterInputType>()
            .BindRuntimeType<Instant?, DateTimeFilterInputType>()
            .BindRuntimeType<LocalDateTime, LocalDateTimeFilterInputType>()
            .BindRuntimeType<LocalDateTime?, LocalDateTimeFilterInputType>()
            .BindRuntimeType<LocalDate, LocalDateFilterInputType>()
            .BindRuntimeType<LocalDate?, LocalDateFilterInputType>()
            .BindRuntimeType<LocalTime, LocalTimeFilterInputType>()
            .BindRuntimeType<LocalTime?, LocalTimeFilterInputType>()
            .BindRuntimeType<TimeSpan, DurationFilterInputType>()
            .BindRuntimeType<TimeSpan?, DurationFilterInputType>()
            .BindRuntimeType<Duration, DurationFilterInputType>()
            .BindRuntimeType<Duration?, DurationFilterInputType>()
            .BindRuntimeType<Uri, UriFilterInputType>()
            .BindRuntimeType<Uri?, UriFilterInputType>();
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