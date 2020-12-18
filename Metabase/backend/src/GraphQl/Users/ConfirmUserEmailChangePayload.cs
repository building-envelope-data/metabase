using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class ConfirmUserEmailChangePayload
    : UserPayload
    {
      public IReadOnlyCollection<ConfirmUserEmailChangeError>? Errors { get; }

      public ConfirmUserEmailChangePayload(
          Data.User user
          )
        : base(user)
      {
      }

      public ConfirmUserEmailChangePayload(
          IReadOnlyCollection<ConfirmUserEmailChangeError> errors
          )
      {
        Errors = errors;
      }

      public ConfirmUserEmailChangePayload(
          ConfirmUserEmailChangeError error
          )
      {
        Errors = new [] { error };
      }
    }
}
