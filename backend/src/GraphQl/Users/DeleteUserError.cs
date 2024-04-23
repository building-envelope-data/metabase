using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class DeleteUserError
    : UserErrorBase<DeleteUserErrorCode>
{
    public DeleteUserError(
        DeleteUserErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}