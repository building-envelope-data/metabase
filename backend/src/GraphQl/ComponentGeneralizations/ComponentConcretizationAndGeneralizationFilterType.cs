using HotChocolate.Data.Filters;
using Metabase.Data;

namespace Metabase.GraphQl.ComponentGeneralizations;

public abstract class ComponentConcretizationAndGeneralizationFilterType
    : FilterInputType<ComponentConcretizationAndGeneralization>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        descriptor.BindFieldsExplicitly();
        descriptor.Field(x => x.ConcreteComponent);
        descriptor.Field(x => x.GeneralComponent);
    }
}