using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using GraphQL;
using HotChocolate;
using HotChocolate.Resolvers;
using Metabase.Data;
using Metabase.GraphQl.DataX;
using Metabase.GraphQl.CalorimetricDataX;
using Metabase.GraphQl.GeometricDataX;
using Metabase.GraphQl.HygrothermalDataX;
using Metabase.GraphQl.LifeCycleDataX;
using Metabase.GraphQl.OpticalDataX;
using Metabase.GraphQl.PhotovoltaicDataX;
using Metabase.Json;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HotChocolate.Types.Pagination;
using Metabase.Extensions;
using System.Text.Json.Serialization;

namespace Metabase.GraphQl.Requests;

public static partial class Log
{
    [LoggerMessage(
        Level = LogLevel.Warning,
        Message = "Failed with errors {Errors} to query the database {Locator} for {Request}.")]
    public static partial void FailedWithErrors(
        this ILogger<DataQueries> logger,
        string Errors,
        Uri Locator,
        string Request
    );
}

public sealed class DataQueries(
    ApplicationDbContext databaseContext,
    QueryingDatabases queryingDatabases,
    GraphQlRequestHelper graphQlRequestHelper,
    ILogger<DataQueries> logger
)
{
    private static readonly string[] s_calorimetricDataFileNames =
    [
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "CalorimetricData.graphql"
    ];

    private static readonly string[] s_geometricDataFileNames =
    [
        "DataFields.graphql",
        "GeometricDataFields.graphql",
        "GeometricData.graphql"
    ];

    private static readonly string[] s_hygrothermalDataFileNames =
    [
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "HygrothermalData.graphql"
    ];

    private static readonly string[] s_lifeCycleDataFileNames =
    [
        "DataFields.graphql",
        "LifeCycleDataFields.graphql",
        "LifeCycleData.graphql"
    ];

    private static readonly string[] s_opticalDataFileNames =
    [
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "OpticalData.graphql"
    ];

    private static readonly string[] s_photovoltaicDataFileNames =
    [
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PhotovoltaicData.graphql"
    ];

    private static readonly string[] s_allCalorimetricDataFileNames =
    [
        "DataFields.graphql",
        "CalorimetricDataFields.graphql",
        "PageInfoFields.graphql",
        "AllCalorimetricData.graphql"
    ];

    private static readonly string[] s_allGeometricDataFileNames =
    [
        "DataFields.graphql",
        "GeometricDataFields.graphql",
        "PageInfoFields.graphql",
        "AllGeometricData.graphql"
    ];

    private static readonly string[] s_allHygrothermalDataFileNames =
    [
        "DataFields.graphql",
        "HygrothermalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllHygrothermalData.graphql"
    ];

    private static readonly string[] s_allLifeCycleDataFileNames =
    [
        "DataFields.graphql",
        "LifeCycleDataFields.graphql",
        "PageInfoFields.graphql",
        "AllLifeCycleData.graphql"
    ];

    private static readonly string[] s_allOpticalDataFileNames =
    [
        "DataFields.graphql",
        "OpticalDataFields.graphql",
        "PageInfoFields.graphql",
        "AllOpticalData.graphql"
    ];

    private static readonly string[] s_allPhotovoltaicDataFileNames =
    [
        "DataFields.graphql",
        "PhotovoltaicDataFields.graphql",
        "PageInfoFields.graphql",
        "AllPhotovoltaicData.graphql"
    ];

    private static readonly string[] s_hasCalorimetricDataFileNames =
    [
        "HasCalorimetricData.graphql"
    ];

    private static readonly string[] s_hasGeometricDataFileNames =
    [
        "HasGeometricData.graphql"
    ];

    private static readonly string[] s_hasHygrothermalDataFileNames =
    [
        "HasHygrothermalData.graphql"
    ];

    private static readonly string[] s_hasLifeCycleDataFileNames =
    [
        "HasLifeCycleData.graphql"
    ];

    private static readonly string[] s_hasOpticalDataFileNames =
    [
        "HasOpticalData.graphql"
    ];

    private static readonly string[] s_hasPhotovoltaicDataFileNames =
    [
        "HasPhotovoltaicData.graphql"
    ];

    private sealed record NeighboringCursors(
        [property: JsonPropertyName("l")] string? Before,
        [property: JsonPropertyName("r")] string? After
    );

    /// The cursor of an edge of an edge list resulting from interleaving edge
    /// lists of various databases. The database the edge belongs to is
    /// identified by the database ID (`DatabaseId`). The cursor of the edge
    /// within its database (hereafter referred to be "local cursor") is
    /// `Cursors[DatabaseId].Before` and `Cursors[DatabaseId].After` (both
    /// values are identical in this case). For each other database with ID
    /// `OtherId`, if for that database there is an edge in the edge list
    /// before the current edge, then its cursor is `Cursors[OtherId].Before`,
    /// otherwise the value is `null`, and if there is an edge after the
    /// current edge, then its cursor is `Cursors[OtherId].After`, otherwise
    /// `null`.
    ///
    /// Database X:  [1, 2, 3, 4, 5]                        (local cursors)
    /// Database Y:  [a, b, c]                              (local cursors)
    /// Interleaved: [1, a, 2, b, 3, c, 4, 5]               (short hand)
    ///              [                                      (long form)
    ///                 [(X, { X: (1, 1), Y: (null, a) })]  (0)
    ///                 [(Y, { X: (1, 2), Y: (a, a) })]     (1)
    ///                 [(X, { X: (2, 2), Y: (a, b) })]     (2)
    ///                 [(X, { X: (2, 3), Y: (b, b) })]     (3)
    ///                 [(X, { X: (3, 3), Y: (b, c) })]     (4)
    ///                 [(X, { X: (3, 4), Y: (c, c) })]     (5)
    ///                 [(X, { X: (4, 4), Y: (c, null) })]  (6)
    ///                 [(X, { X: (5, 5), Y: (c, null) })]  (7)
    ///              ]
    ///
    /// The values of `Before` and `After` are used for queries with `after` or
    /// `before` pagination argument. For example, to query for edges after
    /// `[(X, { X: (3, 4), Y: (c, c) })]` (5), database `X` is asked for edges
    /// after `3` (the `Before` cursor) and `Y` for edges after `c` (the
    /// `Before` cursor), and to query for edges before it, database `X` is
    /// asked for edges before `4` (the `After` cursor) and `Y` for edges
    /// before `c` (the `After` cursor).
    private sealed record CompoundCursor
    {
        [JsonPropertyName("d")]
        public required Guid DatabaseId { get; set; }

        [JsonPropertyName("c")]
        public required Dictionary<Guid, NeighboringCursors> Cursors { get; set; }
    };

    private static string SerializeCompoundCursor(CompoundCursor cursor)
    {
        return JsonSerializer.Serialize(cursor, JsonSerializerSettings.Compact).Base64Encode();
    }

    private static CompoundCursor? DeserializeCompoundCursor(string? cursors)
    {
        if (cursors is null)
        {
            return null;
        }
        return JsonSerializer.Deserialize<CompoundCursor>(
            cursors.Base64Decode(),
            JsonSerializerSettings.Compact
        );
    }

    private enum PaginationDirection
    {
        FORWARD,
        BACKWARD
    }

    public async Task<TDataConnection> GetAllDataAsync<TDataConnection, TDataEdge, TDataNode>(
        int? first,
        string? after,
        int? last,
        string? before,
        Func<IReadOnlyList<TDataEdge>, int, ConnectionPageInfo, TDataConnection> createDataConnection,
        Func<TDataNode, string, TDataEdge> createDataEdge,
        Func<Database, int?, string?, int?, string?, Task<TDataConnection?>> getAllDataAsync,
        CancellationToken cancellationToken
    )
        where TDataConnection : DataConnection<TDataEdge>
        where TDataEdge : DataEdge<TDataNode>
    {
        var paginationDirection = (first, last) switch
        {
            (_, null) => PaginationDirection.FORWARD,
            (null, _) => PaginationDirection.BACKWARD,
            _ => PaginationDirection.FORWARD,
        };
        var compoundAfter = DeserializeCompoundCursor(after);
        var compoundBefore = DeserializeCompoundCursor(before);
        var databases = await databaseContext.Databases.AsNoTracking()
            .Where(_ => _.VerificationState == Enumerations.DatabaseVerificationState.VERIFIED)
            // on follow-up requests include only the previously included databases
            .If(
                compoundAfter is not null || compoundBefore is not null,
                queryable => queryable.Where(_ =>
                    (compoundAfter ?? compoundBefore ?? default!).Cursors.Keys.Contains(_.Id)
                )
            )
            // order databases to stabilize compound cursors and the order of interleaved edges
            .OrderBy(_ => _.CreatedAt)
            .ToListAsync(cancellationToken);
        if (databases.Count is 0)
        {
            return createDataConnection([], 0, new ConnectionPageInfo(false, false, null, null));
        }
        // reorder and rotate the databases get the first edge after or last edge before and so forth of the interleaved edges in the correct order
        var beginAfterDatabaseId =
            paginationDirection is PaginationDirection.FORWARD
            ? compoundAfter?.DatabaseId
            : compoundBefore?.DatabaseId;
        var rotatedDatabases = databases
            .IfList(paginationDirection is PaginationDirection.BACKWARD, _ => _.ToReversed())
            .IfList(beginAfterDatabaseId is not null, _ => _.Rotate(_ => _.Id == beginAfterDatabaseId!));
        // fetch data from the databases concurrently leaving the order intact
        var connections = await Task.WhenAll(
            rotatedDatabases.Select((database) =>
                getAllDataAsync(
                    database,
                    first is null ? null : first + 1,
                    compoundAfter?.Cursors.GetValueOrDefault(database.Id)?.Before,
                    last is null ? null : last + 1,
                    compoundBefore?.Cursors.GetValueOrDefault(database.Id)?.After
                )
            )
        );
        var databaseToConnection = rotatedDatabases
            .Zip(connections, (database, connection) => connection is null ? null : new { databaseId = database.Id, connection })
            .NotNull()
            .ToDictionary(_ => _.databaseId, _ => _.connection);
        // adapt the after and before cursors such that they can serve as seed for the next or previous edge
        compoundAfter?.Cursors[compoundAfter.DatabaseId] = new(
            compoundAfter.Cursors.GetValueOrDefault(compoundAfter.DatabaseId)?.Before,
            databaseToConnection.GetValueOrDefault(compoundAfter.DatabaseId)?.Edges.GetFirstOrDefault()?.Cursor
        );
        compoundBefore?.Cursors[compoundBefore.DatabaseId] = new(
            databaseToConnection.GetValueOrDefault(compoundBefore.DatabaseId)?.Edges.GetLastOrDefault()?.Cursor,
            compoundBefore.Cursors.GetValueOrDefault(compoundBefore.DatabaseId)?.After
        );
        // interleave edges and replace their cursors with compound cursors (see `CompoundCursor`)
        var edges = paginationDirection is PaginationDirection.FORWARD
            ? databaseToConnection
                .Select(_ =>
                    // pair each edge with its right neighbor padding at the
                    // end, where the first entry in each tuple is the edge and
                    // the second its right neighbor `(edge, neighbor)`. For
                    // example [1, 2, 3] becomes [(1, 2), (2, 3), (3, null)]
                    _.Value.Edges.Zip(
                        _.Value.Edges.Skip(1).Append(null),
                        (current, after) => new { current, after }
                    )
                    .Select((neighboringEdges) => (neighboringEdges, databaseId: _.Key))
                )
                // interleave edges of various databases (from the left or
                // left-aligned or padded right with `null`s)
                // Database X:  [1, 2, 3, 4, 5]                        (local cursors)
                // Database Y:  [a, b, c]                              (local cursors)
                // Interleaved: [1, a, 2, b, 3, c, 4, 5]               (short hand)
                //  (long form) [(1, 2), (a, b), (2, 3), (b, c), (3, 4), (c, null), (4, 5), (5, null)]
                .Interleave()
                // give each edge a compound cursor with enough information for
                // using it as `after` and `before` in paginated queries:
                //              [                                      (long form)
                //                 [(X, { X: (1, 1), Y: (null, a) })]  (0)
                //                 [(Y, { X: (1, 2), Y: (a, a) })]     (1)
                //                 [(X, { X: (2, 2), Y: (a, b) })]     (2)
                //                 [(Y, { X: (2, 3), Y: (b, b) })]     (3)
                //                 [(X, { X: (3, 3), Y: (b, c) })]     (4)
                //                 [(Y, { X: (3, 4), Y: (c, c) })]     (5)
                //                 [(X, { X: (4, 4), Y: (c, null) })]  (6)
                //                 [(X, { X: (5, 5), Y: (c, null) })]  (7)
                //              ]
                // if `after` were [(Y, { X: (0, 1), Y: (#, #) })], then `null`
                // in the cursor of (0) would become `#`:
                //                 [(X, { X: (1, 1), Y: (#, a) })]     (0)
                .Scan(
                    compoundAfter ?? new()
                    {
                        Cursors = databases.ToDictionary(
                            _ => _.Id,
                            _ => new NeighboringCursors(
                                null,
                                databaseToConnection.GetValueOrDefault(_.Id)?.Edges.GetFirstOrDefault()?.Cursor
                            )
                        ),
                        DatabaseId = rotatedDatabases[^1].Id
                    },
                    (compoundCursor, _) =>
                    {
                        // adapt the cursor for the current edge
                        compoundCursor.Cursors[_.databaseId] = new(_.neighboringEdges.current.Cursor, _.neighboringEdges.current.Cursor);
                        compoundCursor.DatabaseId = _.databaseId;
                        var currentEdgeCursor = SerializeCompoundCursor(compoundCursor);
                        // adapt the cursor for the edge coming after
                        compoundCursor.Cursors[_.databaseId] = new(_.neighboringEdges.current.Cursor, _.neighboringEdges.after?.Cursor);
                        return (compoundCursor, createDataEdge(_.neighboringEdges.current.Node, currentEdgeCursor));
                    }
                )
                .ToList()
            : databaseToConnection
                .Select(_ =>
                    // pair each edge with its left neighbor padding at the
                    // beginning, where the second entry in each tuple is the
                    // edge and the first its left neighbor `(neighbor, edge)`.
                    // For example [1, 2, 3] becomes [(null, 1), (1, 2), (2, 3)].
                    // By reversing it before interleaving and scanning it with
                    // the edges of other databases, the edges are iterated in
                    // reverse, that is, from the end to the beginning.
                    _.Value.Edges.Prepend(null).SkipLast(1).Zip(
                        _.Value.Edges,
                        (before, current) => new { before, current }
                    )
                    .Reverse()
                    .Select((neighboringEdges) => (neighboringEdges, databaseId: _.Key)))
                // interleave edges of various databases (from the right or
                // right-aligned or padded left with `null`s)
                // Database X:           [1, 2, 3, 4, 5]               (local cursors)
                // Database Y:                 [a, b, c]               (local cursors)
                // Interleaved: [1, 2, a, 3, b, 4, c, 5]               (short hand)
                //  (long form) [(null, 1), (1, 2), (null, a), (2, 3), (a, b), (3, 4), (b, c), (4, 5)]
                .Interleave()
                // give each edge a compound cursor with enough information for
                // using it as `after` and `before` in paginated queries
                //              [                                      (long form)
                //                 [(X, { X: (1, 1), Y: (null, a) })]  (0)
                //                 [(X, { X: (2, 2), Y: (null, a) })]  (1)
                //                 [(Y, { X: (2, 3), Y: (a, a) })]     (2)
                //                 [(X, { X: (3, 3), Y: (a, b) })]     (3)
                //                 [(Y, { X: (3, 4), Y: (b, b) })]     (4)
                //                 [(X, { X: (4, 4), Y: (b, c) })]     (5)
                //                 [(Y, { X: (4, 5), Y: (c, c) })]     (6)
                //                 [(X, { X: (5, 5), Y: (c, null) })]  (7)
                //              ]
                // if `before` were [(Y, { X: (5, 6), Y: (d, d) })], then `null`
                // in the cursor of (7) would become `d`:
                //                 [(X, { X: (5, 5), Y: (c, d) })]     (7)
                .Scan(
                    compoundBefore ?? new()
                    {
                        Cursors = databases.ToDictionary(
                            _ => _.Id,
                            _ => new NeighboringCursors(
                                databaseToConnection.GetValueOrDefault(_.Id)?.Edges.GetFirstOrDefault()?.Cursor,
                                null
                            )
                        ),
                        DatabaseId = rotatedDatabases[^1].Id
                    },
                    (compoundCursor, _) =>
                    {
                        // adapt the cursor for the current edge
                        compoundCursor.Cursors[_.databaseId] = new(_.neighboringEdges.current.Cursor, _.neighboringEdges.current.Cursor);
                        compoundCursor.DatabaseId = _.databaseId;
                        var currentEdgeCursor = SerializeCompoundCursor(compoundCursor);
                        // adapt the cursor for the edge coming before
                        compoundCursor.Cursors[_.databaseId] = new(_.neighboringEdges.before?.Cursor, _.neighboringEdges.current.Cursor);
                        return (compoundCursor, createDataEdge(_.neighboringEdges.current.Node, currentEdgeCursor));
                    }
                )
                // undo the reversal of the edges above
                .Reverse()
                .ToList();
        // clamp the edges taking only the first `first` and the last `last` (or the maximum page size)
        var cappedFirst = (int)Math.Min(first ?? (int)GraphQlConstants.MaximumPageSize, GraphQlConstants.MaximumPageSize);
        var cappedLast = (int)Math.Min(last ?? (int)GraphQlConstants.MaximumPageSize, GraphQlConstants.MaximumPageSize);
        var clampedEdges =
            ((first, last) switch
            {
                (_, null) => edges.Take(cappedFirst),
                (null, _) => edges.TakeLast(cappedLast),
                _ => edges.Take(cappedFirst).TakeLast(cappedLast)
            })
            .ToList()
            .AsReadOnly();
        // compute total cound and page info
        var totalCount = connections.Sum(_ => _?.TotalCount ?? 0);
        var pageInfo = new ConnectionPageInfo(
            // there is a next page if edges were removed from the end
            hasNextPage: (clampedEdges.Count > 0 && clampedEdges[^1] != edges[^1]) || connections.Any(_ => _?.PageInfo.HasNextPage ?? false),
            // there is a previous page if edges were removed from the beginning
            hasPreviousPage: (clampedEdges.Count > 0 && clampedEdges[0] != edges[0]) || connections.Any(_ => _?.PageInfo.HasPreviousPage ?? false),
            startCursor: clampedEdges.Count is 0 ? null : clampedEdges[0].Cursor,
            endCursor: clampedEdges.Count is 0 ? null : clampedEdges[^1].Cursor
        );
        return createDataConnection(clampedEdges, totalCount, pageInfo);
    }

    public async Task<bool> HasDataAsync(
        Func<Database, Task<bool?>> hasDataAsync,
        CancellationToken cancellationToken
    )
    {
        var databases = await databaseContext.Databases.AsNoTracking().ToListAsync(cancellationToken);
        var hasData = await Task.WhenAll(
            databases.Select((database) =>
                hasDataAsync(database)
            )
        );
        return hasData.Any(_ => _ ?? false);
    }

    public async Task<IData?> GetDataAsync(
        Database database,
        Guid id,
        DataKind kind,
        string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return kind switch
        {
            DataKind.CALORIMETRIC_DATA => await GetCalorimetricDataAsync(database, id, locale, resolverContext, cancellationToken),
            DataKind.GEOMETRIC_DATA => await GetGeometricDataAsync(database, id, locale, resolverContext, cancellationToken),
            DataKind.HYGROTHERMAL_DATA => await GetHygrothermalDataAsync(database, id, locale, resolverContext, cancellationToken),
            DataKind.LIFE_CYCLE_DATA => await GetLifeCycleDataAsync(database, id, locale, resolverContext, cancellationToken),
            DataKind.OPTICAL_DATA => await GetOpticalDataAsync(database, id, locale, resolverContext, cancellationToken),
            DataKind.PHOTOVOLTAIC_DATA => await GetPhotovoltaicDataAsync(database, id, locale, resolverContext, cancellationToken),
            _ => throw new ArgumentOutOfRangeException($"The data kind {kind} is not supported.")
        };
    }

    public async Task<bool?> HasDataAsync(
        Database database,
        DataKind kind,
        DataPropositionInput? where,
        string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return kind switch
        {
            DataKind.CALORIMETRIC_DATA => await HasCalorimetricDataAsync(database, where?.ToCalorimetricInput(), locale, resolverContext, cancellationToken),
            DataKind.GEOMETRIC_DATA => await HasGeometricDataAsync(database, where?.ToGeometricInput(), locale, resolverContext, cancellationToken),
            DataKind.HYGROTHERMAL_DATA => await HasHygrothermalDataAsync(database, where?.ToHygrothermalInput(), locale, resolverContext, cancellationToken),
            DataKind.LIFE_CYCLE_DATA => await HasLifeCycleDataAsync(database, where?.ToLifeCycleInput(), locale, resolverContext, cancellationToken),
            DataKind.OPTICAL_DATA => await HasOpticalDataAsync(database, where?.ToOpticalInput(), locale, resolverContext, cancellationToken),
            DataKind.PHOTOVOLTAIC_DATA => await HasPhotovoltaicDataAsync(database, where?.ToPhotovoltaiInput(), locale, resolverContext, cancellationToken),
            _ => throw new ArgumentOutOfRangeException($"The data kind {kind} is not supported.")
        };
    }

    public async Task<CalorimetricData?> GetCalorimetricDataAsync(
        Database database,
        Guid id,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.CalorimetricData;
    }

    public async Task<GeometricData?> GetGeometricDataAsync(
        Database database,
        Guid id,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.GeometricData;
    }

    public async Task<HygrothermalData?> GetHygrothermalDataAsync(
        Database database,
        Guid id,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HygrothermalData;
    }

    public async Task<LifeCycleData?> GetLifeCycleDataAsync(
        Database database,
        Guid id,
        string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<LifeCycleDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_lifeCycleDataFileNames
                        ),
                        new
                        {
                            id,
                            locale
                        },
                        nameof(LifeCycleData)
                    ),
                    resolverContext,
                    cancellationToken
                )
            )?.LifeCycleData;
    }


    public async Task<OpticalData?> GetOpticalDataAsync(
        Database database,
        Guid id,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.OpticalData;
    }

    public async Task<PhotovoltaicData?> GetPhotovoltaicDataAsync(
        Database database,
        Guid id,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.PhotovoltaicData;
    }

    public async Task<CalorimetricDataConnection?> GetAllCalorimetricDataAsync(
        Database database,
        CalorimetricDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
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
                    resolverContext,
                    cancellationToken
                )
            )?.AllCalorimetricData;
    }

    public async Task<GeometricDataConnection?> GetAllGeometricDataAsync(
        Database database,
        GeometricDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
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
                    resolverContext,
                    cancellationToken
                )
            )?.AllGeometricData;
    }

    public async Task<HygrothermalDataConnection?> GetAllHygrothermalDataAsync(
        Database database,
        HygrothermalDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
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
                    resolverContext,
                    cancellationToken
                )
            )?.AllHygrothermalData;
    }

    public async Task<LifeCycleDataConnection?> GetAllLifeCycleDataAsync(
        Database database,
        LifeCycleDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<AllLifeCycleDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_allLifeCycleDataFileNames
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
                        "AllLifeCycleData"
                    ),
                    resolverContext,
                    cancellationToken
                )
            )?.AllLifeCycleData;
    }

    public async Task<OpticalDataConnection?> GetAllOpticalDataAsync(
        Database database,
        OpticalDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
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
                    resolverContext,
                    cancellationToken
                )
            )?.AllOpticalData;
    }

    public async Task<PhotovoltaicDataConnection?> GetAllPhotovoltaicDataAsync(
        Database database,
        PhotovoltaicDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
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
                    resolverContext,
                    cancellationToken
                )
            )?.AllPhotovoltaicData;
    }

    public async Task<bool?> HasCalorimetricDataAsync(
        Database database,
        CalorimetricDataPropositionInput? where,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HasCalorimetricData;
    }

    public async Task<bool?> HasGeometricDataAsync(
        Database database,
        GeometricDataPropositionInput? where,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HasGeometricData;
    }

    public async Task<bool?> HasHygrothermalDataAsync(
        Database database,
        HygrothermalDataPropositionInput? where,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HasHygrothermalData;
    }

    public async Task<bool?> HasLifeCycleDataAsync(
        Database database,
        LifeCycleDataPropositionInput? where,
        string? locale,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return (await QueryDatabase<HasLifeCycleDataData>(
                    database,
                    new GraphQLRequest(
                        await QueryingDatabases.ConstructQuery(
                            s_hasLifeCycleDataFileNames
                        ),
                        new
                        {
                            where,
                            locale
                        },
                        "HasLifeCycleData"
                    ),
                    resolverContext,
                    cancellationToken
                )
            )?.HasLifeCycleData;
    }

    public async Task<bool?> HasOpticalDataAsync(
        Database database,
        OpticalDataPropositionInput? where,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HasOpticalData;
    }

    public async Task<bool?> HasPhotovoltaicDataAsync(
        Database database,
        PhotovoltaicDataPropositionInput? where,
        string? locale,
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
                    resolverContext,
                    cancellationToken
                )
            )?.HasPhotovoltaicData;
    }

    private Task<TGraphQlResponse?> QueryDatabase<TGraphQlResponse>(
        Database database,
        GraphQLRequest request,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    where TGraphQlResponse : class
    {
        return graphQlRequestHelper.TransformExceptionsAsync(
            async () =>
            {
                var deserializedGraphQlResponse =
                    await queryingDatabases.QueryDatabase<TGraphQlResponse>(
                        database,
                        request,
                        cancellationToken
                    );
                if (deserializedGraphQlResponse.Errors?.Length > 0)
                {
                    logger.FailedWithErrors(
                        JsonSerializer.Serialize(deserializedGraphQlResponse.Errors),
                        database.Locator,
                        JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)
                    );
                    foreach (var error in deserializedGraphQlResponse.Errors)
                    {
                        var errorBuilder = ErrorBuilder.New()
                            .SetCode("DATABASE_QUERY_ERROR")
                            // .SetPath(error.Path) // TODO Add the error path. Just using `error.Path` does not work as it contains non-"GraphQlName"s according to HotChocolate sometimes.
                            .SetMessage(
                                $"The GraphQL response received from the database {database.Locator} for the request {JsonSerializer.Serialize(request, JsonSerializerSettings.GraphQl)} reported the error {error.Message}.");
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
            },
            database.Locator,
            request,
            resolverContext
        );
    }

    private sealed record OpticalDataData(OpticalData OpticalData);
    private sealed record HygrothermalDataData(HygrothermalData HygrothermalData);
    private sealed record LifeCycleDataData(LifeCycleData LifeCycleData);
    private sealed record CalorimetricDataData(CalorimetricData CalorimetricData);
    private sealed record PhotovoltaicDataData(PhotovoltaicData PhotovoltaicData);
    private sealed record GeometricDataData(GeometricData GeometricData);
    private sealed record AllOpticalDataData(OpticalDataConnection AllOpticalData);
    private sealed record AllHygrothermalDataData(HygrothermalDataConnection AllHygrothermalData);
    private sealed record AllLifeCycleDataData(LifeCycleDataConnection AllLifeCycleData);
    private sealed record AllCalorimetricDataData(CalorimetricDataConnection AllCalorimetricData);
    private sealed record AllGeometricDataData(GeometricDataConnection AllGeometricData);
    private sealed record AllPhotovoltaicDataData(PhotovoltaicDataConnection AllPhotovoltaicData);
    private sealed record HasOpticalDataData(bool HasOpticalData);
    private sealed record HasCalorimetricDataData(bool HasCalorimetricData);
    private sealed record HasGeometricDataData(bool HasGeometricData);
    private sealed record HasHygrothermalDataData(bool HasHygrothermalData);
    private sealed record HasLifeCycleDataData(bool HasLifeCycleData);
    private sealed record HasPhotovoltaicDataData(bool HasPhotovoltaicData);
}