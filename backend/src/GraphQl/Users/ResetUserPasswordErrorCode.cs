using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum ResetUserPasswordErrorCode
    {
        UNKNOWN,
        INVALID_RESET_CODE,
        PASSWORD_CONFIRMATION_MISMATCH,
        PASSWORD_REQUIRES_DIGIT,
        PASSWORD_REQUIRES_LOWER,
        PASSWORD_REQUIRES_NON_ALPHANUMERIC,
        PASSWORD_REQUIRES_UPPER,
        PASSWORD_TOO_SHORT
    }
}