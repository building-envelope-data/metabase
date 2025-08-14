using HotChocolate.Data.Filters;
using Metabase.Configuration;
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
        descriptor.Name(nameof(ComponentConcretizationOfFilterType)[..^10] + GraphQlConfiguration.FilterInputSuffix);
        descriptor.Field(x => x.ConcreteComponent).Ignore();
    }
}