using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ChangeUserPasswordPayload
    : UserPayload<ChangeUserPasswordError>
{
    public ChangeUserPasswordPayload(
        User user
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
        User user,
        IReadOnlyCollection<ChangeUserPasswordError> errors
    )
        : base(user, errors)
    {
    }

    public ChangeUserPasswordPayload(
        User user,
        ChangeUserPasswordError error
    )
        : base(user, error)
    {
    }
}