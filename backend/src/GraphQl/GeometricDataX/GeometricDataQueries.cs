using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Requests;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.GeometricDataX;

[ExtendObjectType(nameof(Query))]
public sealed class GeometricDataQueries
{
    public async Task<GeometricData?> GetGeometricDataAsync(
        Guid databaseId,
        Guid id,
        string? locale,
        DataQueries dataQueries,
        ApplicationDbContext databaseContext,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        var database = await databaseContext.Databases.AsNoTracking()
            .Where(_ => _.Id == databaseId)
            .SingleOrDefaultAsync(cancellationToken);
        if (database is null)
        {
            return null;
        }
        return await dataQueries.GetGeometricDataAsync(
            database,
            id,
            locale,
            resolverContext,
            cancellationToken
        );
    }

    public Task<GeometricDataConnection> GetAllGeometricDataAsync(
        GeometricDataPropositionInput? where,
        string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllDataAsync<GeometricDataConnection, GeometricDataEdge, GeometricData>(
            first,
            after,
            last,
            before,
            (edges, totalCount, pageInfo) => new GeometricDataConnection(edges, totalCount, pageInfo),
            (node, cursor) => new GeometricDataEdge(cursor, node),
            (database, first, after, last, before) => dataQueries.GetAllGeometricDataAsync(
                database,
                where,
                locale,
                first,
                after,
                last,
                before,
                resolverContext,
                cancellationToken
            ),
            cancellationToken
        );
    }

    public Task<bool> HasGeometricDataAsync(
        GeometricDataPropositionInput? where,
        string? locale,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(
            (database) => dataQueries.HasGeometricDataAsync(
                database,
                where,
                locale,
                resolverContext,
                cancellationToken
            ),
            cancellationToken
        );
    }
}
