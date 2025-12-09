using System.Collections.Generic;
using Metabase.GraphQl.OpenIdConnect.Applications;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AllowGnuPgKeyFingerprintError(
    AllowGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AllowGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}