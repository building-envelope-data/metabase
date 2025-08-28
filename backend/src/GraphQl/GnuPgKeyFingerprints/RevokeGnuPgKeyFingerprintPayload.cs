using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class RevokeGnuPgKeyFingerprintPayload
{
    public RevokeGnuPgKeyFingerprintPayload(
        GnuPgKeyFingerprint gnuPgKeyFingerprint
    )
    {
        GnuPgKeyFingerprint = gnuPgKeyFingerprint;
    }

    public RevokeGnuPgKeyFingerprintPayload(
        IReadOnlyCollection<RevokeGnuPgKeyFingerprintError> errors
    )
    {
        Errors = errors;
    }

    public RevokeGnuPgKeyFingerprintPayload(
        RevokeGnuPgKeyFingerprintError error
    )
        : this([error])
    {
    }

    public GnuPgKeyFingerprint? GnuPgKeyFingerprint { get; }
    public IReadOnlyCollection<RevokeGnuPgKeyFingerprintError>? Errors { get; }
}