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
        descriptor.Field(x => x.Id);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.UpdatedAt);
        descriptor.Field(x => x.Fingerprint);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.AllowedAt);
        descriptor.Field(x => x.ForbiddenAt);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Institution);
    }
}