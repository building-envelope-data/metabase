using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ChangeUserEmailPayload
    : UserPayload<ChangeUserEmailError>
{
    public ChangeUserEmailPayload(
        User user
    )
        : base(user)
    {
    }

    public ChangeUserEmailPayload(
        ChangeUserEmailError error
    )
        : base(error)
    {
    }

    public ChangeUserEmailPayload(
        User user,
        ChangeUserEmailError error
    )
        : base(user, error)
    {
    }
}