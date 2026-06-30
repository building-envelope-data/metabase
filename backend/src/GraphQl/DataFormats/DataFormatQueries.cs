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

namespace Metabase.GraphQl.DataFormats;

[ExtendObjectType(nameof(Query))]
public sealed class DataFormatQueries
{
    [UsePaging]
    [UseFiltering<DataFormatFilterType>]
    [UseSorting<DataFormatSortType>]
    public ValueTask<HotChocolate.Types.Pagination.Connection<DataFormat>> GetDataFormatsAsync(
        IResolverContext resolverContext,
        ApplicationDbContext databaseContext,
        CancellationToken cancellationToken
    )
    {
        return databaseContext.DataFormats
            .AsNoTracking()
            .With(resolverContext.GetQueryContext<DataFormat>(), Sorting.DefaultEntityOrder)
            .ToPageAsync(resolverContext.GetPagingArguments(), cancellationToken)
            .ToConnectionAsync();
    }

    public Task<DataFormat?> GetDataFormatAsync(
        Guid id,
        IDataFormatByIdDataLoader byId,
        CancellationToken cancellationToken
    )
    {
        return byId.LoadAsync(id, cancellationToken);
    }
}