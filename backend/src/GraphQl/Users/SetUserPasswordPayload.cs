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
            SetUserPasswordError error
            )
          : base(error)
        {
        }

        public SetUserPasswordPayload(
            Data.User user,
            IReadOnlyCollection<SetUserPasswordError> errors
            )
          : base(user, errors)
        {
        }

        public SetUserPasswordPayload(
            Data.User user,
            SetUserPasswordError error
            )
          : base(user, error)
        {
        }
    }
}