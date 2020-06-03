using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Guid = System.Guid;

namespace Icon.Models
{
    public sealed class User
      : Model
    {
        private User(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
            : base(id, timestamp)
        {
        }

        public static Result<User, Errors> From(
            ValueObjects.Id id,
            ValueObjects.Timestamp timestamp
            )
        {
            return Result.Ok<User, Errors>(
                new User(
                  id: id,
                  timestamp: timestamp
                )
                );
        }
    }

    // TODO Combine `User` and `UserX` by creating an event sourced `IdentityUser`.
    public sealed class UserX : IdentityUser<Guid>
    {
    }
}