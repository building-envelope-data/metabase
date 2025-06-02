using System.Collections.Generic;

namespace Metabase.GraphQl.KeyFingerprints;

public class VerifyKeyFingerprintError(
    VerifyKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<VerifyKeyFingerprintErrorCode>(code, message, path)
{
}