using System.Collections.Generic;

namespace Metabase.GraphQl.DataFormats;

public sealed class UpdateDataFormatError
    : UserErrorBase<UpdateDataFormatErrorCode>
{
    public UpdateDataFormatError(
        UpdateDataFormatErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}