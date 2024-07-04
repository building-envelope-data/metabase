using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class EnableUserTwoFactorAuthenticatorError
    : UserErrorBase<EnableUserTwoFactorAuthenticatorErrorCode>
{
    public EnableUserTwoFactorAuthenticatorError(
        EnableUserTwoFactorAuthenticatorErrorCode code,
        string message,
        IReadOnlyList<string> path
    )
        : base(code, message, path)
    {
    }
}