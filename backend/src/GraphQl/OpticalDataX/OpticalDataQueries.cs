using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.CostAnalysis.Types;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Scalars;
using Metabase.GraphQl.Requests;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.OpticalDataX;

[ExtendObjectType(nameof(Query))]
public sealed class OpticalDataQueries
{
    public async Task<OpticalData?> GetOpticalDataAsync(
        Guid databaseId,
        Guid id,
        [GraphQLType<LocaleType>] string? locale,
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
        return await dataQueries.GetOpticalDataAsync(
            database,
            id,
            locale,
            resolverContext,
            cancellationToken
        );
    }

    [ListSize(
        AssumedSize = (int)GraphQlConstants.MaximumPageSize - 1,
        SlicingArguments = ["first", "last"],
        SlicingArgumentDefaultValue = (int)GraphQlConstants.MaximumPageSize - 1,
        SizedFields = ["edges", "nodes"],
        RequireOneSlicingArgument = false
    )]
    public Task<OpticalDataConnection> GetAllOpticalDataAsync(
        OpticalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        int? first,
        string? after,
        int? last,
        string? before,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.GetAllDataAsync<OpticalDataConnection, OpticalDataEdge, OpticalData>(
            first,
            after,
            last,
            before,
            (edges, totalCount, pageInfo) => new OpticalDataConnection(edges, totalCount, pageInfo),
            (node, cursor) => new OpticalDataEdge(cursor, node),
            (database, first, after, last, before) => dataQueries.GetAllOpticalDataAsync(
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

    public Task<bool> HasOpticalDataAsync(
        OpticalDataPropositionInput? where,
        [GraphQLType<LocaleType>] string? locale,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(
            (database) => dataQueries.HasOpticalDataAsync(
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