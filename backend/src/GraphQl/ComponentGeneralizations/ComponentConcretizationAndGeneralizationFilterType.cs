using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.ComponentGeneralizations;

public abstract class ComponentConcretizationAndGeneralizationFilterType
    : AuditableAssociationFilterType<ComponentConcretizationAndGeneralization>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<ComponentConcretizationAndGeneralization> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.ConcreteComponent);
        descriptor.Field(_ => _.GeneralComponent);
    }
}
