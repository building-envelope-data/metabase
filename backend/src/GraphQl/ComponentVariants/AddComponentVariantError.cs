using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentVariants;

public sealed class AddComponentVariantError
    : UserErrorBase<AddComponentVariantErrorCode>
{
    public AddComponentVariantError(
        AddComponentVariantErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}