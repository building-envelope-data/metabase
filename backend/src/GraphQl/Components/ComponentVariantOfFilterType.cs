using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentVariantOfFilterType
    : ComponentVariants.ComponentVariantFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentVariant> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentVariantOfFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.ToComponent).Ignore();
    }
}