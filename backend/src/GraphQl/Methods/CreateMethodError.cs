using System.Collections.Generic;

namespace Metabase.GraphQl.Methods;

public sealed class CreateMethodError
    : UserErrorBase<CreateMethodErrorCode>
{
    public CreateMethodError(
        CreateMethodErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}