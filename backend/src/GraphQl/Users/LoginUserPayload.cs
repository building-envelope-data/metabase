using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class LoginUserPayload
    : UserPayload<LoginUserError>
{
    public LoginUserPayload(
        User user,
        bool requiresTwoFactor
    )
        : base(user)
    {
        RequiresTwoFactor = requiresTwoFactor;
    }

    public LoginUserPayload(
        LoginUserError error
    )
        : base(error)
    {
    }

    public bool? RequiresTwoFactor { get; }
}