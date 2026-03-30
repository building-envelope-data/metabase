using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.Entities;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public class GnuPgKeyFingerprintSortType
    : AuditableEntitySortType<GnuPgKeyFingerprint>
{
    protected override void Configure(
        ISortInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(GnuPgKeyFingerprintSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
        descriptor.Field(x => x.Fingerprint);
        descriptor.Field(x => x.AllowedAt);
        descriptor.Field(x => x.ForbiddenAt);
    }
}