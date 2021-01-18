using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class DeletePersonalUserDataPayload
    : UserPayload<DeletePersonalUserDataError>
    {
        public DeletePersonalUserDataPayload(
            Data.User user
            )
          : base(user)
        {
        }

        public DeletePersonalUserDataPayload(
            DeletePersonalUserDataError error
            )
          : base(error)
        {
        }

        public DeletePersonalUserDataPayload(
            Data.User user,
            IReadOnlyCollection<DeletePersonalUserDataError> errors
            )
          : base(user, errors)
        {
        }

        public DeletePersonalUserDataPayload(
            Data.User user,
            DeletePersonalUserDataError error
            )
          : base(user, error)
        {
        }
    }
}
