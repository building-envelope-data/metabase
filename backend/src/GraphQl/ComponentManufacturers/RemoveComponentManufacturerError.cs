using System.Collections.Generic;

namespace Metabase.GraphQl.ComponentManufacturers;

public sealed class RemoveComponentManufacturerError
    : UserErrorBase<RemoveComponentManufacturerErrorCode>
{
    public RemoveComponentManufacturerError(
        RemoveComponentManufacturerErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}