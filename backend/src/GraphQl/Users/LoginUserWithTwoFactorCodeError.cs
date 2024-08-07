using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserWithTwoFactorCodeError
    : UserErrorBase<LoginUserWithTwoFactorCodeErrorCode>
{
    public LoginUserWithTwoFactorCodeError(
        LoginUserWithTwoFactorCodeErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}