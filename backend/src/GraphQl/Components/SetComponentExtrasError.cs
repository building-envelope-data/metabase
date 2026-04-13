using System.Collections.Generic;

namespace Metabase.GraphQl.Components;

public sealed class SetComponentExtrasError(
    SetComponentExtrasErrorCode code,
    string message,
    IReadOnlyList<string> path
    )
        : UserErrorBase<SetComponentExtrasErrorCode>(code, message, path)
{
}