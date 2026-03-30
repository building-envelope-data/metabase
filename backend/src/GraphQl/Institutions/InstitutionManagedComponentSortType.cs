using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class InstitutionManagedComponentSortType
    : ComponentSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedComponentSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}