using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class DisableUserTwoFactorAuthenticationPayload
    : UserPayload<DisableUserTwoFactorAuthenticationError>
{
    public DisableUserTwoFactorAuthenticationPayload(
        User user
    )
        : base(user)
    {
    }

    public DisableUserTwoFactorAuthenticationPayload(
        DisableUserTwoFactorAuthenticationError error
    )
        : base(error)
    {
    }
}