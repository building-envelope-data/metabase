using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users;

[SuppressMessage("Naming", "CA1707")]
public enum ChangeUserPasswordErrorCode
{
    UNKNOWN,
    UNKNOWN_USER,
    NO_PASSWORD,
    PASSWORD_CONFIRMATION_MISMATCH,
    PASSWORD_REQUIRES_DIGIT,
    PASSWORD_REQUIRES_LOWER,
    PASSWORD_REQUIRES_NON_ALPHANUMERIC,
    PASSWORD_REQUIRES_UPPER,
    PASSWORD_TOO_SHORT
}