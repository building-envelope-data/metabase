using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentPartOfSortType
    : ComponentAssemblies.ComponentAssemblySortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentAssembly> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentPartOfSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}