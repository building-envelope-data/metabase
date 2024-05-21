using System.Collections.Generic;
using Metabase.Data;

namespace Metabase.GraphQl.Users;

public sealed class ResendUserEmailVerificationPayload
    : UserPayload<ResendUserEmailVerificationError>
{
    public ResendUserEmailVerificationPayload(
        User user
    )
        : base(user)
    {
    }

    public ResendUserEmailVerificationPayload(
        IReadOnlyCollection<ResendUserEmailVerificationError> errors
    )
        : base(errors)
    {
    }

    public ResendUserEmailVerificationPayload(
        ResendUserEmailVerificationError error
    )
        : base(error)
    {
    }
}