using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum LoginUserWithRecoveryCodeErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        NOT_ALLOWED,
        LOCKED_OUT,
        INVALID_RECOVERY_CODE
    }
}