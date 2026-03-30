using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.InstitutionMethodDevelopers;

public abstract class InstitutionMethodDeveloperFilterType
    : AuditableAssociationFilterType<InstitutionMethodDeveloper>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionMethodDeveloper> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Method);
        descriptor.Field(x => x.Institution);
    }
}