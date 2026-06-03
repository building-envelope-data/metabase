using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Associations;

public abstract class AuditableAssociationSortType<TAssociation>
    : SortInputType<TAssociation>
    where TAssociation : IAssociation, IAuditable
{
    protected override void Configure(
        ISortInputTypeDescriptor<TAssociation> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
    }
}
