using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class SetUserPasswordPayload
    : UserPayload<SetUserPasswordError>
    {
        public SetUserPasswordPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public SetUserPasswordPayload(
            IReadOnlyCollection<SetUserPasswordError> errors
            )
          : base(errors)
        {
        }

        public SetUserPasswordPayload(
            SetUserPasswordError error
            )
          : base(error)
        {
        }
    }
}
