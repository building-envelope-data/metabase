using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.Components;

public sealed class ComponentConcretizationOfFilterType
    : ComponentGeneralizations.ComponentConcretizationAndGeneralizationFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(ComponentConcretizationOfFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.ConcreteComponent).Ignore();
    }
}