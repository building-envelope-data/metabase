using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ResendUserEmailVerificationPayload
    : UserPayload<ResendUserEmailVerificationError>
    {
      public ResendUserEmailVerificationPayload(
          Data.User user
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
}
