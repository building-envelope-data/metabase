using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class VerifyGnuPgKeyFingerprintError(
    VerifyGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<VerifyGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}