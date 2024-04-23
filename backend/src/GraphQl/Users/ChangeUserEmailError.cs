using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class ChangeUserEmailError
    : UserErrorBase<ChangeUserEmailErrorCode>
{
    public ChangeUserEmailError(
        ChangeUserEmailErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}