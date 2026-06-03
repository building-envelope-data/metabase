using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public class GnuPgKeyFingerprintFilterType
    : AuditableEntityFilterType<GnuPgKeyFingerprint>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        // TODO Remove Id, CreatedAt, and UpdatedAt once the base.Configure is respected.
        descriptor.Field(_ => _.Id);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.UpdatedAt);
        descriptor.Field(_ => _.Fingerprint);
        descriptor.Field(_ => _.CreatedAt);
        descriptor.Field(_ => _.AllowedAt);
        descriptor.Field(_ => _.ForbiddenAt);
        descriptor.Field(_ => _.User);
        descriptor.Field(_ => _.Institution);
    }
}
