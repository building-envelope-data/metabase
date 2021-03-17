using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class RegisterUserPayload
      : UserPayload<RegisterUserError>
    {
        public RegisterUserPayload(
            Data.User user
            )
              : base(user)
        {
        }

        public RegisterUserPayload(
            IReadOnlyCollection<RegisterUserError> errors
            )
          : base(errors)
        {
        }

        public RegisterUserPayload(
            RegisterUserError error
            )
          : base(error)
        {
        }
    }
}