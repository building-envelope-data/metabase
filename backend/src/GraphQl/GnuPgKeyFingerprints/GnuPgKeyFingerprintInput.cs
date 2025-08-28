using System;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed record GnuPgKeyFingerprintInput(
    Guid InstitutionId,
    string Fingerprint
);