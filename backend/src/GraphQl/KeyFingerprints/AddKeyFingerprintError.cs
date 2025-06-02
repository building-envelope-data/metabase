using Metabase.GraphQl.OpenIdConnect.Application;
using System.Collections.Generic;

namespace Metabase.GraphQl.KeyFingerprints;

public class AddKeyFingerprintError(
    AddKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AddKeyFingerprintErrorCode>(code, message, path)
{
}