using System.Collections.Generic;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class RemoveUserMethodDeveloperError
    : UserErrorBase<RemoveUserMethodDeveloperErrorCode>
{
    public RemoveUserMethodDeveloperError(
        RemoveUserMethodDeveloperErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}