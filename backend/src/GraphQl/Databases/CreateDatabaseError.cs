using System.Collections.Generic;

namespace Metabase.GraphQl.Databases;

public sealed class CreateDatabaseError
    : UserErrorBase<CreateDatabaseErrorCode>
{
    public CreateDatabaseError(
        CreateDatabaseErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}