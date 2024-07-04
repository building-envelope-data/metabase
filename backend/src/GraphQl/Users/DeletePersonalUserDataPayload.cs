using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class DeletePersonalUserDataPayload
    : UserPayload<DeletePersonalUserDataError>
{
    public DeletePersonalUserDataPayload(
        User user
    )
        : base(user)
    {
    }

    public DeletePersonalUserDataPayload(
        DeletePersonalUserDataError error
    )
        : base(error)
    {
    }

    public DeletePersonalUserDataPayload(
        User user,
        IReadOnlyCollection<DeletePersonalUserDataError> errors
    )
        : base(user, errors)
    {
    }

    public DeletePersonalUserDataPayload(
        User user,
        DeletePersonalUserDataError error
    )
        : base(user, error)
    {
    }
}