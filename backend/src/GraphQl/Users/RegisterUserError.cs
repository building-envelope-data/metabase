using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class RegisterUserError
    : UserErrorBase<RegisterUserErrorCode>
{
    public RegisterUserError(
        RegisterUserErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}