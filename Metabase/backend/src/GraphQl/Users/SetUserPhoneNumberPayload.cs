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
            SetUserPhoneNumberError error
            )
          : base(error)
        {
        }

        public SetUserPhoneNumberPayload(
            Data.User user,
            IReadOnlyCollection<SetUserPhoneNumberError> errors
            )
          : base(user, errors)
        {
        }

        public SetUserPhoneNumberPayload(
            Data.User user,
            SetUserPhoneNumberError error
            )
          : base(user, error)
        {
        }
    }
}
