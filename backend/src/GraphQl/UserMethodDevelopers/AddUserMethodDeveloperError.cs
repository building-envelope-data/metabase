using System.Collections.Generic;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class AddUserMethodDeveloperError
    : UserErrorBase<AddUserMethodDeveloperErrorCode>
{
    public AddUserMethodDeveloperError(
        AddUserMethodDeveloperErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}