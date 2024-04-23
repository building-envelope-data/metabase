namespace Metabase.GraphQl.Users;

public sealed class DisableUserTwoFactorAuthenticationPayload
    : UserPayload<DisableUserTwoFactorAuthenticationError>
{
    public DisableUserTwoFactorAuthenticationPayload(
        Data.User user
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