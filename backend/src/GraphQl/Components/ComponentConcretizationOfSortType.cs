using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentConcretizationOfSortType
    : ComponentGeneralizations.ComponentConcretizationAndGeneralizationSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentConcretizationOfSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}