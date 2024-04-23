using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class ConfirmUserEmailError
    : UserErrorBase<ConfirmUserEmailErrorCode>
{
    public ConfirmUserEmailError(
        ConfirmUserEmailErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}