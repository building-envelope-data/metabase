using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Entities;

public abstract class EntitySortType<TEntity>
    : SortInputType<TEntity>
    where TEntity : IEntity
{
    protected override void Configure(
        ISortInputTypeDescriptor<TEntity> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id);
    }
}