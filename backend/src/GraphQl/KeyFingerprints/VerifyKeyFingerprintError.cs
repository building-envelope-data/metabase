using System.Collections.Generic;

namespace Metabase.GraphQl.KeyFingerprints;

public sealed class VerifyKeyFingerprintError(
    VerifyKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<VerifyKeyFingerprintErrorCode>(code, message, path)
{
}