using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ConfirmUserEmailChangePayload
    : UserPayload<ConfirmUserEmailChangeError>
{
    public ConfirmUserEmailChangePayload(
        User user
    )
        : base(user)
    {
    }

    public ConfirmUserEmailChangePayload(
        IReadOnlyCollection<ConfirmUserEmailChangeError> errors
    )
        : base(errors)
    {
    }

    public ConfirmUserEmailChangePayload(
        ConfirmUserEmailChangeError error
    )
        : base(error)
    {
    }
}