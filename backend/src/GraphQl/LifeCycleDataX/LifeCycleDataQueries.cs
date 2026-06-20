using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Requests;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.LifeCycleDataX;

[ExtendObjectType(nameof(Query))]
public sealed class LifeCycleDataQueries
{
    public async Task<LifeCycleData?> GetLifeCycleDataAsync(
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
        return await dataQueries.GetLifeCycleDataAsync(
            database,
            id,
            locale,
            resolverContext,
            cancellationToken
        );
    }

    public Task<LifeCycleDataConnection> GetAllLifeCycleDataAsync(
        LifeCycleDataPropositionInput? where,
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
        return dataQueries.GetAllDataAsync<LifeCycleDataConnection, LifeCycleDataEdge, LifeCycleData>(
            first,
            after,
            last,
            before,
            (edges, totalCount, pageInfo) => new LifeCycleDataConnection(edges, totalCount, pageInfo),
            (node, cursor) => new LifeCycleDataEdge(cursor, node),
            (database, first, after, last, before) => dataQueries.GetAllLifeCycleDataAsync(
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

    public Task<bool> HasLifeCycleDataAsync(
        LifeCycleDataPropositionInput? where,
        string? locale,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(
            (database) => dataQueries.HasLifeCycleDataAsync(
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
