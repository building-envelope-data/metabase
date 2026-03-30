using GreenDonut.Data;
using Metabase.Data;

namespace Metabase.GraphQl;

public static class Sorting
{
    public static SortDefinition<TEntity> DefaultEntityOrder<TEntity>(
        SortDefinition<TEntity> sort
    )
    where TEntity : class, IEntity//, IAuditable
    {
        // always sort by primary key to make pagination cursors unique
        return sort
            // .IfEmpty(_ => _.AddDescending(_ => _.CreatedAt))
            .AddDescending(_ => _.Id);
    }
}