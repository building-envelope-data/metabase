using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentGeneralizations;

public abstract class ComponentConcretizationAndGeneralizationSortType
    : AuditableAssociationSortType<ComponentConcretizationAndGeneralization>
{
    protected override void Configure(
        ISortInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        base.Configure(descriptor);
    }
}