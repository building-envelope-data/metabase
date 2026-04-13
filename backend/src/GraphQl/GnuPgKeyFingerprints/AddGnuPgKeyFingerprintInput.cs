using System;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed record AddGnuPgKeyFingerprintInput(
    Guid InstitutionId,
    string Fingerprint
);