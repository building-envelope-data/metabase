using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[SuppressMessage("Naming", "CA1707")]
public enum AddGnuPgKeyFingerprintErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    DUPLICATE_FINGERPRINT,
    UNKNOWN_KEY
}