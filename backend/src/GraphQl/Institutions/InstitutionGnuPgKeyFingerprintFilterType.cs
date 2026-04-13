using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.GnuPgKeyFingerprints;

namespace Metabase.GraphQl.Institutions;

public sealed class InstitutionGnuPgKeyFingerprintFilterType
    : GnuPgKeyFingerprintFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(InstitutionGnuPgKeyFingerprintFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.Institution).Ignore();
    }
}