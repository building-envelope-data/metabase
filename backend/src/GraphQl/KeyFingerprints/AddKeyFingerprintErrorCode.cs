using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.KeyFingerprints;

[SuppressMessage("Naming", "CA1707")]
public enum AddKeyFingerprintErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    UNKNOWN_USER,
    USER_UNAUTHORIZED,
    UNKNOWN_REPRESENTATIVE,
}