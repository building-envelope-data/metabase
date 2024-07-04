using System.Diagnostics.CodeAnalysis;

namespace Metabase.GraphQl.Users;

[SuppressMessage("Naming", "CA1707")]
public enum LoginUserWithTwoFactorCodeErrorCode
{
    UNKNOWN,
    UNKNOWN_USER,
    NOT_ALLOWED,
    LOCKED_OUT,
    INVALID_AUTHENTICATOR_CODE
}