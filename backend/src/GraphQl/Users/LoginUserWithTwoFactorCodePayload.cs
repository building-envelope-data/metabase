namespace Metabase.GraphQl.Users;

public sealed class LoginUserWithTwoFactorCodePayload
    : UserPayload<LoginUserWithTwoFactorCodeError>
{
    public LoginUserWithTwoFactorCodePayload(
        Data.User user
    )
        : base(user)
    {
    }

    public LoginUserWithTwoFactorCodePayload(
        LoginUserWithTwoFactorCodeError error
    )
        : base(error)
    {
    }
}