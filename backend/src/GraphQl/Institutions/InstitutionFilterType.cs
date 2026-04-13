using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.Institutions;

public class InstitutionFilterType
    : EntityFilterType<Institution>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<Institution> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(_ => _.Name);
        descriptor.Field(_ => _.Abbreviation);
        descriptor.Field(_ => _.Description);
        descriptor.Field(_ => _.Contact);
        descriptor.Field(_ => _.State);
        descriptor.Field(_ => _.Extras);
        descriptor.Field(_ => _.DevelopedMethods);
        descriptor.Field(_ => _.DevelopedMethodEdges);
        descriptor.Field(_ => _.ManagedMethods);
        descriptor.Field(_ => _.ManagedDataFormats);
        descriptor.Field(_ => _.ManufacturedComponents);
        descriptor.Field(_ => _.ManufacturedComponentEdges);
        descriptor.Field(_ => _.OperatedDatabases);
        descriptor.Field(_ => _.Manager);
        descriptor.Field(_ => _.ManagedInstitutions);
        descriptor.Field(_ => _.Representatives);
        descriptor.Field(_ => _.RepresentativeEdges);
        descriptor.Field(_ => _.GnuPgKeyFingerprints);
    }
}