using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class ChangeUserEmailPayload
      : UserPayload<ChangeUserEmailError>
    {
        public ChangeUserEmailPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public ChangeUserEmailPayload(
            ChangeUserEmailError error
            )
          : base(error)
        {
        }

        public ChangeUserEmailPayload(
            Data.User user,
            ChangeUserEmailError error
            )
          : base(user, error)
        {
        }
    }
}