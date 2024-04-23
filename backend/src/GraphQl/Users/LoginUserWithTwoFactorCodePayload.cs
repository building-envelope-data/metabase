using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserWithTwoFactorCodePayload
    : UserPayload<LoginUserWithTwoFactorCodeError>
{
    public LoginUserWithTwoFactorCodePayload(
        User user
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