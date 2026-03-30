using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.ComponentManufacturers;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionManufacturedComponentSortType
    : ComponentManufacturerSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManufacturedComponentSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}