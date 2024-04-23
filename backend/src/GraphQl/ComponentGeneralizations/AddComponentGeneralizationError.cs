using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentGeneralizations;

public sealed class AddComponentGeneralizationError
    : UserErrorBase<AddComponentGeneralizationErrorCode>
{
    public AddComponentGeneralizationError(
        AddComponentGeneralizationErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}