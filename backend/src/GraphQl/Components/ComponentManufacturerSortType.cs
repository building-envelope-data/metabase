using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentManufacturerSortType
    : ComponentManufacturers.ComponentManufacturerSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentManufacturer> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentManufacturerSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}