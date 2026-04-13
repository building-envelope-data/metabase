using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.GnuPgKeyFingerprints;

[SuppressMessage("Naming", "CA1707")]
public enum AllowGnuPgKeyFingerprintErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_FINGERPRINT
}