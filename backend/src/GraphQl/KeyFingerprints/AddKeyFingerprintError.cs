using Metabase.GraphQl.OpenIdConnect.Applications;
using System.Collections.Generic;

namespace Metabase.GraphQl.KeyFingerprints;

public sealed class AddKeyFingerprintError(
    AddKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AddKeyFingerprintErrorCode>(code, message, path)
{
}