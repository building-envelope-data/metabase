using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public sealed class RegisterUserPayload
    : UserPayload
    {
      public IReadOnlyCollection<RegisterUserError>? Errors { get; }

      public RegisterUserPayload(
          Data.User user
          )
            : base(user)
        {
        }

      public RegisterUserPayload(
          IReadOnlyCollection<RegisterUserError> errors
          )
        {
          Errors = errors;
        }

        public RegisterUserPayload(
            RegisterUserError error
            )
        {
          Errors = new [] { error };
        }
    }
}
