using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ChangeUserPasswordPayload
    : UserPayload<ChangeUserPasswordError>
    {
        public ChangeUserPasswordPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public ChangeUserPasswordPayload(
            IReadOnlyCollection<ChangeUserPasswordError> errors
            )
          : base(errors)
        {
        }

        public ChangeUserPasswordPayload(
            ChangeUserPasswordError error
            )
          : base(error)
        {
        }
    }
}
