using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Authorization;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Authorization;
using Metabase.Data;
using Metabase.Enumerations;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Databases;

[ExtendObjectType(nameof(Query))]
public sealed class DatabaseQueries
{
    [UsePaging]
    [UseFiltering<DatabaseFilterType>]
    [UseSorting<DatabaseSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Database>> GetDatabasesAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Databases
            .AsNoTracking()
            .Where(d => d.VerificationState == DatabaseVerificationState.VERIFIED)
            .With(resolverContext.GetQueryContext<Database>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    [UsePaging]
    [UseFiltering<DatabaseFilterType>]
    [UseSorting<DatabaseSortType>]
    [Authorize(Policy = AuthorizationPolicies.ManageDatabaseScopePolicy)]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Database>> GetPendingDatabasesAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Databases
            .AsNoTracking()
            .Where(d => d.VerificationState == DatabaseVerificationState.PENDING)
            .With(resolverContext.GetQueryContext<Database>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<Database?> GetDatabaseAsync(
        Guid id,
        IDatabaseByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadAsync(id, cancellationToken);
    }
}