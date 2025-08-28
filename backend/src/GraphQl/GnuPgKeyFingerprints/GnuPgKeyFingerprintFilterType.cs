using HotChocolate.Data.Filters;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public class GnuPgKeyFingerprintFilterType
    : EntityFilterType<GnuPgKeyFingerprint>
{
    protected override void Configure(
        IFilterInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Fingerprint);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.AllowedAt);
        descriptor.Field(x => x.RevokedAt);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Institution);
        descriptor.Field(x => x.IsRevoked);
        descriptor.Field(x => x.IsAllowed);
    }
}