using System.Collections.Generic;
using Metabase.GraphQl.OpenIdConnect.Applications;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AddGnuPgKeyFingerprintError(
    AddGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AddGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}