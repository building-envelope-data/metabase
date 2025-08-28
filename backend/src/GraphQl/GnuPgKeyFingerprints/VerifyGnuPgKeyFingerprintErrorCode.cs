using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[SuppressMessage("Naming", "CA1707")]
public enum VerifyGnuPgKeyFingerprintErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    UNKNOWN_USER,
    UNKNOWN_FINGERPRINT,
}