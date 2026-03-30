using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public abstract class InstitutionRepresentativeFilterType
    : AuditableAssociationFilterType<InstitutionRepresentative>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Institution);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Role);
    }
}