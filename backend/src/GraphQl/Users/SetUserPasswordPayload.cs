using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class SetUserPasswordPayload
    : UserPayload<SetUserPasswordError>
{
    public SetUserPasswordPayload(
        User user
    )
        : base(user)
    {
    }

    public SetUserPasswordPayload(
        SetUserPasswordError error
    )
        : base(error)
    {
    }

    public SetUserPasswordPayload(
        User user,
        IReadOnlyCollection<SetUserPasswordError> errors
    )
        : base(user, errors)
    {
    }

    public SetUserPasswordPayload(
        User user,
        SetUserPasswordError error
    )
        : base(user, error)
    {
    }
}