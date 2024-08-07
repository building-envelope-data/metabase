using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class ResendUserEmailVerificationError
    : UserErrorBase<ResendUserEmailVerificationErrorCode>
{
    public ResendUserEmailVerificationError(
        ResendUserEmailVerificationErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}