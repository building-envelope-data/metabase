using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class ChangeUserPasswordPayload
    : UserPayload<ChangeUserPasswordError>
{
    public ChangeUserPasswordPayload(
        Data.User user
    )
        : base(user)
    {
    }

    public ChangeUserPasswordPayload(
        ChangeUserPasswordError error
    )
        : base(error)
    {
    }

    public ChangeUserPasswordPayload(
        Data.User user,
        IReadOnlyCollection<ChangeUserPasswordError> errors
    )
        : base(user, errors)
    {
    }

    public ChangeUserPasswordPayload(
        Data.User user,
        ChangeUserPasswordError error
    )
        : base(user, error)
    {
    }
}