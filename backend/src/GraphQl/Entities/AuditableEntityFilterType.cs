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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
    }
}