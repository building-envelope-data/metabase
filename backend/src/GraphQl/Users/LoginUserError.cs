using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserError
    : UserErrorBase<LoginUserErrorCode>
{
    public LoginUserError(
        LoginUserErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}