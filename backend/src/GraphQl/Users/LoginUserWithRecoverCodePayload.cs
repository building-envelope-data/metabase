using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserWithRecoveryCodePayload
    : UserPayload<LoginUserWithRecoveryCodeError>
{
    public LoginUserWithRecoveryCodePayload(
        User user
    )
        : base(user)
    {
    }

    public LoginUserWithRecoveryCodePayload(
        LoginUserWithRecoveryCodeError error
    )
        : base(error)
    {
    }
}