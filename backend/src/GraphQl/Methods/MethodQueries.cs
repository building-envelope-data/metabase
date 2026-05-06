using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Methods;

[ExtendObjectType(nameof(Query))]
public sealed class MethodQueries
{
    [UsePaging]
    [UseFiltering<MethodFilterType>]
    [UseSorting<MethodSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Method>> GetMethodsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Methods
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<Method>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<Method?> GetMethodAsync(
        Guid id,
        IMethodByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadAsync(id, cancellationToken);
    }
}