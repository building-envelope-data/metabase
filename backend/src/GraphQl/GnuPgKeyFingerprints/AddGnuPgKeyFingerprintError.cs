using Metabase.GraphQl.OpenIdConnect.Applications;
using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AddGnuPgKeyFingerprintError(
    AddGnuPgKeyFingerprintErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<AddGnuPgKeyFingerprintErrorCode>(code, message, path)
{
}