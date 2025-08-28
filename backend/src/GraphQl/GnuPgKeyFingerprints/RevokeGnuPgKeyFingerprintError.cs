using Metabase.GraphQl.OpenIdConnect.Applications;
using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class RevokeGnuPgKeyFingerprintError(
    RevokeGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<RevokeGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}