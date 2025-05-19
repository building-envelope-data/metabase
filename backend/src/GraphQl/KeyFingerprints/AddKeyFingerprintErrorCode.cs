using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.KeyFingerprints;

[SuppressMessage("Naming", "CA1707")]
public enum AddKeyFingerprintErrorCode
{
    UNKNOWN,
    UNAUTHORIZED,
    UNKNOWN_INSTITUTION,
    UNKNOWN_USER,
    NOT_ALLOWED,
    UNKNOWN_REPRESENTATIVE,
}