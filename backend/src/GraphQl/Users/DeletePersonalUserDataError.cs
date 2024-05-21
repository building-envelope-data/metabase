using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class DeletePersonalUserDataError
    : UserErrorBase<DeletePersonalUserDataErrorCode>
{
    public DeletePersonalUserDataError(
        DeletePersonalUserDataErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}