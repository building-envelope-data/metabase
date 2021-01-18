using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ConfirmUserEmailPayload
    : UserPayload<ConfirmUserEmailError>
    {
      public ConfirmUserEmailPayload(
          Data.User user
          )
        : base(user)
      {
      }

      public ConfirmUserEmailPayload(
          Data.User user,
          IReadOnlyCollection<ConfirmUserEmailError> errors
          )
        : base(user, errors)
      {
      }

      public ConfirmUserEmailPayload(
          ConfirmUserEmailError error
          )
        : base(error)
      {
      }
    }
}
