using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Methods;

public class MethodFilterType
    : AuditableEntityFilterType<Method>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Method> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.CalculationLocator);
        descriptor.Field(_ => _.Categories);
        descriptor.Field(_ => _.InstitutionDevelopers);
        descriptor.Field(_ => _.InstitutionDeveloperEdges);
        descriptor.Field(_ => _.UserDevelopers);
        descriptor.Field(_ => _.UserDeveloperEdges);
        descriptor.Field(_ => _.Manager);
    }
}
