using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ResetUserTwoFactorAuthenticatorPayload
    : UserPayload<ResetUserTwoFactorAuthenticatorError>
{
    public ResetUserTwoFactorAuthenticatorPayload(
        User user
    )
        : base(user)
    {
    }

    public ResetUserTwoFactorAuthenticatorPayload(
        ResetUserTwoFactorAuthenticatorError error
    )
        : base(error)
    {
    }
}