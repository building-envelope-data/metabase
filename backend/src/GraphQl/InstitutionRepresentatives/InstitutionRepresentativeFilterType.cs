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
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Institution);
        descriptor.Field(_ => _.User);
        descriptor.Field(_ => _.Role);
    }
}
