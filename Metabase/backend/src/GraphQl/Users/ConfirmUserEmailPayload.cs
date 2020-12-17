using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ConfirmUserEmailPayload
    : UserPayload
    {
      public IReadOnlyCollection<ConfirmUserEmailError>? Errors { get; }

      public ConfirmUserEmailPayload(
          Data.User user
          )
        : base(user)
      {
      }

      public ConfirmUserEmailPayload(
          IReadOnlyCollection<ConfirmUserEmailError> errors
          )
      {
        Errors = errors;
      }

      public ConfirmUserEmailPayload(
          ConfirmUserEmailError error
          )
      {
        Errors = new [] { error };
      }
    }
}
