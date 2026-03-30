using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentManufacturers;

public abstract class ComponentManufacturerFilterType
    : AuditableAssociationFilterType<ComponentManufacturer>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Component);
        descriptor.Field(x => x.Institution);
    }
}