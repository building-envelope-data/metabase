using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Entities;

public abstract class AuditableEntityFilterType<TEntity>
    : FilterInputType<TEntity>
    where TEntity : IEntity, IAuditable
{
    protected override void Configure(
        IFilterInputTypeDescriptor<TEntity> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        // TODO Do we want to filter by: descriptor.Field(_ => _.Version);
    }
}
