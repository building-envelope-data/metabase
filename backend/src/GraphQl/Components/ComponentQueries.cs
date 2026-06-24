using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GreenDonut.Data;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Metabase.Data;
using Metabase.GraphQl.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Metabase.GraphQl.Components;

[ExtendObjectType(nameof(Query))]
public sealed class ComponentQueries
{
    [UsePaging]
    [UseFiltering<ComponentFilterType>]
    [UseSorting<ComponentSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<Component>> GetComponentsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        // PagingArguments pagingArguments, // results in the parameter `pagingArguments: PagingArgumentsInput` in the GraphQL schema
        // QueryContext<Component> queryContext, // starts up the projection engine producing many problems
        CancellationToken cancellationToken
    )
    {
        return databaseContext.Components
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<Component>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<Component?> GetComponentAsync(
        Guid id,
        IComponentByIdDataLoader byId,
        // QueryContext<Component> queryContext, // starts up the projection engine producing many problems
        CancellationToken cancellationToken
    )
    {
        return byId
            // .With(queryContext)
            .LoadAsync(id, cancellationToken);
    }
}