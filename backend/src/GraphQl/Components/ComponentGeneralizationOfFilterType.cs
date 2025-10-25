using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentGeneralizationOfFilterType
    : ComponentGeneralizations.ComponentConcretizationAndGeneralizationFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentGeneralizationOfFilterType)[..^10] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.GeneralComponent).Ignore();
    }
}