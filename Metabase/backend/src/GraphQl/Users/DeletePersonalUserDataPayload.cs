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
            IReadOnlyCollection<DeletePersonalUserDataError> errors
            )
          : base(errors)
        {
        }

        public DeletePersonalUserDataPayload(
            DeletePersonalUserDataError error
            )
          : base(error)
        {
        }
    }
}
