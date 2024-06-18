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
using Microsoft.AspNetCore.Identity;
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

public sealed class DatabaseResolvers
{
    private const string IgsdbUrl = "https://igsdb-v2.herokuapp.com/graphql/";

    private static readonly string[] _dataFileNames =
    {
        "DataFields.graphql",
        "Data.graphql"
    };

    private static readonly string[] _opticalDataFileNames =
    {
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "OpticalData.graphql"
    };

    private static readonly string[] _hygrothermalDataFileNames =
    {
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "HygrothermalData.graphql"
    };

    private static readonly string[] _calorimetricDataFileNames =
    {
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "CalorimetricData.graphql"
    };

    private static readonly string[] _photovoltaicDataFileNames =
    {
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PhotovoltaicData.graphql"
    };

    private static readonly string[] _igsdbAllDataFileNames =
    {
        "DataFields.graphql",
        "AllDataX.graphql"
    };

    private static readonly string[] _allDataFileNames =
    {
        "DataFields.graphql",
        "PageInfoFields.graphql",
        "AllData.graphql"
    };

    private static readonly string[] _igsdbAllOpticalDataFileNames =
    {
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "AllOpticalDataX.graphql"
    };

    private static readonly string[] _allOpticalDataFileNames =
    {
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllOpticalData.graphql"
    };

    private static readonly string[] _allHygrothermalDataFileNames =
    {
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllHygrothermalData.graphql"
    };

    private static readonly string[] _allCalorimetricDataFileNames =
    {
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "PageInfoFields.graphql",
        "AllCalorimetricData.graphql"
    };

    private static readonly string[] _allPhotovoltaicDataFileNames =
    {
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PageInfoFields.graphql",
        "AllPhotovoltaicData.graphql"
    };

    private static readonly string[] _hasDataFileNames =
    {
        "HasData.graphql"
    };

    private static readonly string[] _hasOpticalDataFileNames =
    {
        "HasOpticalData.graphql"
    };

    private static readonly string[] _hasCalorimetricDataFileNames =
    {
        "HasCalorimetricData.graphql"
    };

    private static readonly string[] _hasHygrothermalDataFileNames =
    {
        "HasHygrothermalData.graphql"
    };

    private static readonly string[] _hasPhotovoltaicDataFileNames =
    {
        "HasPhotovoltaicData.graphql"
    };

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DatabaseResolvers> _logger;

    public DatabaseResolvers(
        IHttpClientFactory httpClientFactory,
        ILogger<DatabaseResolvers> logger
    )
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public Task<bool> GetCanCurrentUserUpdateNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return DatabaseAuthorization.IsAuthorizedToUpdate(claimsPrincipal, database.Id, userManager, context,
            cancellationToken);
    }

    public Task<bool> GetCanCurrentUserVerifyNodeAsync(
        [Parent] Database database,
        ClaimsPrincipal claimsPrincipal,
        [Service(ServiceKind.Resolver)] UserManager<User> userManager,
        ApplicationDbContext context,
        CancellationToken cancellationToken
    )
    {
        return DatabaseAuthorization.IsAuthorizedToVerify(claimsPrincipal, database.Id, userManager, context,
            cancellationToken);
    }

    public async Task<IData?> GetDataAsync(
        [Parent] Database database,
        Guid id,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<DataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _dataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            id,
                            timestamp,
                            locale
                        },
                        nameof(Data)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.Data;
    }

    public async Task<OpticalData?> GetOpticalDataAsync(
        [Parent] Database database,
        Guid id,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<OpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _opticalDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            id,
                            timestamp,
                            locale
                        },
                        nameof(OpticalData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.OpticalData;
    }

    public async Task<HygrothermalData?> GetHygrothermalDataAsync(
        [Parent] Database database,
        Guid id,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hygrothermalDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            id,
                            timestamp,
                            locale
                        },
                        nameof(HygrothermalData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HygrothermalData;
    }

    public async Task<CalorimetricData?> GetCalorimetricDataAsync(
        [Parent] Database database,
        Guid id,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<CalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _calorimetricDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            id,
                            timestamp,
                            locale
                        },
                        nameof(CalorimetricData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.CalorimetricData;
    }

    public async Task<PhotovoltaicData?> GetPhotovoltaicDataAsync(
        [Parent] Database database,
        Guid id,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<PhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _photovoltaicDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            id,
                            timestamp,
                            locale
                        },
                        nameof(PhotovoltaicData)
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.PhotovoltaicData;
    }

    public async Task<DataConnection?> GetAllDataAsync(
        [Parent] Database database,
        DataPropositionInput? where,
        DateTime? timestamp,
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
        var dataConnection = (await QueryDatabase<AllDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            database.Locator.AbsoluteUri == IgsdbUrl
                                ? _igsdbAllDataFileNames
                                : _allDataFileNames).ConfigureAwait(false),
                        new
                        {
                            where = RewriteDataPropositionInput(where, database),
                            timestamp,
                            locale,
                            first,
                            after,
                            last,
                            before
                        },
                        "AllData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.AllData;
        return dataConnection is null
            ? null
            : new DataConnection(
                dataConnection.Edges,
                dataConnection.Nodes,
                dataConnection.TotalCount,
                dataConnection.Timestamp
            );
    }

    private static DataPropositionInput? RewriteDataPropositionInput(
        DataPropositionInput? where,
        Database database
    )
    {
        return database.Locator.AbsoluteUri == IgsdbUrl
            ? where ?? new DataPropositionInput(null, null, null, null, null, null, null, null, null, null,
                null, null, null, null)
            : where;
    }

    public async Task<OpticalDataConnection?> GetAllOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        DateTime? timestamp,
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
        return (await QueryDatabase<AllOpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            database.Locator.AbsoluteUri == IgsdbUrl
                                ? _igsdbAllOpticalDataFileNames
                                : _allOpticalDataFileNames).ConfigureAwait(false),
                        new
                        {
                            where = RewriteOpticalDataPropositionInput(where, database),
                            timestamp,
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
                ).ConfigureAwait(false)
            )?.AllOpticalData;
    }

    private static OpticalDataPropositionInput? RewriteOpticalDataPropositionInput(
        OpticalDataPropositionInput? where,
        Database database
    )
    {
        return database.Locator.AbsoluteUri == IgsdbUrl
            ? where ?? new OpticalDataPropositionInput(null, null, null, null, null, null, null, null, null,
                null, null, null)
            : where;
    }

    public async Task<HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        DateTime? timestamp,
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
        return (await QueryDatabase<AllHygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _allHygrothermalDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
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
                ).ConfigureAwait(false)
            )?.AllHygrothermalData;
    }

    public async Task<CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        DateTime? timestamp,
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
        return (await QueryDatabase<AllCalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _allCalorimetricDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
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
                ).ConfigureAwait(false)
            )?.AllCalorimetricData;
    }

    public async Task<PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        DateTime? timestamp,
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
        return (await QueryDatabase<AllPhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _allPhotovoltaicDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
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
                ).ConfigureAwait(false)
            )?.AllPhotovoltaicData;
    }

    public async Task<bool?> GetHasDataAsync(
        [Parent] Database database,
        DataPropositionInput? where,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hasDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where = RewriteDataPropositionInput(where, database),
                            timestamp,
                            locale
                        },
                        "HasData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HasData;
    }

    public async Task<bool?> GetHasOpticalDataAsync(
        [Parent] Database database,
        OpticalDataPropositionInput? where,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasOpticalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hasOpticalDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where = RewriteOpticalDataPropositionInput(where, database),
                            timestamp,
                            locale
                        },
                        "HasOpticalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HasOpticalData;
    }

    public async Task<bool?> GetHasCalorimetricDataAsync(
        [Parent] Database database,
        CalorimetricDataPropositionInput? where,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasCalorimetricDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hasCalorimetricDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
                            locale
                        },
                        "HasCalorimetricData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HasCalorimetricData;
    }

    public async Task<bool?> GetHasHygrothermalDataAsync(
        [Parent] Database database,
        HygrothermalDataPropositionInput? where,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasHygrothermalDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hasHygrothermalDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
                            locale
                        },
                        "HasHygrothermalData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HasHygrothermalData;
    }

    public async Task<bool?> GetHasPhotovoltaicDataAsync(
        [Parent] Database database,
        PhotovoltaicDataPropositionInput? where,
        DateTime? timestamp,
        string? locale,
        [Service] IHttpContextAccessor httpContextAccessor,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasPhotovoltaicDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            _hasPhotovoltaicDataFileNames
                        ).ConfigureAwait(false),
                        new
                        {
                            where,
                            timestamp,
                            locale
                        },
                        "HasPhotovoltaicData"
                    ),
                    httpContextAccessor,
                    resolverContext,
                    cancellationToken
                ).ConfigureAwait(false)
            )?.HasPhotovoltaicData;
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
                    cancellationToken
                );
            if (deserializedGraphQlResponse.Errors?.Length >= 1)
            {
                _logger.FailedWithErrors(JsonSerializer.Serialize(deserializedGraphQlResponse.Errors),
                    database.Locator, JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions));
                foreach (var error in deserializedGraphQlResponse.Errors)
                {
                    var errorBuilder = ErrorBuilder.New()
                        .SetCode("DATABASE_QUERY_ERROR")
                        .SetMessage(
                            $"The GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, QueryingDatabases.SerializerOptions)} reported the error {error.Message}.")
                        .SetPath(error.Path);
                    if (error.Extensions is not null)
                        foreach (var (key, value) in error.Extensions)
                            errorBuilder.SetExtension(key, value);

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
                    .SetPath(resolverContext.Path.ToList().Concat(e.Path?.Split('.') ?? Array.Empty<string>())
                        .ToList()) // TODO Splitting the path at '.' is wrong in general.
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

    private sealed class DataData
    {
        public DataX.Data Data { get; } = default!;
    }

    private sealed class OpticalDataData
    {
        public OpticalData OpticalData { get; } = default!;
    }

    private sealed class HygrothermalDataData
    {
        public HygrothermalData HygrothermalData { get; } = default!;
    }

    private sealed class CalorimetricDataData
    {
        public CalorimetricData CalorimetricData { get; } = default!;
    }

    private sealed class PhotovoltaicDataData
    {
        public PhotovoltaicData PhotovoltaicData { get; } = default!;
    }

    private sealed class AllDataData
    {
        public DataConnection AllData { get; } = default!;
    }

    private sealed class AllOpticalDataData
    {
        public OpticalDataConnection AllOpticalData { get; } = default!;
    }

    private sealed class AllHygrothermalDataData
    {
        public HygrothermalDataConnection AllHygrothermalData { get; } = default!;
    }

    private sealed class AllCalorimetricDataData
    {
        public CalorimetricDataConnection AllCalorimetricData { get; } = default!;
    }

    private sealed class AllPhotovoltaicDataData
    {
        public PhotovoltaicDataConnection AllPhotovoltaicData { get; } = default!;
    }

    private sealed class HasDataData
    {
        public bool HasData { get; }
    }

    private sealed class HasOpticalDataData
    {
        public bool HasOpticalData { get; }
    }

    private sealed class HasCalorimetricDataData
    {
        public bool HasCalorimetricData { get; }
    }

    private sealed class HasHygrothermalDataData
    {
        public bool HasHygrothermalData { get; }
    }

    private sealed class HasPhotovoltaicDataData
    {
        public bool HasPhotovoltaicData { get; }
    }
}