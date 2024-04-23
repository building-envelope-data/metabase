using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ConfirmUserEmailPayload
    : UserPayload<ConfirmUserEmailError>
{
    public ConfirmUserEmailPayload(
        User user
    )
        : base(user)
    {
    }

    public ConfirmUserEmailPayload(
        IReadOnlyCollection<ConfirmUserEmailError> errors
    )
        : base(errors)
    {
    }

    public ConfirmUserEmailPayload(
        ConfirmUserEmailError error
    )
        : base(error)
    {
    }
}