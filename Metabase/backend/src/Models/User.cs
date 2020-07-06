using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Models
{
    public sealed class User
      : Model
    {
        private User(
            Id id,
            Timestamp timestamp
            )
            : base(id, timestamp)
        {
        }

        public static Result<User, Errors> From(
            Id id,
            Timestamp timestamp
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