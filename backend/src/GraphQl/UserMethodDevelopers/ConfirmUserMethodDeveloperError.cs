using System.Collections.Generic;

namespace Metabase.GraphQl.UserMethodDevelopers;

public sealed class ConfirmUserMethodDeveloperError
    : UserErrorBase<ConfirmUserMethodDeveloperErrorCode>
{
    public ConfirmUserMethodDeveloperError(
        ConfirmUserMethodDeveloperErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}