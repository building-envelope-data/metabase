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
        descriptor.Name(nameof(InstitutionManagedComponentFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Manager).Ignore();
    }
}