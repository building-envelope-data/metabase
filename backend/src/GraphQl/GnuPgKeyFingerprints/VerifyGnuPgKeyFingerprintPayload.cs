using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class VerifyGnuPgKeyFingerprintPayload
{
    public VerifyGnuPgKeyFingerprintPayload(
        bool valid
    )
    {
        Valid = valid;
    }

    public VerifyGnuPgKeyFingerprintPayload(
        IReadOnlyCollection<VerifyGnuPgKeyFingerprintError> errors
    )
    {
        Errors = errors;
    }

    public VerifyGnuPgKeyFingerprintPayload(
        VerifyGnuPgKeyFingerprintError error
    )
        : this([error])
    {
    }

    public bool? Valid { get; }
    public IReadOnlyCollection<VerifyGnuPgKeyFingerprintError>? Errors { get; }
}