using Metabase.GraphQl.OpenIdConnect.Applications;
using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class ForbidGnuPgKeyFingerprintError(
    ForbidGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<ForbidGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}