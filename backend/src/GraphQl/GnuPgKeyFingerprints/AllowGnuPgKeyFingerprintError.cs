using Metabase.GraphQl.OpenIdConnect.Applications;
using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AllowGnuPgKeyFingerprintError(
    AllowGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AllowGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}