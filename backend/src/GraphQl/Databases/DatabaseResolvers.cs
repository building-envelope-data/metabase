using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Resolvers;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Metabase.GraphQl.Databases;

public static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Warning,
        Message = "Failed with errors {Errors} to query the database {Locator} for {Request}.")]
    public static partial void FailedWithErrors(
        this ILogger logger,
        string Errors,
        Uri Locator,
        string Request
    );

    [LoggerMessage(
        EventId = 1,
        Level = LogLevel.Error,
        Message = "Failed with status code {StatusCode} to request {Locator} for {Request}.")]
    public static partial void FailedWithStatusCode(
        this ILogger logger,
        Exception exception,
        HttpStatusCode? StatusCode,
        Uri Locator,
        string Request
    );

    [LoggerMessage(
        EventId = 2,
        Level = LogLevel.Error,
        Message =
            "Failed to deserialize GraphQL response of request to {Locator} for {Request}. The details given are: Zero-based number of bytes read within the current line before the exception are {BytePositionInLine}, zero-based number of lines read before the exception are {LineNumber}, message that describes the current exception is '{Message}', path within the JSON where the exception was encountered is {Path}.")]
    public static partial void FailedToDeserialize(
        this ILogger logger,
        Exception exception,
        Uri Locator,
        string Request,
        long? BytePositionInLine,
        long? LineNumber,
        string Message,
        string? Path
    );

    [LoggerMessage(
        EventId = 3,
        Level = LogLevel.Error,
        Message = "Failed to request {Locator} for {Request} or failed to deserialize the response.")]
    public static partial void FailedToRequestOrDeserialize(
        this ILogger logger,
        Exception exception,
        Uri Locator,
        string Request
    );
}

public sealed class DatabaseResolvers(
    AppSettings appSettings,
    IHttpClientFactory httpClientFactory,
    ILogger<DatabaseResolvers> logger
    )
{
    private const string IgsdbUrl = "https://igsdb-v2.herokuapp.com/graphql/";
    private const string IgsdbStagingUrl = "https://igsdb-v2-staging.herokuapp.com/graphql/";

    private static readonly string[] s_opticalDataFileNames =
    [
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "OpticalData.graphql"
    ];

    private static readonly string[] s_hygrothermalDataFileNames =
    [
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "HygrothermalData.graphql"
    ];

    private static readonly string[] s_calorimetricDataFileNames =
    [
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "CalorimetricData.graphql"
    ];

    private static readonly string[] s_photovoltaicDataFileNames =
    [
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PhotovoltaicData.graphql"
    ];

    private static readonly string[] s_geometricDataFileNames =
    [
        "DataFields.graphql",
        "GeometricDataFields.graphql",
        "GeometricData.graphql"
    ];

    private static readonly string[] s_allOpticalDataFileNames =
    [
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllOpticalData.graphql"
    ];

    private static readonly string[] s_allHygrothermalDataFileNames =
    [
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllHygrothermalData.graphql"
    ];

    private static readonly string[] s_allCalorimetricDataFileNames =
    [
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "PageInfoFields.graphql",
        "AllCalorimetricData.graphql"
    ];

    private static readonly string[] s_allPhotovoltaicDataFileNames =
    [
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PageInfoFields.graphql",
        "AllPhotovoltaicData.graphql"
    ];

    private static readonly string[] s_allGeometricDataFileNames =
    [
        "DataFields.graphql",
        "GeometricDataFields.graphql",
        "PageInfoFields.graphql",
        "AllGeometricData.graphql"
    ];

    private static readonly string[] s_hasDataFileNames =
    [
        "HasData.graphql"
    ];

    private static readonly string[] s_hasOpticalDataFileNames =
    [
        "HasOpticalData.graphql"
    ];

    private static readonly string[] s_hasCalorimetricDataFileNames =
    [
        "HasCalorimetricData.graphql"
    ];

    private static readonly string[] s_hasHygrothermalDataFileNames =
    [
        "HasHygrothermalData.graphql"
    ];

    private static readonly string[] s_hasPhotovoltaicDataFileNames =
    [
        "HasPhotovoltaicData.graphql"
    ];

    private static readonly string[] s_hasGeometricDataFileNames =
    [
        "HasGeometricData.graphql"
    ];

    private readonly AppSettings _appSettings = appSettings;
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly ILogger<DatabaseResolvers> _logger = logger;

    private static bool IsIgsdbDatabase(Database database)
    {
        return new[] { IgsdbUrl, IgsdbStagingUrl }
            .Contains(database.Locator.AbsoluteUri);
    }

    public Task<bool> CanCurrentUserUpdateNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        DatabaseAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToUpdate(claimsPrincipal, database.Id, cancellationToken);
    }

    public Task<bool> CanCurrentUserVerifyNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        DatabaseAuthorization authorization,
        CancellationToken cancellationToken
    )
    {
        return authorization.IsAuthorizedToVerify(claimsPrincipal, database.Id, cancellationToken);
    }

    public async Task<OpticalData?> GetOpticalDataAsync(
        [Parent] Database database,
        Guid id,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<OpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_opticalDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(OpticalData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.OpticalData;
    }

    public async Task<HygrothermalData?> GetHygrothermalDataAsync(
        [Parent] Database database,
        Guid id,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hygrothermalDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(HygrothermalData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HygrothermalData;
    }

    public async Task<CalorimetricData?> GetCalorimetricDataAsync(
        [Parent] Database database,
        Guid id,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<CalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_calorimetricDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(CalorimetricData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.CalorimetricData;
    }

    public async Task<PhotovoltaicData?> GetPhotovoltaicDataAsync(
        [Parent] Database database,
        Guid id,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<PhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_photovoltaicDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(PhotovoltaicData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.PhotovoltaicData;
    }

    public async Task<GeometricData?> GetGeometricDataAsync(
        [Parent] Database database,
        Guid id,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<GeometricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_geometricDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(GeometricData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.GeometricData;
    }

    public async Task<OpticalDataConnection?> GetAllOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        string? locale,
        uint? first,
        string? after,
        uint? last,
        string? before,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllOpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allOpticalDataFileNames),
                        new
                        {
                            where,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllOpticalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.AllOpticalData;
    }

    public async Task<HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        string? locale,
        uint? first,
        string? after,
        uint? last,
        string? before,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllHygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allHygrothermalDataFileNames
                        ),
                        new
                        {
                            where,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllHygrothermalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.AllHygrothermalData;
    }

    public async Task<CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        string? locale,
        uint? first,
        string? after,
        uint? last,
        string? before,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllCalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allCalorimetricDataFileNames
                        ),
                        new
                        {
                            where,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllCalorimetricData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.AllCalorimetricData;
    }

    public async Task<PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        string? locale,
        uint? first,
        string? after,
        uint? last,
        string? before,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllPhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allPhotovoltaicDataFileNames
                        ),
                        new
                        {
                            where,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllPhotovoltaicData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.AllPhotovoltaicData;
    }

    public async Task<GeometricDataConnection?> GetAllGeometricDataAsync(
        [Parent] Database database,
        GeometricDataPropositionInput? where,
        string? locale,
        uint? first,
        string? after,
        uint? last,
        string? before,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllGeometricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allGeometricDataFileNames),
                        new
                        {
                            where,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllGeometricData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.AllGeometricData;
    }

    public async Task<bool?> HasOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasOpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasOpticalDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasOpticalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HasOpticalData;
    }

    public async Task<bool?> HasCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasCalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasCalorimetricDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasCalorimetricData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HasCalorimetricData;
    }

    public async Task<bool?> HasHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasHygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasHygrothermalDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasHygrothermalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HasHygrothermalData;
    }

    public async Task<bool?> HasPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        string? locale,
        IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasPhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasPhotovoltaicDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasPhotovoltaicData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HasPhotovoltaicData;
    }

    public async Task<bool?> HasGeometricDataAsync(
        [Parent] Database database,
        GeometricDataPropositionInput? where,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasGeometricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasGeometricDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasGeometricData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                )
            )?.HasGeometricData;
    }

    private async
        Task<TGraphQlResponse?>
        QueryDatabase<TGraphQlResponse>(
            Database database,
            GraphQLRequest request,
            IHttpContextAccessor httpContextAccessor,
            IResolverContext resolverContext,
            CancellationToken cancellationToken
        )
        where TGraphQlResponse : class
    {
        try
        {
            var deserializedGraphQlResponse =
                await QueryingDatabases.QueryDatabase<TGraphQlResponse>(
                    database,
                    request,
                    _httpClientFactory,
                    httpContextAccessor,
                    cancellationToken,
                    IsIgsdbDatabase(database) ? _appSettings.IgsdbApiToken : null
                );
            if (deserializedGraphQlResponse.Errors?.Length >= 1)
            {
                _logger.FailedWithErrors(JsonSerializer.Serialize(deserializedGraphQlResponse.Errors),
                    database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
                foreach (var error in deserializedGraphQlResponse.Errors)
                {
                    var errorBuilder = ErrorBuilder.New()
                        .SetCode("DATABASE_QUERY_ERROR")
                        // .SetPath(error.Path) // TODO Add the error path. Just using `error.Path` does not work as it contains non-"GraphQlName"s according to HotChocolate sometimes.
                        .SetMessage(
                            $"The GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)} reported the error {error.Message}.");
                    if (error.Extensions is not null)
                    {
                        foreach (var (key, value) in error.Extensions)
                        {
                            errorBuilder.SetExtension(key, value);
                        }
                    }

                    // TODO Add `error.Locations` to `errorBuilder`.
                    resolverContext.ReportError(errorBuilder.Build());
                }
            }

            return deserializedGraphQlResponse.Data;
        }
        catch (HttpRequestException e)
        {
            _logger.FailedWithStatusCode(e, e.StatusCode, database.Locator,
                JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage(
                        $"Failed with status code {e.StatusCode} to request {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)}.")
                    .SetException(e)
                    .Build()
            );
            return null;
        }
        catch (JsonException e)
        {
            _logger.FailedToDeserialize(e, database.Locator,
                JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions), e.BytePositionInLine,
                e.LineNumber, e.Message, e.Path);
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("DESERIALIZATION_FAILED")
                    .SetPath(resolverContext.Path) // TODO Add the error path. I would do it as follows as a workaround, however splitting the path at '.' is wrong in general: .SetPath(resolverContext.Path.ToList().Concat(e.Path?.Split('.') ?? []).ToList())
                    .SetMessage(
                        $"Failed to deserialize GraphQL response of request to {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)}. The details given are: Zero-based number of bytes read within the current line before the exception are {e.BytePositionInLine}, zero-based number of lines read before the exception are {e.LineNumber}, message that describes the current exception is '{e.Message}', path within the JSON where the exception was encountered is {e.Path}.")
                    .SetException(e)
                    .Build()
            );
            return null;
        }
        catch (Exception e)
        {
            _logger.FailedToRequestOrDeserialize(e, database.Locator,
                JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
            resolverContext.ReportError(
                ErrorBuilder.New()
                    .SetCode("DATABASE_REQUEST_FAILED")
                    .SetPath(resolverContext.Path)
                    .SetMessage(
                        $"Failed to request {database.Locator} for {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)} or failed to deserialize the response.")
                    .SetException(e)
                    .Build()
            );
            return null;
        }
    }

    private sealed record OpticalDataData(OpticalData OpticalData);
    private sealed record HygrothermalDataData(HygrothermalData HygrothermalData);
    private sealed record CalorimetricDataData(CalorimetricData CalorimetricData);
    private sealed record PhotovoltaicDataData(PhotovoltaicData PhotovoltaicData);
    private sealed record GeometricDataData(GeometricData GeometricData);
    private sealed record AllOpticalDataData(OpticalDataConnection AllOpticalData);
    private sealed record AllHygrothermalDataData(HygrothermalDataConnection AllHygrothermalData);
    private sealed record AllCalorimetricDataData(CalorimetricDataConnection AllCalorimetricData);
    private sealed record AllGeometricDataData(GeometricDataConnection AllGeometricData);
    private sealed record AllPhotovoltaicDataData(PhotovoltaicDataConnection AllPhotovoltaicData);
    private sealed record HasOpticalDataData(bool HasOpticalData);
    private sealed record HasCalorimetricDataData(bool HasCalorimetricData);
    private sealed record HasGeometricDataData(bool HasGeometricData);
    private sealed record HasHygrothermalDataData(bool HasHygrothermalData);
    private sealed record HasPhotovoltaicDataData(bool HasPhotovoltaicData);
}