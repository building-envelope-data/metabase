using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentVariantOfSortType
    : ComponentVariants.ComponentVariantSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentVariant> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentVariantOfSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}