using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class SetUserPhoneNumberPayload
    : UserPayload<SetUserPhoneNumberError>
    {
        public SetUserPhoneNumberPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public SetUserPhoneNumberPayload(
            IReadOnlyCollection<SetUserPhoneNumberError> errors
            )
          : base(errors)
        {
        }

        public SetUserPhoneNumberPayload(
            SetUserPhoneNumberError error
            )
          : base(error)
        {
        }
    }
}
