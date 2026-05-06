using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentVariants;

public abstract class ComponentVariantSortType
    : AuditableAssociationSortType<ComponentVariant>
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentVariant> descriptor
    )
    {
        base.Configure(descriptor);
    }
}