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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Name);
        descriptor.Field(x => x.Description);
        descriptor.Field(x => x.CalculationLocator);
        descriptor.Field(x => x.Categories);
        descriptor.Field(x => x.InstitutionDevelopers);
        descriptor.Field(x => x.InstitutionDeveloperEdges);
        descriptor.Field(x => x.UserDevelopers);
        descriptor.Field(x => x.UserDeveloperEdges);
        descriptor.Field(x => x.Manager);
    }
}