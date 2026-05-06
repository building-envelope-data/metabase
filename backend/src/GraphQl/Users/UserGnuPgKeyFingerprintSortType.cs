using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.GnuPgKeyFingerprints;

namespace Metabase.GraphQl.Users;

public sealed class UserGnuPgKeyFingerprintSortType
    : GnuPgKeyFingerprintSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserGnuPgKeyFingerprintSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}