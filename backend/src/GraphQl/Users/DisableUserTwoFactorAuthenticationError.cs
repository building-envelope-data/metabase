using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class DisableUserTwoFactorAuthenticationError
    : UserErrorBase<DisableUserTwoFactorAuthenticationErrorCode>
{
    public DisableUserTwoFactorAuthenticationError(
        DisableUserTwoFactorAuthenticationErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}