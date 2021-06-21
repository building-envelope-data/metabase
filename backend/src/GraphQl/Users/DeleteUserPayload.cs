using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public sealed class DeleteUserPayload
      : UserPayload<DeleteUserError>
    {
        public DeleteUserPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public DeleteUserPayload(
            DeleteUserError error
            )
          : base(error)
        {
        }

        public DeleteUserPayload(
            Data.User user,
            IReadOnlyCollection<DeleteUserError> errors
            )
          : base(user, errors)
        {
        }

        public DeleteUserPayload(
            Data.User user,
            DeleteUserError error
            )
          : base(user, error)
        {
        }
    }
}