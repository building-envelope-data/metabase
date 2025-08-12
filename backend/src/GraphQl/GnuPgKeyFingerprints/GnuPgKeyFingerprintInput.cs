using System;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

public sealed record GnuPgKeyFingerprintInput(
    Guid InstitutionId,
    Guid UserId,
    string Fingerprint
);