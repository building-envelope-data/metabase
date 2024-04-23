using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class RegisterUserPayload
    : UserPayload<RegisterUserError>
{
    public RegisterUserPayload(
        User user
    )
        : base(user)
    {
    }

    public RegisterUserPayload(
        IReadOnlyCollection<RegisterUserError> errors
    )
        : base(errors)
    {
    }

    public RegisterUserPayload(
        RegisterUserError error
    )
        : base(error)
    {
    }
}