using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public class GnuPgKeyFingerprintSortType
    : EntitySortType<GnuPgKeyFingerprint>
{
    protected override void Configure(
        ISortInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Field(x => x.Fingerprint);
        descriptor.Field(x => x.CreatedAt);
        descriptor.Field(x => x.AllowedAt);
        descriptor.Field(x => x.ForbiddenAt);
        descriptor.Field(x => x.User);
        descriptor.Field(x => x.Institution);
    }
}