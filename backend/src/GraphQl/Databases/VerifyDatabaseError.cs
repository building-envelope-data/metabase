using System.Collections.Generic;

namespace Metabase.GraphQl.Databases;

public sealed class VerifyDatabaseError
    : UserErrorBase<VerifyDatabaseErrorCode>
{
    public VerifyDatabaseError(
        VerifyDatabaseErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}