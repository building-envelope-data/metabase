using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class InstitutionManagedComponentFilterType
    : ComponentFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Component> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionManagedComponentFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(_ => _.Manager).Ignore();
    }
}
