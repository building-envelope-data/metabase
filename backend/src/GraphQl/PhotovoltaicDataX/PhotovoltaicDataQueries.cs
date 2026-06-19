using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Requests;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.PhotovoltaicDataX;

[ExtendObjectType(nameof(Query))]
public sealed class PhotovoltaicDataQueries
{
    public async Task<PhotovoltaicData?> GetPhotovoltaicDataAsync(
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
        return await dataQueries.GetPhotovoltaicDataAsync(
            database,
            id,
            locale,
            resolverContext,
            cancellationToken
        );
    }

    public Task<PhotovoltaicDataConnection> GetAllPhotovoltaicDataAsync(
        PhotovoltaicDataPropositionInput? where,
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
        return dataQueries.GetAllDataAsync<PhotovoltaicDataConnection, PhotovoltaicDataEdge, PhotovoltaicData>(
            first,
            after,
            last,
            before,
            (edges, totalCount, pageInfo) => new PhotovoltaicDataConnection(edges, totalCount, pageInfo),
            (node, cursor) => new PhotovoltaicDataEdge(cursor, node),
            (database, first, after, last, before) => dataQueries.GetAllPhotovoltaicDataAsync(
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

    public Task<bool> HasPhotovoltaicDataAsync(
        PhotovoltaicDataPropositionInput? where,
        string? locale,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(
            (database) => dataQueries.HasPhotovoltaicDataAsync(
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
