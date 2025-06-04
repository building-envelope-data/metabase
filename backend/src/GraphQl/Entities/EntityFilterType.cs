using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Entities;

public abstract class EntityFilterType<TEntity>
    : FilterInputType<TEntity>
    where TEntity : IEntity
{
    protected override void Configure(
        IFilterInputTypeDescriptor<TEntity> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor
            .Field(x => x.Id)
            .Name("uuid");
    }
}