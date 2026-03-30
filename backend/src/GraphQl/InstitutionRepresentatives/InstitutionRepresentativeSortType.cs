using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Associations;

namespace Metabase.GraphQl.InstitutionRepresentatives;

public abstract class InstitutionRepresentativeSortType
    : AuditableAssociationSortType<InstitutionRepresentative>
{
    protected override void Configure(
        ISortInputTypeDescriptor<InstitutionRepresentative> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Role);
    }
}