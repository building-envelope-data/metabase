using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentVariants;

public abstract class ComponentVariantFilterType
    : AuditableAssociationFilterType<ComponentVariant>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentVariant> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.OfComponent);
        descriptor.Field(_ => _.ToComponent);
    }
}
