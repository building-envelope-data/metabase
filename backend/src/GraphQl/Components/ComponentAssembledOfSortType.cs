using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentAssembledOfSortType
    : ComponentAssemblies.ComponentAssemblySortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentAssembledOfSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}