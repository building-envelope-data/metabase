using System.Collections.Generic;

namespace Metabase.GraphQl.Components;

public sealed class UpdateComponentError
    : UserErrorBase<UpdateComponentErrorCode>
{
    public UpdateComponentError(
        UpdateComponentErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}