using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class RequestUserPasswordResetError
    : UserErrorBase<RequestUserPasswordResetErrorCode>
{
    public RequestUserPasswordResetError(
        RequestUserPasswordResetErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}