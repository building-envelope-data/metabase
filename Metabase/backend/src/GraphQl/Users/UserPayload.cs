using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
  public abstract class UserPayload<TUserError>
    : GraphQl.Payload
    where TUserError : UserError
    {
        public Data.User? User { get; }
        public IReadOnlyCollection<TUserError>? Errors { get; }

        protected UserPayload(
            Data.User user
            )
        {
            User = user;
        }

        protected UserPayload(
            IReadOnlyCollection<TUserError> errors
            )
        {
            Errors = errors;
        }

        protected UserPayload(
            TUserError error
            )
          : this(new [] { error })
        {
        }
    }
}
