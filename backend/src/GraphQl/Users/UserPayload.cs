using System.Collections.Generic;

namespace Metabase.GraphQl.Users
{
    public abstract class UserPayload<TUserError>
        : GraphQl.Payload
        where TUserError : IUserError
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
            : this(new[] { error })
        {
        }

        protected UserPayload(
            Data.User user,
            IReadOnlyCollection<TUserError> errors
        )
        {
            User = user;
            Errors = errors;
        }

        protected UserPayload(
            Data.User user,
            TUserError error
        )
            : this(user, new[] { error })
        {
        }
    }
}