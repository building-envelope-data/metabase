using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class DeleteUserPayload
    : UserPayload<DeleteUserError>
{
    public DeleteUserPayload(
        User user
    )
        : base(user)
    {
    }

    public DeleteUserPayload(
        DeleteUserError error
    )
        : base(error)
    {
    }

    public DeleteUserPayload(
        User user,
        IReadOnlyCollection<DeleteUserError> errors
    )
        : base(user, errors)
    {
    }

    public DeleteUserPayload(
        User user,
        DeleteUserError error
    )
        : base(user, error)
    {
    }
}