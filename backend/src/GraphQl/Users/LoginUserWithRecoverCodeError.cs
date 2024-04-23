using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserWithRecoveryCodeError
    : UserErrorBase<LoginUserWithRecoveryCodeErrorCode>
{
    public LoginUserWithRecoveryCodeError(
        LoginUserWithRecoveryCodeErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}