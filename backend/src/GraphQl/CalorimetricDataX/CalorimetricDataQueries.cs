using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.CostAnalysis.Types;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Requests;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.CalorimetricDataX;

[ExtendObjectType(nameof(Query))]
public sealed class CalorimetricDataQueries
{
    public async Task<CalorimetricData?> GetCalorimetricDataAsync(
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
        return await dataQueries.GetCalorimetricDataAsync(
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
    public Task<CalorimetricDataConnection> GetAllCalorimetricDataAsync(
        CalorimetricDataPropositionInput? where,
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
        return dataQueries.GetAllDataAsync<CalorimetricDataConnection, CalorimetricDataEdge, CalorimetricData>(
            first,
            after,
            last,
            before,
            (edges, totalCount, pageInfo) => new CalorimetricDataConnection(edges, totalCount, pageInfo),
            (node, cursor) => new CalorimetricDataEdge(cursor, node),
            (database, first, after, last, before) => dataQueries.GetAllCalorimetricDataAsync(
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

    public Task<bool> HasCalorimetricDataAsync(
        CalorimetricDataPropositionInput? where,
        string? locale,
        DataQueries dataQueries,
        IResolverContext resolverContext,
        CancellationToken cancellationToken
    )
    {
        return dataQueries.HasDataAsync(
            (database) => dataQueries.HasCalorimetricDataAsync(
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