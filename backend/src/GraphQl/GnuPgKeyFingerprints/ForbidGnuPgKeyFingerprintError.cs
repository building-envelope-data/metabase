using System.Collections.Generic;
using Metabase.GraphQl.OpenIdConnect.Applications;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class ForbidGnuPgKeyFingerprintError(
    ForbidGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<ForbidGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}