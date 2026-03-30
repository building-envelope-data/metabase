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
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.OfComponent);
        descriptor.Field(x => x.ToComponent);
    }
}