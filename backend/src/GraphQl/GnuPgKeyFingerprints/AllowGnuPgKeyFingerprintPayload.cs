using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AllowGnuPgKeyFingerprintPayload
{
    public AllowGnuPgKeyFingerprintPayload(
        GnuPgKeyFingerprint gnuPgKeyFingerprint
    )
    {
        GnuPgKeyFingerprint = gnuPgKeyFingerprint;
    }

    public AllowGnuPgKeyFingerprintPayload(
        IReadOnlyCollection<AllowGnuPgKeyFingerprintError> errors
    )
    {
        Errors = errors;
    }

    public AllowGnuPgKeyFingerprintPayload(
        AllowGnuPgKeyFingerprintError error
    )
        : this([error])
    {
    }

    public GnuPgKeyFingerprint? GnuPgKeyFingerprint { get; }
    public IReadOnlyCollection<AllowGnuPgKeyFingerprintError>? Errors { get; }
}