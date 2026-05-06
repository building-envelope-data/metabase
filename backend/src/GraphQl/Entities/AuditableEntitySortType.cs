using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Entities;

public abstract class AuditableEntitySortType<TEntity>
    : SortInputType<TEntity>
    where TEntity : IEntity, IAuditable
{
    protected override void Configure(
        ISortInputTypeDescriptor<TEntity> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
    }
}