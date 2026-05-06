using HotChocolate.Data.Sorting;
using Metabase.Data;
using Metabase.GraphQl.GnuPgKeyFingerprints;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionGnuPgKeyFingerprintSortType
    : GnuPgKeyFingerprintSortType
{
    protected override void Configure(
        ISortInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionGnuPgKeyFingerprintSortType)[..^"SortType".Length] + GraphQlConstants.SortInputSuffix);
    }
}