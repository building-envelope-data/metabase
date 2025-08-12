using System.Collections.Generic;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed class AddGnuPgKeyFingerprintPayload
{
    public AddGnuPgKeyFingerprintPayload(
        string gnuPgKeyFingerprint
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

    public string? GnuPgKeyFingerprint { get; }
    public IReadOnlyCollection<AddGnuPgKeyFingerprintError>? Errors { get; }
}