using System.Collections.Generic;

namespace Metabase.GraphQl.Users;

public sealed class RemoveUserRolePayload
{
    public Data.User? User { get; }
    public IReadOnlyCollection<RemoveUserRoleError>? Errors { get; }

    public RemoveUserRolePayload(
        Data.User user
    )
    {
        User = user;
    }

    public RemoveUserRolePayload(
        IReadOnlyCollection<RemoveUserRoleError> errors
    )
    {
        Errors = errors;
    }

    public RemoveUserRolePayload(
        Data.User user,
        IReadOnlyCollection<RemoveUserRoleError> errors
    )
    {
        User = user;
        Errors = errors;
    }

    public RemoveUserRolePayload(
        RemoveUserRoleError error
    )
        : this(new[] { error })
    {
    }
}