using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentManufacturers;

public abstract class ComponentManufacturerSortType
    : AuditableAssociationSortType<ComponentManufacturer>
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
    }
}