using HotChocolate.Data.Sorting;
using Metabase.Data;

namespace Metabase.GraphQl.DescriptionOrReferences;

public sealed class DescriptionOrReferenceSortType
    : SortInputType<DescriptionOrReference>
{
    protected override void Configure(
        ISortInputTypeDescriptor<DescriptionOrReference> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(DescriptionOrReferenceSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.Description);
    }
}
