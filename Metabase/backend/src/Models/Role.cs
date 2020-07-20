using CSharpFunctionalExtensions;
using Infrastructure.Models;
using Infrastructure.ValueObjects;
using Microsoft.AspNetCore.Identity;
using Errors = Infrastructure.Errors;
using Guid = System.Guid;

namespace Metabase.Models
{
    public sealed class Role
      : Model
    {
        private Role(
            Id id,
            Timestamp timestamp
            )
            : base(id, timestamp)
        {
        }

        public static Result<Role, Errors> From(
            Id id,
            Timestamp timestamp
            )
        {
            return Result.Success<Role, Errors>(
                new Role(
                  id: id,
                  timestamp: timestamp
                )
                );
        }
    }

    // TODO Create an event sourced `IdentityRole`.
    public sealed class RoleX : IdentityRole<Guid>
    {
    }
}