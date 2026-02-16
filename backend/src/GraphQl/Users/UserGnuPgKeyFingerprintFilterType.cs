using HotChocolate.Data.Filters;
using Metabase.Configuration;
using Metabase.Data;
using Metabase.GraphQl.GnuPgKeyFingerprints;

namespace Metabase.GraphQl.Users;

public sealed class UserGnuPgKeyFingerprintFilterType
    : GnuPgKeyFingerprintFilterType
{
    protected override void Configure(
        IFilterInputTypeDescriptor<GnuPgKeyFingerprint> descriptor
    )
    {
        base.Configure(descriptor);
        descriptor.Name(nameof(UserGnuPgKeyFingerprintFilterType)[..^"FilterType".Length] + GraphQlConstants.FilterInputSuffix);
        descriptor.Field(x => x.User).Ignore();
    }
}