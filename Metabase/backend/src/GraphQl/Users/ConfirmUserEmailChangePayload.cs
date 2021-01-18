using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ConfirmUserEmailChangePayload
    : UserPayload<ConfirmUserEmailChangeError>
    {
      public ConfirmUserEmailChangePayload(
          Data.User user
          )
        : base(user)
      {
      }

      public ConfirmUserEmailChangePayload(
          Data.User user,
          IReadOnlyCollection<ConfirmUserEmailChangeError> errors
          )
        : base(user, errors)
      {
      }

      public ConfirmUserEmailChangePayload(
          ConfirmUserEmailChangeError error
          )
        : base(error)
      {
      }
    }
}
