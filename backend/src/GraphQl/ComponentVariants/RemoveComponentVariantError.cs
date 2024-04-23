using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentVariants;

public sealed class RemoveComponentVariantError
    : UserErrorBase<RemoveComponentVariantErrorCode>
{
    public RemoveComponentVariantError(
        RemoveComponentVariantErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}