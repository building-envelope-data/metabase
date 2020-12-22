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
            IReadOnlyCollection<ChangeUserEmailError> errors
            )
          : base(errors)
        {
        }

        public ChangeUserEmailPayload(
            ChangeUserEmailError error
            )
          : base(error)
        {
        }
    }
}
