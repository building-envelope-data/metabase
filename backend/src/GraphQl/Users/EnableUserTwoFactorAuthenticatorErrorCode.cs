using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users
{
    [SuppressMessage("Naming", "CA1707")]
    public enum EnableUserTwoFactorAuthenticatorErrorCode
    {
        UNKNOWN,
        UNKNOWN_USER,
        ENABLING_FAILED,
        INVALID_VERIFICATION_CODE
    }
}