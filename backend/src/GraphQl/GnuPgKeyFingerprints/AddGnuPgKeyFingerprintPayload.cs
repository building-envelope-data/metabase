using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AddGnuPgKeyFingerprintPayload
{
    public AddGnuPgKeyFingerprintPayload(
        GnuPgKeyFingerprint gnuPgKeyFingerprint
    )
    {
        GnuPgKeyFingerprint = gnuPgKeyFingerprint;
    }

    public AddGnuPgKeyFingerprintPayload(
        IReadOnlyCollection<AddGnuPgKeyFingerprintError> errors
    )
    {
        Errors = errors;
    }

    public AddGnuPgKeyFingerprintPayload(
        AddGnuPgKeyFingerprintError error
    )
        : this([error])
    {
    }

    public GnuPgKeyFingerprint? GnuPgKeyFingerprint { get; }
    public IReadOnlyCollection<AddGnuPgKeyFingerprintError>? Errors { get; }
}