using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class ConfirmUserEmailChangeError
    : UserErrorBase<ConfirmUserEmailChangeErrorCode>
{
    public ConfirmUserEmailChangeError(
        ConfirmUserEmailChangeErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}