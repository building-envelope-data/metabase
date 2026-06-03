using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.DescriptionOrReferences;

public sealed class DescriptionOrReferenceFilterType
    : FilterInputType<DescriptionOrReference>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<DescriptionOrReference> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(DescriptionOrReferenceFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.BindFieldsExplicitly();
        descriptor.Field(_ => _.Description);
    }
}
