using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class ForbidGnuPgKeyFingerprintPayload
{
    public ForbidGnuPgKeyFingerprintPayload(
        GnuPgKeyFingerprint gnuPgKeyFingerprint
    )
    {
        GnuPgKeyFingerprint = gnuPgKeyFingerprint;
    }

    public ForbidGnuPgKeyFingerprintPayload(
        IReadOnlyCollection<ForbidGnuPgKeyFingerprintError> errors
    )
    {
        Errors = errors;
    }

    public ForbidGnuPgKeyFingerprintPayload(
        ForbidGnuPgKeyFingerprintError error
    )
        : this([error])
    {
    }

    public GnuPgKeyFingerprint? GnuPgKeyFingerprint { get; }
    public IReadOnlyCollection<ForbidGnuPgKeyFingerprintError>? Errors { get; }
}