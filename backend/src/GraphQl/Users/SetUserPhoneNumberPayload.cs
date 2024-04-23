using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class SetUserPhoneNumberPayload
    : UserPayload<SetUserPhoneNumberError>
{
    public SetUserPhoneNumberPayload(
        User user
    )
        : base(user)
    {
    }

    public SetUserPhoneNumberPayload(
        SetUserPhoneNumberError error
    )
        : base(error)
    {
    }

    public SetUserPhoneNumberPayload(
        User user,
        IReadOnlyCollection<SetUserPhoneNumberError> errors
    )
        : base(user, errors)
    {
    }

    public SetUserPhoneNumberPayload(
        User user,
        SetUserPhoneNumberError error
    )
        : base(user, error)
    {
    }
}